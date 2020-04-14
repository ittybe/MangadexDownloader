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
        [JsonProperty("manga.cover_url")]
        public string CoverUrl { get; set; }
        /// <summary>
        /// title of manga
        /// </summary>
        
        [JsonProperty("manga.title")]
        public string Title { get; set; }


        private JObject chapters;
        /// <summary>
        /// Chapters in manga
        /// </summary>
        [JsonProperty("chapter")]
        public JObject Chapters 
        {
            get 
            {
                return chapters;
            }
            set 
            {
                chapters = value;
                ShortChaptersInfo.Clear();
                foreach (var chapter in chapters) 
                {
                    // chapter Value is JToken
                    ShortChapterInfo chapterInfo = chapter.Value.ToObject<ShortChapterInfo>();
                    
                    // set id because it can't work with JsonProperty
                    chapterInfo.Id = chapter.Key;
                    
                    ShortChaptersInfo.Add(chapterInfo);
                }
            }
        }
        
        /// <summary>
        /// ShortChaptersInfo contains short info about all chapters in manga (volume number, chapter number, id and language code)
        /// </summary>
        public List<ShortChapterInfo> ShortChaptersInfo { get; } = new List<ShortChapterInfo>();

        
    }
}
