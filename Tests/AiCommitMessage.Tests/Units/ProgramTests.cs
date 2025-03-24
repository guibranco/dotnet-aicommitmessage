using System.Diagnostics.CodeAnalysis;
using AiCommitMessage.Options;
using AiCommitMessage.Services;
using AiCommitMessage.Utility;
using CommandLine;


using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;


namespace AiCommitMessage.Tests.Units;


public class ProgramTests
{
    [Fact]
    public void Run_TestingMain()
    {
        // Arrange
        var installHookOptions = new InstallHookOptions { Path = "/hooks", Override = true };
        //var mockInstallHookService = new Mock<InstallHookOptions>();
        //InstallHookService.Instance = mockInstallHookService.Object;

        var options = "set-settings -p LmStudio".Split(" ");

        // Act
        Program.Main(options);

        // Assert
        /*mockInstallHookService.Verify(
            s => s.InstallHook(It.Is<InstallHookOptions>(o => o.Path == "/hooks" && o.Override)),
            Times.Once
        );*/
    }
    
    
    [Fact]
    public void Run_WithInstallHookOptions_CallsInstallHookService()
    {
        // Arrange
        var installHookOptions = new InstallHookOptions { Path = "/hooks", Override = true };
        //var mockInstallHookService = new Mock<InstallHookOptions>();
        //InstallHookService.Instance = mockInstallHookService.Object;

        // Act
        Program.Run(installHookOptions);

        // Assert
        /*mockInstallHookService.Verify(
            s => s.InstallHook(It.Is<InstallHookOptions>(o => o.Path == "/hooks" && o.Override)),
            Times.Once
        );*/
    }

    /*
    [Fact]
    public void Run_WithGenerateCommitMessageOptions_CallsGenerateCommitMessageService()
    {
        // Arrange
        var generateCommitMessageOptions = new GenerateCommitMessageOptions
        {
            Message = "Initial commit",
            Branch = "main",
            Diff = "diff content",
            Debug = true
        };
        var mockGenerateCommitMessageService = new Mock<IGenerateCommitMessageService>();
        mockGenerateCommitMessageService
            .Setup(s => s.GenerateCommitMessage(It.IsAny<GenerateCommitMessageOptions>()))
            .Returns("Generated commit message");
        GenerateCommitMessageService.Instance = mockGenerateCommitMessageService.Object;

        // Act
        Program.Run(generateCommitMessageOptions);

        // Assert
        mockGenerateCommitMessageService.Verify(
            s => s.GenerateCommitMessage(It.Is<GenerateCommitMessageOptions>(o =>
                o.Message == "Initial commit" &&
                o.Branch == "main" &&
                o.Diff == "diff content" &&
                o.Debug)),
            Times.Once
        );
    }

    [Fact]
    public void Run_WithSetSettingsOptions_CallsSettingsService()
    {
        // Arrange
        var setSettingsOptions = new SetSettingsOptions
        {
            Provider = ProviderEnum.LMStudio,
            Url = "http://localhost:1234",
            Key = "api-key",
            Model = "Llama-2",
            Target = "development",
            SaveEncrypted = true
        };
        var mockSettingsService = new Mock<ISettingsService>();
        SettingsService.Instance = mockSettingsService.Object;

        // Act
        Program.Run(setSettingsOptions);

        // Assert
        mockSettingsService.Verify(
            s => s.SetSettings(It.Is<SetSettingsOptions>(o =>
                o.Provider == ProviderEnum.LMStudio &&
                o.Url == "http://localhost:1234" &&
                o.Key == "api-key" &&
                o.Model == "Llama-2" &&
                o.Target == "development" &&
                o.SaveEncrypted)),
            Times.Once
        );
    }

    [Fact]
    public void HandleErrors_WithHelpRequested_DoesNotOutputError()
    {
        // Arrange
        var errors = new List<Error> { new HelpRequestedError() };
        var mockOutput = new Mock<IOutput>();
        Output.Instance = mockOutput.Object;

        // Act
        Program.HandleErrors(errors);

        // Assert
        mockOutput.Verify(o => o.ErrorLine(It.IsAny<string>()), Times.Never);
        Assert.Equal(0, Environment.ExitCode);
    }

    [Fact]
    public void HandleErrors_WithVersionRequested_DoesNotOutputError()
    {
        // Arrange
        var errors = new List<Error> { new VersionRequestedError() };
        var mockOutput = new Mock<IOutput>();
        Output.Instance = mockOutput.Object;

        // Act
        Program.HandleErrors(errors);

        // Assert
        mockOutput.Verify(o => o.ErrorLine(It.IsAny<string>()), Times.Never);
        Assert.Equal(0, Environment.ExitCode);
    }

    [Fact]
    public void HandleErrors_WithOtherErrors_OutputsError()
    {
        // Arrange
        var errors = new List<Error> { new NamedError() };
        var mockOutput = new Mock<IOutput>();
        Output.Instance = mockOutput.Object;

        // Act
        Program.HandleErrors(errors);

        // Assert
        mockOutput.Verify(o => o.ErrorLine("Invalid command-line arguments."), Times.Once);
        Assert.Equal(2, Environment.ExitCode);
    }

    [Fact]
    public void Run_WithInvalidOptions_OutputsError()
    {
        // Arrange
        var invalidOptions = new object();
        var mockOutput = new Mock<IOutput>();
        Output.Instance = mockOutput.Object;

        // Act
        Program.Run(invalidOptions);

        // Assert
        mockOutput.Verify(o => o.ErrorLine("Invalid command-line arguments."), Times.Once);
        Assert.Equal(1, Environment.ExitCode);
    }*/
}
