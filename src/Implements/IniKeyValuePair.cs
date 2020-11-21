using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Shimakaze.Struct.Ini.Implements
{
    /// <summary>
    /// an IniKeyValuePair
    /// </summary>
    [DebuggerDisplay("{" + nameof(ToString) + "(),nq}")]
    public sealed class IniKeyValuePair : IIniKeyValuePair, IReadOnlyIniKeyValuePair
    {
        #region Public Properties

        public bool HasData => !string.IsNullOrEmpty(Key);

        public bool HasSummary => !string.IsNullOrWhiteSpace(Summary);

        public string Key { get; set; }

        public string Summary { get; set; }

        public IniValue Value { get; set; }

        #endregion Public Properties

        internal IniKeyValuePair()
        {
        }

        internal IniKeyValuePair(string key) : this() => Key = key ?? throw new ArgumentNullException(nameof(key));

        internal IniKeyValuePair(string key, IniValue value) : this(key) => Value = value;

        internal IniKeyValuePair(string key, IniValue value, string summary) : this(key, value) => Summary = summary;

        #region Public Methods


        public override bool Equals(object obj) => Equals(obj as IIniKeyValuePair);

        public bool Equals(IIniKeyValuePair other) => other != null && Key == other.Key && Summary == other.Summary && EqualityComparer<IniValue>.Default.Equals(Value, other.Value);
        public bool Equals(IReadOnlyIniKeyValuePair other) => other != null && Key == other.Key && Summary == other.Summary && EqualityComparer<IniValue>.Default.Equals(Value, other.Value);

        public override int GetHashCode() => Key.GetHashCode();

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
