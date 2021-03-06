﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace IdentityProvider.Infrastructure.LatestAdditions
{
    public static class FileHelper
    {
        public static byte[] GetData(this Stream fs)
        {
            using (var ms = new MemoryStream((int)fs.Length))
            {
                fs.CopyTo(ms);
                return ms.GetBuffer();
            }
        }

        private static readonly Dictionary<string, string> MimeTypes = new Dictionary<string, string>
            {
                {".bmp", "image/bmp"},
                {".gif", "image/gif"},
                {".jpeg", "image/jpeg"},
                {".jpg", "image/jpeg"},
                {".png", "image/png"},
                {".tif", "image/tiff"},
                {".tiff", "image/tiff"},
                {".doc", "application/msword"},
                {".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
                {".pdf", "application/pdf"},
                {".ppt", "application/vnd.ms-powerpoint"},
                {".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".xls", "application/vnd.ms-excel"},
                {".csv", "text/csv"},
                {".xml", "text/xml"},
                {".txt", "text/plain"},
                {".zip", "application/zip"},
                {".7z", "application/x-7z-compressed"},
                {".ogg", "application/ogg"},
                {".mp3", "audio/mpeg"},
                {".wma", "audio/x-ms-wma"},
                {".wav", "audio/x-wav"},
                {".wmv", "audio/x-ms-wmv"},
                {".swf", "application/x-shockwave-flash"},
                {".avi", "video/avi"},
                {".mp4", "video/mp4"},
                {".mpeg", "video/mpeg"},
                {".mpg", "video/mpeg"},
                {".qt", "video/quicktime"}
            };

        public static string GetContentType(string fileName)
        {
            var extension = new FileInfo(fileName).Extension;
            return MimeTypes.ContainsKey(extension) ? MimeTypes[extension] : "application/octet-stream";
        }

        public static string GetFileExtension(string mimeType)
        {
            mimeType = mimeType.Split(';').First();
            return MimeTypes.Values.Contains(mimeType) ? MimeTypes.First(x => x.Value == mimeType).Key : null;
        }

        public static string ReplaceIllegalCharacters(string filename, char replaceChar = '_')
        {
            return Regex.Replace(filename, $"[{Path.GetInvalidFileNameChars().Join("|")}]", replaceChar.ToString());
        }
    }
}
