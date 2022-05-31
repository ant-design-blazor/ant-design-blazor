/**
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file at https://github.com/NG-ZORRO/ant-design-blazor/blob/master/LICENSE
 */

import * as fs from 'fs-extra';

import * as path from 'path';

import { buildConfig } from '../build-config';

const sourcePath = buildConfig.publishDir;
const targetPath = path.join(buildConfig.publishDir, `src`);

export function copyStylesToSrc(): void {
  fs.mkdirsSync(targetPath);
  fs.copySync(path.resolve(sourcePath, `style`), path.resolve(targetPath, `style`));
  fs.copySync(path.resolve(sourcePath, `ant-design-blazor.css`), path.resolve(targetPath, `ant-design-blazor.css`));
  fs.copySync(path.resolve(sourcePath, `ant-design-blazor.min.css`), path.resolve(targetPath, `ant-design-blazor.min.css`));
  fs.outputFileSync(
    path.resolve(targetPath, `ant-design-blazor.less`),
    `@root-entry-name: default;
@import "../style/entry.less";
@import "../components.less";`
  );
}
