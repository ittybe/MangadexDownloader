using System;
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
        /// dir for pages
        /// </summary>
        DirectoryInfo Dir { get; set; }

        /// <summary>
        /// parse pages into directory
        /// </summary>
        /// <param name="chapterInfo">chapterInfo for parsing</param>
        /// <param name="numberOfTry">number of try if while parsing something gone wrong it will try to parse this again this amount of time</param>
        void Parse(IChapterInfo chapterInfo, int numberOfTry);
    }
}
