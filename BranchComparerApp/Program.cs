namespace BindingAnalyzer
{
    using Adrichem.BranchAnalyzer;
    using Adrichem.SpecFlowUsageCounter;
    using Fclp;
    using Newtonsoft.Json;
    using System;
    using System.Linq;

    class Arguments
    { 
        public string RepoPath { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }
    }

    class Program
    {
        protected Program() { }

        static void Main(string[] args)
        {
            var Parser = new FluentCommandLineParser<Arguments>();
            Parser.Setup(a => a.RepoPath).As('r', "repo").Required().WithDescription("The full path to the repository");
            Parser.Setup(a => a.Source).As('s', "source").Required().WithDescription("The source branch");
            Parser.Setup(a => a.Target).As('t', "target").Required().WithDescription("The target branch");
            var ParsingResult = Parser.Parse(args);
            if (ParsingResult.HasErrors)
            {
                Console.Error.WriteLine(ParsingResult.ErrorText);
                return;
            }

            var AnalysisResult = Analyzer.Analyze(Parser.Object.RepoPath
                , Parser.Object.Source
                , Parser.Object.Target)
            ;

            var s = AnalysisResult.SourceAnalysis.UnusedDefinitions();
            var t = AnalysisResult.TargetAnalysis.UnusedDefinitions();
            var newUnusedBindings = s.Except(t, new SpecFlowAttributeEqualityComparer());
            Console.Out.WriteLine(JsonConvert.SerializeObject(newUnusedBindings, Formatting.Indented));
        }
    }
}
