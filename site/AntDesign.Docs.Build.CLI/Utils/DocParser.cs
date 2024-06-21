using System.Collections.Generic;
using System.IO;
using System.Linq;
using Markdig;
using Markdig.Extensions.Yaml;
using Markdig.Renderers;
using Markdig.Syntax;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AntDesign.Docs.Build.CLI.Utils
{
    public class DocParser
    {
        public static (Dictionary<string, string> Meta, string Desc, string ApiDoc) ParseDemoDoc(string input)
        {
            var pipeline = new MarkdownPipelineBuilder()
                .UseYamlFrontMatter()
                .UsePipeTables()
                .Build();

            var document = Markdown.Parse(input, pipeline);
            var yamlBlock = document.Descendants<YamlFrontMatterBlock>().FirstOrDefault();

            Dictionary<string, string> meta = null;
            if (yamlBlock != null)
            {
                var yaml = input.Substring(yamlBlock.Span.Start, yamlBlock.Span.Length).Trim('-');
                meta = new Deserializer().Deserialize<Dictionary<string, string>>(yaml);
            }

            var isAfterApi = false;
            var descPart = "";
            var apiPart = "";

            for (var i = yamlBlock?.Line ?? 0; i < document.Count; i++)
            {
                var block = document[i];
                if (block is YamlFrontMatterBlock)
                    continue;

                if (block is HeadingBlock heading && heading.Level == 2 && heading.Inline.FirstChild.ToString() == "API")
                {
                    isAfterApi = true;
                }

                using var writer = new StringWriter();
                var renderer = new HtmlRenderer(writer);
                pipeline.Setup(renderer);

                var blockHtml = renderer.Render(block);

                if (!isAfterApi)
                {
                    descPart += blockHtml;
                }
                else
                {
                    apiPart += blockHtml;
                }
            }

            return (meta, descPart, apiPart);
        }

        public static (DescriptionYaml Meta, string Style, Dictionary<string, string> Descriptions) ParseDescription(string input)
        {
            var pipeline = new MarkdownPipelineBuilder()
                .UseYamlFrontMatter()
                .Build();

            var document = Markdown.Parse(input, pipeline);
            var yamlBlock = document.Descendants<YamlFrontMatterBlock>().FirstOrDefault();

            DescriptionYaml meta = null;
            if (yamlBlock != null)
            {
                var yaml = input.Substring(yamlBlock.Span.Start, yamlBlock.Span.Length).Trim('-');
                meta = Deserializer.FromValueDeserializer(new DeserializerBuilder()
                        .WithNamingConvention(CamelCaseNamingConvention.Instance).BuildValueDeserializer())
                    .Deserialize<DescriptionYaml>(yaml);
            }

            var isAfterEnHeading = false;
            var isStyleBlock = false;
            var isCodeBlock = false;

            var zhPart = "";
            var enPart = "";
            var stylePart = "";
            var codePart = "";

            for (var i = yamlBlock?.Line ?? 0; i < document.Count; i++)
            {
                var block = document[i];
                if (block is YamlFrontMatterBlock)
                    continue;

                if (block is HeadingBlock heading && heading.Level == 2 && heading.Inline.FirstChild.ToString() == "en-US")
                {
                    isAfterEnHeading = true;
                }

                if (block is CodeBlock codeBlock)
                {
                    isCodeBlock = true;
                }

                if (block is HtmlBlock htmlBlock && htmlBlock.Type == HtmlBlockType.ScriptPreOrStyle)
                {
                    isStyleBlock = true;
                }

                if (block is HeadingBlock h && h.Level == 2)
                    continue;

                using var writer = new StringWriter();
                var renderer = new HtmlRenderer(writer);

                var blockHtml = renderer.Render(block);

                if (!isAfterEnHeading)
                {
                    zhPart += blockHtml;
                }
                else if (isStyleBlock)
                {
                    stylePart += blockHtml;
                }
                //else if (isCodeBlock)
                //{
                //    codePart += blockHtml;
                //}
                else
                {
                    enPart += blockHtml;
                }
            }

            stylePart = stylePart.Replace("<style>", "").Replace("</style>", "");
            return (meta, stylePart, new Dictionary<string, string>() { ["zh-CN"] = zhPart, ["en-US"] = enPart });
        }

        public static Dictionary<string, string> ParseHeader(string input)
        {
            var pipeline = new MarkdownPipelineBuilder()
                .UseYamlFrontMatter()
                .Build();

            var writer = new StringWriter();
            var renderer = new HtmlRenderer(writer);
            pipeline.Setup(renderer);

            var document = Markdown.Parse(input, pipeline);
            var yamlBlock = document.Descendants<YamlFrontMatterBlock>().FirstOrDefault();

            Dictionary<string, string> meta = null;
            if (yamlBlock != null)
            {
                var yaml = input.Substring(yamlBlock.Span.Start, yamlBlock.Span.Length).Trim('-');
                meta = new Deserializer().Deserialize<Dictionary<string, string>>(yaml);
            }

            return meta;
        }

        public static (float order, string title, string html) ParseDocs(string input)
        {
            input = input.Trim(' ', '\r', '\n');
            var pipeline = new MarkdownPipelineBuilder()
                .UseYamlFrontMatter()
                .UsePipeTables()
                .Build();

            StringWriter writer = new StringWriter();
            var renderer = new HtmlRenderer(writer);
            pipeline.Setup(renderer);

            MarkdownDocument document = Markdown.Parse(input, pipeline);
            var yamlBlock = document.Descendants<YamlFrontMatterBlock>().FirstOrDefault();
            var title = string.Empty;
            var order = 0f;
            if (yamlBlock != null)
            {
                var yaml = input.Substring(yamlBlock.Span.Start, yamlBlock.Span.Length).Trim('-');
                var meta = new Deserializer().Deserialize<Dictionary<string, string>>(yaml);
                title = meta["title"];
                order = float.TryParse(meta["order"], out var o) ? o : 0;
            }

            renderer.Render(document);
            writer.Flush();
            var html = writer.ToString();

            return (order, title, html);
        }
    }

    public class DescriptionYaml
    {
        public decimal Order { get; set; }

        public int? Iframe { get; set; }

        public string Link { get; set; }

        public Dictionary<string, string> Title { get; set; }

        public bool Debug { get; set; }

        public bool? Docs { get; set; }
    }
}
