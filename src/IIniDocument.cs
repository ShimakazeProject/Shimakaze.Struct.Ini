using System.Collections.Generic;

namespace Shimakaze.Struct.Ini
{
    public interface IIniDocument: IList<IIniSection>, IList<IIniKeyValuePair>
    {
        /// <summary>
        /// Get an <see cref="IIniSection" /> from <see cref="Sections" />
        /// </summary>
        IIniSection this[string sectionName] { get; }

        /// <summary>
        /// Here <see cref="IIniKeyValuePair" /> s are Independent of <see cref="Sections" />
        /// </summary>
        IList<IIniKeyValuePair> NoSectionContent { get; set; }
        /// <summary>
        /// All <see cref="IIniSection" /> s on this <see cref="IIniDocument" />
        /// </summary>
        IList<IIniSection> Sections { get; set; }

        IIniSection TryGetSection(string name);
        IIniKeyValuePair TryGetKey(string name);
    }
    public interface IReadOnlyIniDocument: IReadOnlyList<IReadOnlyIniSection>, IReadOnlyList<IReadOnlyIniKeyValuePair>
    {
        /// <summary>
        /// Get an <see cref="IIniSection" /> from <see cref="Sections" />
        /// </summary>
        IReadOnlyIniSection this[string sectionName] { get; }

        /// <summary>
        /// Here <see cref="IIniKeyValuePair" /> s are Independent of <see cref="Sections" />
        /// </summary>
        IReadOnlyList<IReadOnlyIniKeyValuePair> NoSectionContent { get; set; }
        /// <summary>
        /// All <see cref="IIniSection" /> s on this <see cref="IIniDocument" />
        /// </summary>
        IReadOnlyList<IReadOnlyIniSection> Sections { get; set; }

        IReadOnlyIniSection TryGetSection(string name);
        IReadOnlyIniKeyValuePair TryGetKey(string name);
    }
}