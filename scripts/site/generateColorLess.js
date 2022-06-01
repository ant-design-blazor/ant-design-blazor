#!/usr/bin/env node
const path = require('path');
const { generateTheme } = require('antd-theme-generator');

const options = {
  stylesDir: path.join(__dirname, '../../site/AntDesign.Docs/'),
  antdStylesDir: path.join(__dirname, '../../components'),
  varFile: path.join(__dirname, '../../components/style/themes/default.less'),
  mainLessFile: path.join(__dirname, '../../site/AntDesign.Docs/styles.less'),
  themeVariables: [
    '@primary-color',
  ],
  outputFilePath: path.join(__dirname, '../../site/AntDesign.Docs/wwwroot/color.less'),
};

if (require.main === module) {
  generateTheme(options);
}

module.exports = () => generateTheme(options);