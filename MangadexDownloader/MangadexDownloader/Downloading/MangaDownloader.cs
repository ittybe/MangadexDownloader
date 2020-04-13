using iText.Kernel.Font;
using MangadexDownloader.ContentCollecting;
using MangadexDownloader.ContentInfo;
using MangadexDownloader.Parsing.ContentParsing;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MangadexDownloader.Downloading
{
    /// <summary>
    /// Download manga and convert it to pdf
    /// </summary>
    public class MangaDownloader : IMangaDownloader
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">manga's id</param>
        /// <param name="dir">temp directory</param>
        public MangaDownloader(int id, DirectoryInfo dir) 
        {
            Dir = dir;
            SetMangaToParse(id);
        }
        /// <summary>
        /// if you already have parser
        /// </summary>
        /// <param name="parser">parser </param>
        public MangaDownloader(IMangaParser parser) 
        {
            MangaParser = parser;
            Dir = parser.Dir;
        }
        /// <summary>
        /// create random temp directory in system temp dir and set manga by id
        /// </summary>
        /// <param name="id">manga's id</param>
        public MangaDownloader(int id) 
        {
            // set temp dir
            SetTempDir();
            // only then we set manga to parse!
            SetMangaToParse(id);
        }

        /// <summary>
        /// Temp Dir for pages
        /// </summary>
        public DirectoryInfo Dir { get; set; }

        /// <summary>
        /// manga parser for parsing chapters
        /// </summary>
        public IMangaParser MangaParser { get; set; }

        /// <summary>
        /// collect all pages to pdf format
        /// </summary>
        /// <param name="outputPath">output file path</param>
        public void CollectContentToPdf(string outputPath)
        {
            ContentCollector contentCollector = new ContentCollector(Dir.FullName);
            contentCollector.CollectContentToPdf(outputPath);
        }
        /// <summary>
        /// collect all pages to pdf format, also make table of content in pdf file
        /// </summary>
        /// <param name="outputPath">output file path</param>
        /// <param name="font">font for table of content</param>
        /// <param name="fontSizeInfo">size of information text</param>
        /// <param name="fontSizeHeader">size of header text</param>
        /// <param name="pageSize">page size of information and table of content</param>
        public void CollectContentToPdf(string outputPath, PdfFont font, float fontSizeInfo, float fontSizeHeader, iText.Kernel.Geom.PageSize pageSize)
        {
            ContentCollector contentCollector = new ContentCollector(Dir.FullName);
            contentCollector.CollectContentToPdf(outputPath, font, fontSizeInfo, fontSizeHeader, pageSize);
        }
        /// <summary>
        /// parse chapter's pages into Dir
        /// </summary>
        /// <param name="match">match for parsing chapters</param>
        /// <param name="numberOfTry">number of try (if while page parsing something gone wrong it will try to parse this again this amount of time)</param>
        public void Parse(Predicate<ShortChapterInfo> match, int numberOfTry)
        {
            MangaParser.Dir = Dir;
            MangaParser.Parse(match, numberOfTry);
        }

        /// <summary>
        /// set manga to parse by manga's id
        /// </summary>
        /// <param name="id">id of manga</param>
        public void SetMangaToParse(int id)
        {
            MangaParser = new MangaParser(id, Dir);
        }

        /// <summary>
        /// set temp dir for Dir, 
        /// Generate name for directory and then Creates directory in system Temp dir
        /// </summary>
        public void SetTempDir()
        {
            //Path.DirectorySeparatorChar;
            
            // get temp system dir 
            DirectoryInfo tempSysDir =new DirectoryInfo(Path.GetTempPath());
            
            // check if tempDir name is not Exists, if exists random another name
            string tempDirName;
            while (true) 
            {
                // full path to temp directory
                tempDirName = $"{tempSysDir.FullName}{Path.DirectorySeparatorChar}{RandomString(20, false)}";

                DirectoryInfo tempDir = new DirectoryInfo(tempDirName);
                if (!tempDir.Exists)
                    break;
            }

            // create unique name directory for our pages
            Dir = new DirectoryInfo(tempDirName);
            Dir.Create();
        }


        // Generate a random string with a given size  
        private string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        
    }
}
