/**
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file at https://github.com/ant-design-blazor/ant-design-blazor/blob/master/LICENSE
 */

import * as fs from 'fs-extra';
import * as less from 'less';

import * as path from 'path';

import { buildConfig } from '../build-config';

const lessPluginCleanCSS = require('less-plugin-clean-css');
const npmImportPlugin = require('less-plugin-npm-import');

async function compileLess(
  content: string,
  savePath: string,
  min: boolean,
  sub?: boolean,
  rootPath?: string
): Promise<void> {
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  const plugins: any[] = [];
  const lessOptions: Less.Options = { plugins, javascriptEnabled: true };

  if (min) {
    plugins.push(new lessPluginCleanCSS({ advanced: true }));
  }

  if (sub) {
    lessOptions.paths = [path.dirname(rootPath as string)];
    lessOptions.filename = rootPath;
    plugins.push(
      new npmImportPlugin({
        prefix: '~'
      })
    );
  }

  return less
    .render(content, lessOptions)
    .then(({ css }) => {
      return fs.writeFile(savePath, css);
    })
    .catch(err => Promise.reject(err));
}

const sourcePath = buildConfig.componentsDir;
const targetPath = buildConfig.publishDir;

export async function compile(): Promise<void | void[]> {
  const componentFolders = fs.readdirSync(sourcePath);
  const promiseList = [];

  for (const dir of componentFolders) {
    if (await fs.pathExists(`${sourcePath}/${dir}/style/index.less`)) {
      // Copy style files for each component.
      await fs.copy(`${sourcePath}/${dir}/style`, `${targetPath}/${dir}/style`);

      // Compile less files to CSS and delete the `entry.less` file.
      const buildFilePath = `${sourcePath}/${dir}/style/entry.less`;
      const componentLess = await fs.readFile(buildFilePath, { encoding: 'utf8' });
      if (await fs.pathExists(buildFilePath)) {
        // Rewrite `entry.less` file with `root-entry-name`
        const entryLessFileContent = needTransformStyle(componentLess)
          ? `@root-entry-name: default;\n${componentLess}`
          : componentLess;

        promiseList.push(
          compileLess(
            entryLessFileContent,
            path.join(targetPath, dir, 'style', `index.css`),
            false,
            true,
            buildFilePath
          )
        );
        promiseList.push(
          compileLess(
            entryLessFileContent,
            path.join(targetPath, dir, 'style', `index.min.css`),
            true,
            true,
            buildFilePath
          )
        );
      }
    }
  }

  // Copy concentrated less files.
  await fs.copy(path.resolve(sourcePath, 'style'), path.resolve(targetPath, 'style'));
  await fs.writeFile(`${targetPath}/components.less`, await fs.readFile(`${sourcePath}/components.less`));
  await fs.writeFile(`${targetPath}/ant-design-blazor.less`, await fs.readFile(`${sourcePath}/ant-design-blazor.less`));
  await fs.writeFile(
    `${targetPath}/ant-design-blazor.dark.less`,
    await fs.readFile(`${sourcePath}/ant-design-blazor.dark.less`)
  );
  await fs.writeFile(
    `${targetPath}/ant-design-blazor.aliyun.less`,
    await fs.readFile(`${sourcePath}/ant-design-blazor.aliyun.less`)
  );
  await fs.writeFile(
    `${targetPath}/ant-design-blazor.compact.less`,
    await fs.readFile(`${sourcePath}/ant-design-blazor.compact.less`)
  );

  await fs.writeFile(
    `${targetPath}/ant-design-blazor.variable.less`,
    await fs.readFile(`${sourcePath}/ant-design-blazor.variable.less`)
  );

  await fs.writeFile(
    `${targetPath}/ant-design-blazor.compactdark.less`,
    await fs.readFile(`${sourcePath}/ant-design-blazor.compactdark.less`)
  );

  // Compile concentrated less file to CSS file.
  const lessContent = `@import "${path.posix.join(targetPath, 'ant-design-blazor.less')}";`;
  promiseList.push(compileLess(lessContent, path.join(targetPath, 'ant-design-blazor.css'), false));
  promiseList.push(compileLess(lessContent, path.join(targetPath, 'ant-design-blazor.min.css'), true));

  // Compile the dark theme less file to CSS file.
  const darkLessContent = `@import "${path.posix.join(targetPath, 'ant-design-blazor.dark.less')}";`;
  promiseList.push(compileLess(darkLessContent, path.join(targetPath, 'ant-design-blazor.dark.css'), false));
  promiseList.push(compileLess(darkLessContent, path.join(targetPath, 'ant-design-blazor.dark.min.css'), true));

  // Compile the compact theme less file to CSS file.
  const compactLessContent = `@import "${path.posix.join(targetPath, 'ant-design-blazor.compact.less')}";`;
  promiseList.push(compileLess(compactLessContent, path.join(targetPath, 'ant-design-blazor.compact.css'), false));
  promiseList.push(compileLess(compactLessContent, path.join(targetPath, 'ant-design-blazor.compact.min.css'), true));

  // Compile the aliyun theme less file to CSS file.
  const aliyunLessContent = `@import "${path.posix.join(targetPath, 'ant-design-blazor.aliyun.less')}";`;
  promiseList.push(compileLess(aliyunLessContent, path.join(targetPath, 'ant-design-blazor.aliyun.css'), false));
  promiseList.push(compileLess(aliyunLessContent, path.join(targetPath, 'ant-design-blazor.aliyun.min.css'), true));

  // Compile the aliyun theme less file to CSS file.
  const variableLessContent = `@import "${path.posix.join(targetPath, 'ant-design-blazor.variable.less')}";`;
  promiseList.push(compileLess(variableLessContent, path.join(targetPath, 'ant-design-blazor.variable.css'), false));
  promiseList.push(compileLess(variableLessContent, path.join(targetPath, 'ant-design-blazor.variable.min.css'), true));

  const compactDarkLessContent = `@import "${path.posix.join(targetPath, 'ant-design-blazor.compactdark.less')}";`;
  promiseList.push(compileLess(compactDarkLessContent, path.join(targetPath, 'ant-design-blazor.compactdark.css'), false));
  promiseList.push(compileLess(compactDarkLessContent, path.join(targetPath, 'ant-design-blazor.compactdark.min.css'), true));


  // Compile css file that doesn't have component-specific styles.
  const cssIndexPath = path.join(sourcePath, 'style', 'entry.less');
  const cssIndex = await fs.readFile(cssIndexPath, { encoding: 'utf8' });
 
  // Rewrite `entry.less` file with `root-entry-name`
  const entryLessInStyle = needTransformStyle(cssIndex) ? `@root-entry-name: default;\n${cssIndex}` : cssIndex;

  promiseList.push(
    compileLess(entryLessInStyle, path.join(targetPath, 'style', 'index.css'), false, true, cssIndexPath)
  );
  promiseList.push(
    compileLess(entryLessInStyle, path.join(targetPath, 'style', 'index.min.css'), true, true, cssIndexPath)
  );
  return Promise.all(promiseList).catch(e => console.log(e));
}

function needTransformStyle(content: string): boolean {
  return (
    content.includes('../../style/index.less') || content.includes('./index.less') || content.includes('/entry.less')
  );
}
