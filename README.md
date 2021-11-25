# SpecFlowUsageCounter

Counts where Given/When/Then/StepDefinitions are used in .feature files. Usage:

```ps1
$ .\BindingAnalyzer.exe C:\Repos\TestAutomation\VisualStudioSolution.sln
```
Example output
```json
{
  "UnusedAttributes": [
    {
      "File": "C:\\Repos\\TestAutomation\\Bindings\\File1.cs",
      "Line": 58,
      "Keyword": "Given",
      "StepText": "I wait for '(.*)'"
    },
    {
      "File": "C:\\Repos\\TestAutomation\\Bindings\\File1.cs",
      "Line": 59,
      "Keyword": "When",
      "StepText": "I Wait for '(.*)'"
    },
 ]
 ```
