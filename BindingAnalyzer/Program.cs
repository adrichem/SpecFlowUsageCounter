using Adrichem.SpecFlowUsageCounter;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BindingAnalyzer
{
    class Program
    {
        static async Task Main(string[] args)
        {
			MSBuildLocator.RegisterInstance(MSBuildLocator.QueryVisualStudioInstances().First());

            using var workspace = MSBuildWorkspace.Create();
            // Print message for WorkspaceFailed event to help diagnosing project load failures.
            workspace.WorkspaceFailed += (o, e) =>
            {
                if (e.Diagnostic.Kind == WorkspaceDiagnosticKind.Failure)
                {
                    Console.Error.WriteLine(e.Diagnostic.Message);
                }
            };

            var solution = await workspace.OpenSolutionAsync(args.First());

            var FeatureFiles = solution
                .Projects
                .Select(prj => Directory.GetFiles(
                           Path.GetDirectoryName(prj.FilePath),
                           "*.feature",
                           SearchOption.AllDirectories).ToList()
                           )
                .SelectMany(x => x)
            ;

            Console.Out.WriteLine(JsonConvert.SerializeObject(
                    SpecFlowUsageCounter.Count(solution.Projects, FeatureFiles),
                    Formatting.Indented)
            );
        }
    }
}
