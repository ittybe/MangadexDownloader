using Newtonsoft.Json.Linq;
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
        int Id { get; }
        
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
        /// JArray with all pages in it
        /// </summary>
        JArray PageArray { get; }

        /// <summary>
        /// language code
        /// </summary>
        string LangCode { get; }
    }
}
