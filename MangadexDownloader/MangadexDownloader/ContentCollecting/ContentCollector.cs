using iText.IO.Font;
using iText.IO.Font.Otf;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Kernel.Pdf.Navigation;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Layout;
using iText.Layout.Properties;
using iText.Layout.Renderer;
using MangadexDownloader.ContentInfo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MangadexDownloader.ContentCollecting
{
    public class ContentCollector : IContentCollector
    {
        /// <summary>
        /// dir that contains all pages
        /// </summary>
        public DirectoryInfo Dir { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DirPath">dir that contains all pages</param>
        public ContentCollector(string DirPath)
        {
            Dir = new DirectoryInfo(DirPath);
        }
        private PdfDocument pdfDocument;
        private Document document;
        /// <summary>
        /// Collect all pages from dir to pdf file 
        /// </summary>
        /// <param name="outputPath">pdf file output</param>
        public void CollectContentToPdf(string outputPath)
        {
            // get pages info from Dir
            var pages = GetPagesInfo();

            // sort pages by volume chapter and page number
            // using default comparer in page info
            pages.Sort();

            pdfDocument = new PdfDocument(new PdfWriter(outputPath));
            document = new Document(pdfDocument);

            int pageIndex = 1;
            
            // create pdf file from pages
           
            foreach (var page in pages)
            {
                // ADD PAGE IMAGE SHAPE 

                AddPageWithImageShape(page, pageIndex++);

                // ADD IMAGE PAGE

                SetImageToPage(page);
            }
            // save pdf file
            if (pdfDocument.GetNumberOfPages() > 0)
            {
                pdfDocument.Close();
            }
#if DEBUG
            Trace.WriteLine($"{DateTime.Now}: pdf document saved successfully, path \"{outputPath}\"");
#endif
        }

        /// <summary>
        /// collect all pages to pdf format, also make table of content in pdf file
        /// </summary>
        /// <param name="outputPath">output file path</param>
        /// <param name="font">font for table of content</param>
        /// <param name="fontSizeInfo">size of information text</param>
        /// <param name="fontSizeHeader">size of header text</param>
        /// <param name="pageSize">page size of information and table of content</param>
        public void CollectContentToPdf(string outputPath, PdfFont font, float fontSizeInfo, float fontSizeHeader, PageSize pageSize)
        {
            // get pages info from Dir
            var pages = GetPagesInfo();

            // sort pages by volume chapter and page number
            // using default comparer in page info
            pages.Sort();

            pdfDocument = new PdfDocument(new PdfWriter(outputPath));
            document = new Document(pdfDocument);

            List<int> beginChaptersInfo = new List<int>();
            List<PageInfo> pagesInfo = new List<PageInfo>();
            string prevChapter = "";
            int pageIndex = 1;

            // create pdf file from pages

            foreach (var page in pages)
            {
                //// ADD INFO CHAPTER PAGE

                if (page.ChapterNumber.CompareTo(prevChapter) != 0)
                {
                    prevChapter = page.ChapterNumber;

                    AddBeginChapterInfo(page, font, fontSizeHeader, fontSizeInfo, pageIndex++, pageSize);

                    // number of page is last page index of this pdf
                    beginChaptersInfo.Add(pdfDocument.GetNumberOfPages());

                    // page for textLink
                    pagesInfo.Add(page);
                }

                // ADD PAGE IMAGE SHAPE 

                AddPageWithImageShape(page, pageIndex++);

                // ADD IMAGE PAGE

                SetImageToPage(page);
            }

            // create link to every chapter begin content 

            // for page index, first page we already created, then we will create second
            pageIndex = 1;

            pdfDocument.AddNewPage(pageIndex, pageSize);
            document = new Document(pdfDocument);

            Paragraph p = new Paragraph("Table of content").SetFont(font).SetFontSize(fontSizeHeader);

            document.Add(p);

            // get height tmp is for wrap text on next new page
            float heightTmp = document.GetTopMargin();

            // page Effective Area 
            var pageEffectiveArea = document.GetPageEffectiveArea(pageSize);

            // space between paragraphs
            float multiLeadingValue = 2f;

            for (int i = 0; i < pagesInfo.Count; i++)
            {
                // get info 

                PageInfo page = pagesInfo[i];
                int index = beginChaptersInfo[i];

                string textLink = $"Volume {page.VolumeNumber}, Chapter {page.ChapterNumber}: page number {index}";


                // create link and set margin of link

                p = CreateLinkToPage(index, textLink);
                p.SetMarginLeft(30);
                p.SetMultipliedLeading(multiLeadingValue);

                // set font and size
                p.SetFont(font);
                p.SetFontSize(fontSizeInfo);

                // calc height of paragraph

                //var area = GetParagraphArea(p, pageIndex);
                var rec = GetTextSize(textLink, font, fontSizeInfo);
                float heightParagraph = rec.GetHeight();

                // then check is paragraph out of Work area

                heightTmp += heightParagraph + (heightParagraph * multiLeadingValue);  // * 2 up and down space
                // Console.WriteLine($"heightTmp = {heightTmp}, pageEffectiveArea.GetHeight() = {pageEffectiveArea.GetHeight()}");
                
                // if yes we create new page
                if (heightTmp >= pageEffectiveArea.GetHeight())
                {
                    pageIndex++;

                    pdfDocument.AddNewPage(pageIndex, pageSize);
                    document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

                    heightTmp = document.GetTopMargin();
#if DEBUG
                    Trace.WriteLine($"{DateTime.Now}: new page content has created, pageIndex: {pageIndex}");
#endif
                }
                // add
                document.Add(p);
            }
            // save pdf file
            if (pdfDocument.GetNumberOfPages() > 0)
            {
                document.Close();
                pdfDocument.Close();
            }
#if DEBUG
            Trace.WriteLine($"{DateTime.Now}: pdf document saved successfully, path \"{outputPath}\"");
#endif
        }


        protected Rectangle GetTextSize(string text, PdfFont pdfFont, float fontSize) 
        {
            GlyphLine glyphLine = pdfFont.CreateGlyphLine(text);

            int width = 0;
            for (int i = 0; i < glyphLine.Size(); i++)
            {
                Glyph glyph = glyphLine.Get(i);
                width += glyph.GetWidth();
            }

            float userSpaceWidth = width * fontSize / 1000.0f;

            float ascent = pdfFont.GetAscent(text, fontSize);
            float descent = pdfFont.GetDescent(text, fontSize);

            float userSpaceHeight = ascent - descent;
            return new Rectangle(userSpaceWidth, userSpaceHeight);
        }

        protected LayoutArea GetParagraphArea(Paragraph p, int pageIndex) 
        {
            IRenderer paragraphRenderer = p.CreateRendererSubTree();
            LayoutResult result = paragraphRenderer.SetParent(document.GetRenderer()).
                                    Layout(new LayoutContext(new LayoutArea(pageIndex, new Rectangle(100, 1000))));
            
            return result.GetOccupiedArea();
        }

        /// <summary>
        /// creates link to specific page
        /// </summary>
        /// <param name="pageIndex">page index</param>
        /// <param name="textLink">text link</param>
        /// <returns>paragraph link</returns>
        protected Paragraph CreateLinkToPage(int pageIndex, string textLink) 
        {
            //PdfArray array = new PdfArray();
            //array.Add(document.GetPdfDocument().GetPage(pageIndex).GetPdfObject());
            //array.Add(PdfName.Fit);
            //PdfDestination dest = PdfDestination.MakeDestination(array);
            PdfExplicitDestination dest = PdfExplicitDestination.CreateFit(pdfDocument.GetPage(pageIndex));
            Paragraph link = new Paragraph(new Link(textLink, PdfAction.CreateGoTo(dest)));
            return link;
        }

        private void AddBeginChapterInfo(PageInfo page, PdfFont font, float volumeFontSize, float chapterFontSize, int pageIndex, PageSize pageSize)
        {
            // add PageSize A4
            pdfDocument.AddNewPage(pageIndex, pageSize);

            // add Volume info
            Paragraph volumeInfo = new Paragraph($"Volume {page.VolumeNumber}");
            volumeInfo.SetFont(font);
            volumeInfo.SetTextAlignment(TextAlignment.CENTER);
            volumeInfo.SetFontSize(volumeFontSize);

            document.Add(volumeInfo);

            // add ChapterInfo
            Paragraph chapterInfo = new Paragraph($"Chapter {page.ChapterNumber}");
            chapterInfo.SetFont(font);
            chapterInfo.SetTextAlignment(TextAlignment.CENTER);
            chapterInfo.SetFontSize(chapterFontSize);

            document.Add(chapterInfo);

            // document to next
            document.Add(new AreaBreak());
#if DEBUG
            Trace.WriteLine($"{DateTime.Now}: new page content info has created, pageIndex: {pageIndex}");
#endif
        }

        private void AddPageWithImageShape(PageInfo page, int pageIndex) 
        {
            // read image 
            char dirSeparatorChar = System.IO.Path.DirectorySeparatorChar;  // Path already exists in itext7
            string inputImagePath = $"{Dir.FullName}{dirSeparatorChar}{page.Fullname}";
            ImageData imageData = ImageDataFactory.Create(inputImagePath);

            // page size equals image size

            PageSize pageSize = new PageSize(imageData.GetWidth(), imageData.GetHeight());

            // add page-image-size-shape
            pdfDocument.AddNewPage(pageIndex, pageSize);
#if DEBUG
            Trace.WriteLine($"{DateTime.Now}: pdf document page has created with image shape, Volume = {page.VolumeNumber}, Chapter = {page.ChapterNumber}, Page = {page.PageNumber}, pdfPage = {pdfDocument.GetNumberOfPages()}");
#endif
        }

        private void SetImageToPage(PageInfo page)
        {
            // read image 

            char dirSeparatorChar = System.IO.Path.DirectorySeparatorChar;  // Path already exists in itext7
            string inputImagePath = $"{Dir.FullName}{dirSeparatorChar}{page.Fullname}";
            ImageData imageData = ImageDataFactory.Create(inputImagePath);

            // image
            Image image = new Image(imageData);

            // set in full page

            image.SetAutoScale(false);
            image.SetFixedPosition(0, 0);

            // add image

            document.Add(image);

            // go to next page

            document.Add(new AreaBreak());
#if DEBUG
            Trace.WriteLine($"{DateTime.Now}: pdf document page has filled with image, Volume = {page.VolumeNumber}, Chapter = {page.ChapterNumber}, Page = {page.PageNumber}, pdfPage = {pdfDocument.GetNumberOfPages()}");
#endif
        }
        

        /// <summary>
        /// get pages info from Dir
        /// </summary>
        /// <returns>pages info</returns>
        protected List<PageInfo> GetPagesInfo()
        {
            List<PageInfo> pages = new List<PageInfo>();
            FileInfo[] files = Dir.GetFiles();
            foreach (var file in files)
            {
                pages.Add((PageInfo)file.Name);
            }
            return pages;
        }

        
    }
}
