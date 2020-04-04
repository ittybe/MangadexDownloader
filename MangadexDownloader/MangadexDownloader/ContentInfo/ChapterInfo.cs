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
        // private set for json property

        [JsonProperty("id")]
        public int Id { get; private set; }
        
        
        [JsonProperty("manga_id")]
        public int MangaId { get; private set; }

        
        [JsonProperty("volume")]
        public string Volume { get; private set; }
        
        
        [JsonProperty("chapter")]
        public string Chapter { get; private set; }
        
        [JsonProperty("page_array")]
        public JArray PageArray { get; private set; }

        [JsonProperty("lang_code")]
        public string LangCode { get; private set; }

        [JsonProperty("hash")]
        public string Hash { get; private set; }
        
        [JsonProperty("server")]
        public string ServerUrl { get; private set; }
    }
}
