using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangadexDownloader.ContentInfo
{
    public class MangaInfo : IMangaInfo
    {
        /// <summary>
        /// manga's cover url
        /// </summary>
        [JsonProperty("cover_url")]
        public string CoverUrl { get; private set; }
        /// <summary>
        /// title of manga
        /// </summary>
        
        [JsonProperty("title")]
        public string Title { get; private set; }
        /// <summary>
        /// Chapters in manga
        /// </summary>
        [JsonProperty("chapter")]
        public JObject Chapters { get; private set; }
    }
}
