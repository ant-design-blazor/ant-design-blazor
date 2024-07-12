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

export function genToken() {
    const tokenSeed = './scripts/migrator/token_seed.json';
    const dist = './components/theme/themes/Seed.cs';
    const tab = '            ';
    const template = `namespace AntDesign
{
    public static class Seed
    {
        public static readonly GlobalToken DefaultSeedToken = new GlobalToken
        {
${tokenCode(tokenSeed, tab)}
        };
    }
}
`;
    writeAllText(dist, template);
}

export function genTokenTests() {

}