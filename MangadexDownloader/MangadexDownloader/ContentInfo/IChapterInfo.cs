using System;
using System.Collections.Generic;
using System.Text;

namespace MangadexDownloader.ContentInfo
{
    /// <summary>
    /// Interface for class ChapterInfo
    /// </summary>
    public interface IChapterInfo
    {
        /// <summary>
        /// chapter id
        /// </summary>
        int Id { get; set; }
        
        /// <summary>
        /// manga id
        /// </summary>
        int MangaId { get; }
        
        /// <summary>
        /// volume number
        /// </summary>
        string Volume { get; }
        
        /// <summary>
        /// chapter number
        /// </summary>
        string Chapter { get; }

        /// <summary>
        /// page count for this chapter
        /// </summary>
        int PageCount { get; }

        /// <summary>
        /// language code
        /// </summary>
        string LangCode { get; }
    }
}
