using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
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

            List<int> beginChaptersInfo = new List<int>();
            string prevChapter = "";
            int pageIndex = 1;
            // create image size shape pages
            foreach (var page in pages)
            {
                //// ADD INFO CHAPTER PAGE

                if (page.ChapterNumber.CompareTo(prevChapter) != 0)
                {
                    prevChapter = page.ChapterNumber;

                    AddBeginChapterInfo(page, 80, 40, pageIndex++);

                    // number of page is last page index of this pdf
                    beginChaptersInfo.Add(pdfDocument.GetNumberOfPages());
                }

                // ADD PAGE IMAGE SHAPE 

                AddPageWithImageShape(page, pageIndex++);

                // ADD IMAGE PAGE

                SetImageToPage(page);
            }
            if (pdfDocument.GetNumberOfPages() > 0)
            {
                pdfDocument.Close();
            }
#if DEBUG
            Trace.WriteLine($"{DateTime.Now}: pdf document saved successfully, path \"{outputPath}\"");
#endif
        }

        private void AddBeginChapterInfo(PageInfo page, float volumeFontSize, float chapterFontSize, int pageIndex)
        {
            // add PageSize A4
            pdfDocument.AddNewPage(pageIndex,PageSize.A4);
            PageSize pageSize = PageSize.A4;

            // add Volume info
            Paragraph volumeInfo = new Paragraph($"Volume {page.VolumeNumber}");
            volumeInfo.SetFontSize(volumeFontSize);
            //Paragraph volumeInfo = GetParagraphWithTabs($"Volume {page.VolumeNumber}", line, width);

            //volumeInfo.SetTextAlignment(TextAlignment.CENTER);
            volumeInfo.SetFontSize(volumeFontSize);
            document.Add(volumeInfo);

            // add ChapterInfo
            Paragraph chapterInfo = new Paragraph($"Chapter {page.ChapterNumber}");
            chapterInfo.SetFontSize(chapterFontSize);
            //Paragraph chapterInfo = GetParagraphWithTabs($"Chapter {page.ChapterNumber}", line, width);

            //chapterInfo.SetTextAlignment(TextAlignment.CENTER);
            chapterInfo.SetFontSize(chapterFontSize);

            document.Add(chapterInfo);

            // document to next
            document.Add(new AreaBreak());

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
