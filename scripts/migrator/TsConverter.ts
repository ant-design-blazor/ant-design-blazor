import * as ts from 'typescript';
import { ArrayExpression, CallExpression, CsBuilder, CsFunction, CsKinds, CsOptions, CsVariable, ObjectBinding, ObjectExpression, ConditionalExpression, ParameterType } from "./CsBuilder";

type Context<T> = {
    node: T;
    sourceFile: ts.SourceFile;
    typeChecker: ts.TypeChecker;
    csBuilder: CsBuilder;
}

function convertInterface(context: Context<ts.InterfaceDeclaration>) {
    // parse class name
    const name = context.node.name?.text;
    context.csBuilder.addClass(name);
    // parse properties
    context.node.members.forEach(x => {
        const memberName = x.name?.getText() || '';
        const memberType = context.typeChecker.getTypeAtLocation(x);
        const typeName = context.typeChecker.typeToString(memberType);
        context.csBuilder.addClassProperty(name, memberName, typeName);
    });
}

function convertType(context: Context<ts.TypeAliasDeclaration>) {
    // parse class name
    const name = context.node.name.getText();
    context.csBuilder.addClass(name);
    // parse properties
    const addProps = (members: any[]) => {
        members.forEach((x) => {
            const memberName = x.name?.getText() || '';
            const memberType = context.typeChecker.getTypeAtLocation(x);
            const typeName = context.typeChecker.typeToString(memberType);
            context.csBuilder.addClassProperty(name, memberName, typeName);
        });
    }
    const type = context.node.type as any;
    if (type.types) {
        type.types.forEach((type: any) => {
            if (type.kind === ts.SyntaxKind.TypeLiteral) {
                addProps(type.members);
            }
        });
    } else if (type.members) {
        addProps(type.members);
    }
}

function createObjectExpression(type: string, expression: ts.ObjectLiteralExpression, args?: string[]): ObjectExpression {
    const objectExpression: ObjectExpression = {
        kind: CsKinds.ObjectExpression,
        type: type,
        properties: [],
        args: args
    };

    const recursion = (objectExp: ObjectExpression, properties: ts.NodeArray<ts.ObjectLiteralElementLike>) => {
        for (const item of properties) {
            switch (item.kind) {
                case ts.SyntaxKind.PropertyAssignment:
                    const initializer = (item as any).initializer;
                    const fieldName = item.name?.getText() || '';
                    let fieldValue: string | ObjectExpression | ArrayExpression | CallExpression | ConditionalExpression = '';

                    if (initializer.expression && initializer.expression.kind === ts.SyntaxKind.PropertyAccessExpression) {
                        const propAccessExp = initializer.expression as ts.PropertyAccessExpression;
                        switch (propAccessExp.expression.kind) {
                            case ts.SyntaxKind.ArrayLiteralExpression:
                                fieldValue = createArrayExpression('', (propAccessExp.expression as any).elements, '.Join(",")');
                                break;
                            case ts.SyntaxKind.CallExpression:
                                fieldValue = createCallExpression('', '', propAccessExp.expression as any);
                                break;
                        }
                    } else {
                        switch (initializer.kind) {
                            case ts.SyntaxKind.Identifier:
                                fieldValue = initializer.getText();
                                break;
                            case ts.SyntaxKind.ObjectLiteralExpression:
                                fieldValue = { type: type, properties: [] }
                                recursion(fieldValue, initializer.properties);
                                break;
                            case ts.SyntaxKind.CallExpression:
                                fieldValue = createCallExpression('', '', initializer as ts.CallExpression);
                                break;
                            case ts.SyntaxKind.ArrayLiteralExpression:
                                fieldValue = createArrayExpression('', initializer.elements);
                                break;
                            case ts.SyntaxKind.ConditionalExpression:
                                fieldValue = createConditionalExpression(initializer as ts.ConditionalExpression);
                                break;
                            default:
                                fieldValue = initializer.getText() as string;
                                break;
                        }
                    }
                    objectExp.properties.push({ fieldName, fieldValue });
                    break;
                case ts.SyntaxKind.ShorthandPropertyAssignment:
                    const shortName = item.name?.getText() || '';
                    objectExp.properties.push({ fieldName: shortName, fieldValue: shortName });
                    break;
                case ts.SyntaxKind.SpreadAssignment:
                    switch (item.expression.kind) {
                        case ts.SyntaxKind.CallExpression:
                            const callExp = createCallExpression('', '', item.expression as ts.CallExpression);
                            objectExp.properties.push({ fieldName: `['...']`, fieldValue: callExp });
                            break;
                        case ts.SyntaxKind.ParenthesizedExpression:
                            const parentExp = item.expression as ts.ParenthesizedExpression;
                            switch (parentExp.expression.kind) {
                                case ts.SyntaxKind.ConditionalExpression:
                                    const conExp = createConditionalExpression(parentExp.expression as ts.ConditionalExpression);
                                    objectExp.properties.push({ fieldName: `['...']`, fieldValue: conExp });
                                    break
                            }
                            break;
                        default:
                            const exp = item.expression.getText();
                            objectExp.properties.push({ fieldName: `['...']`, fieldValue: exp });
                            break;
                    }
                    break;
                default:
                    break;
            }

        }
    };

    recursion(objectExpression, expression.properties);
    return objectExpression;
}

