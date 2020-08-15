using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shimakaze.Struct.Ini
{
    /// <summary>
    /// 表示一个Ini文档
    /// </summary>
    public struct IniDocument
    {
        /// <summary>
        /// 这里的 <see cref="IniKeyValuePair"/> 不属于当前 <see cref="IniDocument"/> 中的任何一个 <see cref="IniSection"/>
        /// </summary>
        public IniKeyValuePair[] NoSectionContent { get; set; }

        /// <summary>
        /// 当前 <see cref="IniDocument"/> 中的所有 <see cref="IniSection"/>
        /// </summary>
        public IniSection[] Sections { get; set; }

        /// <summary>
        /// 获取一个 <see cref="IniSection"/>
        /// </summary>
        public IniSection this[string sectionName] => Sections.First(i => i.Name.Equals(sectionName));

        /// <summary>
        /// 从 <see cref="NoSectionContent"/> 中获取一个 <see cref="IniKeyValuePair"/>
        /// </summary>
        public IniKeyValuePair GetFromNoSectionContent(string key) => NoSectionContent.First(i => i.Key.Equals(key));

        public override bool Equals(object obj) => obj is IniDocument document &&
                   EqualityComparer<IniKeyValuePair[]>.Default.Equals(NoSectionContent, document.NoSectionContent) &&
                   EqualityComparer<IniSection[]>.Default.Equals(Sections, document.Sections);

        public override int GetHashCode()
        {
            int hashCode = -180461457;
            hashCode = hashCode * -1521134295 + EqualityComparer<IniKeyValuePair[]>.Default.GetHashCode(NoSectionContent);
            hashCode = hashCode * -1521134295 + EqualityComparer<IniSection[]>.Default.GetHashCode(Sections);
            return hashCode;
        }

        /// <summary>
        /// 转换为ini文档
        /// </summary>
        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var item in NoSectionContent)
                sb.AppendLine(item.ToString());
            foreach (var item in Sections)
                sb.AppendLine(item.ToString());
            return sb.ToString();
        }
    }
}
