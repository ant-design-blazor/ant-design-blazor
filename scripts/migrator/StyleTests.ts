import * as fs from 'fs';
import * as path from 'path';
import { writeAllText } from './Util';

export function genComponentStyleTests() {
    const sourceDir = './tests/AntDesign.Tests/Styles/css';
    const dist = './tests/AntDesign.Tests/Styles/GenerateStyleTests.cs';
    const ignore = ["Flex", "Image", "InputNumber"]

    const methods: string[] = []
    fs.readdirSync(sourceDir).forEach(file => {
        const name = path.parse(file).name;
        if (ignore.includes(name)) return true;
        const str = `
        [Fact]
        public void Generate_${name}_Style()
        {
            var content = LoadStyleHtml("Styles/css/${name}.css");
            var (html, hashId) = ${name}Style.UseComponentStyle()("ant-${name.toLowerCase()}");
            var cut = Render(html);
            cut.MarkupMatches(content);
        }`;
        methods.push(str);
    });

    let template = `using Bunit;
using Xunit;

namespace AntDesign.Tests.Styles
{
    public class GenerateStyleTests : StyleTestsBase
    {
${methods.join('\r\n')}
    }
}
`;
    writeAllText(dist, template);
}