using AngleSharp;
using AngleSharp.Io;
using MangadexDownloader.ContentInfo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace MangadexDownloader.Parsing.ContentParsing
{
    public class ChapterParser : IChapterParser
    {
        /// <summary>
        /// pattern to file naming
        /// this struct of name
        /// VOLUMENUMBER_CHAPTERNUMBER_PAGENUMBER.EXTENTION;
        /// </summary>
        public const string Pattern = @"[0-9\.]+_[0-9\.]+_[0-9.]+.\S+";

        public DirectoryInfo Dir { get; set; }

        public ChapterParser(DirectoryInfo dir) 
        {
            Dir = dir;
        }

        public void Parse(IChapterInfo chapterInfo)
        {
            foreach (var page in chapterInfo.Pages) 
            {
                // parse one page
                ParsePage(page, chapterInfo);
            }
        }

        /// <summary>
        /// parse page 
        /// </summary>
        /// <param name="page">page info in chapter inf</param>
        /// <param name="chapterInfo">chapterInfo</param>
        protected void ParsePage(ChapterInfo.Page page, IChapterInfo chapterInfo) 
        {
            // page's names have string type
            string pageName = page.PageName;
            // local page name and full path to page
            // VOLUMENUMBER_CHAPTERNUMBER_PAGENUMBER
            string localPageName = $"{chapterInfo.Volume}_{chapterInfo.Chapter}_{page.PageNumber}";
            string pageExtension = Path.GetExtension(pageName);
            string fullPath = $"{Dir.FullName}\\{localPageName}{pageExtension}";

            // get url to image page
            string pageUrl = $"{chapterInfo.ServerUrl}/{chapterInfo.Hash}/{pageName}";

            // make request to server with page
            WebRequest request = WebRequest.Create(pageUrl);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            // open file

            BinaryReader reader = new BinaryReader(response.GetResponseStream());

            FileStream fileStream = File.OpenWrite(fullPath);
            BinaryWriter writer = new BinaryWriter(fileStream);

            // write content to image file

            // size of buffer
            int bufferSize = 1024;
            byte[] buffer;
            while (true) 
            {
                buffer = reader.ReadBytes(bufferSize);
                writer.Write(buffer);
                if (buffer.Length == 0)
                    break;
            }
            // save writed data
            writer.Flush();

            // close all streams

            writer.Close();
            reader.Close();
        }
    }
}
