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
        public IChapterInfo ChapterInfo { get; set; }

        public string Pattern { get; }

        public DirectoryInfo Dir { get; set; }

        public ChapterParser(IChapterInfo chapterInfo, DirectoryInfo dir) 
        {
            ChapterInfo = chapterInfo;
            Dir = dir;
        }

        public void Parse()
        {
            foreach (var page in ChapterInfo.PageArray) 
            {
                // page's names have string type
                string pageName = page.ToObject<string>();

                // local page name and full path to page
                // VOLUMENUMBER_CHAPTERNUMBER_PAGENAME
                string localPageName = $"{ChapterInfo.Volume}_{ChapterInfo.Chapter}_{pageName}";
                string fullPath = $"{Dir.FullName}\\{localPageName}";
               
                // get url to image page
                string pageUrl = $"{ChapterInfo.ServerUrl}/{ChapterInfo.Hash}/{pageName}";

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
