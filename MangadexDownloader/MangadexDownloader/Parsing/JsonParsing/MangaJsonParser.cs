﻿using AngleSharp;
using System;
using System.Collections.Generic;
using System.Text;
using MangadexDownloader.ContentInfo;
using Newtonsoft.Json;

namespace MangadexDownloader.Parsing.JsonParsing
{
    public class MangaJsonParser : IMangaJsonParser
    {
        public string GetJson(int id)
        {
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            var document = context.OpenAsync($"https://mangadex.org/api/manga/{id}").Result;
            // select tag with only json info
            var element = document.QuerySelector("pre");
            return element.TextContent;
        }
        
        public MangaInfo ConvertJson(string json)
        {
            MangaInfo mangaInfo = JsonConvert.DeserializeObject<MangaInfo>(json);
            return mangaInfo;
        }

        public MangaInfo GetMangaInfo(int id)
        {
            string json = GetJson(id);
            return ConvertJson(json);
        }
    }
}