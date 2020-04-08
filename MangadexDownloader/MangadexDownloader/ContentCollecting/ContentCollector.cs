using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using MangadexDownloader.ContentInfo;
using System;
using System.Collections.Generic;
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
            
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(outputPath));
            int pagePdf = 0;
            foreach (var page in pages)
            {
                // read image 

                string inputImagePath = $@"{Dir.FullName}\{page.Fullname}";
                ImageData imageData = ImageDataFactory.Create(inputImagePath);

                // Load image from disk
                
                Image image = new Image(imageData);
                image.SetAutoScale(false);
                image.SetFixedPosition(0, 0);

                PageSize pageSize = new PageSize(image.GetImageWidth(), image.GetImageHeight());
                
                // Document to add layout elements: paragraphs, images etc
                
                Document document = new Document(pdfDocument, pageSize);

                // add content to next pages, we re doing that because we have pages that has different size and so on 
                
                document.Add(new AreaBreak(AreaBreakType.LAST_PAGE));

                // Create layout image object and provide parameters.

                document.Add(image);
            }
            if (pdfDocument.GetNumberOfPages() > 0) 
            {
                pdfDocument.Close();
            }
        }


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
