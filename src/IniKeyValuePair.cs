using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Shimakaze.Struct.Ini
{
    /// <summary>
    /// an IniKeyValuePair
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + "(),nq}")]
    public struct IniKeyValuePair
    {
        private string DebuggerDisplay => this.ToString();

        public bool HasData => !string.IsNullOrEmpty(this.Key);

        public bool HasSummary => !string.IsNullOrWhiteSpace(this.Summary);

        public string Key { get; set; }

        public string Summary { get; set; }

        public IniValue Value { get; set; }

        public IniKeyValuePair(string key, string value = null, string summary = null)
        {
            this.Key = key;
            this.Value = value;
            this.Summary = summary;
        }
        public static implicit operator IniKeyValuePair(KeyValuePair<string, string> kv) => new IniKeyValuePair(kv.Key, kv.Value);

        public static implicit operator KeyValuePair<string, string>(IniKeyValuePair ikv) => new KeyValuePair<string, string>(ikv.Key, ikv.Value);
        /// <summary>
        /// Get KeyValuePair From String
        /// </summary>
        public static IniKeyValuePair Parse(string s)
        {
            var (data, summary) = getSummary(s);
            var (key, value) = getValue(data);

            return new IniKeyValuePair(key, value, summary);

            (string data, string summary) getSummary(string str)
            {
                int? summarySeparatorIndex = null;
                // 是否有注释
                if (str.Contains(";"))
                    // 有就设置分隔符索引
                    summarySeparatorIndex = str.IndexOf(';');
                // 有注释写注释
                if (summarySeparatorIndex.HasValue)
                    return (str.Substring(0, summarySeparatorIndex.Value).Trim(), str.Substring(summarySeparatorIndex.Value + 1).Trim());
                else return (str, null);
            }

            (string key, string value) getValue(string str)
            {
                int? keyValueSeparatorIndex = null;
                // 有没有键值对
                if (str.Contains("="))
                    // 获取键值对等号位置
                    keyValueSeparatorIndex = str.IndexOf('=');

                // 有数据写数据
                if (keyValueSeparatorIndex.HasValue)
                    return (str.Substring(0, keyValueSeparatorIndex.Value).Trim(), str.Substring(keyValueSeparatorIndex.Value + 1).Trim());
                else return (null, null);
            }
        }

        public async Task DepraseAsync(TextWriter writer)
        {
            if (!string.IsNullOrWhiteSpace(this.Key) && !string.IsNullOrWhiteSpace(this.Value))
                await writer.WriteAsync($"{this.Key}={this.Value}");

            if (HasSummary)
                await writer.WriteAsync($";{this.Summary}");

        }

        public override bool Equals(object obj) => obj is IniKeyValuePair pair
                                                   && this.Key == pair.Key
                                                   && this.Value == pair.Value
                                                   && this.Summary == pair.Summary;

        public override int GetHashCode()
        {
            int hashCode = 1424931193;
            hashCode = hashCode * -1521134295 + this.Key.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Value.GetHashCode();
            hashCode = (hashCode * -1521134295) + this.Summary.GetHashCode();
            return hashCode;
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(this.Key) && !string.IsNullOrWhiteSpace(this.Value))
                sb.Append($"{this.Key}={this.Value}");
            if (this.HasSummary)
                sb.Append($";{this.Summary}");
            return sb.ToString();
        }
    }
}