function createArrayExpression(type: string, elements: any[], endInsert?: string) {
    const arrayExpression: ArrayExpression = {
        kind: CsKinds.ArrayExpression,
        type: type,
        items: [],
        endInsert: endInsert,
    }
    elements.forEach((x: any) => {
        switch (x.kind) {
            case ts.SyntaxKind.ObjectLiteralExpression:
                const obj = createObjectExpression('', x);
                arrayExpression.items.push(obj);
                break;
            case ts.SyntaxKind.CallExpression:
                const callExp = createCallExpression('', '', x, '');
                arrayExpression.items.push(callExp);
                break;
            case ts.SyntaxKind.ArrayLiteralExpression:
                const arrExp = createArrayExpression('', x.elements);
                arrayExpression.items.push(arrExp);
                break;
            default:
                arrayExpression.items.push(x.getText());
                break;
        }
    });
    return arrayExpression;
}

function createObjectBinding(initializer: string, elements: any[]): ObjectBinding {
    const objectBinding: ObjectBinding = {
        kind: CsKinds.ObjectBinding,
        initializer: initializer || '',
        bindings: [],
    };
    elements.forEach((el: any) => {
        objectBinding.bindings.push({ name: el.name.getText(), propertyName: el.propertyName?.getText() });
    });

    return objectBinding;
}

function createConditionalExpression(exp: ts.ConditionalExpression): ConditionalExpression {
    const cond = exp.condition as any;
    const condition: ConditionalExpression = {
        kind: CsKinds.ConditionalExpression,
        left: cond.left.getText(),
        right: cond.right.getText(),
        operator: cond.operatorToken.getText()
    }
    const getWhenCond = (exp: ts.Expression) => {
        if (!exp) return undefined;
        switch (exp.kind) {
            case ts.SyntaxKind.ObjectLiteralExpression:
                return createObjectExpression('', exp as ts.ObjectLiteralExpression);
        }
    }
    condition.whenTrue = getWhenCond(exp.whenTrue);
    condition.whenFalse = getWhenCond(exp.whenFalse);
    return condition;
}

function createCallExpression(assignment: string, returnType: string, callExp: ts.CallExpression, returnFlag: string = ''): CallExpression {
    const callExpression: CallExpression = {
        kind: CsKinds.CallExpression,
        funcName: callExp.expression.getText(),
        assignment: assignment,
        paramaters: [],
        returnFlag: returnFlag
    };
    callExp.arguments.forEach(arg => {
        switch (arg.kind) {
            case ts.SyntaxKind.Identifier:
                callExpression.paramaters.push(arg.getText());
                break;
            case ts.SyntaxKind.ObjectLiteralExpression:
                const object = createObjectExpression(returnType, arg as ts.ObjectLiteralExpression);
                callExpression.paramaters.push(object);
                break;
            case ts.SyntaxKind.ArrowFunction:
                const func = createArrowFunction('', arg as ts.ArrowFunction, CsKinds.Func);
                callExpression.paramaters.push(func);
                break;
            case ts.SyntaxKind.CallExpression:
                const callExp = createCallExpression('', '', arg as ts.CallExpression);
                callExpression.paramaters.push(callExp);
                break;
            default:
                callExpression.paramaters.push(arg.getText());
                break;
        }
    });
    return callExpression;
}

