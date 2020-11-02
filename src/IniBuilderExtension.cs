using System.Linq;

namespace Shimakaze.Struct.Ini
{
    public static class IniBuilderExtension
    {
        public static IniSection ToSection(this IniSectionBuilder @this) => new IniSection(@this.Name, @this.Summary, @this.Content.ToArray());

        public static IniDocument ToDocument(this IniBuilder @this) => new IniDocument
        {
            NoSectionContent = @this.NoSectionContent.ToArray(),
            Sections = @this.Sections.Select(IniBuilderExtension.ToSection).ToArray()
        };
    }
}
