using System;
using System.Collections.Generic;

using Shimakaze.Struct.Ini.Options;

namespace Shimakaze.Struct.Ini
{
    public interface IIniDocument : IEnumerable<IIniSection>
    {
        IIniSection this[string section, IniGetOption option = default] { get; }
        IniValue? this[string section, string key] { get; set; }
        IniValue? this[string section, string key, IniValue @default = default, IniGetOption option = default] { get; }
        IniValue? this[string section, string key, string summary = default, IniSetOption option = default] { set; }
        IIniSection New(string section);
        bool Remove(string section);
        void RemoveAll();
        int Count { get; }
        bool Contains(string section);

        IIniSection Head { get; }

        void Put(IIniSection section);
        int IndexOf(string section);
    }
}