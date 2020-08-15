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
            int? summarySeparatorIndex = null;
            int? keyValueSeparatorIndex = null;
            IniKeyValuePair keyValuePair = new IniKeyValuePair();
            // 是否有注释
            if (s.Contains(";"))
                // 有就设置分隔符索引
                summarySeparatorIndex = s.IndexOf(';');

            // 有没有键值对
            if (s.Contains("="))
            {
                // 获取键值对等号位置
                var tmp = s.IndexOf('=');
                // 检查等号是否是注释内容
                if (tmp < summarySeparatorIndex)
                    // 不是注释内容 设置等号索引位置
                    keyValueSeparatorIndex = tmp;
            }
            // 有注释写注释
            if (summarySeparatorIndex.HasValue) keyValuePair.Summary = s.Substring(summarySeparatorIndex.Value + 1).Trim();
            // 有数据写数据
            if (keyValueSeparatorIndex.HasValue)
            {
                keyValuePair.Key = s.Substring(0, keyValueSeparatorIndex.Value).Trim();
                keyValuePair.Value = s.Substring(
                    keyValueSeparatorIndex.Value + 1,
                    summarySeparatorIndex.HasValue
                    // 有注释
                    ? summarySeparatorIndex.Value - keyValueSeparatorIndex.Value - 1
                    // 没注释
                    : s.Length - keyValueSeparatorIndex.Value - 1)
                    .Trim();
            }
            return keyValuePair;
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
