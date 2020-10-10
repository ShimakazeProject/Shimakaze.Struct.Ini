using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Shimakaze.Struct.Ini
{
    /// <summary>
    /// a value for IniKeyValuePair
    /// </summary>
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public struct IniValue
    {
        // 数据源
        internal string raw;

        public IniValue(string raw) => this.raw = raw;

        /// <summary>
        /// if (this is Not Empty) return true;<br/>
        /// else return false
        /// </summary>
        public bool HasData => !string.IsNullOrEmpty(this.raw);
        public override bool Equals(object obj) => obj is IniValue value && this.raw.Equals(value.raw);

        public override int GetHashCode() => 875105689 + this.raw.GetHashCode();
        private string GetDebuggerDisplay() => this.ToString();

        public override string ToString() => this.raw;

        public static implicit operator string(IniValue value) => value.ToString();

        public static explicit operator byte(IniValue value) => byte.Parse(value.raw);
        public static explicit operator short(IniValue value) => short.Parse(value.raw);
        public static explicit operator int(IniValue value) => int.Parse(value.raw);
        public static explicit operator long(IniValue value) => long.Parse(value.raw);

        public static explicit operator sbyte(IniValue value) => sbyte.Parse(value.raw);
        public static explicit operator ushort(IniValue value) => ushort.Parse(value.raw);
        public static explicit operator uint(IniValue value) => uint.Parse(value.raw);
        public static explicit operator ulong(IniValue value) => ulong.Parse(value.raw);

        public static explicit operator float(IniValue value) => float.Parse(value.raw);
        public static explicit operator double(IniValue value) => double.Parse(value.raw);

        public static explicit operator decimal(IniValue value) => decimal.Parse(value.raw);
        /// <summary>
        /// IgoneCase<br/>
        /// Y(es), T(ure) 1 return true<br/>
        /// N(o), F(alse) 0 return false<br/>
        /// else throw FormatException
        /// </summary>
        public static explicit operator bool(IniValue value) =>
            (new char[] { 'y', 'Y', 't', 'T', '1' }).Contains(value.raw[0]) ||
            ((new char[] { 'n', 'N', 'f', 'F', '0' }).Contains(value.raw[0])
            ? false : throw new FormatException($"{value.raw} is not bool"));

        public static implicit operator IniValue(string s) => new IniValue(s);

        public static implicit operator IniValue(int i) => new IniValue(i.ToString());
        public static implicit operator IniValue(long i) => new IniValue(i.ToString());
        public static implicit operator IniValue(double d) => new IniValue(d.ToString());
        public static implicit operator IniValue(decimal i) => new IniValue(i.ToString());

    }
}
