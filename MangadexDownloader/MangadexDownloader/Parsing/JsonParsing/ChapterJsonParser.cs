using System;
using System.Collections.Generic;
using System.Text;
using MangadexDownloader.ContentInfo;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace MangadexDownloader.Parsing.JsonParsing
{
    public class ChapterJsonParser : IChapterJsonParser
    {
        /// <summary>
        /// Get chapter json
        /// </summary>
        /// <param name="id">chapter's id</param>
        /// <returns>json string about chapter</returns>
        public string GetJson(int id)
        {
            string urlChapter = $"https://mangadex.org/api/chapter/{id}";

            WebRequest request = WebRequest.Create(urlChapter);
#if DEBUG
            Trace.WriteLine($"{DateTime.Now}: web request has created to this url \"{urlChapter}\"");
#endif

            // get server response
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
#if DEBUG
            Trace.WriteLine($"{DateTime.Now}: recieve response from \"{urlChapter}\"");
#endif

            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();

            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);

            string responseFromServer;
            try
            {
                // Read the content. (json object)
                responseFromServer = reader.ReadToEnd();
#if DEBUG
                Trace.WriteLine($"{DateTime.Now}: parsing json complete successfully, url \"{urlChapter}\"");
#endif
            }
            finally
            {
                // close all streams
                reader.Close();
                dataStream.Close();
                response.Close();
            }
            
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
