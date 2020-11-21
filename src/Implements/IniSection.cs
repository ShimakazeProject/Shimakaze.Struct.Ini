using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shimakaze.Struct.Ini.Implements
{
    /// <summary>
    /// Ini Section Structure
    /// </summary>
    public sealed class IniSection : IIniSection
    {
        #region Public Properties

        public IList<IIniKeyValuePair> Content { get; internal set; } = new List<IIniKeyValuePair>();

        public string Name { get; set; }

        public string Summary { get; set; }

        public int Count => Content.Count;

        bool ICollection<IIniKeyValuePair>.IsReadOnly => throw new NotImplementedException();

        #endregion Public Properties

        #region Public Constructors

        internal IniSection()
        {
        }

        internal IniSection(string name) : this() => Name = name ?? throw new ArgumentNullException(nameof(name));

        internal IniSection(string name, IEnumerable<IIniKeyValuePair> content) : this(name) => Content = new List<IIniKeyValuePair>(content) ?? throw new ArgumentNullException(nameof(content));

        internal IniSection(string name, IEnumerable<IIniKeyValuePair> content, string summary) : this(name, content) => Summary = summary ?? throw new ArgumentNullException(nameof(summary));

        [Obsolete]
        public IniSection(string name, string summary, IEnumerable<IIniKeyValuePair> content) : this(name, content, summary) { }

        #endregion Public Constructors

        #region Public Indexers

        public IIniKeyValuePair this[int index] { get => Content[index]; set => Content[index] = value; }
        public IIniKeyValuePair this[string key] => Content.First(i => key.Equals(i.Key));

        #endregion Public Indexers

        #region Public Methods

        public void Add(IIniKeyValuePair item)
        {
            if (Contains(item))
                throw new ArgumentException("元素已存在");
            Content.Add(item);
        }

        public void Clear() => Content.Clear();

        public bool Contains(IIniKeyValuePair item)
        {
            for (int i = 0; i < Content.Count; i++)
            {
                if (Content[i].GetHashCode() == item.GetHashCode())
                    return true;
            }
            return false;
        }

        public void CopyTo(IIniKeyValuePair[] array, int arrayIndex) => Content.CopyTo(array, arrayIndex);

        public IEnumerator<IIniKeyValuePair> GetEnumerator() => Content.GetEnumerator();

        public int IndexOf(IIniKeyValuePair item) => Content.IndexOf(item);

        public void Insert(int index, IIniKeyValuePair item)
        {
            if (Contains(item))
                throw new ArgumentException("元素已存在");
            Content.Insert(index, item);
        }

        public bool Remove(IIniKeyValuePair item) => Content.Remove(item);

        public void RemoveAt(int index) => Content.RemoveAt(index);

        public override string ToString()
        {
            var sb = new StringBuilder($"[{Name}]");
            if (!string.IsNullOrEmpty(Summary))
                sb.Append($"; {Summary}");
            sb.AppendLine();
            foreach (var item in Content)
                sb.AppendLine(item.ToString());
            return sb.ToString();
        }

        [Obsolete]
        public bool TryGetKey(string name, out IIniKeyValuePair keyValuePair)
        {
            keyValuePair = null;
            if (TryGetKey(name) is IIniKeyValuePair kvp)
            {
                keyValuePair = kvp;
                return true;
            }
            return false;
        }

        public IIniKeyValuePair TryGetKey(string name)
        {
            foreach (var item in Content)
                if (item.HasData && name.Equals(item.Key))
                    return item;
            return null;
        }

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Content).GetEnumerator();

        #endregion Public Methods
    }
}
