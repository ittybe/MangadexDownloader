using System;
using System.Collections.Generic;
using System.Text;
using MangadexDownloader.ContentInfo;
namespace MangadexDownloader.Parsing
{
    public interface IChapterJsonParser : IJsonParser
    {
        /// <summary>
        /// convert json string to ChapterInfo instance
        /// </summary>
        /// <param name="json"></param>
        /// <returns>ChapterInfo instance</returns>
        ChapterInfo ConvertJson(string json);

        /// <summary>
        /// convert chapter's id into ChapterInfo instance
        /// </summary>
        /// <param name="id">chapter's id</param>
        /// <returns>ChapterInfo instance</returns>
        ChapterInfo GetChapterInfo(int id);
    }
}
