# MangadexDownloader
Lib for downloading manga from site https://mangadex.org/

nuget package link : https://www.nuget.org/packages/MangadexDownloader/

Example code

```
using System;
using System.Collections.Generic;
using MangadexDownloader.ContentInfo;
using MangadexDownloader.Parsing.JsonParsing;
using iText.Kernel.Geom;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using MangadexDownloader.Downloading;
using System.IO;

class Program
{
    public static void Main()
    {

        int numberOfTry = 10;    // number of try
        int mangaId = 528;       // manga id
        string LangCode = "gb";  // english code 


        var tempDir = new DirectoryInfo("Temp");

        MangaJsonParser parserJson = new MangaJsonParser();
        var mangaInfo = parserJson.GetMangaInfo(mangaId);
        Console.WriteLine(mangaInfo.Title);
        Console.WriteLine(mangaInfo.CoverUrl);
        Console.WriteLine(mangaInfo.ShortChaptersInfo.Count);
        // parse volumes numbers into list
        List<string> Volumes = new List<string>();
        foreach (var chapter in mangaInfo.ShortChaptersInfo)
        {
            if (!Volumes.Contains(chapter.Volume))
            {
                Volumes.Add(chapter.Volume);
                
            }
        }
        
        // parse manga s chapters into seperated pdf files
        foreach (var volume in Volumes)
        {
            Predicate<ShortChapterInfo> match = chapter =>
            {
                return chapter.LangCode.CompareTo(LangCode) == 0 && chapter.Volume.CompareTo(volume) == 0;
            };
            // set manga id and set auto generated dir in system temp directory
            MangaDownloader downloader = new MangaDownloader(mangaId);
            downloader.OnProgress += (sender, e) =>
            {
                Console.WriteLine($"{e.ParsedPages}/{e.NumberOfPages}, {e.Message}");
            };
            // parse chapters into temp directory
            downloader.Parse(match, numberOfTry);
            // collect pages into pdf files 
            downloader.CollectContentToPdf($@"\March Comes in like a lion Volume {volume}.pdf", PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN), 25, 60, PageSize.A4);
            // delete auto generated dir in system temp directory with pages
            downloader.Dir.Delete(true);
        }
    }
}

