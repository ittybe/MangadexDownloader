using System;
using System.Collections.Generic;
using System.Text;

namespace MangadexDownloader.Parsing.ContentParsing
{
    /// <summary>
    /// on progress event args for delegate
    /// </summary>
    public class OnProgressParserEventArgs
    {
        public int NumberOfPages { get; set; }

        public int ParsedPages { get; set; }

        public string Message { get; set; }
    }
    /// <summary>
    /// on progress 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void OnProgressParserEventHandler(object sender, OnProgressParserEventArgs e);
}
