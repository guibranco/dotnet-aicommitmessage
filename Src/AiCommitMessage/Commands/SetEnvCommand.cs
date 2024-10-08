using System;
using Microsoft.Win32;

namespace AiCommitMessage.Commands
{
    public class SetEnvCommand
    {
        public static void Execute(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: set-env VAR_NAME=value [--target User|Machine]");
                return;
            }

            string[] variableParts = args[0].Split('=');
            if (variableParts.Length != 2)
            {
                Console.WriteLine("Invalid format. Please use VAR_NAME=value.");
                return;
            }

            string variableName = variableParts[0];
            string variableValue = variableParts[1];
            string target = args.Length > 1 && args[1].StartsWith("--target") ? args[1].Split('=')[1] : "User";

            SetEnvironmentVariable(variableName, variableValue, target);
        }

        private static void SetEnvironmentVariable(string variable, string value, string target = "User")
        {
            EnvironmentVariableTarget envTarget;

            if (string.Equals(target, "Machine", StringComparison.OrdinalIgnoreCase))
            {
                envTarget = EnvironmentVariableTarget.Machine;
            }
            else
            {
                envTarget = EnvironmentVariableTarget.User;
            }

            Environment.SetEnvironmentVariable(variable, value, envTarget);
            Console.WriteLine($"Environment variable '{variable}' set to '{value}' for {envTarget}.");
        }
    }
}
