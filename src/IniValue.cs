using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Shimakaze.Struct.Ini
{
    /// <summary>
    /// 表示一个INI值
    /// </summary>
    public struct IniValue
    {
        // 数据源
        internal string raw;
        /// <summary>
        /// 是否存在数据内容
        /// </summary>
        public bool HasData => !string.IsNullOrEmpty(raw);

        public override bool Equals(object obj) => obj is IniValue value && raw.Equals(value.raw);

        public override int GetHashCode() => 875105689 + EqualityComparer<string>.Default.GetHashCode(raw);

        public override string ToString() => raw;

        public static implicit operator string(IniValue value) => value.raw;

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

        public static implicit operator bool(IniValue value) =>
            (new char[] { 'y', 'Y', 't', 'T', '1' }).Contains(value.raw[0]) ||
            ((new char[] { 'n', 'N', 'f', 'F', '0' }).Contains(value.raw[0])
            ? false : throw new FormatException($"{value.raw} is not bool"));

        public static implicit operator IniValue(string s) => new IniValue { raw = s };

        public static implicit operator IniValue(int i) => new IniValue { raw = i.ToString() };
        public static implicit operator IniValue(long i) => new IniValue { raw = i.ToString() };
        public static implicit operator IniValue(double d) => new IniValue { raw = d.ToString() };
        /// <summary>
        /// 为了与<see cref="double"/>转换区分, 这个转换是强制转换
        /// </summary>
        public static explicit operator IniValue(decimal i) => new IniValue { raw = i.ToString() };

    }
}
