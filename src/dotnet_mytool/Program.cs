using System;
using System.CommandLine;

namespace DotNetMyTool
{
    class Program
    {
        static int Main(string[] args)
        {
            var rootCommand = new RootCommand();

            var setEnvCommand = new Command("set-env", "Set an environment variable")
            {
                new Argument<string>("variable", "The environment variable in the format VAR_NAME=value"),
                new Option<string>("--target", () => "User", "The target scope: User or Machine")
            };

            rootCommand.AddCommand(setEnvCommand);

            return rootCommand.InvokeAsync(args).Result;
        }
    }
}
