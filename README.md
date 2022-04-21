# BindingAnalyzerApp

Reports which SpecFlow step definitions are not used by any .feature file. Supports Given, When, Then and StepDefinition attributes.

## Usage:

```ps1
$ .\BindingAnalyzerApp.exe `
    --root C:\Repos\TestAutomation\ `
    --features project1/**/*.features project2/**/*.features `
    --code project1/**/*.cs bindingproject/**/*.cs `
    --exclude-features **/ignored/*.feature `
    --exclude-code *.feature.cs project2/**/ignored.cs
```

| Argument        | Default                   | Description                                                          |
|-----------------|---------------------------|----------------------------------------------------------------------|
| root            | Current working directory | The directory to search for feature files and code                   |
| features        | **/*.feature              | Glob patterns describing which feature files to include in analysis  |
| code            | **/*.cs                   | Glob patterns describing which .cs files to include in analysis      |
| exclude-features| none                      | Glob patterns describing which feature files to exclude from analysis|
| exclude-code    | **/*.feature.cs           | Glob patterns describing which cs files to exclude from analysis     |

Example output
```json
{
    "unused": [
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
     ],
     "features":[
         "C:\\Repos\\TestAutomation\\project1\feature1.feature",
         "C:\\Repos\\TestAutomation\\project1\feature2.feature",
         "C:\\Repos\\TestAutomation\\project2\feature3.feature",
     ],
     "code":[
        "C:\\Repos\\TestAutomation\\Bindings\\File1.cs",
        "C:\\Repos\\TestAutomation\\Bindings\\File2.cs"
        "C:\\Repos\\TestAutomation\\bindingproject\\somefile.cs"
     ]
 }
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

