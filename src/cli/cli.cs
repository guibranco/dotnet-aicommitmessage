using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using Cli.Commands;

namespace Cli
{
    class Program
    {
        static int Main(string[] args)
        {
            var rootCommand = new RootCommand
            {
                new SetEnvCommand()
            };

            var builder = new CommandLineBuilder(rootCommand);
            var parser = builder.UseDefaults().Build();

            return parser.Invoke(args);
        }
    }
}
