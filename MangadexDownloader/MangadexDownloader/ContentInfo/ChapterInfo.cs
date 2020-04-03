using AngleSharp;
using AngleSharp.Dom;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MangadexDownloader.ContentInfo
{
    public class ChapterInfo : IChapterInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("manga_id")]
        public int MangaId { get; set; }

        [JsonProperty("volume")]
        public string Volume { get; set; }
        
        [JsonProperty("chapter")]
        public string Chapter { get; set; }
        
        public int PageCount { get; set; }

        [JsonProperty("lang_code")]
        public string LangCode { get; set; }
    }
}
