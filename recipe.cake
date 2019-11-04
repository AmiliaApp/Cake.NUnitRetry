#load nuget:?package=Cake.Recipe&version=1.0.0

Environment.SetVariableNames();


BuildParameters.SetParameters(
                            context: Context,
                            buildSystem: BuildSystem,
                            sourceDirectoryPath: "./src",
                            title: "Cake.NUnitRetry",
                            repositoryOwner: "AmiliaApp",
                            repositoryName: "Cake.NUnitRetry",
                            shouldGenerateDocumentation: false,
                            shouldRunCodecov: true,
                            shouldRunDotNetCorePack: true,
                            shouldRunDupFinder : false,
                            shouldRunGitVersion: true);

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(
                            context: Context,
                            testCoverageExcludeByFile: "*/*Designer.cs;*/*.g.cs;*/*.g.i.cs");

Build.RunDotNetCore();

