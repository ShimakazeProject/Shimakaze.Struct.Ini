using System.Collections.Generic;
using System.Text;

namespace Shimakaze.Struct.Ini
{
    /// <summary>
    /// an IniKeyValuePair
    /// </summary>
    public struct IniKeyValuePair
    {
        internal string key;
        internal IniValue value;
        internal string summary;

        public IniKeyValuePair(string key, string value = null, string summary = null)
        {
            this.key = key;
            this.value = value;
            this.summary = summary;
        }

        public string Key
        {
            get => this.key;
            set => this.key = value;
        }
        public IniValue Value
        {
            get => this.value;
            set => this.value = value;
        }
        public string Summary
        {
            get => this.summary;
            set => this.summary = value;
        }
        public bool HasData => !string.IsNullOrEmpty(Key);
        public bool HasSummary => !string.IsNullOrEmpty(Summary);

        public static implicit operator KeyValuePair<string, string>(IniKeyValuePair ikv) => new KeyValuePair<string, string>(ikv.key, ikv.value);
        public static implicit operator IniKeyValuePair(KeyValuePair<string, string> kv) => new IniKeyValuePair(kv.Key, kv.Value);

        /// <summary>
        /// Get KeyValuePair From String
        /// </summary>
        public static IniKeyValuePair Parse(string s)
        {
            var summaryTuple = getSummary(s);
            var dataTuple = getValue(summaryTuple.data);

            return new IniKeyValuePair(dataTuple.key, dataTuple.value, summaryTuple.summary);

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

        public override int GetHashCode()
        {
            int hashCode = 1424931193;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Key);
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Summary);
            return hashCode;
        }

        public override bool Equals(object obj) => obj is IniKeyValuePair pair &&
                   Key == pair.Key &&
                   EqualityComparer<IniValue>.Default.Equals(Value, pair.Value) &&
                   Summary == pair.Summary;

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (string.IsNullOrWhiteSpace(Key) && string.IsNullOrWhiteSpace(Value))
            {
                sb.Append(Key);
                sb.Append('=');
                sb.Append(Key);
            }
            if (string.IsNullOrWhiteSpace(Summary))
            {
                sb.Append(';');
                sb.Append(Summary);
            }
            return sb.ToString();
        }
    }
}
