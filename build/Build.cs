using System.Collections.Concurrent;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Coverlet;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.ReportGenerator;
using Nuke.Common.Utilities.Collections;

sealed class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(x => x.Compile);

    [Solution]
    readonly Solution Solution = null!;

    [Parameter("Extra properties passed to MSBuild commands")]
    readonly string[] MsbuildProperties = [];

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild
        ? Configuration.Debug
        : Configuration.Release;

    const string CoverageFolderName = "coverage";
    string CoveragePrefix => $"{CoverageFolderName}.*";
    string CoverageReportFile => "coverage.cobertura.xml";

    readonly ConcurrentDictionary<Target, Target> Targets = new();

    static Target GetEmptyTarget() => d => d.Executes(() => { });

    static int IndexOfOrThrow(string x, char y)
    {
        var idx = x.IndexOf(y);
        if (idx == -1)
        {
            throw new ArgumentException();
        }

        return idx;
    }

    bool HasProcessedProperties { get; set; }

    Dictionary<string, object> ProcessedMsbuildProperties
    {
        get
        {
            if (!HasProcessedProperties)
            {
                field = MsbuildProperties.ToDictionary(
                    x => x[..IndexOfOrThrow(x, '=')],
                    object (x) =>
                    {
                        var idx = IndexOfOrThrow(x, '=');
                        return x?.Substring(idx + 1, x.Length - idx - 1)!;
                    }
                );
            }

            return field;
        }
    } = [];

    Target CommonTarget(Target? actualTarget = null) =>
        Targets.GetOrAdd(
            actualTarget ??= GetEmptyTarget(),
            def => actualTarget is null ? def : actualTarget(def)
        );

    Target Clean =>
        CommonTarget(x =>
            x.Before(Restore)
                .Executes(() =>
                {
                    var outputs = Enumerable.Empty<Output>();
                    outputs = outputs.Concat(
                        DotNetTasks.DotNetClean(s =>
                            s.SetProject(Solution).SetProperties(ProcessedMsbuildProperties)
                        )
                    );

                    if (Directory.Exists(RootDirectory / "build" / "output_packages"))
                    {
                        Directory.Delete(RootDirectory / "build" / "output_packages", true);
                    }

                    Directory.CreateDirectory(RootDirectory / "build" / "output_packages");

                    return outputs;
                })
        );

    Target Restore =>
        CommonTarget(x =>
            x.After(Clean)
                .Executes(() =>
                {
                    Solution
                        .GetAllProjects("")
                        .ForEach(p =>
                            DotNetTasks.DotNetRestore(s =>
                                s.SetProjectFile(p).SetProperties(ProcessedMsbuildProperties)
                            )
                        );
                })
        );

    Target Compile =>
        CommonTarget(x =>
            x.DependsOn(Restore)
                .After(Clean)
                .Executes(
                    () =>
                        Solution
                            ?.GetAllProjects("*")
                            .ForEach(p =>
                                DotNetTasks.DotNetBuild(s =>
                                    s.SetProjectFile(Solution)
                                        .SetConfiguration(Configuration)
                                        .EnableNoRestore()
                                        .SetProperties(ProcessedMsbuildProperties)
                                )
                            )
                )
        );

    Target Recompile => CommonTarget(x => x.DependsOn(Clean, Compile));

    Target Test =>
        CommonTarget(x =>
            x.After(Compile)
                .Executes(() =>
                {
                    Solution
                        ?.GetAllProjects("*")
                        .Where(p => p.Name.EndsWith("Tests"))
                        .ForEach(p =>
                            DotNetTasks.DotNetTest(s =>
                                s.SetProjectFile(p)
                                    .SetConfiguration(Configuration)
                                    .SetNoRestore(true)
                            )
                        );
                })
        );

    Target TestWithCoverage =>
        CommonTarget(x =>
            x.DependsOn(Compile)
                .Executes(() =>
                {
                    Solution
                        ?.GetAllProjects("*")
                        .Where(p => p.Name.EndsWith("Tests"))
                        .ForEach(project =>
                            DotNetTasks.DotNetTest(s =>
                                s.SetProjectFile(project)
                                    .SetConfiguration(Configuration)
                                    .SetCollectCoverage(true)
                                    .SetCoverletOutputFormat(CoverletOutputFormat.cobertura)
                                    .SetCoverletOutput(
                                        RootDirectory / CoverageFolderName / CoverageReportFile
                                    )
                                    .SetExcludeByFile("**/Migrations/*.cs%2C**/Function/*.cs")
                                    .SetNoRestore(true)
                            )
                        );
                })
        );

    Target GenerateHtmlTestReport =>
        CommonTarget(x =>
            x.DependsOn(TestWithCoverage)
                .Executes(
                    () =>
                        ReportGeneratorTasks.ReportGenerator(s =>
                            s.SetReports(RootDirectory / CoverageFolderName / CoverageReportFile)
                                .SetTargetDirectory(RootDirectory / CoverageFolderName)
                                .SetReportTypes(ReportTypes.HtmlInline)
                        )
                )
        );

    Target Reset =>
        CommonTarget(x =>
            x.Before(Clean)
                .Executes(
                    () => RootDirectory.GlobDirectories(CoverageFolderName).DeleteDirectories()
                )
                .Executes(() => RootDirectory.GlobFiles(CoveragePrefix).DeleteFiles())
        );
}
