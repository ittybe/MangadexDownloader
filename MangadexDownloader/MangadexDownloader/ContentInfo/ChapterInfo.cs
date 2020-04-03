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
        public int Id { get; }
        
        
        [JsonProperty("manga_id")]
        public int MangaId { get; }

        
        [JsonProperty("volume")]
        public string Volume { get; }
        
        
        [JsonProperty("chapter")]
        public string Chapter { get; }
        
        
        [JsonProperty("page_array")]
        public JArray PageArray { get; }


        [JsonProperty("lang_code")]
        public string LangCode { get; }
    }
}
