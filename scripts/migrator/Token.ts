import { isNumber, isString, readAllText, writeAllText } from "./Util";

function tokenCode(source: string, tab: string): string {
    const json = readAllText(source);
    const token = JSON.parse(json);
    const items: string[] = [];
    const ignores = ['_tokenKey', '_themeKey', '_hashId'];
    const keys = Object.keys(token); //.sort();
    for (const key of keys) {
        if (ignores.includes(key)) continue;
        let value = token[key];
        if (isString(value)) {
            value = value.replace(/\r\n|\n/g, '\\n');
            value = `"${value}"`;
        } else if (isNumber(value)) {
            value = `${value}d`;
        }
        items.push(`${tab}["${key}"] = ${value}`);
    }
    return items.join(',\r\n');
}

function genTheme(name: string, source: string, dist: string) {
    const tab = '            ';
    const template = `namespace AntDesign.Themes
{
    public class ${name}
    {
        private static readonly GlobalToken _token = new GlobalToken()
        {
${tokenCode(source, tab)}        
        };

        public static GlobalToken Derivative()
        {
            return _token;
        }
    }
}
`;
    writeAllText(dist, template);
}

export function genToken() {
    genTheme('Default', './scripts/migrator/token_default.json', './components/theme/themes/Default.cs');
    genTheme('Dark', './scripts/migrator/token_dark.json', './components/theme/themes/Dark.cs');
    genTheme('Compact', './scripts/migrator/token_compact.json', './components/theme/themes/Compact.cs');
}

export function genTokenTests() {

}