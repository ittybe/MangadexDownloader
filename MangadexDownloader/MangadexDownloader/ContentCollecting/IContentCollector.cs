using MangadexDownloader.ContentInfo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

//PdfImage image = PdfImage.FromFile(@"D:\images\bear.tif");
////Set image display location and size in PDF

//float widthFitRate = image.PhysicalDimension.Width / page.Canvas.ClientSize.Width;

//float heightFitRate = image.PhysicalDimension.Height / page.Canvas.ClientSize.Height;

//float fitRate = Math.Max(widthFitRate, heightFitRate);

//float fitWidth = image.PhysicalDimension.Width / fitRate;

//float fitHeight = image.PhysicalDimension.Height / fitRate;

//page.Canvas.DrawImage(image, 30, 30, fitWidth, fitHeight);
namespace MangadexDownloader.ContentCollecting
{
    public interface IContentCollector
    {
        /// <summary>
        /// dir where all pages
        /// </summary>
        DirectoryInfo Dir { get; set; }
        
        /// <summary>
        /// collect all pages to pdf format
        /// </summary>
        /// <param name="outputPath">output file path</param>
        void CollectContentToPdf(string outputPath);
    }
}
