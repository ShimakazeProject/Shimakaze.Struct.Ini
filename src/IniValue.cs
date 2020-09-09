using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Shimakaze.Struct.Ini
{
    /// <summary>
    /// a value for IniKeyValuePair
    /// </summary>
    public struct IniValue
    {
        // 数据源
        internal string raw;
        /// <summary>
        /// if (this is Not Empty) return true;
        /// else return false
        /// </summary>
        public bool HasData => !string.IsNullOrEmpty(raw);
        public override bool Equals(object obj) => obj is IniValue value && raw.Equals(value.raw);

        public override int GetHashCode() => 875105689 + EqualityComparer<string>.Default.GetHashCode(raw);

        public override string ToString() => raw;


        /// <summary>
        /// Get a String copy
        /// </summary>
        public static implicit operator string(IniValue value) => value.raw.Clone() as string;

        public static implicit operator byte(IniValue value) => byte.Parse(value.raw);
        public static implicit operator short(IniValue value) => short.Parse(value.raw);
        public static implicit operator int(IniValue value) => int.Parse(value.raw);
        public static implicit operator long(IniValue value) => long.Parse(value.raw);

        public static implicit operator sbyte(IniValue value) => sbyte.Parse(value.raw);
        public static implicit operator ushort(IniValue value) => ushort.Parse(value.raw);
        public static implicit operator uint(IniValue value) => uint.Parse(value.raw);
        public static implicit operator ulong(IniValue value) => ulong.Parse(value.raw);

        public static implicit operator float(IniValue value) => float.Parse(value.raw);
        public static implicit operator double(IniValue value) => double.Parse(value.raw);

        public static implicit operator decimal(IniValue value) => decimal.Parse(value.raw);
        /// <summary>
        /// IgoneCase<br/>
        /// Y(es), T(ure) 1 return true
        /// N(o), F(alse) 0 return alse
        /// else throw FormatException
        /// </summary>
        public static implicit operator bool(IniValue value) =>
            (new char[] { 'y', 'Y', 't', 'T', '1' }).Contains(value.raw[0]) ||
            ((new char[] { 'n', 'N', 'f', 'F', '0' }).Contains(value.raw[0])
            ? false : throw new FormatException($"{value.raw} is not bool"));

        public static implicit operator IniValue(string s) => new IniValue { raw = s };

        public static implicit operator IniValue(int i) => new IniValue { raw = i.ToString() };
        public static implicit operator IniValue(long i) => new IniValue { raw = i.ToString() };
        public static implicit operator IniValue(double d) => new IniValue { raw = d.ToString() };
        public static explicit operator IniValue(decimal i) => new IniValue { raw = i.ToString() };

    }
}
