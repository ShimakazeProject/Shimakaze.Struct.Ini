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
    public sealed class IniKeyValuePair : IEquatable<IniKeyValuePair>
    {
        #region Public Properties

        public bool HasData => !string.IsNullOrEmpty(Key);

        public bool HasSummary => !string.IsNullOrWhiteSpace(Summary);

        public string Key { get; set; }

        public string Summary { get; set; }

        public IniValue Value { get; set; }

        #endregion Public Properties

        #region Public Constructors

        public IniKeyValuePair()
        {
        }

        public IniKeyValuePair(string key) : this() => Key = key ?? throw new ArgumentNullException(nameof(key));

        public IniKeyValuePair(string key, IniValue value) : this(key) => Value = value;

        public IniKeyValuePair(string key, IniValue value, string summary) : this(key, value) => Summary = summary ?? throw new ArgumentNullException(nameof(summary));

        #endregion Public Constructors

        #region Public Methods

        public static IniKeyValuePair CreateDataLine(string key, IniValue value) => new IniKeyValuePair(key, value, string.Empty);

        public static IniKeyValuePair CreateEmptyLine() => new IniKeyValuePair(string.Empty, string.Empty, string.Empty);

        public static IniKeyValuePair CreateFullLine(string key, IniValue value, string summary) => new IniKeyValuePair(key, value, summary);

        public static IniKeyValuePair CreateSummaryLine(string summary) => new IniKeyValuePair(string.Empty, string.Empty, summary);

        public static implicit operator IniKeyValuePair(KeyValuePair<string, string> @this) => CreateDataLine(@this.Key, @this.Value);

        public static implicit operator IniKeyValuePair(KeyValuePair<string, IniValue> @this) => CreateDataLine(@this.Key, @this.Value);

        public static implicit operator IniKeyValuePair(ValueTuple<string, string> @this) => CreateDataLine(@this.Item1, @this.Item2);

        public static implicit operator IniKeyValuePair(ValueTuple<string, IniValue> @this) => CreateDataLine(@this.Item1, @this.Item2);

        public static implicit operator IniKeyValuePair(ValueTuple<string, string, string> @this) => CreateFullLine(@this.Item1, @this.Item2, @this.Item3);

        public static implicit operator IniKeyValuePair(ValueTuple<string, IniValue, string> @this) => CreateFullLine(@this.Item1, @this.Item2, @this.Item3);

        public static implicit operator KeyValuePair<string, IniValue>(IniKeyValuePair @this) => new KeyValuePair<string, IniValue>(@this.Key, @this.Value);

        public static implicit operator KeyValuePair<string, string>(IniKeyValuePair @this) => new KeyValuePair<string, string>(@this.Key, @this.Value);

        public static implicit operator ValueTuple<string, IniValue>(IniKeyValuePair @this) => (@this.Key, @this.Value);

        public static implicit operator ValueTuple<string, string>(IniKeyValuePair @this) => (@this.Key, @this.Value);

        public static bool operator !=(IniKeyValuePair left, IniKeyValuePair right) => !(left == right);

        public static bool operator ==(IniKeyValuePair left, IniKeyValuePair right) => EqualityComparer<IniKeyValuePair>.Default.Equals(left, right);

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

        public Task DepraseAsync(Stream stream) => DepraseAsync(new StreamWriter(stream));

        public async Task DepraseAsync(TextWriter writer)
        {
            if (!string.IsNullOrWhiteSpace(Key) && !string.IsNullOrWhiteSpace(Value))
                await writer.WriteAsync($"{Key}={Value}");

            if (HasSummary)
                await writer.WriteAsync($"; {Summary}");
        }

        public override bool Equals(object obj) => (obj is IniKeyValuePair pair) && Equals(pair);

        public bool Equals(IniKeyValuePair other) => other != null && Key == other.Key && Summary == other.Summary && EqualityComparer<IniValue>.Default.Equals(Value, other.Value);

        public override int GetHashCode()
        {
            var hashCode = -1547869727;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Key);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Summary);
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(Key) && !string.IsNullOrWhiteSpace(Value))
                _ = sb.Append($"{Key}={Value}");
            if (HasSummary)
                sb.Append($"; {Summary}");
            return sb.ToString();
        }

        #endregion Public Methods
    }
}
