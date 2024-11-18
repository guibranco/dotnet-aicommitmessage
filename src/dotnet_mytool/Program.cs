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
            setEnvCommand.Handler = CommandHandler.Create<string, string>((variable, target) =>
            {
                var parts = variable.Split('=');
                if (parts.Length != 2)

        static void SetEnvironmentVariable(string variable, string value, string target)
        {
            EnvironmentVariableTarget envTarget = target.Equals("Machine", StringComparison.OrdinalIgnoreCase)
                ? EnvironmentVariableTarget.Machine
                : EnvironmentVariableTarget.User;

            Environment.SetEnvironmentVariable(variable, value, envTarget);
            Console.WriteLine($"Environment variable {variable} set to {value} for {target}.");
        }
                {
                {
                    Console.WriteLine("Invalid format. Use VAR_NAME=value.");
                    return;
                }
                }

            rootCommand.AddCommand(setEnvCommand);

            return rootCommand.InvokeAsync(args).Result;
        }
    }
}
