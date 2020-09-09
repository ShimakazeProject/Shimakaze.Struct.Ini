using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shimakaze.Struct.Ini
{
    /// <summary>
    /// Ini Section Structure
    /// </summary>
    public struct IniSection
    {
        internal string name;
        internal string summary;
        internal IniKeyValuePair[] content;

        public IniKeyValuePair this[string key] => Content.First(i => i.Key.Equals(key));

        /// <summary>
        /// Section Head
        /// </summary>
        public string Name { get => name; set => name = value; }
        /// <summary>
        /// Summary in Section Head Line
        /// </summary>
        public string Summary { get => summary; set => summary = value; }

        public IniKeyValuePair[] Content { get => content; set => content = value; }

        public override bool Equals(object obj) => obj is IniSection section &&
                   Name == section.Name &&
                   Summary == section.Summary &&
                   EqualityComparer<IniKeyValuePair[]>.Default.Equals(Content, section.Content);

        public override int GetHashCode()
        {
            int hashCode = 117231219;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Summary);
            hashCode = hashCode * -1521134295 + EqualityComparer<IniKeyValuePair[]>.Default.GetHashCode(Content);
            return hashCode;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append('[');
            sb.Append(Name);
            sb.Append(']');
            if (!string.IsNullOrEmpty(Summary))
            {
                sb.Append(';');
                sb.Append(Summary);
            }
            sb.AppendLine();
            foreach (var item in Content)
            {
                sb.AppendLine(item.ToString());
            }
            return sb.ToString();
        }


        public bool TryGetKey(string name, out IniKeyValuePair? keyValuePair)
        {
            keyValuePair = null;
            foreach (var item in Content)
            {
                if (item.HasData && item.Key.Equals(name))
                {
                    keyValuePair = item;
                    return true;
                }
            }
            return false;
        }

        public IniKeyValuePair? TryGetKey(string name)
        {
            foreach (var item in Content)
            {
                if (item.HasData && item.Key.Equals(name))
                {
                    return item;
                }
            }
            return null;
        }
    }
}
