namespace BindingAnalyzer
{
    using Adrichem.SpecFlowUsageCounter;
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    class Program
    {
        protected Program() { }
        static void Main(string[] args)
        {
            var root = args.First();
            var codeFiles = Directory.GetFiles(root, "*.cs", SearchOption.AllDirectories).Where(file => !Regex.IsMatch(file,@".+\.feature.cs$", RegexOptions.IgnoreCase));
            var FeatureFiles =  Directory.GetFiles(root, "*.feature",SearchOption.AllDirectories );
            var UnUsedStepDefs = new StepDefinitionUsageCounter()
                .Analyze(codeFiles, FeatureFiles)
                .BindingsUsage
                .Where(kvp => kvp.Value == 0)
                .Select(kvp => kvp.Key)
                .OrderBy(x => x.File)
                .ThenBy(x=>x.Line)
            ;
            Console.Out.WriteLine(JsonConvert.SerializeObject(UnUsedStepDefs, Formatting.Indented) );
        }
    }
}
