using System;
using System.Diagnostics;
using System.Linq;

namespace Shimakaze.Struct.Ini
{
    /// <summary>
    /// a value for <see cref="IIniKeyValuePair"/>
    /// </summary>
    [DebuggerDisplay("{" + nameof(ToString) + "(),nq}")]
    public struct IniValue
    {
        // 数据源
        internal string raw;

        /// <summary>
        /// if (this is Not Empty) return true; <br /> else return false
        /// </summary>
        public bool HasData => !string.IsNullOrEmpty(raw);

        public IniValue(string value) => raw = value;

        /// <summary>
        /// IgoneCase <br /> Y(es), T(ure) 1 return true <br /> N(o), F(alse) 0 return false <br />
        /// else throw FormatException
        /// </summary>
        public static explicit operator bool(IniValue value) =>
            !(value.raw is null)
            && ((new char[] { 'y', 'Y', 't', 'T', '1' }).Contains(value.raw[0])
            || ((new char[] { 'n', 'N', 'f', 'F', '0' }).Contains(value.raw[0])
            ? false : throw new FormatException($"{value.raw} is not bool")));

        public static explicit operator byte(IniValue value) => byte.Parse(value.raw);

        public static explicit operator decimal(IniValue value) => decimal.Parse(value.raw);

        public static explicit operator double(IniValue value) => double.Parse(value.raw);

        public static explicit operator float(IniValue value) => float.Parse(value.raw);

        public static explicit operator int(IniValue value) => int.Parse(value.raw);

        public static explicit operator long(IniValue value) => long.Parse(value.raw);

        public static explicit operator sbyte(IniValue value) => sbyte.Parse(value.raw);

        public static explicit operator short(IniValue value) => short.Parse(value.raw);

        public static explicit operator uint(IniValue value) => uint.Parse(value.raw);

        public static explicit operator ulong(IniValue value) => ulong.Parse(value.raw);

        public static explicit operator ushort(IniValue value) => ushort.Parse(value.raw);

        public static implicit operator IniValue(string s) => new IniValue(s);

        public static implicit operator IniValue(int i) => new IniValue(i.ToString());

        public static implicit operator IniValue(long i) => new IniValue(i.ToString());

        public static implicit operator IniValue(bool i) => new IniValue(i.ToString());

        public static implicit operator IniValue(double d) => new IniValue(d.ToString());

        public static implicit operator IniValue(decimal i) => new IniValue(i.ToString());

        public static implicit operator string(IniValue value) => value.ToString();

        public override bool Equals(object obj) => raw.Equals((obj as IniValue?)?.raw);

        public override int GetHashCode() => raw.GetHashCode();

        public override string ToString() => raw;

    }
}
