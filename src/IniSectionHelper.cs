using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Shimakaze.Struct.Ini.Implements;

namespace Shimakaze.Struct.Ini
{
    public static class IniSectionHelper
    {
        public static async Task DepraseAsync(this IIniSection @this, TextWriter writer)
        {
            await writer.WriteAsync($"[{@this.Name}]");
            if (!string.IsNullOrEmpty(@this.Summary))
                await writer.WriteAsync($"; {@this.Summary}");
            await writer.WriteLineAsync();
            foreach (var item in @this.Content)
            {
                await item.DepraseAsync(writer);
                await writer.WriteLineAsync();
            }
        }
        public static async Task DepraseAsync(this IReadOnlyIniSection @this, TextWriter writer)
        {
            await writer.WriteAsync($"[{@this.Name}]");
            if (!string.IsNullOrEmpty(@this.Summary))
                await writer.WriteAsync($"; {@this.Summary}");
            await writer.WriteLineAsync();
            foreach (var item in @this.Content)
            {
                await item.DepraseAsync(writer);
                await writer.WriteLineAsync();
            }
        }


        public static async Task<IIniSection> ParseAsync(TextReader reader)
        {
            IIniSection result = null;
            while (reader.Peek() > 0)
            {
                var line = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line)) continue;

                if (result is null)
                {
                    if (!line.TrimStart()[0].Equals('['))
                        throw new FormatException("这不是一个标准INI节");

                    int? summarySeparatorIndex = null;
                    if (line.Contains(";")) summarySeparatorIndex = line.IndexOf(';');

                    result = new IniSection { Name = line.TrimStart().Substring(1, line.IndexOf(']') - 1) };
                    if (summarySeparatorIndex.HasValue) result.Summary = line.Substring(summarySeparatorIndex.Value);
                    continue;
                }
                result.Add(IniKeyValuePairHelper.Parse(line));

                if (reader.Peek() == '[') break;
            }
            return result;
        }


        public static IIniSection CreateEmptySection() => new IniSection();

        public static IIniSection CreateSection(string name) => new IniSection(name);

        public static IIniSection CreateSection(string name, IEnumerable<IIniKeyValuePair> content) => new IniSection(name, content);

        public static IIniSection CreateSection(string name, IEnumerable<IIniKeyValuePair> content, string summary) => new IniSection(name, content, summary);
    }
}
