using MangadexDownloader.ContentInfo;
using MangadexDownloader.Parsing.JsonParsing;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MangadexDownloader.Parsing.ContentParsing
{
    public class MangaParser : IMangaParser
    {
        /// <summary>
        /// get directly mangainfo object
        /// </summary>
        /// <param name="mangaInfo">manga to parse</param>
        /// <param name="dir">directory for all pages</param>
        public MangaParser(IMangaInfo mangaInfo, DirectoryInfo dir) 
        {
            MangaInfo = mangaInfo;
            Dir = dir;
        }
        
        /// <summary>
        /// get manga info by id
        /// </summary>
        /// <param name="id">manga's id to parse</param>
        /// <param name="dir">directory for all pages</param>
        public MangaParser(int id, DirectoryInfo dir) 
        {
            IMangaJsonParser jsonParser = new MangaJsonParser();
            MangaInfo = jsonParser.GetMangaInfo(id);
            Dir = dir;
        }

        /// <summary>
        /// set only dir, but you still have to define MangaInfo!
        /// </summary>
        /// <param name="dir">directory for all pages</param>
        public MangaParser(DirectoryInfo dir)
        {
            Dir = dir;
        }

        /// <summary>
        /// manga to parse
        /// </summary>
        public IMangaInfo MangaInfo { get; set; }
        /// <summary>
        /// chapters info (to get info use method ParseChaptersInfo)
        /// </summary>
        public List<IChapterInfo> ChaptersInfo { get; } = new List<IChapterInfo>();
        /// <summary>
        /// Directory where save all pages
        /// </summary>
        public DirectoryInfo Dir { get; set; }
        /// <summary>
        /// parse all chapters in ChaptersInfo
        /// </summary>
        /// <param name="match">predicate for chapters</param>
        public void Parse(Predicate<IChapterInfo>match)
        {
            IChapterParser chapterParser = new ChapterParser(Dir);
            foreach (var chapter in ChaptersInfo)
            {
                chapterParser.Parse(chapter);
            }
        }
        /// <summary>
        /// parse chapters info into chaptersInfo list
        /// </summary>
        /// <param name="match">predicate for chapters</param>
        public void ParseChaptersInfo(Predicate<MangaInfo.ShortChapterInfo> match)
        {
            IChapterJsonParser jsonParser = new ChapterJsonParser();
            foreach (var chapter in MangaInfo.ShortChaptersInfo) 
            {
                // check if it s match the request
                if (match(chapter)) 
                {
                    int id = Convert.ToInt32(chapter.Id);
                    IChapterInfo info = jsonParser.GetChapterInfo(id);
                    ChaptersInfo.Add(info);
                }
            }
        }
        /// <summary>
        /// paralell parse chapters info
        /// </summary>
        /// <param name="processes">how many processes is running at the same time</param>
        /// <param name="match">add ChapterInfo to list if ShortChapterInfo match</param>
        public void ParseChaptersInfoThreadPool(int processes, Predicate<MangaInfo.ShortChapterInfo> match)
        {
            IChapterJsonParser jsonParser = new ChapterJsonParser();
            ThreadPool.SetMaxThreads(processes, processes);

            foreach (var chapter in MangaInfo.ShortChaptersInfo)
            {
                // check if it s match the request
                if (match(chapter))
                {
                    WaitCallback parse = state =>
                    {
                        var chapterLocal = chapter;
                        int id = Convert.ToInt32(chapterLocal.Id);
                        IChapterInfo info = jsonParser.GetChapterInfo(id);
                        ChaptersInfo.Add(info);
                    };
                    ThreadPool.QueueUserWorkItem(parse);
                }
            }
            
        }
    }
}
