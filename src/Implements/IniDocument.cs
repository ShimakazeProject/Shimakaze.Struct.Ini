using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Shimakaze.Struct.Ini.Exceptions;
using Shimakaze.Struct.Ini.Options;
using Shimakaze.Struct.Ini.Utils;

namespace Shimakaze.Struct.Ini.Implements
{
    /// <summary>
    /// 表示一个Ini文档
    /// </summary>
    internal sealed class IniDocument : IIniDocument
    {
        internal IniDocument()
        {
        }

        public IIniSection this[string section, IniGetOption option = default]
        {
            get
            {
                if (option is null)
                    option = new IniGetOption();

                foreach (var item in Sections)
                {
                    if (item.Name == section)
                        return item;
                }
                if (option.Create)
                {
                    var result = New(section);
                    Sections.Add(result);
                    return result;
                }
                else
                    throw new DataNotExistsException($"Cannot Found Section : {section}");
            }
        }
        public IniValue? this[string section, string key]
        {
            get => this[section][key];
            set => this[section][key] = value;
        }
        public IniValue? this[string section, string key, IniValue @default = default, IniGetOption option = default] => this[section][key, @default, option];
        public IniValue? this[string section, string key, string summary = default, IniSetOption option = default] { set => this[section][key, summary, option] = value; }

        public IIniSection Head { get; internal set; } = IniSectionUtils.CreateSection("<Ini Document Head>");

        public IList<IIniSection> Sections { get; set; } = new List<IIniSection>();

        public int Count => Sections.Count;


        public bool Contains(string section) => IndexOf(section) != -1;
        public int IndexOf(string section)
        {
            for (int i = 0; i < Sections.Count; i++)
            {
                if (Sections[i].Name == section)
                    return i;
            }
            return -1;
        }


        public IIniSection New(string section) => IniSectionUtils.CreateSection(section);
        public bool Remove(string section)
        {
            var index = IndexOf(section);
            if (index > -1)
            {
                Sections.RemoveAt(index);
                return true;
            }
            return false;
        }
        public void RemoveAll() => Sections.Clear();
        public void Put(IIniSection section) => Sections.Add(section);
        public IEnumerator<IIniSection> GetEnumerator() => Sections.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        /// <summary>
        /// Convert to String
        /// </summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (IIniKeyValuePair item in Head)
                sb.AppendLine(item.ToString());
            foreach (IIniSection item in Sections)
                sb.AppendLine(item.ToString());
            return sb.ToString();
        }
    }
}
