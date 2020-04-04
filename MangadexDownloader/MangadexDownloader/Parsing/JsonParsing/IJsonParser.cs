using System;
using System.Collections.Generic;
using System.Text;

namespace MangadexDownloader.Parsing.JsonParsing
{
    /// <summary>
    /// interface for json parsing
    /// </summary>
    public interface IJsonParser
    {
        /// <summary>
        /// get json method
        /// </summary>
        /// <param name="id">id for manga or chapter</param>
        /// <returns>returns json string</returns>
        string GetJson(int id);
    }
}
