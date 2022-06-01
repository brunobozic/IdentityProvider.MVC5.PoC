using System.IO;

namespace Module.CrossCutting.LatestAdditions
{
    public static class StringExtensions
    {
        public static Stream GetStreamFromString(this string str)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            writer.Write(str);
            writer.Flush();
            stream.Position = 0;

            return stream;
        }
    }
}