function createArrowFunction(funcName: string, arrowFunc: ts.ArrowFunction, kind: CsKinds = CsKinds.Method): CsFunction {
    // const arrowFunc = declaration.initializer as ts.ArrowFunction;
    // const funcName = declaration.name.getText();
    const returnType = arrowFunc.type?.getText() || '';
    const parameters: ParameterType[] = []
    arrowFunc.parameters.forEach(x => {
        switch (x.name.kind) {
            case ts.SyntaxKind.ObjectBindingPattern:
                const bindingPattern = x.name as ts.ObjectBindingPattern;
                const p: ParameterType = { name: 'args', type: '', bindings: [] };
                bindingPattern.elements.forEach(y => {
                    p.bindings?.push({ name: y.name.getText(), propertyName: '' });
                });
                parameters.push(p);
                break;
            default:
                parameters.push({ name: x.name.getText(), type: x.type?.getText() || '', defaultValue: x.initializer?.getText() });
                break;
        }
    });
    const funcBody = arrowFunc.body as any;
    const statements: any[] = []
    if (funcBody.kind === ts.SyntaxKind.CallExpression) {
        statements.push(createCallExpression('', '', funcBody, 'return '));
    }
    // if func body is expression
    else if (funcBody.expression) {
        // Object Expression, eg: () => {};
        switch (funcBody.expression.kind) {
            case ts.SyntaxKind.ObjectLiteralExpression:
                statements.push(createObjectExpression(returnType, funcBody.expression));
                break;
        }
    }
    // if func body is statement
    else if (funcBody.statements) {
        funcBody.statements.forEach((x: any) => {
            switch (x.kind) {
                case ts.SyntaxKind.VariableStatement:
                    const declaration = (x as ts.VariableStatement).declarationList.declarations[0];
                    if (declaration.name.kind === ts.SyntaxKind.ObjectBindingPattern) {
                        if (declaration.initializer?.kind === ts.SyntaxKind.CallExpression) {
                            /**
                             * 这里无法直接映射成C#调用方式
                             * 源代码示例：
                             * const { propA, propB } = getProps(a, b);
                             * 只能转换成
                             * var xxx = getProps(a, b);
                             * var propA = xxx.PropA;
                             * var propB = xxx.PropB;
                             */
                            const objectBinding = createObjectBinding('unknown', (declaration.name as any).elements);
                            objectBinding.callExp = createCallExpression('unknown', '', declaration.initializer as ts.CallExpression);
                            statements.push(objectBinding);
                        } else {
                            /**
                             * 对象绑定
                             * 示例：
                             * const { propA, propB } = token;
                             * 转换示例：
                             * var propA = token.PropA;
                             * var propB = token.PropB;
                             */
                            const initializerName = declaration.initializer?.getText() || '';
                            const objectBinding = createObjectBinding(initializerName, (declaration.name as any).elements)
                            statements.push(objectBinding);
                        }
                    } else if (declaration.initializer?.kind === ts.SyntaxKind.CallExpression) {
                        const assignment = declaration.name.getText();
                        const callExp = declaration.initializer as ts.CallExpression;
                        statements.push(createCallExpression(assignment, returnType, callExp));
                    } else if (declaration.initializer?.kind === ts.SyntaxKind.ArrowFunction) {
                        const name = declaration.name.getText();
                        const arrFunc = createArrowFunction(name, declaration.initializer as ts.ArrowFunction, CsKinds.Func);
                        statements.push(arrFunc);
                    } else if (declaration.initializer?.kind === ts.SyntaxKind.ObjectLiteralExpression) {
                        const objExp = createObjectExpression('', declaration.initializer as ts.ObjectLiteralExpression);
                        statements.push({ kind: CsKinds.VariableDeclaration, name: declaration.name.getText(), value: objExp });
                    } else {
                        statements.push({ kind: CsKinds.VariableDeclaration, name: declaration.name.getText(), value: declaration.initializer?.getText() });
                    }
                    break;
                case ts.SyntaxKind.ReturnStatement:
                    const rs = x as ts.ReturnStatement;
                    if (rs.expression?.kind === ts.SyntaxKind.ObjectLiteralExpression) {
                        const objectExpression = createObjectExpression(returnType, rs.expression as ts.ObjectLiteralExpression);
                        statements.push(objectExpression);
                    } else if (rs.expression?.kind === ts.SyntaxKind.ArrayLiteralExpression) {
                        statements.push(createArrayExpression(returnType, (rs.expression as any).elements));
                    } else if (rs.expression?.kind === ts.SyntaxKind.CallExpression) {
                        statements.push(createCallExpression('', '', rs.expression as ts.CallExpression, 'return '));
                    } else {
                        statements.push({ kind: CsKinds.Identifier, text: x.getText() });
                    }
                    break;
                default:
                    const codes = x.getText().split(/\r?\n/);
                    statements.push({ kind: CsKinds.Block, blocks: codes });
                    break;
            }
        });
    }
    else if (funcBody.elements) {
        statements.push(createArrayExpression(returnType, funcBody.elements));
    }
    return new CsFunction(funcName, parameters, returnType, { statements }, kind);
}

