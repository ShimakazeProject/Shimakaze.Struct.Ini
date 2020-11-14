using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shimakaze.Struct.Ini
{
    /// <summary>
    /// Ini Section Structure
    /// </summary>
    public sealed class IniSection : IList<IniKeyValuePair>
    {
        #region Public Properties

        public List<IniKeyValuePair> Content { get; internal set; } = new List<IniKeyValuePair>();

        /// <summary>
        /// Section Head
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Summary in Section Head Line
        /// </summary>
        public string Summary { get; set; }

        public int Count => Content.Count;

        bool ICollection<IniKeyValuePair>.IsReadOnly => throw new NotImplementedException();

        #endregion Public Properties

        #region Public Constructors

        public IniSection()
        {
        }

        public IniSection(string name) : this() => Name = name ?? throw new ArgumentNullException(nameof(name));

        public IniSection(string name, IEnumerable<IniKeyValuePair> content) : this(name) => Content = new List<IniKeyValuePair>(content) ?? throw new ArgumentNullException(nameof(content));

        public IniSection(string name, IEnumerable<IniKeyValuePair> content, string summary) : this(name, content) => Summary = summary ?? throw new ArgumentNullException(nameof(summary));

        [Obsolete]
        public IniSection(string name, string summary, IEnumerable<IniKeyValuePair> content) : this(name, content, summary) { }

        #endregion Public Constructors

        #region Public Indexers

        public IniKeyValuePair this[int index] { get => Content[index]; set => Content[index] = value; }
        public IniKeyValuePair this[string key] => Content.First(i => key.Equals(i.Key));

        #endregion Public Indexers

        #region Public Methods

        public void Add(IniKeyValuePair item) => ((ICollection<IniKeyValuePair>)Content).Add(item);

        public void Clear() => ((ICollection<IniKeyValuePair>)Content).Clear();

        public bool Contains(IniKeyValuePair item) => ((ICollection<IniKeyValuePair>)Content).Contains(item);

        public void CopyTo(IniKeyValuePair[] array, int arrayIndex) => ((ICollection<IniKeyValuePair>)Content).CopyTo(array, arrayIndex);

        public async Task DepraseAsync(TextWriter writer)
        {
            await writer.WriteAsync($"[{Name}]");
            if (!string.IsNullOrEmpty(Summary))
                await writer.WriteAsync($"; {Summary}");
            await writer.WriteLineAsync();
            foreach (var item in Content)
            {
                await item.DepraseAsync(writer);
                await writer.WriteLineAsync();
            }
        }

        public IEnumerator<IniKeyValuePair> GetEnumerator() => ((IEnumerable<IniKeyValuePair>)Content).GetEnumerator();

        public int IndexOf(IniKeyValuePair item) => ((IList<IniKeyValuePair>)Content).IndexOf(item);

        public void Insert(int index, IniKeyValuePair item) => ((IList<IniKeyValuePair>)Content).Insert(index, item);

        public bool Remove(IniKeyValuePair item) => ((ICollection<IniKeyValuePair>)Content).Remove(item);

        public void RemoveAt(int index) => ((IList<IniKeyValuePair>)Content).RemoveAt(index);

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
        public bool TryGetKey(string name, out IniKeyValuePair keyValuePair)
        {
            keyValuePair = null;
            if (TryGetKey(name) is IniKeyValuePair kvp)
            {
                keyValuePair = kvp;
                return true;
            }
            return false;
        }

        public IniKeyValuePair TryGetKey(string name)
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
