using System;
using System.Collections.Generic;

using Shimakaze.Struct.Ini.Options;

namespace Shimakaze.Struct.Ini
{
    public interface IIniSection : IEnumerable<IIniKeyValuePair>
    {
        /// <summary>
        /// Section Head
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Summary in Section Head Line
        /// </summary>
        string Summary { get; set; }
        IniValue? this[string key] { get; set; }
        IniValue? this[string key, IniValue @default = default, IniGetOption option = default] { get; }
        IniValue? this[string key, string summary = default, IniSetOption option = default] { set; }
        string GetSummary(string key);
        bool Remove(string key);
        void RemoveAll();
        int Count { get; }
        bool Contains(string key);
        int IndexOf(string key);
        void Put(IIniKeyValuePair keyValuePair);
    }
}