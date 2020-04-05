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
        // this for multi threding parse
        public List<Page> Pages { get; private set; }

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
        public string LangCode { get; private set; }

        
        [JsonProperty("hash")]
        public string Hash { get; private set; }
        
     
        [JsonProperty("server")]
        public string ServerUrl { get; private set; }


        [JsonProperty("lang_name")]
        public string LangName { get; private set; }

        
    }
}
