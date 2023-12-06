import { isString, toPascalCase, castType, castFieldName, castFieldValue, init, unknown, castParameter, castFunName, defaultValue, increaseIndex } from "./Util";

export type Transform = {
    source: string;
    target: string;
    regex?: RegExp;
}

export type TypeMap = {
    from: string;
    to: string;
    ranges?: number[][];
    includes?: (number | string)[];
}

export type CsOptions = {
    usings: string[];
    namespace: string;
    defaultClass: string;
    defaultTab: string;
    typeMap?: TypeMap[];
    transforms?: Transform[];
    propertyMap?: string;
}

export enum CsKinds {
    ObjectExpression = 0, // create object with expression.
    ObjectBinding = 1,    // extract properties from an object.
    ArrayExpression = 2,  // create array with expression.
    CallExpression = 3,
    Method = 4,
    Func = 5,
    Action = 6,
    VariableDeclaration = 7,
    Identifier = 8,
}

export type ParameterType = {
    name: string;
    type: string;
    bindings?: BindingItem[];
    defaultValue?: string;
}

export type PropertyAssignment = {
    fieldName: string;
    fieldValue: string | ObjectExpression | ArrayExpression | CallExpression;
}

export type BindingItem = {
    name: string;
    propertyName: string;
}

export type ObjectExpression = {
    kind?: CsKinds;
    type: string;
    args?: string[];
    properties: PropertyAssignment[];
}

export type ArrayExpression = {
    kind: CsKinds;
    type: string;
    items: any[];
}

export type CallExpression = {
    kind: CsKinds;
    assignment?: string;
    funcName: string;
    paramaters: any[];
    returnFlag?: string;
}

export type ObjectBinding = {
    kind: CsKinds;
    initializer: string;
    bindings: BindingItem[];
}

export type FunctionBody = {
    statements: any[];
}

export type VariableDeclaration = {
    kind: CsKinds;
    name: string;
    value: string | ObjectExpression;
}

export class CsVariable {
    name: string;
    type: string;
    value: any;

    constructor(name: string, type: string, value: any) {
        this.name = name;
        this.type = type;
        this.value = value;
    }

    format(tab: string): string[] {
        const codes: string[] = [];
        if (isString(this.value)) {
            codes.push(`${tab}var ${this.name} = ${this.value};`);
        } else {
            switch (this.value.kind) {
                case CsKinds.ObjectExpression:
                    codes.push(...this.createObjectExpression(tab, this.value));
                    break;
            }
        }
        return codes;
    }

    private createObjectExpression(tab: string, objectExpression: ObjectExpression): string[] {
        const codes: string[] = [];
        const recursion = (rootTab: string, fieldName: string, exp: ObjectExpression) => {
            const end = fieldName ? ',' : ';';
            const args = exp.args && exp.args.length > 0 ? exp.args.map(x => castParameter(x)).join(', ') : '';
            if (!fieldName) {
                codes.push(`${rootTab}private ${this.type} ${this.name} = new ${this.type}(${args})`);
            } else {
                const type = castType(exp.type || unknown());
                codes.push(`${rootTab}${fieldName} = new ${type}(${args})`);
            }
            codes.push(`${rootTab}{`);
            for (const item of exp.properties) {
                const name = castFieldName(item.fieldName);
                if (isString(item.fieldValue)) {
                    codes.push(`${rootTab}    ${name} = ${castFieldValue(item.fieldValue as string)},`);
                } else {
                    recursion(rootTab + '    ', name, item.fieldValue as ObjectExpression);
                }
            }
            codes.push(`${rootTab}}${end}`);
        }
        recursion(tab, '', objectExpression);
        return codes;
    }
}

export class CsProperty {
    name: string;
    type: string;
    access: string;
    propMap?: string;
    constructor(name: string, type: string, access: string = 'public', propMap?: string) {
        this.name = name;
        this.type = type;
        this.access = access;
        this.propMap = propMap;
    }

