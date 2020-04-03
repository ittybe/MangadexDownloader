using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangadexDownloader.ContentInfo
{
    class MangaInfo : IMangaInfo
    {
        [JsonProperty("cover_url")]
        public string CoverUrl { get; private set; }
        
        [JsonProperty("title")]
        public string Title { get; private set; }

        [JsonProperty("chapter")]
        public IEnumerable<JToken> Chapters { get; private set; }
    }
}
