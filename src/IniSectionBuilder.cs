using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shimakaze.Struct.Ini
{
    public class IniSectionBuilder : IList<IniKeyValuePair>
    {
        #region 用户属性
        public List<IniKeyValuePair> Content { get; }

        /// <summary>
        /// Section Head
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Summary in Section Head Line
        /// </summary>
        public string Summary { get; set; }
        #endregion

        #region List属性
        public int Count => this.Content.Count;

        bool ICollection<IniKeyValuePair>.IsReadOnly => false;
        #endregion

        #region 访问器
        public IniKeyValuePair this[int index] { get => (this.Content)[index]; set => (this.Content)[index] = value; }
        public IniKeyValuePair this[string key] => this.Content.First(i => i.Key.Equals(key));
        #endregion

        #region 构造方法
        public IniSectionBuilder() => this.Content = new List<IniKeyValuePair>();
        public IniSectionBuilder(string name) => this.Name = name;
        public IniSectionBuilder(string name, string summary) : this(name) => this.Summary = summary;
        public IniSectionBuilder(string name, string summary, IEnumerable<IniKeyValuePair> content) : this(name, summary)
            => this.Content = content is List<IniKeyValuePair> list ? list : new List<IniKeyValuePair>(content);

        public IniSectionBuilder(IniSection section) : this(section.Name, section.Summary, section.Content) { }

        #endregion

        #region 反解析方法
        public async Task DepraseAsync(TextWriter writer)
        {
            await writer.WriteAsync($"[{this.Name}]");
            if (!string.IsNullOrEmpty(this.Summary))
                await writer.WriteAsync($";{this.Summary}");
            await writer.WriteLineAsync();
            foreach (var item in this.Content)
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
            foreach (var item in this.Content)
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
            foreach (var item in this.Content)
                if (item.HasData && item.Key.Equals(name))
                    return item;
            return null;
        }
        #endregion

        #region List方法
        public void Add(IniKeyValuePair keyValuePair) => this.Content.Add(keyValuePair);
        public void Clear() => this.Content.Clear();

        public bool Contains(IniKeyValuePair item) => this.Content.Contains(item);

        public void CopyTo(IniKeyValuePair[] array, int arrayIndex) => this.Content.CopyTo(array, arrayIndex);

        public IEnumerator<IniKeyValuePair> GetEnumerator() => this.Content.GetEnumerator();

        public int IndexOf(IniKeyValuePair item) => this.Content.IndexOf(item);
        public void Insert(int index, IniKeyValuePair item) => this.Content.Insert(index, item);
        public bool Remove(IniKeyValuePair item) => this.Content.Remove(item);

        public void RemoveAt(int index) => this.Content.RemoveAt(index);
        IEnumerator IEnumerable.GetEnumerator() => this.Content.GetEnumerator();
        #endregion
    }
}
