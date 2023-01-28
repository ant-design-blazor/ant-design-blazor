# Documentation

The documentation on the website (www.antblazor.com) is automatically generated based on XML comments in the code files for the library. The idea is to keep the code and documentation site automatically in sync when changes are made. For this to be the case though, we must maintain the documentation comments in the code, using particular tags and formatting along with some data attributes. More details on this are given below. 

For details on XML documentation comments in C# in general, see [the Microsoft website](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/).



## Documenting A Component

To mark a component to render a documentation page, tag it with the `Documentation` attribute, example:

`[Documentation(DocumentationCategory.Components, DocumentationType.DataDisplay, "https://fullUrl.com/to/image.jpg")]`

- The first argument is the category - choose from the enum
- The second argument is the type - choose from the enum
- The third argument is the image used for the "Overview" page
- There are many optional properties that can be set which affect the outcome of the page:
  - `Title`- use a custom title for the page. This affects URL as well. Default is the component's class name.
  - `Columns ` - the number of columns demos should be in. Default is 2.
  - `Subtitle ` - sub title for the documentation page. Default is null.
  - `OutputApi` - This one allows turning off outputting the decorated class's API. Default is true (output API).
    - The primary use of this currently is for pages that group multiple components (such as the Typography page, Layout page, etc) and don't have their own main component. Choose a component that makes sense to decorate and put this option. 
    - Typically used in combination with the <seealso> tag in the XML comments of the component to output API of grouping components (see below for details on XML tags.



## Supported XML Tags

There are specific tags that the build project looks at the build the documentation of the website. There are a mixture of standard and custom tags we look for in the documentation comments.

- <summary> - The summary tag is used for the descriptions on most places of the site. 
  - For a component/page it is used at the block at the very top of the page (right below the title and above the demos).
  - For class members, such as properties, parameters, fields, methods, etc, it is used to put a description next to the name of the member in the API section of the pages.
- <default value="something" /> - The default tag is a custom tag used to provide the default value of a member to the API section of the site.
  - In the example above, `something` would display in the default column next to the member it is on
  - Use of this is highly recommended even if the default is not directly set on the member itself, but is instead done through logic in the code itself.
  - This can be short statements such as "if true, 1, else 0"
  - No formatting is applied to these default values on the site
- <seealso cref="SomethingTheLibrary" /> - The see and seealso tags are used primarily to refer to other spots in the library and, when possible, link to those spots in the site.
  - `see` is supported inside summary tags. When provided inside a summary, code style text will be inserted in that spot
    - Types and members are supported here
    - When a Type is used, we will attempt to put a link to the type's page on the site
  - `seealso` is supported at the root level of components/page documentation. 
    - When provided here, API documentation for the member referenced in the `cref` will be added, in order, to the API section of the component's documentation page.
    - Currently only types are supported here (ex: a class, not a property on a class)
- <para>Some text</para>  - This tag will wrap text inside it in a paragraph when rendered in the site. This is supported inside summary tags and is recommended for use in the main summary of a component
- <list type="bullet">, <list type="number"> and <item> inside those tags. These are supported in summary tags and their use is recommended for the main summary of a component. This will cause a bulleted or numbered list to be rendered.
- <c> can be used in summary tags to indicate code and will be styled as such on the site.
- HTML header tags (<h1>, <h2>, etc) are supported inside summary tags. Their use is recommended for the main summary of a component



## Add FAQ section to documentation

FAQ sections can be added to documentation by placing markdown files in the proper spot, with the proper naming conventions. 

- Place a file in the folder: `Demos/Components/[COMPONENT_NAME]/` 
  - Replacing `[COMPONENT_NAME]` with the name of the component the FAQ should go on the page for
- In the folder, name the file `faq.[LANG].md`
  - Replacing `[LANG]` with the language code the file is in. Ex: `en-US`
  - These files are not currently translated so each language must be provided.



## Miscellaneous 

- `[Obsolete("Some short description on why and what else to use")]` 
  - Obsolete will display on the site with the message provided, along with a short message indicating it is to be removed in a future version.
  - An Obsolete tag will display next to the description on the site
- Parameters will have a tag displayed next to them on the site indicating they are a component parameter.
- Things ignored and not put on the documentation:
  - Injected services (using the `[Inject]` attribute) even if they have documentation comments
  - Cascading parameters (using the `[CascadingParameter]` attribute) even if they have documentation comments
  - Special members and constructors, regardless of documentation comments
  - Methods ignored: (a full list can be seen in the `_methodNamesToIgnore` field in the `GenerateDemoJsonCommand`)
    - Built-in methods such as `Dispose`, `Equals`, etc. 
    - Blazor lifecycle methods such as `OnAfterRender`, regardless of documentation comments



## Languages/Translations

All code documentation comments and messages are expected to be in English. Text is automatically translated from English to Chinese to generate the Chinese documentation where it makes sense.

<u>Translated</u>:

- Summary tags
- Obsolete messages
- Headers on API tables

<u>Not translated</u>:

- Markdown files (ex: for FAQ sections)
- Member/Class names
- Data types
- Default value text
- Method signatures, any part

### Override Translations

There may be cases when an automatic translation is insufficient or invalid. In these cases, there is the ability to specify the translation for <summary> tags by using a custom attribute on the tag. This is currently only supported for summary tags. If a translation is unable to be determined for any reason then the English will be used.

The example below demonstrates the usage: 

```c#
/// <summary>
/// English summary
/// </summary>
/// <summary xml:lang="zh-CN">
/// Chinese summary
/// </summary>
public string SomeProperty { get; set; }
```

This property has English and Chinese summaries provided at once. By using the `xml:lang` attribute you can specify the language the summary is for. If the attribute is not provided then English is assumed.

### Translation Service

Documentation will be translated into Chinese with either Google or Azure.

- Azure: The default service used
  - Requires providing an API key if you want it to run locally. Provide this key and the region for your key in the `appsettings.private.json` file in the Build.CLI folder. This file is gitignored so it will not be committed and reveal your key.
  - Azure provides 2 million characters of translation per month for free. See details on [Microsoft's site](https://azure.microsoft.com/en-us/pricing/details/cognitive-services/translator/#pricing).
- Google: Not used unless a code change makes it used
  - Available as a fallback option if desired
  - Does not require an API key
  - Very limited in number of requests that can be made. A single build of the docs will exceed this limit.



To avoid making excessive calls to either translation service, we cache the translations as "known translations" for later use (in the `KnownChineseTranslations.json` file)



<u>How it works</u>:

1. Translation requested
2. Check known translations - exists? Return the known translation. Doesn't exist? Continue.
3. Request translation
4. Add translation to list of known translations
5. At the end of the build, write known translations to disk
   - This will exclude any un-used translations so we clean up removed text when we change something.