function convertVariableStatement(context: Context<ts.VariableStatement>) {
    const declaration = context.node.declarationList.declarations[0];
    switch (declaration.initializer?.kind) {
        case ts.SyntaxKind.ArrowFunction:
            const funcName = declaration.name.getText();
            const func = createArrowFunction(funcName, declaration.initializer as ts.ArrowFunction);
            context.csBuilder.addFunction(func);
            break;
        case ts.SyntaxKind.NewExpression:
            const name = declaration.name.getText();
            const initializer = declaration.initializer as ts.NewExpression;
            const type = initializer.expression.getText();
            let value: any;
            let args: string[] = [];
            initializer.arguments?.forEach(x => {
                if (x.kind === ts.SyntaxKind.ObjectLiteralExpression) {
                    value = createObjectExpression(type, x as ts.ObjectLiteralExpression, args);
                } else if (x.kind === ts.SyntaxKind.StringLiteral) {
                    args.push(x.getText());
                }
            });
            context.csBuilder.addVariable(new CsVariable(name, type, value));
            break;
        default:
            break;
    }
}

function convertExportAssignment(context: Context<ts.ExportAssignment>) {
    const func = new CsFunction("ExportDefault", [], "UseComponentStyleResult", { statements: [] }, CsKinds.Method);
    switch (context.node.expression.kind) {
        case ts.SyntaxKind.CallExpression:
            const callExp = context.node.expression as ts.CallExpression;
            const funcName = callExp.expression.getText();
            const parameters: any[] = [];
            callExp.arguments.forEach(x => {
                switch (x.kind) {
                    case ts.SyntaxKind.ArrowFunction:
                        const funBody = createArrowFunction('', x as ts.ArrowFunction, CsKinds.Func);
                        parameters.push(funBody);
                        break;
                    case ts.SyntaxKind.StringLiteral:
                        parameters.push(x.getText());
                        break;
                    case ts.SyntaxKind.ObjectLiteralExpression:
                        const objectExp = createObjectExpression('', x as ts.ObjectLiteralExpression);
                        parameters.push(objectExp);
                        break;
                    default:
                        parameters.push(x.getText());
                        break;
                }
            });
            const callExpression: CallExpression = {
                kind: CsKinds.CallExpression,
                assignment: '',
                funcName: funcName,
                paramaters: parameters,
                returnFlag: 'return '
            };
            func.body.statements.push(callExpression);
            break;
        case ts.SyntaxKind.ArrowFunction:
            const arrayFunc = createArrowFunction('', context.node.expression as ts.ArrowFunction);
            func.body.statements = arrayFunc.body.statements;
            break;
    }
    if (func.body.statements.length > 0) {
        context.csBuilder.addFunction(func);
    }
}

function convertFunctionDeclaration(context: Context<ts.FunctionDeclaration>) {
    const funcName = context.node.name?.getText() || '';
    const func = createArrowFunction(funcName, context.node as any);
    context.csBuilder.addFunction(func);
}

export function convert(files: string[], csOptions: CsOptions): string {
    const options: ts.CompilerOptions = {};
    let program = ts.createProgram(files, options);
    const context = {
        typeChecker: program.getTypeChecker(),
        csBuilder: new CsBuilder(csOptions),
    } as any;

    for (const file of files) {
        const sourceFile = program.getSourceFile(file) as any;
        context.sourceFile = sourceFile;
        ts.forEachChild(sourceFile, (node: ts.Node) => {
            // interface
            if (ts.isInterfaceDeclaration(node)) {
                convertInterface({ ...context, node, });
            }
            // type
            else if (ts.isTypeAliasDeclaration(node)) {
                convertType({ ...context, node, });
            }
            // variable statement
            else if (ts.isVariableStatement(node)) {
                convertVariableStatement({ ...context, node, });
            }
            // export assignment
            else if (ts.isExportAssignment(node)) {
                convertExportAssignment({ ...context, node });
            }
            // function declaration
            else if (ts.isFunctionDeclaration(node)) {
                convertFunctionDeclaration({ ...context, node });
            }
        });
    }

    return context.csBuilder.format();
}


