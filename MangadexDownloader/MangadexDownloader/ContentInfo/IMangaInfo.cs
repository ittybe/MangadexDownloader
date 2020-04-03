using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangadexDownloader.ContentInfo
{
    public interface IMangaInfo
    {
        /// <summary>
        /// url to cover of manga
        /// </summary>
        string CoverUrl { get; }
        
        /// <summary>
        /// title of manga
        /// </summary>
        string Title { get; }

        /// <summary>
        /// chapters info
        /// </summary>
        IEnumerable<JToken> Chapters { get; }
    }
}
