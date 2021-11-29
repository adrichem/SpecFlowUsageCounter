namespace Adrichem.SpecFlowUsageCounter
{
    using Gherkin;
    using Gherkin.Ast;
    using MoreLinq;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class StepDefinitionUsageCounter
    {

        /// <summary>
        /// Result of the analysis. Contains the usage count per step definition.
        /// </summary>
        public IDictionary<SpecFlowAttribute, int> BindingsUsage { get; private set; }
        
        /// <summary>
        /// Performs the analysis.
        /// </summary>
        /// <returns>this</returns>
        public StepDefinitionUsageCounter Analyze(IEnumerable<string> codeFiles, IEnumerable<string> FeatureFiles )
        {
            if (null == codeFiles) throw new ArgumentNullException(nameof(codeFiles));
            if (null == FeatureFiles) throw new ArgumentNullException(nameof(FeatureFiles));

            DiscoveredAttributes = SpecFlowAttributeFinder.FindSpecFlowAttributes(codeFiles);
            BindingsUsage = DiscoveredAttributes.ToDictionary(x => x, x => 0);
            FeatureFiles.ForEach(f => AnalyzeFeatureFile(f));
            return this;
        }

        #region privates

        private string LastStepKeyWord = string.Empty;
        private IEnumerable<SpecFlowAttribute> DiscoveredAttributes { get; set; }

        private void AnalyzeFeatureFile(string FullPath)
        {
            GherkinDocument gherkinDocument = new Parser().Parse(FullPath);
            gherkinDocument
                .Feature
                .Children
                .OfType<StepsContainer>()
                .ForEach(c => AnalyzeStepContainer(c))
            ;
        }

        private void AnalyzeStepContainer(StepsContainer c)
        {
            LastStepKeyWord = string.Empty;
            c.Steps.ForEach(step => AnalyzeStep(step));
        }
        private void AnalyzeStep(Step step)
        {
            if (step.Keyword.Trim().ToLower() != "and")
            {
                LastStepKeyWord = step.Keyword.Trim();
            }
            string StepText = step.Text.Trim();

            DiscoveredAttributes
                  .Where(attr => attr.Keyword == LastStepKeyWord || attr.Keyword == "StepDefinition")
                  .Where(attr => attr.Re.IsMatch(StepText))
                  .ForEach(attr => BindingsUsage[attr] = BindingsUsage[attr] + 1)
           ;
        }
        #endregion
    }
}
