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
