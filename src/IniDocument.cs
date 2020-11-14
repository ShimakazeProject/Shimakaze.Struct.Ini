using System.Collections;
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
    public sealed class IniDocument : IList<IniSection>, IList<IniKeyValuePair>
    {

        #region Public Properties

        /// <summary>
        /// Here <see cref="IniKeyValuePair" /> s are Independent of <see cref="Sections" />
        /// </summary>
        public List<IniKeyValuePair> NoSectionContent { get; set; }

        /// <summary>
        /// All <see cref="IniSection" /> s on this <see cref="IniDocument" />
        /// </summary>
        public List<IniSection> Sections { get; set; }

        public int Count => ((ICollection<IniSection>)Sections).Count;

        public bool IsReadOnly => ((ICollection<IniSection>)Sections).IsReadOnly;

        #endregion Public Properties

        #region Public Indexers

        /// <summary>
        /// Get an <see cref="IniSection" /> from <see cref="Sections" />
        /// </summary>
        public IniSection this[string sectionName] => Sections.First(i => sectionName.Equals(i.Name));

        public IniSection this[int index] { get => ((IList<IniSection>)Sections)[index]; set => ((IList<IniSection>)Sections)[index] = value; }
        IniKeyValuePair IList<IniKeyValuePair>.this[int index] { get => ((IList<IniKeyValuePair>)NoSectionContent)[index]; set => ((IList<IniKeyValuePair>)NoSectionContent)[index] = value; }

        #endregion Public Indexers

        #region Public Methods

        /// <summary>
        /// 从流中分析并返回 <see cref="IniDocument" />
        /// </summary>
        public static Task<IniDocument> ParseAsync(Stream stream) => ParseAsync(new StreamReader(stream));

        public static async Task<IniDocument> ParseAsync(TextReader reader)
        {
            var document = new IniDocument();
            IniSection lastSection = null;
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
            return document;

            void SaveSection()
            {
                if (lastSection is IniSection)
                {
                    lastSection.Content = lastSectionContent;
                    document.Sections.Add(lastSection);
                }
                else document.NoSectionContent = lastSectionContent;
                lastSectionContent.Clear();
            }
        }

        public void Add(IniSection item) => ((ICollection<IniSection>)Sections).Add(item);

        public void Add(IniKeyValuePair item) => ((ICollection<IniKeyValuePair>)NoSectionContent).Add(item);

        public void Clear() => ((ICollection<IniSection>)Sections).Clear();

        public bool Contains(IniSection item) => ((ICollection<IniSection>)Sections).Contains(item);

        public bool Contains(IniKeyValuePair item) => ((ICollection<IniKeyValuePair>)NoSectionContent).Contains(item);

        public void CopyTo(IniSection[] array, int arrayIndex) => ((ICollection<IniSection>)Sections).CopyTo(array, arrayIndex);

        public void CopyTo(IniKeyValuePair[] array, int arrayIndex) => ((ICollection<IniKeyValuePair>)NoSectionContent).CopyTo(array, arrayIndex);

        public Task DeparseAsync(Stream stream) => DeparseAsync(new StreamWriter(stream));

        public async Task DeparseAsync(TextWriter writer)
        {
            if ((NoSectionContent?.Count ?? 0) > 0)
                foreach (var item in NoSectionContent)
                {
                    await item.DepraseAsync(writer);
                    await writer.WriteLineAsync();
                }
            if ((Sections?.Count ?? 0) > 0)
                foreach (var item in Sections)
                {
                    await item.DepraseAsync(writer);
                    await writer.WriteLineAsync();
                }
        }

        public IEnumerator<IniSection> GetEnumerator() => ((IEnumerable<IniSection>)Sections).GetEnumerator();

        /// <summary>
        /// Get an <see cref="IniKeyValuePair" /> from <see cref="NoSectionContent" />
        /// </summary>
        public IniKeyValuePair GetFromNoSectionContent(string key) => NoSectionContent.First(i => key.Equals(i.Key));

        public int IndexOf(IniSection item) => ((IList<IniSection>)Sections).IndexOf(item);

        public int IndexOf(IniKeyValuePair item) => ((IList<IniKeyValuePair>)NoSectionContent).IndexOf(item);

        public void Insert(int index, IniSection item) => ((IList<IniSection>)Sections).Insert(index, item);

        public void Insert(int index, IniKeyValuePair item) => ((IList<IniKeyValuePair>)NoSectionContent).Insert(index, item);

        public bool Remove(IniSection item) => ((ICollection<IniSection>)Sections).Remove(item);

        public bool Remove(IniKeyValuePair item) => ((ICollection<IniKeyValuePair>)NoSectionContent).Remove(item);

        public void RemoveAt(int index) => ((IList<IniSection>)Sections).RemoveAt(index);

        /// <summary>
        /// Convert to String
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

        public bool TryGetKey(string name, out IniKeyValuePair keyValuePair)
        {
            keyValuePair = null;
            foreach (var item in NoSectionContent.Where(item => name.Equals(item.Key)))
            {
                keyValuePair = item;
                return true;
            }

            return false;
        }

        public IniKeyValuePair TryGetKey(string name)
        {
            foreach (var item in NoSectionContent.Where(item => name.Equals(item.Key)))
                return item;

            return null;
        }

        public bool TryGetSection(string name, out IniSection section)
        {
            section = null;
            foreach (var item in Sections.Where(item => name.Equals(item.Name)))
            {
                section = item;
                return true;
            }

            return false;
        }

        public IniSection TryGetSection(string name)
        {
            foreach (var item in Sections.Where(item => name.Equals(item.Name)))
                return item;

            return null;
        }

        IEnumerator IEnumerable.GetEnumerator() => Sections.GetEnumerator();

        IEnumerator<IniKeyValuePair> IEnumerable<IniKeyValuePair>.GetEnumerator() => ((IEnumerable<IniKeyValuePair>)NoSectionContent).GetEnumerator();

        #endregion Public Methods

    }
}
