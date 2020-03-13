﻿namespace BeeSharp.Types
{
    public partial struct AbsDirPath
    {
        private const char AltSepChar = '/';
        private const char SepChar = '\\';
        private const string SepString = "\\";

        public readonly CIString value;

        private AbsDirPath(string s) => this.value = CIString.UncheckedNew(s);
    }
}
