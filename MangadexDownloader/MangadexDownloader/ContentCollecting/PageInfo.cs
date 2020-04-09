using MangadexDownloader.Parsing.ContentParsing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MangadexDownloader.ContentCollecting
{
    /// <summary>
    /// page info for local images in folder
    /// </summary>
    public struct PageInfo : IComparable<PageInfo>
    {
        /// <summary>
        /// set all properties in constructor
        /// </summary>
        /// <param name="volumeNumber"></param>
        /// <param name="chapterNumber"></param>
        /// <param name="pageNumber"></param>
        /// <param name="extension"></param>
        public PageInfo(string volumeNumber, string chapterNumber, string pageNumber, string extension) 
        {
            VolumeNumber = volumeNumber;
            ChapterNumber = chapterNumber;
            PageNumber = pageNumber;
            Extension = extension;
        }
        /// <summary>
        /// volume number
        /// </summary>
        public string VolumeNumber { get; set; }
        /// <summary>
        /// chapter number
        /// </summary>
        public string ChapterNumber { get; set; }
        /// <summary>
        /// page number
        /// </summary>
        public string PageNumber { get; set; }
        /// <summary>
        /// for full name
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// return VolumeNumber in double format
        /// </summary>
        public double VolumeNumberDouble 
        {
            get 
            {
                return StringToNumber(VolumeNumber);
            }
        }
        /// <summary>
        /// return ChapterNumber in double format
        /// </summary>
        public double ChapterNumberDouble 
        {
            get { return StringToNumber(ChapterNumber); }
        }
        /// <summary>
        /// return PageNumber in double format
        /// </summary>
        public double PageNumberDouble
        {
            get { return StringToNumber(PageNumber); }
        }

        /// <summary>
        /// special method for volume chapter page convert into number (we have exception where chapter have point number)
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        static private double StringToNumber(string strNumber) 
        {
            if (strNumber != null)
            {
                strNumber.Replace('.', ',');
                double result;
                if (double.TryParse(strNumber, out result))
                    return result;
                else
                    throw new ArgumentException($"strNumber is \"{strNumber}\", can't parse it to double");
            }
            else 
            {
                throw new ArgumentNullException("strNumber is null, try to fill all props in PageInfo");
            }
        }
        /// <summary>
        /// call if all properties is filled with some data, otherwise it will raise exception
        /// </summary>
        public string Fullname 
        {
            get 
            {
                // add splitter char 
                return $"{VolumeNumber}_{ChapterNumber}_{PageNumber}{Extension}";
            }
        }
        /// <summary>
        /// to page info
        /// </summary>
        /// <param name="filename">file name</param>
        public static explicit operator PageInfo(string filename)
        {
            return ToPageInfo(filename);
        }
        /// <summary>
        /// convert filename to pageInfo
        /// </summary>
        /// <param name="filename">file name</param>
        /// <returns>pageInfo</returns>
        public static PageInfo ToPageInfo(string filename) 
        {
            if (!Regex.IsMatch(filename, ChapterParser.Pattern)) 
            {
                throw new ArgumentException($"Filename ({filename}) doesn't match ChapterParser.Pattern ({ChapterParser.Pattern}), can't convert name that doens't match this pattern");
            }

            // replace last extension point to '_' (splitting char)
            
            int pointExtension = filename.LastIndexOf('.');
            char[] filenameChar = filename.ToCharArray();
            filenameChar[pointExtension] = '_';
            filename = new string(filenameChar);

            // split filename to parts
            PageInfo pageInfo = new PageInfo();
            
            string[] parts = filename.Split('_');
            
            pageInfo.VolumeNumber = parts[0];
            pageInfo.ChapterNumber = parts[1];
            pageInfo.PageNumber = parts[2];
            // we splitted string by point, we dont have point so we add one
            pageInfo.Extension = '.' + parts[3];

            return pageInfo;
        }
        /// <summary>
        /// compare pages by volume chapter and page number
        /// </summary>
        /// <param name="other">other page</param>
        /// <returns>result</returns>
        public int CompareTo(PageInfo other)
        {
            int PageNumberCompare = PageNumberDouble.CompareTo(other.PageNumberDouble);
            int ChapterNumberCompare = ChapterNumberDouble.CompareTo(other.ChapterNumberDouble);
            int VolumeNumberCompare = VolumeNumberDouble.CompareTo(other.VolumeNumberDouble);

            // volume different
            if (VolumeNumberCompare != 0)
            {
                return VolumeNumberCompare;
            }
            // chapter different
            else if (ChapterNumberCompare != 0)
            {
                return ChapterNumberCompare;
            }
            // page different
            else if (PageNumberCompare != 0)
            {
                return PageNumberCompare;
            }
            // this is mean that pages info is similar
            else
                return 0;
        }
    }
}
