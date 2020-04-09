using AngleSharp;
using System;
using System.Collections.Generic;
using System.Text;
using MangadexDownloader.ContentInfo;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;

namespace MangadexDownloader.Parsing.JsonParsing
{
    public class ChapterJsonParser : IChapterJsonParser
    {
        /// <summary>
        /// Get chapter json
        /// </summary>
        /// <param name="id">chapter's id</param>
        /// <exception cref="ApplicationException"></exception>
        /// <returns>json string about chapter</returns>
        public string GetJson(int id)
        {
            string urlChapter = $"https://mangadex.org/api/chapter/{id}";

            WebRequest request = WebRequest.Create(urlChapter);
            
            // get server response
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            
            // Read the content. (json object)
            string responseFromServer = reader.ReadToEnd();
            
            // close all streams
            reader.Close();
            dataStream.Close();
            response.Close();
            
            return responseFromServer;
        }
        /// <summary>
        /// convert json string to ChapterInfo instance
        /// </summary>
        /// <param name="json">chapter's json</param>
        /// <returns>ChapterInfo instance</returns>
        public ChapterInfo ConvertJson(string json)
        {
            ChapterInfo chapterInfo = JsonConvert.DeserializeObject<ChapterInfo>(json);
            if (chapterInfo.Volume.CompareTo(string.Empty) == 0)
                throw new ApplicationException("ChapterInfo json is invalid");
            return chapterInfo;
        }
        /// <summary>
        /// convert chapter's id into ChapterInfo instance
        /// </summary>
        /// <param name="id">chapter's id</param>
        /// <returns>ChapterInfo instance</returns>
        public ChapterInfo GetChapterInfo(int id) 
        {
            string json = GetJson(id);
            return ConvertJson(json);
        }
    }
}
