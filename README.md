# BindingAnalyzerApp

Reports which SpecFlow step definitions are not used by any .feature file. Supports Given, When, Then and StepDefinition attributes.

Usage:

```ps1
$ .\BindingAnalyzerApp.exe C:\Repos\TestAutomation\
```
Example output
```json
 [
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

# BranchComparerApp
Reports which new unused step definitions are caused by source branch as compared to target branch.

Usage:
 ```ps1
$ .\BranchComparerApp.exe `
    --repo C:\Repos\TestAutomation\ `
    --source test `
    --target main
```
Example output
```json
[
  {
    "Keyword": "Given",
    "Text": "I am unused",
    "File": "C:\\Repos\\TestAutomation\\Bindings\\File1.cs",
    "Line": 54
  }
]
```

