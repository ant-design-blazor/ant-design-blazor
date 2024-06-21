## Css Migrator
Migrator can migrate antd style to v5.

### Usage
In the root dir, exec migrate command.
```sh
npm run migrate
```

### Config
```ts
const config = {
    version: '5.11.4',
    gitHash: '3fbed04e4b03ef1754a78a9245a2bb59f8b72fd1',
    remotePath: 'https://github.com/ant-design/ant-design.git',
    localPath: '../ant-design'
}
```

| Property   | Description                                         |
| ---------- | --------------------------------------------------- |
| version    | release version of antd, for display purposes only. |
| gitHash    | git hash of specified version.                      |
| remotePath | antd git url.                                       |
| localPath  | local path to save antd code.                       |

If you want to specify the version, change the githash. Migrator will check out the code specified githash in the configuration.

### Cssincs Converter
Converter use ast to parse ts code, and convert it to csharp. You can check the ast with [ast viewer](https://ts-ast-viewer.com).
