using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Shimakaze.Struct.Ini.Implements
{
    [DebuggerDisplay("{" + nameof(ToString) + "(),nq}")]
    internal class IniKeyValuePair : IIniKeyValuePair
    {
        private string key;

        public string Key
        {
            get => key;
            set => key = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Summary { get; set; } = null;

        public IniValue? Value { get; set; } = null;

        internal IniKeyValuePair()
        {
        }

        internal IniKeyValuePair(string key) : this() => Key = key ?? throw new ArgumentNullException(nameof(key));

        internal IniKeyValuePair(string key, IniValue? value) : this(key) => Value = value;

        internal IniKeyValuePair(string key, IniValue? value, string summary) : this(key, value) => Summary = summary;

        public override int GetHashCode() => Key.GetHashCode();

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(Key) && !string.IsNullOrWhiteSpace(Value))
                sb.Append($"{Key}={Value}");
            if (!string.IsNullOrWhiteSpace(Summary))
                sb.Append($"; {Summary}");
            return sb.ToString();
        }
    }
}
