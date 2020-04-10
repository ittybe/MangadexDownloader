using AngleSharp;
using System;
using System.Collections.Generic;
using System.Text;
using MangadexDownloader.ContentInfo;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace MangadexDownloader.Parsing.JsonParsing
{
    public class MangaJsonParser : IMangaJsonParser
    {
        /// <summary>
        /// Get manga json
        /// </summary>
        /// <param name="id">manga's id</param>
        /// <returns>json string about manga</returns>
        public string GetJson(int id)
        {
            string urlManga = $"https://mangadex.org/api/manga/{id}";

            WebRequest request = WebRequest.Create(urlManga);
            Trace.WriteLine($"{DateTime.Now}: web request has created to this url \"{urlManga}\"");

            // get server response
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Trace.WriteLine($"{DateTime.Now}: recieve response from \"{urlManga}\"");

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

            Trace.WriteLine($"{DateTime.Now}: parsing json complete successfully, url \"{urlManga}\"");

            return responseFromServer;
        }
        /// <summary>
        /// convert json string into mangaInfo object
        /// </summary>
        /// <param name="json">manga json</param>
        /// <returns>mangainfo</returns>
        public MangaInfo ConvertJson(string json)
        {
            MangaInfo mangaInfo = JsonConvert.DeserializeObject<MangaInfo>(json);
            if (mangaInfo.ShortChaptersInfo.Count < 1)
                throw new ApplicationException($"MangaInfo json is invalid");
            return mangaInfo;
        }
        /// <summary>
        /// get mangaInfo by id
        /// </summary>
        /// <param name="id">manga's id</param>
        /// <returns>mangaInfo</returns>
        public MangaInfo GetMangaInfo(int id)
        {
            string json = GetJson(id);
            return ConvertJson(json);
        }
    }
}