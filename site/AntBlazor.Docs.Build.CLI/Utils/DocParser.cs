using System.Collections.Generic;
using System.IO;
using System.Linq;
using Markdig;
using Markdig.Extensions.Yaml;
using Markdig.Renderers;
using Markdig.Syntax;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AntBlazor.Docs.Build.CLI.Utils
{
    public class DocParser
    {
        public static (Dictionary<string, string> Meta, string Content) ParseDemoDoc(string input)
        {
            var pipeline = new MarkdownPipelineBuilder()
                .UseYamlFrontMatter()
                .Build();

            StringWriter writer = new StringWriter();
            var renderer = new HtmlRenderer(writer);
            pipeline.Setup(renderer);

            MarkdownDocument document = Markdown.Parse(input, pipeline);
            var yamlBlock = document.Descendants<YamlFrontMatterBlock>().FirstOrDefault();

            Dictionary<string, string> meta = null;
            if (yamlBlock != null)
            {
                string yaml = input.Substring(yamlBlock.Span.Start, yamlBlock.Span.Length).Trim('-');
                meta = new Deserializer().Deserialize<Dictionary<string, string>>(yaml);
            }

            renderer.Render(document);
            writer.Flush();
            string html = writer.ToString();

            return (meta, html);
        }

        public static (DescriptionYaml Meta, string Style, Dictionary<string, string> Descriptions) ParseDescription(string input)
        {
            var pipeline = new MarkdownPipelineBuilder()
                .UseYamlFrontMatter()
                .Build();

            MarkdownDocument document = Markdown.Parse(input, pipeline);
            var yamlBlock = document.Descendants<YamlFrontMatterBlock>().FirstOrDefault();

            DescriptionYaml meta = null;
            if (yamlBlock != null)
            {
                string yaml = input.Substring(yamlBlock.Span.Start, yamlBlock.Span.Length).Trim('-');
                meta = Deserializer.FromValueDeserializer(new DeserializerBuilder()
                        .WithNamingConvention(CamelCaseNamingConvention.Instance).BuildValueDeserializer())
                    .Deserialize<DescriptionYaml>(yaml);
            }

            var isAfterENHeading = false;
            var isStyleBlock = false;

            var zhPart = "";
            var enPart = "";
            var stylePart = "";

            for (int i = yamlBlock?.Line ?? 0; i < document.Count; i++)
            {
                var block = document[i];
                if (block is YamlFrontMatterBlock)
                    continue;

                if (block is HeadingBlock heading && heading.Level == 2 && heading.Inline.FirstChild.ToString() == "en-US")
                {
                    isAfterENHeading = true;
                }

                if (block is HtmlBlock htmlBlock && htmlBlock.Type == HtmlBlockType.ScriptPreOrStyle)
                {
                    isStyleBlock = true;
                }

                if (block is HeadingBlock h && h.Level == 2)
                    continue;

                using StringWriter writer = new StringWriter();
                var renderer = new HtmlRenderer(writer);

                var blockHtml = renderer.Render(block);

                if (!isAfterENHeading)
                {
                    zhPart += blockHtml;
                }
                else if (isStyleBlock)
                {
                    stylePart += blockHtml;
                }
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

            StringWriter writer = new StringWriter();
            var renderer = new HtmlRenderer(writer);
            pipeline.Setup(renderer);

            MarkdownDocument document = Markdown.Parse(input, pipeline);
            var yamlBlock = document.Descendants<YamlFrontMatterBlock>().FirstOrDefault();

            Dictionary<string, string> meta = null;
            if (yamlBlock != null)
            {
                string yaml = input.Substring(yamlBlock.Span.Start, yamlBlock.Span.Length).Trim('-');
                meta = new Deserializer().Deserialize<Dictionary<string, string>>(yaml);
            }

            return meta;
        }
    }

    public class DescriptionYaml
    {
        public int Order { get; set; }

        public Dictionary<string, string> Iframe { get; set; }

        public Dictionary<string, string> Title { get; set; }

        public bool Debug { get; set; }
    }
}