    public format(tab: string = ''): string[] {
        if (this.propMap) {
            const code: string[] = [];
            code.push(`${tab}${this.access} ${castType(this.type)} ${toPascalCase(this.name)}`);
            code.push(`${tab}{`);
            code.push(`${tab}    get => (${castType(this.type)})${this.propMap}["${this.name}"];`);
            code.push(`${tab}    set => ${this.propMap}["${this.name}"] = value;`);
            code.push(`${tab}}`);
            return code;
        }
        return [`${tab}${this.access} ${castType(this.type)} ${toPascalCase(this.name)} { get; set; }`];
    }
}

export class CsFunction {
    name: string;
    paramaters: ParameterType[];
    returnType: string;
    body: FunctionBody;
    kind: CsKinds;

    constructor(name: string, parameters: ParameterType[], returnType: string, body: FunctionBody, kind: CsKinds = CsKinds.Method) {
        this.name = name;
        this.paramaters = parameters;
        this.returnType = returnType;
        this.body = body;
        this.kind = kind;
    }

    format(tab: string = '', end: string = ';'): string[] {
        const codes: string[] = [];
        const bindingParamaters = () => {
            const ps = this.paramaters.filter(x => x.bindings !== undefined);
            if (ps && ps.length > 0) {
                ps.forEach(x => {
                    x.bindings?.forEach(y => {
                        codes.push(`${tab}    var ${y.name} = ${x.name}.${toPascalCase(y.name)};`);
                    })
                });
            }
        }
        switch (this.kind) {
            case CsKinds.Method:
                const ps = () => this.paramaters.map(x => `${castType(x.type || unknown())} ${defaultValue(x.name, x.defaultValue)}`).join(', ');
                codes.push(`${tab}public ${castType(this.returnType || unknown())} ${toPascalCase(this.name)}(${ps()})`);
                codes.push(`${tab}{`);
                codes.push(...this.createBody(tab + '    '));
                codes.push(`${tab}}`);
                break;
            case CsKinds.Func:
                if (this.name) {
                    const ps = this.paramaters.map(x => `${castType(x.type || unknown())} ${x.name}`).join(', ');
                    codes.push(`${tab}var ${this.name} = (${ps}) => {`);
                    codes.push(...this.createBody(tab + '    '));
                    codes.push(`${tab}}${end}`);
                } else {
                    const ps = this.paramaters.map(x => `${x.name}`).join(', '); // reset parameters
                    codes.push(`${tab}(${ps}) =>`); // anonymous func
                    codes.push(`${tab}{`)
                    bindingParamaters();
                    codes.push(...this.createBody(tab + '    '));
                    codes.push(`${tab}}${end}`);
                }

                break;
        }
        return codes;
    }

    private createBody(tab: string): string[] {
        const codes: string[] = [];
        this.body.statements.forEach(x => {
            switch (x.kind) {
                case CsKinds.ObjectExpression:
                    codes.push(...this.createObjectExpression(tab, x));
                    break;
                case CsKinds.ObjectBinding:
                    codes.push(...this.createObjectBinding(tab, x));
                    break;
                case CsKinds.ArrayExpression:
                    codes.push(...this.createArrayExpression(tab, x));
                    break;
                case CsKinds.CallExpression:
                    codes.push(...this.createCallExpression(tab, x));
                    break;
                case CsKinds.Func:
                    codes.push(...x.format(tab));
                    break;
                case CsKinds.VariableDeclaration:
                    codes.push(...this.createVariableDeclaration(tab, x));
                    break;
                case CsKinds.Identifier:
                    codes.push(`${tab}${x.text}`);
                    break;
            }
        });
        return codes;
    }

    private createObjectBinding(tab: string, objectBinding: ObjectBinding): string[] {
        const codes: string[] = [];
        if (!objectBinding.bindings || objectBinding.bindings.length <= 0) return codes;
        objectBinding.bindings.forEach((item) => {
            const name = item.propertyName ? item.propertyName : item.name;
            codes.push(`${tab}var ${item.name} = ${objectBinding.initializer}.${toPascalCase(name)};`);
        });
        return codes;
    }

