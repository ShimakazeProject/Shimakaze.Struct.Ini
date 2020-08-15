using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Shimakaze.Struct.Ini.Helper
{
    public static class IniDocumentHelper
    {
        public static IniDocument Parse(string s)
        {
            var sr = new StringReader(s);
            var data = new List<IniSection>();
            IniDocument document = new IniDocument();
            IniSection? lastSection = null;
            var lastSectionContent = new List<IniKeyValuePair>();
            while (sr.Peek() > 0)
            {
                var line = sr.ReadLine();
                if (line.TrimStart()[0].Equals("["))
                {
                    SaveSection();
                    lastSectionContent.Clear();
                    int? summarySeparatorIndex = null;
                    if (s.Contains(";")) summarySeparatorIndex = s.IndexOf(';');

                    var tmpSection = new IniSection { Name = line.TrimStart().Substring(1, line.IndexOf(']')) };
                    if (summarySeparatorIndex.HasValue) tmpSection.Summary = line.Substring(summarySeparatorIndex.Value);
                    lastSection = tmpSection;
                }
                lastSectionContent.Add(IniKeyValuePair.Parse(line));
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

            }
        }
        public static async Task<IniDocument> ParseAsync(string s)
        {
            var sr = new StringReader(s);
            var data = new List<IniSection>();
            IniDocument document = new IniDocument();
            IniSection? lastSection = null;
            var lastSectionContent = new List<IniKeyValuePair>();
            while (sr.Peek() > 0)
            {
                var line = await sr.ReadLineAsync();
                if (line.TrimStart()[0].Equals("["))
                {
                    SaveSection();
                    lastSectionContent.Clear();
                    int? summarySeparatorIndex = null;
                    if (line.Contains(";")) summarySeparatorIndex = line.IndexOf(';');

                    var tmpSection = new IniSection { Name = line.TrimStart().Substring(1, line.IndexOf(']')) };
                    if (summarySeparatorIndex.HasValue) tmpSection.Summary = line.Substring(summarySeparatorIndex.Value);
                    lastSection = tmpSection;
                }
                lastSectionContent.Add(IniKeyValuePair.Parse(line));
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

            }
        }


        public static IniDocument Parse(Stream s)
        {
            var sr = new StreamReader(s);
            var data = new List<IniSection>();
            IniDocument document = new IniDocument();
            IniSection? lastSection = null;
            var lastSectionContent = new List<IniKeyValuePair>();
            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();
                if (line.TrimStart()[0].Equals("["))
                {
                    SaveSection();
                    lastSectionContent.Clear();
                    int? summarySeparatorIndex = null;
                    if (line.Contains(";")) summarySeparatorIndex = line.IndexOf(';');

                    var tmpSection = new IniSection { Name = line.TrimStart().Substring(1, line.IndexOf(']')) };
                    if (summarySeparatorIndex.HasValue) tmpSection.Summary = line.Substring(summarySeparatorIndex.Value);
                    lastSection = tmpSection;
                }
                lastSectionContent.Add(IniKeyValuePair.Parse(line));
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

            }
        }
        public static async Task<IniDocument> ParseAsync(Stream s)
        {
            var sr = new StreamReader(s);
            var data = new List<IniSection>();
            IniDocument document = new IniDocument();
            IniSection? lastSection = null;
            var lastSectionContent = new List<IniKeyValuePair>();
            while (!sr.EndOfStream)
            {
                var line = await sr.ReadLineAsync();
                if (line.TrimStart()[0].Equals("["))
                {
                    SaveSection();
                    lastSectionContent.Clear();
                    int? summarySeparatorIndex = null;
                    if (line.Contains(";")) summarySeparatorIndex = line.IndexOf(';');

                    var tmpSection = new IniSection { Name = line.TrimStart().Substring(1, line.IndexOf(']')) };
                    if (summarySeparatorIndex.HasValue) tmpSection.Summary = line.Substring(summarySeparatorIndex.Value);
                    lastSection = tmpSection;
                }
                lastSectionContent.Add(IniKeyValuePair.Parse(line));
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

            }
        }

    }
}
