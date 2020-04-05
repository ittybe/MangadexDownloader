﻿using Newtonsoft.Json;
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
            private set 
            {
                chapters = value;
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

        /// <summary>
        /// short information about chapter
        /// </summary>
        public class ShortChapterInfo 
        {
            /// <summary>
            /// Id of this chapter
            /// </summary>
            // string because id is a key in key - value, and a key is always string
            public string Id { get; set; }
            /// <summary>
            /// volume number
            /// </summary>
            [JsonProperty("volume")]
            public string Volume { get; private set; }
            
            /// <summary>
            /// chapter number
            /// </summary>
            [JsonProperty("chapter")]
            public string Chapter { get; private set; }
            
            /// <summary>
            /// lang Code of this chapter
            /// </summary>
            [JsonProperty("lang_code")]
            public string LangCode { get; private set; }
        }
    }
}
