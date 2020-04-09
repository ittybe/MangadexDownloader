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
    /// <summary>
    /// parse chapter's pages into Dir
    /// </summary>
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
        /// manga to parse
        /// </summary>
        public IMangaInfo MangaInfo { get; set; }
        
        /// <summary>
        /// chapters info (to get info use method ParseChaptersInfo)
        /// </summary>
        private List<IChapterInfo> ChaptersInfo { get; set; }
        
        /// <summary>
        /// Directory where save all pages
        /// </summary>
        public DirectoryInfo Dir { get; set; }

        /// <summary>
        /// parse chapter's pages into Dir
        /// </summary>
        /// <param name="match">match for parsing chapters</param>
        public void Parse(Predicate<ShortChapterInfo> match)
        {
            // parse info into List<IChapterInfo>
            ChaptersInfo = ParseChaptersInfo(match);
            
            // always true , because we already get chapters we needed
            ParseChapters(chapter => true);
        }

        /// <summary>
        /// parse all chapters in ChaptersInfo
        /// </summary>
        /// <param name="match">predicate for chapters</param>
        protected void ParseChapters(Predicate<IChapterInfo>match)
        {
            IChapterParser chapterParser = new ChapterParser(Dir);

            foreach (var chapter in ChaptersInfo)
            {
                chapterParser.Parse(chapter);
            }
        }
        
        /// <summary>
        /// parse chapters info
        /// </summary>
        /// <param name="match">predicate for chapters</param>
        public List<IChapterInfo> ParseChaptersInfo(Predicate<ShortChapterInfo> match)
        {
            IChapterJsonParser jsonParser = new ChapterJsonParser();
            List<IChapterInfo> chaptersInfo = new List<IChapterInfo>();
            foreach (var chapter in MangaInfo.ShortChaptersInfo) 
            {
                // check if it s match the request
                if (match(chapter)) 
                {
                    int id = Convert.ToInt32(chapter.Id);
                    IChapterInfo info = jsonParser.GetChapterInfo(id);
                    chaptersInfo.Add(info);
                }
            }
            return chaptersInfo;
        }

        /// <summary>
        /// paralell parse chapters info
        /// </summary>
        /// <param name="threadsNumber">how many threads is running at the same time</param>
        /// <param name="match">add ChapterInfo to list if ShortChapterInfo match</param>
        protected void ParseChaptersInfoMultiThreading(int threadsNumber, Predicate<ShortChapterInfo> match)
        {
            IChapterJsonParser jsonParser = new ChapterJsonParser();
            Thread[] threads = new Thread[threadsNumber];
            int threadsIndex = 0;

            foreach (var chapter in MangaInfo.ShortChaptersInfo)
            {
                // check if it s match the request
                if (match(chapter))
                {
                    // we use post increament so we just compare this values
                    if (threadsIndex == threadsNumber) 
                    {
                        threadsIndex = 0;
                        // start threads
                        foreach (var thread in threads)
                        {
                            thread.Start();
                        }
                        //// wait for thread's work complete
                        //foreach (var thread in threads)
                        //{
                        //    thread.Join();
                        //}
                    }
                    // parse json func 
                    ThreadStart parseFunc = () =>
                    {
                        // wait for calling thread wait for end of the parsing
                        

                        // parse data from site
                        var chapterLocal = chapter;
                        int id = Convert.ToInt32(chapterLocal.Id);
                        IChapterInfo info = jsonParser.GetChapterInfo(id);
                        ChaptersInfo.Add(info);
                    };
                    threads[threadsIndex++] = new Thread(parseFunc);
                }
            }
            // if something left in threads array and yeah there only matched chapters
            if (threadsIndex > 0)
            {
                // threadsIndex is size of the array with useful parseFuncs
                // start threads  
                for (int i = 0; i < threadsIndex; i++)
                {
                    threads[i].Start();
                }
                // wait for thread's work complete
                for (int i = 0; i < threadsIndex; i++)
                {
                    threads[i].Join();
                }
            }

        }
    }
}