    private createObjectExpression(tab: string, objectExpression: ObjectExpression, rootEnd: string = ';', returnFlag: string = 'return '): string[] {
        const codes: string[] = [];
        const recursion = (rootTab: string, fieldName: string, exp: ObjectExpression) => {
            const end = fieldName ? ',' : rootEnd;
            const type = castType(exp.type || unknown(), exp.properties.map(x => x.fieldName));
            codes.push(fieldName ? `${rootTab}${fieldName} = new ${type}()` : `${rootTab}${returnFlag}new ${type}()`);
            codes.push(`${rootTab}{`);
            exp.properties.forEach((item, i) => {
                const name = castFieldName(item.fieldName);
                const value = item.fieldValue as any;
                if (isString(value)) {
                    codes.push(`${rootTab}    ${name} = ${castFieldValue(value as string)},`);
                } else if (value.kind === CsKinds.ArrayExpression) {
                    const end = i === exp.properties.length - 1 ? '' : ',';
                    const arrExp = this.createArrayExpression(rootTab + '    ', value as ArrayExpression, end, '');
                    codes.push(`${rootTab}    ${name} = ${arrExp[0].trimStart()}`);
                    codes.push(...arrExp.slice(1));
                } else if (value.kind === CsKinds.CallExpression) {
                    const end = i === exp.properties.length - 1 ? '' : ',';
                    const callExp = this.createCallExpression(rootTab + '    ', value as CallExpression, end);
                    codes.push(`${rootTab}    ${name} = ${callExp[0].trimStart()}`);
                    if (callExp.length > 1) {
                        codes.push(...callExp.slice(1));
                    }
                } else {
                    recursion(rootTab + '    ', name, value as ObjectExpression);
                }
            });
            codes.push(`${rootTab}}${end}`);
        }
        recursion(tab, '', objectExpression);
        return codes;
    }

    private createArrayExpression(tab: string, arrayExpression: ArrayExpression, rootEnd: string = ';', returnFlag: string = 'return '): string[] {
        const codes: string[] = [];
        const precode = `${returnFlag}new ${castType(arrayExpression.type || unknown())}`;
        if (arrayExpression.items.length > 1) {
            codes.push(`${tab}${precode}`);
            codes.push(`${tab}{`);
            arrayExpression.items.forEach((x, i) => {
                const end = i === arrayExpression.items.length - 1 ? '' : ',';
                if (isString(x)) {
                    codes.push(`${tab}    ${toPascalCase(castParameter(x))}${end}`);
                } else {
                    switch (x.kind) {
                        case CsKinds.ObjectExpression:
                            codes.push(...this.createObjectExpression(tab + '    ', x as ObjectExpression, ',', ''));
                            break;
                    }
                }
            });
            codes.push(`${tab}}${rootEnd}`);
        } else {
            const args = arrayExpression.items.map(x => toPascalCase(x)).join(',');
            codes.push(`${tab}${precode} { ${args} }${rootEnd}`);
        }

        return codes;
    }

    private createCallExpression(tab: string, callExpression: CallExpression, rootEnd: string = ';'): string[] {
        const codes: string[] = [];
        const multiLine = callExpression.paramaters.findIndex(x => !isString(x)) >= 0;
        const name = castFunName(callExpression.funcName);
        const precode = callExpression.assignment ?
            `var ${callExpression.assignment} = ${name}` :
            `${callExpression.returnFlag}${name}`;
        if (!multiLine) {
            const args = callExpression.paramaters.map(x => castParameter(x)).join(', ');
            codes.push(`${tab}${precode}(${args})${rootEnd}`);
        } else {
            codes.push(`${tab}${precode}(`);
            const tab2 = `${tab}    `;
            callExpression.paramaters.forEach((x, i) => {
                const end = i === callExpression.paramaters.length - 1 ? `)${rootEnd}` : ',';
                if (isString(x)) {
                    codes.push(`${tab2}${castParameter(x)}${end}`);
                } else if ('kind' in x) {
                    switch (x.kind) {
                        case CsKinds.ObjectExpression:
                            const objCodes = this.createObjectExpression(tab2, x as ObjectExpression, end, '');
                            codes.push(...objCodes);
                            break;
                        case CsKinds.Func:
                            const funcCodes = (x as CsFunction).format(tab + '    ', end);
                            codes.push(...funcCodes);
                            break;
                        case CsKinds.Method:
                            const methodCodes = (x as CsFunction).format(tab + '    ', end);
                            codes.push(...methodCodes);
                            break;
                        case CsKinds.CallExpression:
                            const callCodes = this.createCallExpression(tab + '    ', x as CallExpression, ');');
                            codes.push(...callCodes);
                            break;
                    }
                }
            });
        }
        return codes;
    }

