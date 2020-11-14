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
    /// Ini Section Structure
    /// </summary>
    internal class IniSection : IIniSection
    {
        internal IList<IIniKeyValuePair> Content { get; set; } = new List<IIniKeyValuePair>();
        internal IniSection()
        {
        }

        internal IniSection(string name) : this() => Name = name ?? throw new ArgumentNullException(nameof(name));

        internal IniSection(string name, IEnumerable<IIniKeyValuePair> content) : this(name) => Content = new List<IIniKeyValuePair>(content);

        internal IniSection(string name, IEnumerable<IIniKeyValuePair> content, string summary) : this(name, content) => Summary = summary;

        public string Name { get; set; }
        public string Summary { get; set; }
        public int Count => Content.Count;

        public IniValue? this[string key] { get => this[key, @default: default]; set => this[key, summary: default] = value; }

        public IniValue? this[string key, string summary = default, IniSetOption option = default]
        {
            set
            {
                if (option is null)
                    option = new IniSetOption();

                foreach (var item in Content)
                {
                    if (item.Key == key)
                    {
                        if (option.Cover)
                        {
                            if (!(value is null))
                                item.Value = value;
                            if (!(summary is null))
                                item.Summary = summary;
                            return;
                        }
                        else
                            throw new DataExistsException(key + " was Exists");
                    }
                }
                if (option.Create)
                    Content.Add(IniKeyValuePairUtils.CreateFullLine(key, value, summary));
                else
                    throw new KeyNotFoundException($"Cannot Found Key : {key}");
            }
        }
        public IniValue? this[string key, IniValue @default = default, IniGetOption option = default]
        {
            get
            {
                if (option is null)
                    option = new IniGetOption { Create = false };

                foreach (var item in Content)
                {
                    if (item.Key == key)
                        return item.Value;
                }
                if (option.Create)
                    Content.Add(IniKeyValuePairUtils.CreateDataLine(key, @default));
                return @default;
            }
        }

        public bool Contains(string key) => IndexOf(key) != -1;


        public IEnumerator<IIniKeyValuePair> GetEnumerator() => Content.GetEnumerator();

        public string GetSummary(string key)
        {
            foreach (var item in Content)
            {
                if (item.Key == key)
                    return item.Value;
            }
            return null;
        }

        public int IndexOf(string key)
        {
            for (int i = 0; i < Content.Count; i++)
            {
                if (Content[i].Key == key)
                    return i;
            }
            return -1;
        }

        public void Put(IIniKeyValuePair keyValuePair) => Content.Add(keyValuePair);

        public bool Remove(string key)
        {
            var index = IndexOf(key);
            if (index > -1)
            {
                Content.RemoveAt(index);
                return true;
            }
            return false;
        }

        public void RemoveAll() => Content.Clear();

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder($"[{Name}]");
            if (!string.IsNullOrEmpty(Summary))
                sb.Append($"; {Summary}");
            sb.AppendLine();
            foreach (IIniKeyValuePair item in Content)
                sb.AppendLine(item.ToString());
            return sb.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator() => Content.GetEnumerator();
    }
}
