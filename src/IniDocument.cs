using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        /// <summary>
        /// 从流中分析并返回<see cref="IniDocument"/>
        /// </summary>
        public static async Task<IniDocument> ParseAsync(Stream stream)
        {
            var sr = new StreamReader(stream);
            var data = new List<IniSection>();
            IniDocument document = new IniDocument();
            IniSection? lastSection = null;
            var lastSectionContent = new List<IniKeyValuePair>();
            while (!sr.EndOfStream)
            {
                var line = await sr.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line)) continue;
                if (line.TrimStart()[0].Equals('['))
                {
                    SaveSection();
                    int? summarySeparatorIndex = null;
                    if (line.Contains(";")) summarySeparatorIndex = line.IndexOf(';');

                    var tmpSection = new IniSection { Name = line.TrimStart().Substring(1, line.IndexOf(']') - 1) };
                    if (summarySeparatorIndex.HasValue) tmpSection.Summary = line.Substring(summarySeparatorIndex.Value);
                    lastSection = tmpSection;
                }
                else
                    lastSectionContent.Add(IniKeyValuePair.Parse(line));
            }
            SaveSection();
            document.Sections = data.ToArray();
            return document;

            void SaveSection()
            {
                if (lastSection.HasValue)
                {
                    var section = lastSection.Value;
                    section.Content = lastSectionContent.ToArray();
                    data.Add(section);
                }
                else document.NoSectionContent = lastSectionContent.ToArray();
                lastSectionContent.Clear();
            }
        }

        public bool TryGetSection(string name, out IniSection? section)
        {
            section = null;
            foreach (var item in Sections)
            {
                if (item.Name.Equals(name))
                {
                    section = item;
                    return true;
                }
            }
            return false;
        }
        public IniSection? TryGetSection(string name)
        {
            foreach (var item in Sections)
            {
                if (item.Name.Equals(name))
                {
                    return item;
                }
            }
            return null;
        }

        public bool TryGetKey(string name, out IniKeyValuePair? keyValuePair)
        {
            keyValuePair = null;
            foreach (var item in NoSectionContent)
            {
                if (item.Key.Equals(name))
                {
                    keyValuePair = item;
                    return true;
                }
            }
            return false;
        }
        public IniKeyValuePair? TryGetKey(string name)
        {
            foreach (var item in NoSectionContent)
            {
                if (item.Key.Equals(name))
                {
                    return item;
                }
            }
            return null;
        }
    }
}
