using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shimakaze.Struct.Ini
{
    /// <summary>
    /// Ini Section Structure
    /// </summary>
    public struct IniSection
    {
        #region 用户属性
        public IniKeyValuePair[] Content { get; set; }

        /// <summary>
        /// Section Head
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Summary in Section Head Line
        /// </summary>
        public string Summary { get; set; }
        #endregion

        #region 构造方法
        public IniSection(string name, string summary, IniKeyValuePair[] content)
        {
            this.Name = name;
            this.Summary = summary;
            this.Content = content;
        }

        #endregion

        #region 访问器
        public IniKeyValuePair this[string key] => this.Content.First(i => key.Equals(i.Key));

        #endregion

        #region 用户方法
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

        #region Object重载
        public override bool Equals(object obj) => obj is IniSection section &&
                                   this.Name == section.Name &&
                   this.Summary == section.Summary &&
                   EqualityComparer<IniKeyValuePair[]>.Default.Equals(this.Content, section.Content);

        public override int GetHashCode()
        {
            int hashCode = 117231219;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.Summary);
            hashCode = hashCode * -1521134295 + EqualityComparer<IniKeyValuePair[]>.Default.GetHashCode(this.Content);
            return hashCode;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"[{this.Name}]");
            if (!string.IsNullOrEmpty(this.Summary))
                sb.Append($";{this.Summary}");
            sb.AppendLine();
            foreach (var item in this.Content)
                sb.AppendLine(item.ToString());
            return sb.ToString();
        }
        #endregion

        #region Try方法
        public bool TryGetKey(string name, out IniKeyValuePair? keyValuePair)
        {
            keyValuePair = null;
            foreach (var item in this.Content)
            {
                if (item.HasData && name.Equals(item.Key))
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
                if (item.HasData && name.Equals(item.Key))
                    return item;
            return null;
        }
        #endregion
    }
}
