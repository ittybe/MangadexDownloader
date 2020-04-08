using AngleSharp;
using System;
using System.Collections.Generic;
using System.Text;
using MangadexDownloader.ContentInfo;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MangadexDownloader.Parsing.JsonParsing
{
    public class ChapterJsonParser : IChapterJsonParser
    {
        /// <summary>
        /// Get chapter json
        /// </summary>
        /// <param name="id">chapter's id</param>
        /// <exception cref="ApplicationException"></exception>
        /// <returns>json string about chapter</returns>
        public string GetJson(int id)
        {
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            var document = context.OpenAsync($"https://mangadex.org/api/chapter/{id}").Result;
            // select tag with only json info
            var element = document.QuerySelector("pre");
            if (element == null)
                throw new ApplicationException($"Parsing json is failed! chapter id: {id}");
            return element.TextContent;
        }
        public ChapterInfo ConvertJson(string json)
        {
            ChapterInfo chapterInfo = JsonConvert.DeserializeObject<ChapterInfo>(json);
            if (chapterInfo.Volume.CompareTo(string.Empty) == 0)
                throw new ApplicationException("ChapterInfo json is invalid");
            return chapterInfo;
        }
        public ChapterInfo GetChapterInfo(int id) 
        {
            string json = GetJson(id);
            return ConvertJson(json);
        }
    }
}
