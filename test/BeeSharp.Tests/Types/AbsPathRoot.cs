namespace BeeSharp.Tests.Types
{
    /// <summary>
    /// Root part of a file / directory path. Valid examples are C:\\ D:\\ or network shares
    /// like \\servername\
    /// </summary>
    public partial struct AbsPathRoot
    {
        public static AbsPathRoot New(string r)
        {

        }

        public static AbsPathRoot UncheckedNew(string s)
        {

        }

        private void CheckIsValid()
        {

        }

    }
}
