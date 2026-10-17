const less = require('less');
const CleanCSS = require('clean-css');
const path = require('path');
const fs = require('fs-extra');
const { darkPaletteLess } = require('../build/dark-vars');
const { compactPaletteLess } = require('../build/compact-vars');
const { aliyunPaletteLess } = require('../build/aliyun-vars');
const themePath = path.join(__dirname, '../../site/AntDesign.Docs/styles.less');
const colorPalettePath = path.join(__dirname, '../../components/style/color/colorPalette.less');
const themeContent = `
@import '${themePath}';
`;

function generateTheme(vars, fileName) {

  return less.render(themeContent, {
    javascriptEnabled: true,
    modifyVars: {
      'hack': `true;@import '${colorPalettePath}';`,
      ...vars,
      '@site-markdown-code-bg': '@input-bg',
      '@site-text-color': '@heading-color'
    }
  }).then(data => {
    const css = new CleanCSS({ level: 2 }).minify(data.css).styles;
    return fs.outputFile(path.join(__dirname, `../../site/AntDesign.Docs/wwwroot/css/${fileName}`), css);
  }).catch(e => {
    console.log(e);
  });
}

function generateAllTheme() {
  return generateTheme(compactPaletteLess, 'compact.css')
    .then(() => generateTheme(darkPaletteLess, 'dark.css'))
    .then(() => generateTheme(aliyunPaletteLess, 'aliyun.css'))
    .then(() => generateTheme([], 'default.css'))
    ;
}

if (require.main === module) {
  generateAllTheme().then();
}

module.exports = () => generateAllTheme();
