using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shimakaze.Struct.Ini.Implements
{
    /// <summary>
    /// 表示一个Ini文档
    /// </summary>
    public sealed class IniDocument : IIniDocument
    {
        internal IniDocument()
        {
        }
        #region Public Properties

        public IList<IIniKeyValuePair> NoSectionContent { get; set; }

        public IList<IIniSection> Sections { get; set; }

        public int Count => Sections.Count;

        public bool IsReadOnly => Sections.IsReadOnly;

        #endregion Public Properties

        #region Public Indexers

        public IIniSection this[string sectionName] => Sections.First(i => sectionName.Equals(i.Name));

        public IIniSection this[int index] { get => Sections[index]; set => Sections[index] = value; }
        IIniKeyValuePair IList<IIniKeyValuePair>.this[int index] { get => NoSectionContent[index]; set => NoSectionContent[index] = value; }

        #endregion Public Indexers

        #region Public Methods

        public void Add(IIniSection item) => Sections.Add(item);

        public void Add(IIniKeyValuePair item) => NoSectionContent.Add(item);

        public void Clear() => Sections.Clear();

        public bool Contains(IIniSection item) => Sections.Contains(item);

        public bool Contains(IIniKeyValuePair item) => NoSectionContent.Contains(item);

        public void CopyTo(IIniSection[] array, int arrayIndex) => Sections.CopyTo(array, arrayIndex);

        public void CopyTo(IIniKeyValuePair[] array, int arrayIndex) => NoSectionContent.CopyTo(array, arrayIndex);



        public IEnumerator<IIniSection> GetEnumerator() => Sections.GetEnumerator();

        /// <summary>
        /// Get an <see cref="IIniKeyValuePair" /> from <see cref="NoSectionContent" />
        /// </summary>
        [Obsolete]
        public IIniKeyValuePair GetFromNoSectionContent(string key) => NoSectionContent.First(i => key.Equals(i.Key));

        public int IndexOf(IIniSection item) => Sections.IndexOf(item);

        public int IndexOf(IIniKeyValuePair item) => NoSectionContent.IndexOf(item);

        public void Insert(int index, IIniSection item) => Sections.Insert(index, item);

        public void Insert(int index, IIniKeyValuePair item) => NoSectionContent.Insert(index, item);

        public bool Remove(IIniSection item) => Sections.Remove(item);

        public bool Remove(IIniKeyValuePair item) => NoSectionContent.Remove(item);

        public void RemoveAt(int index) => Sections.RemoveAt(index);

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

        [Obsolete]
        public bool TryGetKey(string name, out IIniKeyValuePair keyValuePair)
        {
            keyValuePair = null;
            foreach (var item in NoSectionContent.Where(item => name.Equals(item.Key)))
            {
                keyValuePair = item;
                return true;
            }

            return false;
        }

        public IIniKeyValuePair TryGetKey(string name)
        {
            foreach (var item in NoSectionContent.Where(item => name.Equals(item.Key)))
                return item;

            return null;
        }
        [Obsolete]
        public bool TryGetSection(string name, out IIniSection section)
        {
            section = null;
            foreach (var item in Sections.Where(item => name.Equals(item.Name)))
            {
                section = item;
                return true;
            }

            return false;
        }

        public IIniSection TryGetSection(string name)
        {
            foreach (var item in Sections.Where(item => name.Equals(item.Name)))
                return item;

            return null;
        }

        IEnumerator IEnumerable.GetEnumerator() => Sections.GetEnumerator();

        IEnumerator<IIniKeyValuePair> IEnumerable<IIniKeyValuePair>.GetEnumerator() => NoSectionContent.GetEnumerator();

        #endregion Public Methods

    }
}
