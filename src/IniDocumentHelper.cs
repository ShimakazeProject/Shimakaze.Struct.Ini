using System.IO;
using System.Threading.Tasks;

using Shimakaze.Struct.Ini.Implements;

namespace Shimakaze.Struct.Ini
{
    public static class IniDocumentHelper
    {
        /// <summary>
        /// 从流中分析并返回 <see cref="IniDocument" />
        /// </summary>
        public static Task<IIniDocument> ParseAsync(Stream stream) => ParseAsync(new StreamReader(stream));

        public static async Task<IIniDocument> ParseAsync(TextReader reader)
        {
            var document = new IniDocument();
            var readHeader = true;
            while (reader.Peek() > 0)
            {
                var line = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line)) continue;
                if (line.TrimStart()[0].Equals('['))
                {
                    readHeader = false;
                }
                if (readHeader)
                    document.Add(IniKeyValuePairHelper.Parse(line));
                else
                    document.Add(await IniSectionHelper.ParseAsync(reader));
            }
            return document;
        }


        public static Task DeparseAsync(this IIniDocument @this, Stream stream) => @this.DeparseAsync(new StreamWriter(stream));

        public static async Task DeparseAsync(this IIniDocument @this, TextWriter writer)
        {
            if ((@this.NoSectionContent?.Count ?? 0) > 0)
            {
                foreach (var item in @this.NoSectionContent)
                {
                    await item.DepraseAsync(writer);
                    await writer.WriteLineAsync();
                }
            }
            if ((@this.Sections?.Count ?? 0) > 0)
            {
                foreach (var item in @this.Sections)
                {
                    await item.DepraseAsync(writer);
                    await writer.WriteLineAsync();
                }
            }
        }

        public static IIniDocument Create() => new IniDocument();
    }
}
