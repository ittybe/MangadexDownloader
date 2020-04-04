﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MangadexDownloader.ContentInfo;
namespace MangadexDownloader.Parsing.ContentParsing
{
    /// <summary>
    /// interface for chapter parser
    /// </summary>
    public interface IChapterParser
    {
        /// <summary>
        /// Chapter info for parsing
        /// </summary>
        IChapterInfo ChapterInfo { get; set; }
        
        /// <summary>
        /// pattern for naming every page, regular expression
        /// </summary>
        string Pattern { get; }

        /// <summary>
        /// dir for pages
        /// </summary>
        DirectoryInfo Dir { get; set; }

        /// <summary>
        /// parse pages into directory
        /// </summary>
        void Parse();
    }
}