﻿using System;
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
        /// call when page parsed
        /// </summary>
        OnProgressParserEventHandler OnProgress { get; set; }
        /// <summary>
        /// information about manga
        /// </summary>
        IMangaInfo MangaInfo { get; set; }

        /// <summary>
        /// dir for pages
        /// </summary>
        DirectoryInfo Dir { get; set; }

        /// <summary>
        /// parse chapter's pages into Dir
        /// </summary>
        /// <param name="match">match for parsing chapters</param>
        /// <param name="numberOfTry">number of try if while parsing something gone wrong it will try to parse this again this amount of time, for chapter parser</param>

        void Parse(Predicate<ShortChapterInfo> match, int numberOfTry);

        /// <summary>
        /// parse chapter's info from manga
        /// </summary>
        /// <param name="match">match for what chapter will be in result</param>
        /// <returns>list of info</returns>
        List<IChapterInfo> ParseChaptersInfo(Predicate<ShortChapterInfo> match);
    }
}