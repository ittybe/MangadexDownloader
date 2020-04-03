using AngleSharp;
using System;
using System.Collections.Generic;
using System.Text;
using MangadexDownloader.ContentInfo;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MangadexDownloader.Parsing
{
    public class ChapterJsonParser : IJsonParser
    {
        public string GetJson(int id)
        {
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            var document = context.OpenAsync($"https://mangadex.org/api/chapter/{id}").Result;
            // select tag with only json info
            var element = document.QuerySelector("pre");
            return element.TextContent;
        }
        public ChapterInfo ConvertJson(string json)
        {
            ChapterInfo chapterInfo = JsonConvert.DeserializeObject<ChapterInfo>(json);
            return chapterInfo;
        }
    }
}
