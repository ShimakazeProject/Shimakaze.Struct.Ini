using System.Collections.Generic;
using System.Text;

namespace Shimakaze.Struct.Ini
{
    /// <summary>
    /// 表示一个INI键值对
    /// </summary>
    public struct IniKeyValuePair
    {
        /// <summary>
        /// 键
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public IniValue Value { get; set; }
        /// <summary>
        /// 注释
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// 是否有可用数据
        /// </summary>
        public bool HasData => !string.IsNullOrEmpty(Key);
        /// <summary>
        /// 是否存在注释
        /// </summary>
        public bool HasSummary => !string.IsNullOrEmpty(Summary);


        /// <summary>
        /// 从字符串生成键值对
        /// </summary>
        public static IniKeyValuePair Parse(string s)
        {
            var summaryTuple = getSummary(s);
            var dataTuple = getValue(summaryTuple.data);

            return new IniKeyValuePair
            {
                Key = dataTuple.key,
                Value = dataTuple.value,
                Summary = summaryTuple.summary
            };

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
        /// <summary>
        /// 转换为常见的键值对形式
        /// </summary>
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
