using AngleSharp;
using AngleSharp.Io;
using MangadexDownloader.ContentInfo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                string pageUrl = $"{chapterInfo.ServerUrl}/{chapterInfo.Hash}/{page.PageName}";
                
                // parse one page
                ParsePage(page, chapterInfo);
                Trace.WriteLine($"{DateTime.Now}: page \"{pageUrl}\" has parsed successfully, page number {page.PageNumber}, chapter number: {chapterInfo.Chapter}, volume number: {chapterInfo.Volume}, chapter id: {chapterInfo.Id}");
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
            Trace.WriteLine($"{DateTime.Now}: web request has created to this url \"{pageUrl}\", chapter id: {chapterInfo.Id}");

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Trace.WriteLine($"{DateTime.Now}: recieve response from \"{pageUrl}\", chapter id: {chapterInfo.Id}");
            
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
            Trace.WriteLine($"{DateTime.Now}: page has writed to file \"{fullPath}\", chapter id: {chapterInfo.Id}");

            // close all streams

            writer.Close();
            reader.Close();
        }
    }
}
