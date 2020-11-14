using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Shimakaze.Struct.Ini.Implements;
using Shimakaze.Struct.Ini.Utils;

namespace Shimakaze.Struct.Ini.Utils
{
    public static class IniDocumentUtils
    {
        public static IIniDocument Create() => new IniDocument();

        public static Task DeparseAsync(this IIniDocument @this, Stream stream) => @this.DeparseAsync(new StreamWriter(stream));

        public static async Task DeparseAsync(this IIniDocument @this, TextWriter writer)
        {
            if (@this.Head.Count > 0)
            {
                foreach (IIniKeyValuePair item in @this.Head)
                {
                    await item.DepraseAsync(writer);
                    await writer.WriteLineAsync();
                }
                await writer.WriteLineAsync();
            }
            if (@this.Count > 0)
            {
                foreach (IIniSection item in @this)
                {
                    await item.DepraseAsync(writer);
                    await writer.WriteLineAsync();
                }
            }
            await writer.FlushAsync();
        }

        /// <summary>
        /// 从流中分析并返回 <see cref="IniDocument" />
        /// </summary>
        public static Task<IIniDocument> ParseAsync(Stream stream) => ParseAsync(new StreamReader(stream));

        public static async Task<IIniDocument> ParseAsync(TextReader reader)
        {
            IniDocument document = new IniDocument();
            bool readHeader = true;
            while (reader.Peek() > 0)
            {
                string line = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                if (line.TrimStart()[0].Equals('['))
                    readHeader = false;
                if (readHeader)
                {
                    document.Head.Put(IniKeyValuePairUtils.Parse(line));
                }
                else
                {
                    document.Put(await IniSectionUtils.ParseAsync(reader, line));
                }
            }
            return document;
        }
    }
}
