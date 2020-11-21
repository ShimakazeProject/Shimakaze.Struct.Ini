using System.IO;
using System.Threading.Tasks;

using Shimakaze.Struct.Ini.Implements;

namespace Shimakaze.Struct.Ini
{
    public static class IniKeyValuePairHelper
    {
        public static Task DepraseAsync(this IIniKeyValuePair @this, Stream stream) => @this.DepraseAsync(new StreamWriter(stream));

        public static async Task DepraseAsync(this IIniKeyValuePair @this, TextWriter writer)
        {
            if (!string.IsNullOrWhiteSpace(@this.Key) && !string.IsNullOrWhiteSpace(@this.Value))
                await writer.WriteAsync($"{@this.Key}={@this.Value}");

            if (@this.HasSummary)
                await writer.WriteAsync($"; {@this.Summary}");
        }
        public static Task DepraseAsync(this IReadOnlyIniKeyValuePair @this, Stream stream) => @this.DepraseAsync(new StreamWriter(stream));

        public static async Task DepraseAsync(this IReadOnlyIniKeyValuePair @this, TextWriter writer)
        {
            if (!string.IsNullOrWhiteSpace(@this.Key) && !string.IsNullOrWhiteSpace(@this.Value))
                await writer.WriteAsync($"{@this.Key}={@this.Value}");

            if (@this.HasSummary)
                await writer.WriteAsync($"; {@this.Summary}");
        }

        public static IIniKeyValuePair CreateDataLine(string key, IniValue value) => new IniKeyValuePair(key, value, string.Empty);

        public static IniKeyValuePair CreateEmptyLine() => new IniKeyValuePair(string.Empty, string.Empty, string.Empty);

        public static IIniKeyValuePair CreateFullLine(string key, IniValue value, string summary) => new IniKeyValuePair(key, value, summary);

        public static IIniKeyValuePair CreateSummaryLine(string summary) => new IniKeyValuePair(string.Empty, string.Empty, summary);

        public static IReadOnlyIniKeyValuePair CreateReadOnlyDataLine(string key, IniValue value) => new IniKeyValuePair(key, value, string.Empty);

        public static IReadOnlyIniKeyValuePair CreateReadOnlyFullLine(string key, IniValue value, string summary) => new IniKeyValuePair(key, value, summary);

        public static IReadOnlyIniKeyValuePair CreateReadOnlySummaryLine(string summary) => new IniKeyValuePair(string.Empty, string.Empty, summary);

        /// <summary>
        /// Get KeyValuePair From String
        /// </summary>
        public static IIniKeyValuePair Parse(string s)
        {
            var result = CreateEmptyLine();
            // 是否有注释
            if (s.Contains(";"))
            {
                // 有就设置分隔符索引
                var summarySeparatorIndex = s.IndexOf(';');

                // 有注释写注释
                result.Summary = s.Substring(summarySeparatorIndex + 1).Trim();
                s = s.Substring(0, summarySeparatorIndex).Trim();
            }
            // 有没有键值对
            if (s.Contains("="))
            {
                // 获取键值对等号位置
                var keyValueSeparatorIndex = s.IndexOf('=');

                // 有数据写数据
                result.Key = s.Substring(0, keyValueSeparatorIndex).Trim();
                result.Value = s.Substring(keyValueSeparatorIndex + 1).Trim();
            }
            return result;
        }
        /// <summary>
        /// Get KeyValuePair From String
        /// </summary>
        public static IReadOnlyIniKeyValuePair ParseReadOnly(string s)
        {
            var result = CreateEmptyLine();
            // 是否有注释
            if (s.Contains(";"))
            {
                // 有就设置分隔符索引
                var summarySeparatorIndex = s.IndexOf(';');

                // 有注释写注释
                result.Summary = s.Substring(summarySeparatorIndex + 1).Trim();
                s = s.Substring(0, summarySeparatorIndex).Trim();
            }
            // 有没有键值对
            if (s.Contains("="))
            {
                // 获取键值对等号位置
                var keyValueSeparatorIndex = s.IndexOf('=');

                // 有数据写数据
                result.Key = s.Substring(0, keyValueSeparatorIndex).Trim();
                result.Value = s.Substring(keyValueSeparatorIndex + 1).Trim();
            }
            return result;
        }
    }
}
