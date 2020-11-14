using System;

namespace Shimakaze.Struct.Ini
{
    public interface IIniKeyValuePair
    {
        /// <summary>
        /// Key
        /// </summary>
        string Key { get; set; }
        /// <summary>
        /// Summary
        /// </summary>
        string Summary { get; set; }
        /// <summary>
        /// Value
        /// </summary>
        IniValue? Value { get; set; }
        int GetHashCode();
    }
}