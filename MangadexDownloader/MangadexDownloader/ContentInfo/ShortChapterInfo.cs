using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangadexDownloader.ContentInfo
{
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


        private string volume;
        /// <summary>
        /// volume number
        /// </summary>
        [JsonProperty("volume")]
        public string Volume 
        {
            get { return volume; }
            set 
            {
                // for some fricking reason some chapters doesnt have volume!
                if (value.CompareTo(string.Empty) == 0)
                    value = "0";
                volume = value;
            }
        }

        private string chapter;
        /// <summary>
        /// chapter number
        /// </summary>
        [JsonProperty("chapter")]
        public string Chapter 
        {
            get { return chapter; }
            set 
            {
                // for some fricking reason some chapters doesnt have chapter!
                if (value.CompareTo(string.Empty) == 0)
                    value = "0";
                chapter = value;
            } 
        }

        /// <summary>
        /// lang Code of this chapter
        /// </summary>
        [JsonProperty("lang_code")]
        public string LangCode { get; set; }
    }
}
