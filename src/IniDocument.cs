using System;
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
        /// Here <see cref="IniKeyValuePair"/>s are Independent of <see cref="Sections"/>
        /// </summary>
        public IniKeyValuePair[] NoSectionContent { get; set; }

        /// <summary>
        /// All <see cref="IniSection"/>s on this <see cref="IniDocument"/>
        /// </summary>
        public IniSection[] Sections { get; set; }

        /// <summary>
        /// Get an <see cref="IniSection"/> from <see cref="Sections"/>
        /// </summary>
        public IniSection this[string sectionName] => this.Sections.First(i => i.Name.Equals(sectionName));

        /// <summary>
        /// 从流中分析并返回<see cref="IniDocument"/>
        /// </summary>
        public static Task<IniDocument> ParseAsync(Stream stream) => ParseAsync(new StreamReader(stream));

        public static async Task<IniDocument> ParseAsync(TextReader reader)
        {
            var data = new List<IniSection>();
            IniDocument document = new IniDocument();
            IniSection? lastSection = null;
            var lastSectionContent = new List<IniKeyValuePair>();
            while (reader.Peek() > 0)
            {
                var line = await reader.ReadLineAsync();
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
                    lastSectionContent.Add(await Task.Run(() => IniKeyValuePair.Parse(line)));
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


        public async Task DeparseAsync(TextWriter writer)
        {
            foreach (var item in this.NoSectionContent)
                await item.DepraseAsync(writer);
            foreach (var item in this.Sections)
                await item.DepraseAsync(writer);
        }
        public override bool Equals(object obj) => obj is IniDocument document &&
                           EqualityComparer<IniKeyValuePair[]>.Default.Equals(this.NoSectionContent, document.NoSectionContent) &&
                           EqualityComparer<IniSection[]>.Default.Equals(this.Sections, document.Sections);

        /// <summary>
        /// Get an <see cref="IniKeyValuePair"/> from <see cref="NoSectionContent"/>
        /// </summary>
        public IniKeyValuePair GetFromNoSectionContent(string key) => this.NoSectionContent.First(i => i.Key.Equals(key));
        public override int GetHashCode()
        {
            int hashCode = -180461457;
            hashCode = hashCode * -1521134295 + EqualityComparer<IniKeyValuePair[]>.Default.GetHashCode(this.NoSectionContent);
            hashCode = hashCode * -1521134295 + EqualityComparer<IniSection[]>.Default.GetHashCode(this.Sections);
            return hashCode;
        }

        /// <summary>
        /// Convert to String 
        /// </summary>
        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var item in this.NoSectionContent)
                sb.AppendLine(item.ToString());
            foreach (var item in this.Sections)
                sb.AppendLine(item.ToString());
            return sb.ToString();
        }
        public bool TryGetKey(string name, out IniKeyValuePair? keyValuePair)
        {
            keyValuePair = null;
            foreach (var item in this.NoSectionContent.Where(item => item.HasData && item.Key.Equals(name)))
            {
                keyValuePair = item;
                return true;
            }

            return false;
        }

        public IniKeyValuePair? TryGetKey(string name)
        {
            foreach (var item in this.NoSectionContent.Where(item => item.HasData && item.Key.Equals(name)))            
                return item;            

            return null;
        }

        public bool TryGetSection(string name, out IniSection? section)
        {
            section = null;
            foreach (var item in this.Sections.Where(item => item.Name.Equals(name)))
            {
                section = item;
                return true;
            }

            return false;
        }
        public IniSection? TryGetSection(string name)
        {
            foreach (var item in this.Sections.Where(item => item.Name.Equals(name)))
                return item;
            
            return null;
        }
    }
}