    private createVariableDeclaration(tab: string, declaration: VariableDeclaration): string[] {
        const codes: string[] = [];
        if (isString(declaration.value)) {
            codes.push(`${tab}var ${declaration.name} = ${castFieldValue(declaration.value as string)};`);
        } else {
            const value = declaration.value as any;
            switch (value.kind) {
                case CsKinds.ObjectExpression:
                    const objCodes = this.createObjectExpression(tab, value as ObjectExpression, ';', '');
                    codes.push(`${tab}var ${declaration.name} = ${objCodes[0].trimStart()}`);
                    codes.push(...objCodes.slice(1));
                    break;
            }
        }

        return codes;
    }
}

export class CsClass {
    name: string;
    access: string;
    partial: boolean;
    props: CsProperty[] = [];
    funcs: CsFunction[] = [];
    variables: CsVariable[] = [];

    constructor(
        name: string,
        access: string = 'public',
        partial: boolean = false
    ) {
        this.name = name;
        this.access = access;
        this.partial = partial;
    }

    public addProperty(name: string, type: string, propMap?: string) {
        this.props.push(new CsProperty(name, type, 'public', propMap));
    }

    public addFunction(func: CsFunction) {
        this.funcs.push(func);
    }

    public addVariable(variable: CsVariable) {
        this.variables.push(variable);
    }

    public format(tab: string): string[] {
        const codes: string[] = [];
        increaseIndex(true);
        codes.push(`${tab}${this.access}${this.partial ? ' partial' : ''} class ${this.name}`);
        codes.push(`${tab}{`);

        this.variables.forEach(x => {
            increaseIndex(false);
            codes.push(...x.format(tab + tab));
            codes.push('');
        });

        this.props.forEach(x => {
            increaseIndex(false);
            codes.push(...x.format(tab + tab));
            codes.push('');
        });

        this.funcs.forEach(x => {
            increaseIndex(false);
            codes.push(...x.format(tab + tab));
            codes.push('');
        });

        codes.push(`${tab}}`);
        return codes;
    }
}

export class CsBuilder {
    options: CsOptions;
    classes: { [key: string]: CsClass } = {};

    constructor(options: CsOptions) {
        this.options = options;
        init(options);
    }

    public addClass(name: string): CsClass {
        return this.classes[name] = new CsClass(name);
    }

    public addClassProperty(className: string, propName: string, propType: string) {
        this.classes[className].addProperty(propName, propType, this.options.propertyMap);
    }

    public addFunction(func: CsFunction) {
        const defaultCls = this.getDefaultClass();
        defaultCls.addFunction(func);
    }

    public addVariable(variable: CsVariable) {
        const defaultCls = this.getDefaultClass();
        defaultCls.addVariable(variable);
    }

    public format(): string {
        const codes: string[] = [];
        if (this.options.usings.length > 0) {
            codes.push(...this.options.usings);
            codes.push('');
        }
        codes.push(`namespace ${this.options.namespace}`);
        codes.push(`{`);
        for (const cls in this.classes) {
            codes.push(...this.classes[cls].format(this.options.defaultTab));
            codes.push('');
        }
        codes.push(`}`);
        this.trasnform(codes);
        return codes.join('\r\n');
    }

    private getDefaultClass(): CsClass {
        if (this.options.defaultClass in this.classes) {
            return this.classes[this.options.defaultClass];
        }
        return this.classes[this.options.defaultClass] = new CsClass(this.options.defaultClass);
    }

    private trasnform(codes: string[]) {
        for (let i = 0; i < codes.length; i++) {
            if (!this.options.transforms) continue;
            let code = codes[i];
            if (code == '') continue;
            const item = this.options.transforms.find(x => code.includes(x.source));
            if (!item) continue;
            if (item.regex) {
                codes[i] = code.replace(item.regex, item.target);
            } else {
                codes[i] = code.replace(item.source, item.target);
            }
        }
    }
}