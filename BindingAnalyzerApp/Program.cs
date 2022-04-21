namespace BindingAnalyzer
{
    using Adrichem.SpecFlowUsageCounter;
    using Fclp;
    using Microsoft.Extensions.FileSystemGlobbing;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    class Arguments
    {
        public string Root {get; set; }
        public List<string> IncludeFeatures { get; set; } = new List<string>();
        public List<string> IncludeCs { get; set; } = new List<string>();
        public List<string> ExcludeFeatures { get; set; } = new List<string>();
        public List<string> ExcludeCs { get; set; } = new List<string>();
    }


    class Program
    {
        static IEnumerable<string> DoGlob(string Root, IList<string> Include, IList<string> Exclude)
        {
            Matcher Globber = new();
            Globber.AddIncludePatterns(Include);
            Globber.AddExcludePatterns(Exclude);
            return Globber.GetResultsInFullPath(Root);
        }

        protected Program() { }
        static void Main(string[] args)
        {
            var Parser = new FluentCommandLineParser<Arguments>();
            Parser.Setup(a => a.Root).As("root").SetDefault(Directory.GetCurrentDirectory()).WithDescription("The path to search for .feature and .cs files.");
            Parser.Setup(a => a.IncludeFeatures).As("features").SetDefault(new List<string> {"**/*.feature" }).WithDescription("Glob patterns for feature files to include.");
            Parser.Setup(a => a.IncludeCs).As("code").SetDefault(new List<string> { "**/*.cs" }).WithDescription("Glob patterns for cs files to include.");
            Parser.Setup(a => a.ExcludeFeatures).As("exclude-features").WithDescription("Glob patterns for feature files to exclude.");
            Parser.Setup(a => a.ExcludeCs).As("exclude-code").SetDefault(new List<string> { "**/*.feature.cs" }).WithDescription("Glob patterns for cs files to exclude.");
            Parser.SetupHelp("?", "help").Callback(text => Console.WriteLine(text));

            var ParsingResult = Parser.Parse(args);
            if (ParsingResult.HasErrors)
            {
                Console.Error.WriteLine(ParsingResult.ErrorText);
                return;
            }
            if (ParsingResult.HelpCalled) return;

            var codeFiles = DoGlob(Parser.Object.Root, Parser.Object.IncludeCs, Parser.Object.ExcludeCs);
            var FeatureFiles = DoGlob(Parser.Object.Root, Parser.Object.IncludeFeatures, Parser.Object.ExcludeFeatures);
            var UnUsedStepDefs = new StepDefinitionUsageCounter()
                .Analyze(codeFiles, FeatureFiles)
                .BindingsUsage
                .Where(kvp => kvp.Value == 0)
                .Select(kvp => kvp.Key)
                .OrderBy(x => x.File)
                .ThenBy(x=>x.Line)
            ;
            Console.Out.WriteLine(JsonConvert.SerializeObject(new { unused = UnUsedStepDefs, features = FeatureFiles, code = codeFiles }, Formatting.Indented) );
        }
    }
}
