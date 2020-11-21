using System;
using System.IO;
using System.Threading.Tasks;

namespace Shimakaze.Struct.Ini
{
    public interface IIniKeyValuePair : IEquatable<IIniKeyValuePair>
    {
        bool HasData { get; }
        bool HasSummary { get; }
        string Key { get; set; }
        string Summary { get; set; }
        IniValue Value { get; set; }
        int GetHashCode();
    }
    public interface IReadOnlyIniKeyValuePair : IEquatable<IReadOnlyIniKeyValuePair>
    {
        bool HasData { get; }
        bool HasSummary { get; }
        string Key { get; }
        string Summary { get; }
        IniValue Value { get; }
        int GetHashCode();
    }
}