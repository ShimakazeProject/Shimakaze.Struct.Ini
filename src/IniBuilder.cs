using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Shimakaze.Struct.Ini
{
    public class IniBuilder : IList<IniSectionBuilder>, IList<IniKeyValuePair>
    {
        #region 用户属性
        /// <summary>
        /// Here <see cref="IniKeyValuePair"/>s are Independent of <see cref="Sections"/>
        /// </summary>
        public List<IniKeyValuePair> NoSectionContent { get; }

        /// <summary>
        /// All <see cref="IniSectionBuilder"/>s on this <see cref="IniSectionBuilder"/>
        /// </summary>
        public List<IniSectionBuilder> Sections { get; }
        #endregion

        #region List属性
        public int Count => this.Sections.Count;
        int ICollection<IniKeyValuePair>.Count => this.NoSectionContent.Count;

        bool ICollection<IniSectionBuilder>.IsReadOnly => false;
        bool ICollection<IniKeyValuePair>.IsReadOnly => false;
        #endregion

        #region 访问器

        /// <summary>
        /// Get an <see cref="IniSectionBuilder"/> from <see cref="Sections"/>.
        /// It will return <see cref="null"/> if not found.
        /// </summary>
        public IniSectionBuilder this[string sectionName] => TryGetSection(sectionName);

        public IniSectionBuilder this[int index] { get => (this.Sections)[index]; set => (this.Sections)[index] = value; }
        IniKeyValuePair IList<IniKeyValuePair>.this[int index] { get => (this.NoSectionContent)[index]; set => (this.NoSectionContent)[index] = value; }
        #endregion

        #region 构造方法
        public IniBuilder()
        {
            this.Sections = new List<IniSectionBuilder>();
            this.NoSectionContent = new List<IniKeyValuePair>();
        }

        public IniBuilder(IEnumerable<IniSectionBuilder> sections) : this()
            => this.Sections = sections is List<IniSectionBuilder> list ? list : new List<IniSectionBuilder>(sections);

        public IniBuilder(IEnumerable<IniSectionBuilder> sections, IEnumerable<IniKeyValuePair> noSectionContent) : this(sections)
            => this.NoSectionContent = noSectionContent is List<IniKeyValuePair> list ? list : new List<IniKeyValuePair>(noSectionContent);

        public IniBuilder(IniDocument ini) : this(ini.Sections.Select(i => new IniSectionBuilder(i)), ini.NoSectionContent) { }
        #endregion

        #region 解析/反解析方法
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


        public Task DeparseAsync(Stream stream) => DeparseAsync(new StreamWriter(stream));

        public async Task DeparseAsync(TextWriter writer)
        {
            if ((this.NoSectionContent?.Count ?? 0) > 0)
                foreach (var item in this.NoSectionContent)
                {
                    await item.DepraseAsync(writer);
                    await writer.WriteLineAsync();
                }
            if ((this.Sections?.Count ?? 0) > 0)
                foreach (var item in this.Sections)
                {
                    await item.DepraseAsync(writer);
                    await writer.WriteLineAsync();
                }
        }
        #endregion

        #region Try方法

        public bool TryGetKey(string name, out IniKeyValuePair? keyValuePair)
        {
            keyValuePair = null;
            foreach (var item in this.NoSectionContent.Where(item => name.Equals(item.Key)))
            {
                keyValuePair = item;
                return true;
            }

            return false;
        }

        public IniKeyValuePair? TryGetKey(string name)
        {
            foreach (var item in this.NoSectionContent.Where(item => name.Equals(item.Key)))
                return item;

            return null;
        }

        public bool TryGetSection(string name, out IniSectionBuilder section)
        {
            section = null;
            foreach (var item in this.Sections.Where(item => name.Equals(item.Name)))
            {
                section = item;
                return true;
            }

            return false;
        }

        public IniSectionBuilder TryGetSection(string name)
        {
            foreach (var item in this.Sections.Where(item => name.Equals(item.Name)))
                return item;

            return null;
        }
        #endregion

        #region List方法
        public void Add(IniSection iniSection) => Add(new IniSectionBuilder(iniSection));
        public void Add(IniSectionBuilder iniSection) => this.Sections.Add(iniSection);
        public void Add(IniKeyValuePair keyValuePair) => this.NoSectionContent.Add(keyValuePair);
        public void Clear() => this.Sections.Clear();
        public bool Contains(IniSectionBuilder item) => this.Sections.Contains(item);

        public bool Contains(IniKeyValuePair item) => this.NoSectionContent.Contains(item);

        public void CopyTo(IniSectionBuilder[] array, int arrayIndex) => this.Sections.CopyTo(array, arrayIndex);

        public void CopyTo(IniKeyValuePair[] array, int arrayIndex) => this.NoSectionContent.CopyTo(array, arrayIndex);

        public IEnumerator<IniSectionBuilder> GetEnumerator() => this.Sections.GetEnumerator();

        public int IndexOf(IniSectionBuilder item) => this.Sections.IndexOf(item);

        public int IndexOf(IniKeyValuePair item) => this.NoSectionContent.IndexOf(item);

        public void Insert(int index, IniSectionBuilder item) => this.Sections.Insert(index, item);

        public void Insert(int index, IniKeyValuePair item) => this.NoSectionContent.Insert(index, item);

        public bool Remove(IniSectionBuilder item) => this.Sections.Remove(item);

        public bool Remove(IniKeyValuePair item) => this.NoSectionContent.Remove(item);

        public void RemoveAt(int index) => this.Sections.RemoveAt(index);

        void ICollection<IniKeyValuePair>.Clear() => this.NoSectionContent.Clear();
        IEnumerator IEnumerable.GetEnumerator() => this.Sections.GetEnumerator();

        IEnumerator<IniKeyValuePair> IEnumerable<IniKeyValuePair>.GetEnumerator() => this.NoSectionContent.GetEnumerator();

        void IList<IniKeyValuePair>.RemoveAt(int index) => this.NoSectionContent.RemoveAt(index);
        #endregion
    }
}
