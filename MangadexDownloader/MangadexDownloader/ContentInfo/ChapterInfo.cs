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
        // all sets is public because this class like form for chapters

        [JsonProperty("id")]
        public int Id { get; set; }
        
        
        [JsonProperty("manga_id")]
        public int MangaId { get; set; }

        
        [JsonProperty("volume")]
        public string Volume { get; set; }
        
        
        [JsonProperty("chapter")]
        public string Chapter { get; set; }

        private JArray pageArray;

        
        [JsonProperty("page_array")]
        public JArray PageArray 
        {
            get 
            {
                return pageArray;
            }
            private set 
            {
                pageArray = value;
                List<Page> pages = new List<Page>();
                int pageNumber = 0;
                foreach (var page in PageArray)
                {
                    // get name
                    string PageNumber = pageNumber.ToString();//page.Previous.ToObject<string>()
                    pageNumber++;
                    // get page name
                    string PageName = page.ToObject<string>();

                    pages.Add(new Page() { PageName = PageName, PageNumber = PageNumber });
                }
                Pages = pages;
            } 
        }
        
        
        /// <summary>
        /// all pages
        /// </summary>
        public List<Page> Pages { get; set; }

        public class Page
        {
            /// <summary>
            /// page file name
            /// </summary>
            public string PageName{ get; set; }

            /// <summary>
            /// page number in chapter
            /// </summary>
            public string PageNumber { get; set; }
        }
        
        [JsonProperty("lang_code")]
        public string LangCode { get; set; }

        
        [JsonProperty("hash")]
        public string Hash { get; set; }
        
     
        [JsonProperty("server")]
        public string ServerUrl { get; set; }


        [JsonProperty("lang_name")]
        public string LangName { get; set; }
    }
}
