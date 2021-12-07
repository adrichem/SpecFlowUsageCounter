namespace Adrichem.SpecFlowUsageCounter
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    internal class SpecFlowAttributeFinder
    {
        public static IEnumerable<SpecFlowAttribute> FindSpecFlowAttributes(IEnumerable<string> codeFiles)
        {
            var result = codeFiles
                .Select(file => new {
                    file,
                    ast = CSharpSyntaxTree.ParseText(File.ReadAllText(file)).GetCompilationUnitRoot()
                })
                .Select(unit => new {
                    unit.file,
                    methods = unit.ast.DescendantNodes().OfType<MethodDeclarationSyntax>()
                })
                .Select(unit => new {
                    unit.file,
                    methodAttributes = unit.methods.Select(m => m.DescendantNodes().OfType<AttributeSyntax>()).SelectMany(x => x)
                })
                .Select(unit => new {
                    unit.file,
                    attributes = unit
                        .methodAttributes
                        .Select(attr => IfIsSpecFlowAttribute(attr, unit.file))
                        .Where(attr => null != attr)
                })
                .Where(unit => unit.attributes.Any())
                .Select(unit => unit.attributes)
                .SelectMany(x => x)
            ;
            return result.ToList();
        }

        static SpecFlowAttribute IfIsSpecFlowAttribute(AttributeSyntax attr, string File)
        {
            try
            {
                var AttrName = (attr.Name as IdentifierNameSyntax)
                    .Identifier
                    .Text
                    .Replace("TechTalk.", string.Empty)
                    .Replace("SpecFlow.", string.Empty)
                    .Replace("Attribute", string.Empty)
                ;
                if (AttrName != "Given" && AttrName != "When" && AttrName != "Then" && AttrName != "StepDefinition") return null;
                if (attr.ArgumentList?.Arguments.Count != 1) return null;
                if (attr.ArgumentList?.Arguments.First().Expression is not LiteralExpressionSyntax) return null;

                var RegEx = attr
                    .ArgumentList
                    .Arguments
                    .Take(1)
                    .Select( arg => arg.Expression)
                    .Cast<LiteralExpressionSyntax>()
                    .Single()
                    .Token
                    .ValueText
                    .TrimStart('@')
                ;
                return new SpecFlowAttribute
                {
                    Keyword = AttrName,
                    Text = RegEx,
                    File = File,
                    Line = attr.GetLocation().GetLineSpan().StartLinePosition.Line + 1,
                };
            }
            catch
            {
                return null;
            }
        }
    }
}
