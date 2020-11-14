using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Shimakaze.Struct.Ini.Implements;

namespace Shimakaze.Struct.Ini.Utils
{
    public static class IniSectionUtils
    {
        internal static async Task<IIniSection> ParseAsync(TextReader reader, string line)
        {
            string summary = default;
            // 是否有注释
            if (line.Contains(";"))
            {
                // 有就设置分隔符索引
                int summarySeparatorIndex = line.IndexOf(';');

                // 有注释写注释
                summary = line.Substring(summarySeparatorIndex + 1).Trim();
                line = line.Substring(0, summarySeparatorIndex).Trim();
            }
            return await ParseAsync(line.TrimStart().Substring(1, line.IndexOf(']') - 1), reader, summary);
        }

        public static IIniSection CreateEmptySection() => new IniSection();

        public static IIniSection CreateSection(string name) => new IniSection(name);

        public static IIniSection CreateSection(string name, IEnumerable<IIniKeyValuePair> content) => new IniSection(name, content);

        public static IIniSection CreateSection(string name, IEnumerable<IIniKeyValuePair> content, string summary) => new IniSection(name, content, summary);

        public static async Task DepraseAsync(this IIniSection @this, TextWriter writer)
        {
            await writer.WriteAsync($"[{@this.Name}]");
            if (!string.IsNullOrEmpty(@this.Summary))
                await writer.WriteAsync($"; {@this.Summary}");
            await writer.WriteLineAsync();
            foreach (var item in @this)
            {
                await item.DepraseAsync(writer);
                await writer.WriteLineAsync();
            }
        }

        public static async Task<IIniSection> ParseAsync(TextReader reader)
        {
            string line = null;
            while (reader.Peek() < 0 || string.IsNullOrWhiteSpace(line))
                line = await reader.ReadLineAsync();

            return line.TrimStart()[0].Equals('[')
                ? await ParseAsync(reader, line)
                : throw new FormatException("这不是一个标准INI节");
        }
        public static async Task<IIniSection> ParseAsync(string name, TextReader reader, string summary = null)
        {
            var result = CreateSection(name);
            if (string.IsNullOrWhiteSpace(summary))
                result.Summary = summary;
            while (reader.Peek() > 0)
            {
                string line = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line))
                {
                    if (reader.Peek() == '[')
                        break;
                    continue;
                }

                result.Put(IniKeyValuePairUtils.Parse(line));

                if (reader.Peek() == '[')
                    break;
            }
            return result;
        }

    }
}
