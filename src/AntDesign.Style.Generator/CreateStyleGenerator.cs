using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace AntDesign.Docs.Generator
{
    [Generator]
    public class CreateStyleGenerator : IIncrementalGenerator
    {
        private const string ExtensionMethodTemplate = """
                public static {{fieldName}}Property Create{{fieldName}}(this AntDesign.Core.Style.IStyleManager styleManager)
                {
                    ((StyleManager)styleManager).AddStyleBuilder("{{fieldName}}",token =>
                    {
                        {{localStatements}}
            
                        var dict = new Dictionary<string, string>()
                        {
                            {{cssObjectStatements}}
                        };
            
                        return dict;
                    });
            
                    return new {{fieldName}}Property();
                }
                """;

        private const string ProprtyClassTemplate = """
            public class {{fieldName}}Property
            {
                {{properties}}
            }
            """;

        private const string Template = """
                        using System;
                        using System.Collections.Generic;
                        using System.Linq;
                        using System.Text;
                        using System.Threading.Tasks;
                        using CssInCSharp;
                        using AntDesign.Core.Style;

                        namespace {{namespace}};

                        {{propertyClass}}

                        public static class StyleManagerExtensions
                        {
                            {{extensionMethods}}
                        }
                        """;

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(ctx => ctx.AddSource("CreateStyles.g.cs",
              SourceText.From("""   
                namespace AntDesign;

                public delegate object CreateStyles(GlobalToken token);
                """, Encoding.UTF8)));

            var declares = context.SyntaxProvider.CreateSyntaxProvider((s, _) =>
            {
                if (s is not ClassDeclarationSyntax ctx) return false;

                if (ctx.Members.Any(x => x is FieldDeclarationSyntax f && f.Declaration.Type is IdentifierNameSyntax id && id.Identifier.Text == "CreateStyles"))
                    return true;

                return false;
            },
            (context, _) => (context.SemanticModel.GetDeclaredSymbol(context.Node) as INamedTypeSymbol,
                            ((ClassDeclarationSyntax)context.Node).Members.OfType<FieldDeclarationSyntax>().Where(m => m.Declaration.Type is IdentifierNameSyntax id && id.Identifier.Text == "CreateStyles").ToList()))
            .Collect();

            var compilation = context.CompilationProvider.Combine(declares);

            context.RegisterSourceOutput(compilation, (spc, pair) =>
            {
                var assembly = pair.Left;
                var members = pair.Right;

                var properites = new StringBuilder();
                var methods = new StringBuilder();

                foreach (var member in members)
                {
                    var classSymbol = member.Item1;
                    //var actions = classSymbol.GetMembers().OfType<IFieldSymbol>().Where(x => x.Type.Name == "CreateStyles").ToList();
                    foreach (var variable in member.Item2.SelectMany(x => x.Declaration.Variables))
                    {
                        var properties = new Dictionary<string, string>();
                        var localStatements = new List<string>();
                        var fieldName = ToPascalCase(variable.Identifier.Text.Replace("_", "").Replace("use", ""));

                        if (variable.Initializer.Value is LambdaExpressionSyntax lambda && lambda.Body is BlockSyntax block)
                        {
                            foreach (var statement in block.Statements)
                            {
                                if (statement is ReturnStatementSyntax returnStatement && returnStatement.Expression is AnonymousObjectCreationExpressionSyntax anonymousObjectCreationExpression)
                                {
                                    foreach (var initializer in anonymousObjectCreationExpression.Initializers)
                                    {
                                        properties.Add(initializer.NameEquals.Name.Identifier.Text, initializer.Expression.ToFullString());
                                    }
                                }

                                if (statement is LocalDeclarationStatementSyntax localDeclarationStatement)
                                {
                                    localStatements.Add(statement.ToString());
                                }
                            }
                        }

                        var propertyHashDict = properties.ToDictionary(x => x.Key, x => "css-" + Guid.NewGuid());

                        var sb = new StringBuilder();
                        var sb3 = new StringBuilder();
                        foreach (var property in properties)
                        {
                            sb.AppendLine($"    public string {property.Key} => \"{propertyHashDict[property.Key]}\";");

                            sb3.AppendLine($"   [\"{propertyHashDict[property.Key]}\"] = {property.Value}.SerializeCss(\"{propertyHashDict[property.Key]}\"),");
                        }

                        var sb2 = new StringBuilder();
                        foreach (var property in properties.Keys)
                        {
                            sb2.AppendLine($"   {property} = \"{propertyHashDict[property]}\",");
                        }

                        properites.AppendLine(ProprtyClassTemplate
                            .Replace("{{fieldName}}", fieldName)
                             .Replace("{{properties}}", sb.ToString())
                            );

                        methods.AppendLine(ExtensionMethodTemplate
                             .Replace("{{fieldName}}", fieldName)
                             .Replace("{{localStatements}}", string.Join("\r\n", localStatements))
                             .Replace("{{cssObjectStatements}}", sb3.ToString())
                             .Replace("{{propertiesValues}}", sb2.ToString()));
                    }

                    var source = Template
                        .Replace("{{namespace}}", classSymbol.ContainingNamespace.ToString())
                        .Replace("{{className}}", classSymbol.Name)
                        .Replace("{{propertyClass}}", properites.ToString())
                        .Replace("{{extensionMethods}}", methods.ToString())
                     ;

                    spc.AddSource($"{classSymbol.Name}.g.cs", SourceText.From(source, Encoding.UTF8));
                }
            });
        }

        private string ToPascalCase(string name)
        {
            return name.Substring(0, 1).ToUpper() + name.Substring(1);
        }
    }
}
