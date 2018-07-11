/*
Copyright © 2018 Jean Pascal Bellot

This file is part of Tiles.

Tiles is free software: you can redistribute it and/or modify it under the terms
of the GNU General Public License as published by the Free Software Foundation,
either version 3 of the License, or (at your option) any later version.

Tiles is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A
PARTICULAR PURPOSE.  See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with
Tiles.  If not, see http://www.gnu.org/licenses/
*/

namespace TileEngine.Loaders
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    [Flags]
    enum IniFileOptions
    {
        None = 0,
        StripComments = 1,
        EscapeLineFeeds = 2,
        CaseSensitive = 4,
        StripQuotes = 8,
        FormatSettingsActive = 16,
        StripInvalid = 32
    }

    class IniFile
    {
        private const char Separator = '=';
        private const char Comment = ';';
        private const char Comment2 = '#';
        private const char LF_Escape = '\\';

        private IniFileSectionList sectionList;
        private IniFileOptions options;
        private string include;

        private List<string> sections;

        public IniFile(Stream stream)
            : this(stream, IniFileOptions.StripComments)
        {
        }
        public IniFile(Stream stream, IniFileOptions options)
        {
            sectionList = new IniFileSectionList();
            this.options = options;

            TextReader ts = new StreamReader(stream);
            List<string> strings = new List<string>();
            string sLine = ts.ReadLine();
            while (sLine != null)
            {
                strings.Add(sLine);
                sLine = ts.ReadLine();
            }
            FillSectionList(strings);
            FillInfos();
        }

        public string Include
        {
            get { return include; }
        }

        public IList<string> SectionNames
        {
            get { return sections; }
        }

        public IList<IniFileSection> Sections
        {
            get
            {
                return sectionList;
            }
        }
        public IniFileOptions Options
        {
            get { return options; }
            set { options = value; }
        }

        public bool CaseSensitive
        {
            get { return (options & IniFileOptions.CaseSensitive) == IniFileOptions.CaseSensitive; }
            set
            {
                if (value) { options |= IniFileOptions.CaseSensitive; }
                else { options &= ~IniFileOptions.CaseSensitive; }
            }
        }

        public bool EscapeLineFeeds
        {
            get { return (options & IniFileOptions.EscapeLineFeeds) == IniFileOptions.EscapeLineFeeds; }
            set
            {
                if (value) { options |= IniFileOptions.EscapeLineFeeds; }
                else { options &= ~IniFileOptions.EscapeLineFeeds; }
            }
        }

        public bool StripQuotes
        {
            get { return (options & IniFileOptions.StripQuotes) == IniFileOptions.StripQuotes; }
            set
            {
                if (value) { options |= IniFileOptions.StripQuotes; }
                else { options &= ~IniFileOptions.StripQuotes; }
            }
        }

        public bool StripComments
        {
            get { return (options & IniFileOptions.StripComments) == IniFileOptions.StripComments; }
            set
            {
                if (value) { options |= IniFileOptions.StripComments; }
                else { options &= ~IniFileOptions.StripComments; }
            }
        }

        public bool StripInvalid
        {
            get { return (options & IniFileOptions.StripInvalid) == IniFileOptions.StripInvalid; }
            set
            {
                if (value) { options |= IniFileOptions.StripInvalid; }
                else { options &= ~IniFileOptions.StripInvalid; }
            }
        }

        public bool SectionExists(string name)
        {
            IniFileSection s = sectionList.SectionByName(name, CaseSensitive);
            return s != null && !s.Empty;
        }

        public int ReadInt(string section, string ident, int def = 0)
        {
            int res;
            if (int.TryParse(ReadString(section, ident, ""), out res)) { return res; }
            return def;
        }

        public bool ReadBool(string section, string ident, bool def = false)
        {
            bool res;
            if (bool.TryParse(ReadString(section, ident, ""), out res)) { return res; }
            return def;
        }

        public double ReadDouble(string section, string ident, double def = 0)
        {
            double res;
            if (double.TryParse(ReadString(section, ident, ""), out res)) { return res; }
            return def;
        }

        public void WriteInt(string section, string ident, int value)
        {
            WriteString(section, ident, value.ToString());
        }

        public void ReadSectionValues(string section, List<string> strings)
        {
            bool includeComments = !StripComments;
            bool includeInvalid = !StripInvalid;
            bool keyIsComment = false;
            bool doStripQuotes = StripQuotes;
            strings.Clear();
            IniFileSection oSection = sectionList.SectionByName(section, CaseSensitive);
            if (oSection == null) return;
            for (int i = 0; i < oSection.KeyList.Count; i++)
            {
                string s = null;
                IniFileKey k = oSection.KeyList[i];
                if (includeInvalid || !string.IsNullOrEmpty(k.Ident))
                {
                    s = k.Value;
                    keyIsComment = IsComment(k.Ident);
                    if (includeComments || !keyIsComment)
                    {
                        if (doStripQuotes)
                        {
                            int j = s.Length;
                            if (j > 1)
                            {
                                s = s.Trim('\'', '"');
                            }
                        }
                        if (keyIsComment)
                        {
                            s = k.Ident;
                        }
                        else if (!string.IsNullOrEmpty(k.Ident))
                        {
                            s = k.Ident + Separator + s;
                        }
                        strings.Add(s);
                    }
                }
            }
        }

        public bool ValueExists(string section, string ident)
        {
            bool result = false;
            IniFileSection oSection = sectionList.SectionByName(section, CaseSensitive);
            if (oSection != null)
            {
                result = oSection.KeyList.KeyByName(ident, CaseSensitive) != null;
            }
            return result;
        }

        public void AddIni(IniFile other)
        {
            foreach (var sec in other.sectionList)
            {
                var exSec = sectionList.SectionByName(sec.Name, true);
                if (exSec != null)
                {
                    foreach (var v in sec.KeyList)
                    {
                        exSec.KeyList.Add(v);
                    }
                }
                else
                {
                    sectionList.Add(sec);
                    sections.Add(sec.Name);
                }
            }
        }

        private void FillSectionList(List<string> strings)
        {

            IniFileSection oSection = null;
            sectionList.Clear();
            for (int i = 0; i < strings.Count; i++)
            {
                string sLine = strings[i].Trim();
                if (!string.IsNullOrEmpty(sLine))
                {
                    if (IsInclude(sLine))
                    {
                        include = sLine.Replace("INCLUDE ", "");
                        continue;
                    }
                    if (IsComment(sLine) && oSection == null)
                    {
                        if (!StripComments)
                        {
                            oSection = new IniFileSection(sLine);
                            sectionList.Add(oSection);
                        }
                        continue;
                    }
                    if (sLine.StartsWith("[") && sLine.EndsWith("]"))
                    {
                        oSection = new IniFileSection(sLine.Substring(1, sLine.Length - 2));
                        sectionList.Add(oSection);
                    }
                    else if (oSection != null)
                    {
                        bool addKey = false;
                        string sIdent = "";
                        string sValue = "";
                        if (IsComment(sLine))
                        {
                            addKey = !StripComments;
                            sIdent = sLine;
                            sValue = "";
                        }
                        else
                        {
                            int j = sLine.IndexOf(Separator);
                            if (j < 0)
                            {
                                addKey = !StripInvalid;
                                sIdent = "";
                                sValue = sLine;
                            }
                            else if (j < 1)
                            {
                                addKey = !StripInvalid;
                                sIdent = "";
                                sValue = sLine.Substring(1);
                            }
                            else
                            {
                                addKey = true;
                                sIdent = sLine.Substring(0, j).Trim();
                                sValue = sLine.Substring(j + 1).Trim();
                            }
                        }
                        if (addKey)
                        {
                            oSection.KeyList.Add(new IniFileKey(sIdent, sValue));
                        }
                    }
                    else
                    {
                        oSection = new IniFileSection("");
                        sectionList.Add(oSection);
                        i--;
                    }
                }
            }
        }

        private static bool IsComment(string s)
        {
            if (!string.IsNullOrEmpty(s)) return (s[0] == Comment || s[0] == Comment2);
            return false;
        }

        private static bool IsInclude(string s)
        {
            if (!string.IsNullOrEmpty(s)) return (s.StartsWith("INCLUDE ", StringComparison.Ordinal));
            return false;
        }

        private void FillInfos()
        {
            sections = new List<string>();
            ReadSections(sections);
        }

        private void DeleteSection(IniFileSection section)
        {
            sectionList.Remove(section);
        }

        private void MaybeDeleteSection(IniFileSection section)
        {
            if (section.Empty) DeleteSection(section);
        }

        private void MaybeUpdateFile()
        {

        }

        public void ReadSection(string section, IList<string> strings)
        {
            strings.Clear();
            IniFileSection oSection = sectionList.SectionByName(section, CaseSensitive);
            if (oSection != null)
            {
                foreach (var item in oSection.KeyList)
                {
                    if (!IsComment(item.Ident))
                    {
                        strings.Add(item.Ident);
                    }
                }
            }
        }

        public void ReadSectionRaw(string section, IList<string> strings)
        {
            strings.Clear();
            IniFileSection oSection = sectionList.SectionByName(section, CaseSensitive);
            if (oSection != null)
            {
                foreach (var item in oSection.KeyList)
                {
                    if (!IsComment(item.Ident))
                    {
                        if (!string.IsNullOrEmpty(item.Ident))
                        {
                            strings.Add(item.Ident + Separator + item.Value);
                        }
                        else
                        {
                            strings.Add(item.Value);
                        }

                    }
                }
            }
        }

        public string ReadString(string section, string ident, string def = "")
        {
            string result = def;
            IniFileSection oSection = sectionList.SectionByName(section, CaseSensitive);
            if (oSection != null)
            {
                IniFileKey oKey = oSection.KeyList.KeyByName(ident, CaseSensitive);
                if (oKey != null)
                {
                    if (StripQuotes)
                    {
                        int j = oKey.Value.Length;
                        if (j > 1)
                        {
                            result = oKey.Value.Trim('\'', '"');
                        }
                        else result = oKey.Value;
                    }
                    else result = oKey.Value;
                }
            }
            return result;
        }

        public void WriteString(string section, string ident, string value)
        {
            if (!string.IsNullOrEmpty(section) && !string.IsNullOrEmpty(ident))
            {
                IniFileSection oSection = sectionList.SectionByName(section, CaseSensitive);
                if (oSection == null)
                {
                    oSection = new IniFileSection(section);
                    sectionList.Add(oSection);
                }
                IniFileKey oKey = oSection.KeyList.KeyByName(ident, CaseSensitive);
                if (oKey != null)
                {
                    oKey.Value = value;
                }
                else
                {
                    oSection.KeyList.Add(new IniFileKey(ident, value));
                }
            }
            MaybeUpdateFile();
        }

        public void ReadSections(IList<string> strings)
        {
            strings.Clear();
            foreach (var sec in sectionList)
            {
                if (!IsComment(sec.Name))
                {
                    strings.Add(sec.Name);
                }
            }
        }

        public void EraseSection(string section)
        {
            IniFileSection oSection = sectionList.SectionByName(section, CaseSensitive);
            if (oSection != null)
            {
                DeleteSection(oSection);
                MaybeUpdateFile();
            }
        }

        public class IniFileKey
        {
            private string ident;
            private string val;
            public IniFileKey(string ident, string val)
            {
                this.ident = ident;
                this.val = val;
            }

            public string Ident { get { return ident; } set { ident = value; } }
            public string Value { get { return val; } set { val = value; } }
            public override string ToString()
            {
                return ident + "=" + val;
            }
        }

        public class IniFileKeyList : List<IniFileKey>
        {
            public IniFileKey KeyByName(string name, bool caseSensitive)
            {
                StringComparison sc = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
                foreach (var key in this)
                {
                    if (key.Ident.Equals(name, sc))
                    {
                        return key;
                    }
                }
                return null;
            }
        }

        public class IniFileSection
        {
            private string name;
            private IniFileKeyList keyList;
            public IniFileSection(string name)
            {
                this.name = name;
                keyList = new IniFileKeyList();
            }

            public bool Empty
            {
                get
                {
                    bool result = true;
                    int i = 0;
                    while (result && i < keyList.Count)
                    {
                        result = IsComment(keyList[i].Ident);
                        i++;
                    }
                    return result;
                }
            }

            public string Name { get { return name; } }
            public IniFileKeyList KeyList { get { return keyList; } }

            public string ReadString(string ident, string def = "")
            {
                string result = def;
                IniFileKey oKey = KeyList.KeyByName(ident, true);
                if (oKey != null)
                {
                    int j = oKey.Value.Length;
                    if (j > 1)
                    {
                        result = oKey.Value.Trim('\'', '"');
                    }
                    else result = oKey.Value;
                }
                return result;
            }
            public int ReadInt(string ident, int def = 0)
            {
                int res;
                if (int.TryParse(ReadString(ident, ""), out res)) { return res; }
                return def;
            }

            public double ReadDouble(string ident, double def = 0)
            {
                double res;
                if (double.TryParse(ReadString(ident, ""), out res)) { return res; }
                return def;
            }

        }

        class IniFileSectionList : List<IniFileSection>
        {
            public IniFileSection SectionByName(string name, bool caseSensitive)
            {
                StringComparison sc = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
                foreach (var sec in this)
                {
                    if (sec.Name.Equals(name, sc))
                    {
                        return sec;
                    }
                }
                return null;
            }
        }

    }
}
