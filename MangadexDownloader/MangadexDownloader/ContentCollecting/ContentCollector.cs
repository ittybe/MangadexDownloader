using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
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
            List<ImageData> imagesData= new List<ImageData>();
            // create image size shape pages
            foreach (var page in pages)
            {
                // read image 
                char dirSeparatorChar = System.IO.Path.DirectorySeparatorChar;  // Path already exists in itext7
                string inputImagePath = $"{Dir.FullName}{dirSeparatorChar}{page.Fullname}";
                ImageData imageData = ImageDataFactory.Create(inputImagePath);

                // page size equals image size

                PageSize pageSize = new PageSize(imageData.GetWidth(), imageData.GetHeight());

                // add page-image-size-shape

                pdfDocument.AddNewPage(pageSize);

                // save path

                imagesData.Add(imageData);
            }
            Trace.WriteLine($"{DateTime.Now}: pdf document empty pages has created, pages = {pages.Count}, path \"{outputPath}\"");
            // document 
            Document document = new Document(pdfDocument);
            foreach (var imageData in imagesData)
            {
                // image
                Image image = new Image(imageData);

                // set in full page

                image.SetAutoScale(false);
                image.SetFixedPosition(0, 0);

                // add image

                document.Add(image);

                // go to next page

                document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            }
            Trace.WriteLine($"{DateTime.Now}: pdf document pages has filled with images, imagesData = {imagesData.Count}, path \"{outputPath}\"");

            if (pdfDocument.GetNumberOfPages() > 0)
            {
                pdfDocument.Close();
            }
            Trace.WriteLine($"{DateTime.Now}: pdf document saved successfully, path \"{outputPath}\"");
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
