using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MangadexDownloader.ContentInfo;

namespace MangadexDownloader.Parsing.ContentParsing
{
    /// <summary>
    /// interface for MangaParser
    /// </summary>
    public interface IMangaParser
    {
        /// <summary>
        /// information about manga
        /// </summary>
        IMangaInfo MangaInfo { get; set; }
        /// <summary>
        /// Contains information about all Chapters
        /// </summary>
        List<IChapterInfo> ChaptersInfo { get; }

        /// <summary>
        /// dir for pages
        /// </summary>
        DirectoryInfo Dir { get; set; }

        /// <summary>
        /// parse all pages
        /// </summary>
        /// <param name="match">parse if only ChapterInfo is match</param>
        void Parse(Predicate<IChapterInfo> match);
     
        /// <summary>
        /// parse chapters info into List IChapterInfo property
        /// </summary>
        /// <param name="match">add ChapterInfo to list if ShortChapterInfo match</param>
        void ParseChaptersInfo(Predicate<MangaInfo.ShortChapterInfo> match);
    }
}
