namespace Adrichem.SpecFlowUsageCounter
{
    using Microsoft.CodeAnalysis;
    using System.Collections.Generic;
    using System.Linq;

    public static class SpecFlowUsageCounter
    {
        public static object Count(IEnumerable<Project> Projects, IEnumerable<string> FeatureFiles)
        {
            var GivenWhenThens = new List<DiscoveredAttribute>();
            object Result = null;

            foreach (var Project in Projects)
            {
                var compilation = Project.GetCompilationAsync().Result;
                var Walker = new AttributeUsageFinder
                {
                    Attributes = new List<INamedTypeSymbol>
                    {
                        compilation.GetTypeByMetadataName("TechTalk.SpecFlow.GivenAttribute"),
                        compilation.GetTypeByMetadataName("TechTalk.SpecFlow.WhenAttribute"),
                        compilation.GetTypeByMetadataName("TechTalk.SpecFlow.ThenAttribute"),
                        compilation.GetTypeByMetadataName("TechTalk.SpecFlow.StepDefinitionAttribute")
                    },
                    ReportAttr = (syntax, symbol, attr) =>
                    {
                        GivenWhenThens.Add(new DiscoveredAttribute
                        {
                            Keyword = attr.AttributeClass.Name,
                            Text = attr.ConstructorArguments.First().Value.ToString(),
                            Method = symbol,
                            MethodSyntax = syntax
                        });
                    }
                };

                foreach (var Tree in compilation.SyntaxTrees)
                {
                    Walker.SemanticModel = compilation.GetSemanticModel(Tree);
                    Walker.Visit(Tree.GetRoot());
                }
            }

            if (GivenWhenThens.Any())
            {
                var Counter = new GivenWhenThenCounter
                {
                    DiscoveredAttributes = GivenWhenThens,
                    FeatureFiles = FeatureFiles
                }.Analyze();


                Result = new
                {
                    UnusedAttributes = Counter
                        .BindingsUsage
                        .Where(x => x.Value == 0)
                        .Select(x => new
                        {
                            File = x.Key.MethodSyntax.GetLocation().SourceTree.FilePath,
                            Line = x.Key.MethodSyntax.GetLocation().GetLineSpan().StartLinePosition.Line + 1,
                            Keyword = x.Key.Keyword.Substring(0, x.Key.Keyword.LastIndexOf("Attribute")),
                            StepText = x.Key.Text
                        })
                };
            }

            return Result;
        }
    }
}
