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

                foreach (var member in members)
                {

                    var classSymbol = member.Item1;
                    //var actions = classSymbol.GetMembers().OfType<IFieldSymbol>().Where(x => x.Type.Name == "CreateStyles").ToList();
                    foreach (var action in member.Item2)
                    {
                        var properties = new Dictionary<string, string>();
                        var localStatements = new List<string>();

                        foreach (var statement in action.Declaration.Variables.Select(x => x.Initializer.Value)
                        .OfType<LambdaExpressionSyntax>().Select(x => x.Body).OfType<BlockSyntax>().SelectMany(x => x.Statements))
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


                        var template = """
                        using System;
                        using System.Collections.Generic;
                        using System.Linq;
                        using System.Text;
                        using System.Threading.Tasks;
                        using CssInCSharp;


                        namespace {{namespace}};

                        public partial class {{className}}
                        {

                            public class ClassProperty
                            {
                                {{properties}}
                            }
                        
                            public Func<ClassProperty> UseStylesFunc = default!;

                            Dictionary<string, string> CreateCssObject(GlobalToken token)
                            {
                                {{localStatements}}

                                var dict = new Dictionary<string, string>()
                                {
                                    {{cssObjectStatements}}
                                };

                                UseStylesFunc = () =>
                                {
                                    return new ClassProperty
                                    {
                                        {{propertiesValues}}
                                    };
                                };

                                return dict;
                            }
                        }
                        """;

                        var propertyHashDict = properties.ToDictionary(x => x.Key, x => "css-" + Guid.NewGuid());

                        var sb = new StringBuilder();
                        var sb3 = new StringBuilder();
                        foreach (var property in properties)
                        {
                            sb.AppendLine($"    public string {property.Key} {{ get;set; }}");

                            sb3.AppendLine($"   [\"{propertyHashDict[property.Key]}\"] = {property.Value}.SerializeCss(\"{propertyHashDict[property.Key]}\"),");
                        }

                        var sb2 = new StringBuilder();
                        foreach (var property in properties.Keys)
                        {
                            sb2.AppendLine($"   {property} = \"{propertyHashDict[property]}\",");
                        }

                        var source = template
                         .Replace("{{namespace}}", classSymbol.ContainingNamespace.ToString())
                         .Replace("{{className}}", classSymbol.Name)
                         .Replace("{{properties}}", sb.ToString())
                         .Replace("{{localStatements}}", string.Join("\r\n", localStatements))
                         .Replace("{{cssObjectStatements}}", sb3.ToString())
                         .Replace("{{propertiesValues}}", sb2.ToString());

                        spc.AddSource($"{classSymbol.Name}.g.cs", SourceText.From(source, Encoding.UTF8));
                    }
                }
            });
        }
    }
}
