namespace Adrichem.BranchAnalyzer
{
    using Adrichem.SpecFlowUsageCounter;

    public class BranchAnalysisResult
    {
        public UsageCounts SourceAnalysis { get; set; }
        public UsageCounts TargetAnalysis { get; set; }
    }
}
