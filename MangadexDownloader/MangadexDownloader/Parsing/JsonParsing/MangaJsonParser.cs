using AngleSharp;
using System;
using System.Collections.Generic;
using System.Text;
using MangadexDownloader.ContentInfo;
using Newtonsoft.Json;

namespace MangadexDownloader.Parsing.JsonParsing
{
    public class MangaJsonParser : IMangaJsonParser
    {
        /// <summary>
        /// Get manga json
        /// </summary>
        /// <param name="id">manga's id</param>
        /// <exception cref="ApplicationException"></exception>
        /// <returns>json string about manga</returns>
        public string GetJson(int id)
        {
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            var document = context.OpenAsync($"https://mangadex.org/api/manga/{id}").Result;

            // select tag with only json info
            var element = document.QuerySelector("pre");
            if (element == null)
                throw new ApplicationException("Parsing json is failed!");
            return element.TextContent;
        }
        
        public MangaInfo ConvertJson(string json)
        {
            MangaInfo mangaInfo = JsonConvert.DeserializeObject<MangaInfo>(json);
            if (mangaInfo.ShortChaptersInfo.Count < 1)
                throw new ApplicationException($"MangaInfo json is invalid");
            return mangaInfo;
        }

        public MangaInfo GetMangaInfo(int id)
        {
            string json = GetJson(id);
            return ConvertJson(json);
        }
    }
}