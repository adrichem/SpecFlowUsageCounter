namespace Adrichem.BranchAnalyzer
{
    using Adrichem.SpecFlowUsageCounter;
    using LibGit2Sharp;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public static class Analyzer
    {
        public static BranchAnalysisResult Analyze(string RepoPath, string SourceBranch, string TargetBranch)
        {
            using var repo = new Repository(RepoPath);
            Commands.Checkout(repo, SourceBranch);

            var codeFiles = Directory
                .GetFiles(RepoPath, "*.cs", SearchOption.AllDirectories)
                .Where(file => !Regex.IsMatch(file, @".+\.feature.cs$", RegexOptions.IgnoreCase))
            ;

            var featureFiles = Directory
                .GetFiles(RepoPath, "*.feature", SearchOption.AllDirectories)
            ;

            var SourceBranchAnalysis = new StepDefinitionUsageCounter().Analyze(codeFiles, featureFiles).BindingsUsage;

            Commands.Checkout(repo, TargetBranch);
            codeFiles = Directory
                           .GetFiles(RepoPath, "*.cs", SearchOption.AllDirectories)
                           .Where(file => !Regex.IsMatch(file, @".+\.feature.cs$", RegexOptions.IgnoreCase))
                       ;

            featureFiles = Directory.GetFiles(RepoPath, "*.feature", SearchOption.AllDirectories);
            var TargetBranchAnalysis = new StepDefinitionUsageCounter().Analyze(codeFiles, featureFiles).BindingsUsage;

            return new BranchAnalysisResult
            {
                SourceAnalysis = SourceBranchAnalysis,
                TargetAnalysis = TargetBranchAnalysis,
            };
        }
    }
}