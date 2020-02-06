using CommandLine;

namespace CodeMangler
{
    public class CommandLineOptions
    {
        [Option('i', "input", Required = false, HelpText = "Directory where to search for source files (Default is working dir)")]
        public string InputDir { get; set; }

        [Option('o', "out", Required = true, HelpText = "Output file name")]
        public string OutputFile { get; set; }
    }
}
