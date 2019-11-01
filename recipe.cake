#load nuget:?package=Cake.Recipe&version=1.0.0

Environment.SetVariableNames();


BuildParameters.SetParameters(context: Context,
                            buildSystem: BuildSystem,
                            sourceDirectoryPath: "./src",
                            title: "Cake.NunitRetry",
                            repositoryOwner: "AmiliaApp",
                            repositoryName: "Cake.NunitRetry");

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(context: Context);

Build.RunNuGet();
