using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shimakaze.Struct.Ini
{

    public struct IniSection
    {
        public IniKeyValuePair this[string key] => Content.First(i => i.Key.Equals(key));

        /// <summary>
        /// 节名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 注释
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public IniKeyValuePair[] Content { get; set; }

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
