using AngleSharp;
using AngleSharp.Io;
using MangadexDownloader.ContentInfo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MangadexDownloader.Parsing.ContentParsing
{
    public class ChapterParser : IChapterParser
    {
        public string Pattern { get; } = @"[0-9\.]+_[0-9\.]+_[0-9.]+.\S+";

        public DirectoryInfo Dir { get; set; }

        public ChapterParser(DirectoryInfo dir) 
        {
            Dir = dir;
        }

        public void Parse(IChapterInfo chapterInfo)
        {
            int pageNumber = 1;
            foreach (var page in chapterInfo.PageArray) 
            {
                // page's names have string type
                string pageName = page.ToObject<string>();
                // local page name and full path to page
                // VOLUMENUMBER_CHAPTERNUMBER_PAGENUMBER
                string localPageName = $"{chapterInfo.Volume}_{chapterInfo.Chapter}_{pageNumber++}";
                string pageExtension = Path.GetExtension(pageName);
                string fullPath = $"{Dir.FullName}\\{localPageName}{pageExtension}";
               
                // get url to image page
                string pageUrl = $"{chapterInfo.ServerUrl}/{chapterInfo.Hash}/{pageName}";

                var config = Configuration.Default.WithDefaultLoader();
                var context = BrowsingContext.New(config);

                var download = context.GetService<IDocumentLoader>().FetchAsync(new DocumentRequest(new Url(pageUrl)));
                
                var response = download.Task.Result;

                // write content to image file

                FileStream writer = File.OpenWrite(fullPath);
                response.Content.CopyTo(writer);
            }
        }
    }
}
