import { CsOptions, TypeMap } from "./CsBuilder";
import * as fs from 'fs';
import * as path from 'path';

let blockIndex = 0;
let typeMap: { [key: string]: string } = {};
let options: CsOptions | undefined = undefined;
let emptyMap: { [key: string]: number } = {};
let htmlTag: { [key: string]: string } = {
    'a': `["a"]`,
    'ol': `["ol"]`,
    'th': `["th"]`,
    'img': `["img"]`,
    'li': `["li"]`,
    'button': `["button"]`,
    'svg': `["svg"]`,
    'td': `["td"]`,
    'ul': `["ul"]`,
    'table': `["table"]`,
    'bdi': `["bdi"]`,
    'span': `["span"]`,
    'input': `["input"]`,
    'to': `["to"]`,
    'stroke': `["stroke"]`,
    'fill': `["fill"]`,
    'i': `["i"]`,
    'code': '["code"]',
    'kbd': '["kbd"]',
    'mark': '["mark"]',
    'strong': '["strong"]',
    'pre': '["pre"]',
    'blockquote': '["blockquote"]',
    'textarea': '["Textarea"]',
}

let operatorMap: { [key: string]: string } = {
    '===': '==',
    '==': '=='
}

export const spFieldName = '_skip_check_';

export const increaseIndex = (reset: boolean) => {
    if (reset) blockIndex = 0;
    else blockIndex++;
}

export const toPascalCase = (str?: string): string => {
    if (!str) return str || '';
    if (!isString(str)) return str;
    return str.charAt(0).toUpperCase() + str.slice(1);
}

export const isString = (obj: any): boolean => {
    return typeof obj === 'string' || obj instanceof String;
}

export const init = (ops: CsOptions) => {
    emptyMap = {};
    options = { ...ops };
    typeMap = {
        'number': 'double', // use double for all number types
        'string': 'string',
        'boolean': 'bool',
        // todo: add union type support.
        // 'string | number': 'string',
        // 'string | false': 'string',
    }
    if (options.typeMap) {
        for (const item of options.typeMap) {
            if (item.ranges || item.includes) {
                if (item.ranges && item.ranges.length > 0) {
                    for (const range of item.ranges) {
                        for (let i = range[0]; i <= range[1]; i++) {
                            typeMap[`${item.from}_${i}`] = item.to;
                        }
                    }
                }

                if (item.includes && item.includes.length > 0) {
                    for (const inc of item.includes) {
                        if (isString(inc)) {
                            typeMap[inc] = item.to;
                        } else {
                            typeMap[`${item.from}_${inc}`] = item.to;
                        }
                    }
                }
            } else {
                typeMap[item.from] = item.to;
            }
        }
    }
}

export const castType = (tsType: string, checker?: string[]): string => {
    if (tsType === 'CSSObject' && checker && checker.includes(spFieldName)) {
        return 'PropertySkip';
    }

    if (typeMap[tsType]) {
        return typeMap[tsType];
    }
    return tsType;
}

export const castFieldName = (name: string): string => {
    if (name in htmlTag) {
        return htmlTag[name];
    }

    let str = name
        .replace(/\$/g, '')    // 剔除$
        .replace(/'/g, '"')    // 单引号转双引号
        .replace(/`/g, '"');
    const char = str.charAt(0);
    if (/[a-z]/.test(char)) {
        str = str.replace(char, char.toUpperCase());
    } else if (char === `"`) {
        str = `[${str}]`;
    }
    if (/\r|\n/.test(name)) {
        str = str.replace(/\r|\n/g, '').replace(/\s/g, '');
    }
    str = str.replace(/("[\s\S]*{[\s\S]+}[\s\S]*")/, '$$$1');

    if (str.includes(spFieldName))
        str = str.replace(spFieldName, 'SkipCheck');

    for (const match of str.matchAll(/(\w+)\.(\w+)/g)) {
        if (match[2]) {
            str = str.replace(match[2], toPascalCase(match[2]));
        }
    }
    str = str.replace(/"(\w+)"/g, `\\"$1\\"`);


    return str;
}

export const castFieldValue = (value: string) => {
    if (value.includes('a0')) return value;
    let str = value
        .replace(/\$/g, '')    // 剔除$
        .replace(/'"(.*)"'/g, `'\\"$1\\"'`)
        .replace(/"'(.*)'"/g, `"\\"$1\\""`)
        .replace(/'/g, '"')    // 单引号转双引号

    if (str.includes('`')) {
        str = str.replace('`', `@$"`).replace('`', `"`);
    }

    if (/\w+\(.+\)/.test(str)) {
        str = toPascalCase(str);
    }

    if (/^-*\d+\.\d+$/.test(str)) {
        str = str + 'f';
    }

    for (const match of str.matchAll(/(\w+)\.(\w+)/g)) {
        if (match[2]) {
            str = str.replace(match[2], toPascalCase(match[2]));
        }
    }

    if (str.includes('||')) {
        str = str.substring(0, str.indexOf('|')).trimEnd();
    }
    return str;
}

export const castParameter = (value: string) => {
    if (!value) return value;
    if (!isString(value)) return value;
    let str = value
        .replace(/\$/g, '')    // 剔除$
        .replace(/'/g, '"');    // 单引号转双引号

    if (str.includes('`')) {
        str = str.replace('`', `$"`).replace('`', `"`);
    }
    for (const match of str.matchAll(/(\w+)\.(\w+)/g)) {
        if (match[2]) {
            str = str.replace(match[2], toPascalCase(match[2]));
        }
    }
    return str;
}

export const castFunName = (name: string) => {
    let str = name;
    if (!name.startsWith('new')) {
        str = toPascalCase(name);
    }
    // 方法连缀 a.b().c();
    const matches = str.matchAll(/\.(\w+)\(*/g);
    for (const m of matches) {
        str = str.replace(m[1], toPascalCase(m[1]));
    }
    return str;
}

export const castOperator = (operator: string) => {
    if (operatorMap[operator])
        return operatorMap[operator];
    else return operator;
}

export const formatCode = (code: string) => {
    let str = code
        .replace(/\$/g, '')    // 剔除$
        .replace(/'/g, '"');    // 单引号转双引号

    if (str.includes('`')) {
        str = str.replace('`', `@$"`).replace('`', `"`);
    }
    return str;
}

export const unknown = () => {
    const key = `Unknown${blockIndex}`;
    if (!(key in emptyMap)) {
        emptyMap[key] = 0;
    }
    const index = emptyMap[key] = emptyMap[key] + 1;
    return `${key}_${index}`;
}

export const defaultValue = (param: string, value?: string) => {
    if (value === undefined) {
        return param;
    }

    let str = value.replace(/'/g, '"');
    return `${param} = ${str}`;
}

export const writeAllText = (filePath: string, content: string) => {
    const distPath = path.resolve(filePath);
    const dir = path.dirname(distPath);
    if (!fs.existsSync(dir)) {
        fs.mkdirSync(dir, { recursive: true });
    }
    fs.writeFileSync(distPath, content, 'utf8');
}