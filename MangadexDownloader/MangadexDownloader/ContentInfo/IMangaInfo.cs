using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangadexDownloader.ContentInfo
{
    /// <summary>
    /// interface MangaInfo
    /// </summary>
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
        JObject Chapters { get; }

        /// <summary>
        /// short info about manga
        /// </summary>
        List<MangaInfo.ShortChapterInfo> ShortChaptersInfo { get; }
    }
}
