using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using MangadexDownloader.ContentCollecting;
using MangadexDownloader.ContentInfo;
using MangadexDownloader.Parsing.ContentParsing;

namespace MangadexDownloader.Downloading
{
    /// <summary>
    /// interface for MangaDownloader
    /// </summary>
    public interface IMangaDownloader
    {
        /// <summary>
        /// Temp Dir for pages
        /// </summary>
        DirectoryInfo Dir { get; set; }

        /// <summary>
        /// set temp dir for Dir, 
        /// Generate name for directory and then Creates directory in system Temp dir
        /// </summary>
        void SetTempDir();

        /// <summary>
        /// manga parser for parsing chapters
        /// </summary>
        // set is public because we can change manga's id really quick
        IMangaParser MangaParser { get; set; }

        /// <summary>
        /// parse chapter's pages into Dir
        /// </summary>
        /// <param name="match">match for parsing chapters</param>
        /// <param name="numberOfTry">number of try (if while page parsing something gone wrong it will try to parse this again this amount of time)</param>
        void Parse(Predicate<ShortChapterInfo> match, int numberOfTry);

        /// <summary>
        /// set manga to parse by manga's id
        /// </summary>
        /// <param name="id">id of manga</param>
        void SetMangaToParse(int id);

        /// <summary>
        /// collect all pages to pdf format
        /// </summary>
        /// <param name="outputPath">output file path</param>
        void CollectContentToPdf(string outputPath);
    }
}
