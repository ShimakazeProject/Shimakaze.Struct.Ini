using System;
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
    [DebuggerDisplay("{" + nameof(ToString) + "(),nq}")]
    public struct IniKeyValuePair
    {
        #region 用户属性
        public bool HasData => !string.IsNullOrEmpty(this.Key);

        public bool HasSummary => !string.IsNullOrWhiteSpace(this.Summary);

        public string Key { get; set; }

        public string Summary { get; set; }

        public IniValue Value { get; set; }
        #endregion

        #region 构造方法
        public IniKeyValuePair(string key, IniValue value, string summary)
        {
            this.Key = key;
            this.Value = value;
            this.Summary = summary;
        }
        #endregion

        #region 转换
        public static implicit operator IniKeyValuePair(KeyValuePair<string, string> @this) => CreateDataLine(@this.Key, @this.Value);
        public static implicit operator IniKeyValuePair(KeyValuePair<string, IniValue> @this) => CreateDataLine(@this.Key, @this.Value);
        public static implicit operator IniKeyValuePair(ValueTuple<string, string> @this) => CreateDataLine(@this.Item1, @this.Item2);
        public static implicit operator IniKeyValuePair(ValueTuple<string, IniValue> @this) => CreateDataLine(@this.Item1, @this.Item2);
        public static implicit operator IniKeyValuePair(ValueTuple<string, string, string> @this) => CreateFullLine(@this.Item1, @this.Item2, @this.Item3);
        public static implicit operator IniKeyValuePair(ValueTuple<string, IniValue, string> @this) => CreateFullLine(@this.Item1, @this.Item2, @this.Item3);

        public static implicit operator KeyValuePair<string, string>(IniKeyValuePair @this) => new KeyValuePair<string, string>(@this.Key, @this.Value);
        public static implicit operator KeyValuePair<string, IniValue>(IniKeyValuePair @this) => new KeyValuePair<string, IniValue>(@this.Key, @this.Value);
        public static implicit operator ValueTuple<string, string>(IniKeyValuePair @this) => (@this.Key, @this.Value);
        public static implicit operator ValueTuple<string, IniValue>(IniKeyValuePair @this) => (@this.Key, @this.Value);
        #endregion

        #region 用户方法
        /// <summary>
        /// Get KeyValuePair From String
        /// </summary>
        public static IniKeyValuePair Parse(string s)
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
        public static IniKeyValuePair CreateEmptyLine() => new IniKeyValuePair(string.Empty, string.Empty, string.Empty);
        public static IniKeyValuePair CreateSummaryLine(string summary) => new IniKeyValuePair(string.Empty, string.Empty, summary);
        public static IniKeyValuePair CreateDataLine(string key, IniValue value) => new IniKeyValuePair(key, value, string.Empty);
        public static IniKeyValuePair CreateFullLine(string key, IniValue value, string summary) => new IniKeyValuePair(key, value, summary);
        public async Task DepraseAsync(TextWriter writer)
        {
            if (!string.IsNullOrWhiteSpace(this.Key) && !string.IsNullOrWhiteSpace(this.Value))
                await writer.WriteAsync($"{this.Key}={this.Value}");

            if (HasSummary)
                await writer.WriteAsync($";{this.Summary}");
        }
        #endregion

        #region Object重载
        public override bool Equals(object obj)
            => obj is IniKeyValuePair pair
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
        #endregion
    }
}
