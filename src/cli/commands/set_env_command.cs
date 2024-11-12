using System;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace Cli.Commands
{
    public class SetEnvCommand : Command
    {
        public SetEnvCommand()
            : base("set-env", "Sets an environment variable.")
        {
        {
            var variableArgument = new Argument<string>("variable", "The environment variable in the format VAR_NAME=value.");
            var targetOption = new Option<string>("--target", () => "User", "The target scope: User or Machine.");

            AddArgument(variableArgument);
            AddOption(targetOption);

            Handler = CommandHandler.Create<string, string>((variable, target) =>
            {
                var parts = variable.Split('=');
                if (parts.Length != 2)
                {
                    Console.WriteLine("Invalid format. Use VAR_NAME=value.");
                    return;
                }

                var varName = parts[0];
                var varValue = parts[1];
                var targetScope = target.Equals("Machine", StringComparison.OrdinalIgnoreCase) ? EnvironmentVariableTarget.Machine : EnvironmentVariableTarget.User;

                try
                {
                    Environment.SetEnvironmentVariable(varName, varValue, targetScope);
                    Console.WriteLine($"Set {varName}={varValue} for {targetScope}.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error setting environment variable: {ex.Message}");
                }
            });
        }
    }
}
