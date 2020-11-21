using System.Collections.Generic;

namespace Shimakaze.Struct.Ini
{
    public interface IIniSection : IList<IIniKeyValuePair>
    {
        /// <summary>
        /// Get an <see cref="IIniKeyValuePair" /> from <see cref="Content" />
        /// </summary>
        IIniKeyValuePair this[string key] { get; }
        /// <summary>
        /// All <see cref="IIniKeyValuePair" /> s on this <see cref="IIniSection" />
        /// </summary>
        IList<IIniKeyValuePair> Content { get; }
        /// <summary>
        /// Section Head
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Summary in Section Head Line
        /// </summary>
        string Summary { get; set; }

        IIniKeyValuePair TryGetKey(string name);
    }
    public interface IReadOnlyIniSection : IReadOnlyList<IReadOnlyIniKeyValuePair>
    {
        /// <summary>
        /// Get an <see cref="IIniKeyValuePair" /> from <see cref="Content" />
        /// </summary>
        IReadOnlyIniKeyValuePair this[string key] { get; }
        /// <summary>
        /// All <see cref="IIniKeyValuePair" /> s on this <see cref="IIniSection" />
        /// </summary>
        IReadOnlyList<IReadOnlyIniKeyValuePair> Content { get; }
        /// <summary>
        /// Section Head
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Summary in Section Head Line
        /// </summary>
        string Summary { get; }

        IReadOnlyIniKeyValuePair TryGetKey(string name);
    }
}