/* eslint-disable no-await-in-loop, no-console */
const chalk = require('chalk');
const { spawn } = require('child_process');
const jsdom = require('jsdom');
const jQuery = require('jquery');
const fetch = require('node-fetch');
const open = require('open');
const fs = require('fs-extra');
const path = require('path');
const simpleGit = require('simple-git/promise');
const inquirer = require('inquirer');

const { JSDOM } = jsdom;
const { window } = new JSDOM();
const { document } = new JSDOM('').window;
global.document = document;

const $ = jQuery(window);

const QUERY_TITLE = '.gh-header-title .js-issue-title';
const QUERY_DESCRIPTION_LINES = '.comment-body table tbody tr';
const QUERY_AUTHOR = '.timeline-comment-header .author:first';
// https://github.com/orgs/ant-design-blazor/teams/collaborators/members
const MAINTAINERS = [
  'elderjames',
  'mutouzdl',
  'TimChen44',
  'zxyao145',
  'Brian-Ding',
  '1002527441',
  'Epictek',
  'anddrzejb',
  'zonciu',
  'yoli799480165',
  'rabbitism',
].map(author => author.toLowerCase());

const cwd = process.cwd();
const git = simpleGit(cwd);

// Helper: perform fetch while temporarily disabling common proxy env vars to avoid local proxy errors
// This helps when a misconfigured local proxy (or environment) redirects connections to 127.0.0.1:443.
const proxyEnvKeys = [
  'HTTP_PROXY',
  'http_proxy',
  'HTTPS_PROXY',
  'https_proxy',
  'ALL_PROXY',
  'all_proxy',
  'NO_PROXY',
  'no_proxy',
];
async function safeFetch(url, options = {}, timeout = 10000) {
  const saved = {};
  proxyEnvKeys.forEach(k => { saved[k] = process.env[k]; delete process.env[k]; });

  const controller = new AbortController();
  const id = setTimeout(() => controller.abort(), timeout);
  try {
    const res = await fetch(url, Object.assign({}, options, { signal: controller.signal }));
    clearTimeout(id);
    return res;
  } finally {
    clearTimeout(id);
    // restore saved env vars
    proxyEnvKeys.forEach(k => {
      if (typeof saved[k] !== 'undefined') process.env[k] = saved[k];
      else delete process.env[k];
    });
  }
}

// Helper: fetch PR metadata. Prefer GitHub API when GITHUB_TOKEN is set (more reliable).
async function getPRInfo(pr) {
  const repoBase = 'ant-design-blazor/ant-design-blazor';
  const token = process.env.GITHUB_TOKEN || process.env.GH_TOKEN;

  // attempt GitHub API first if token available
  if (token) {
    try {
      const apiUrl = `https://api.github.com/repos/${repoBase}/pulls/${pr}`;
      const res = await safeFetch(apiUrl, {
        headers: {
          Accept: 'application/vnd.github.v3+json',
          Authorization: `token ${token}`,
          'User-Agent': 'print-changelog-script',
        },
      }, 10000);
      if (res && res.ok) {
        const json = await res.json();
        const title = json.title || '';
        const author = (json.user && json.user.login) || '';
        const body = json.body || '';

        // Try to heuristically extract English/Chinese sections from markdown body
        const englishMatch = body.match(/🇺🇸[\s\S]*?(?=🇨🇳|$)/);
        const chineseMatch = body.match(/🇨🇳[\s\S]*?(?=🇺🇸|$)/);

        const clean = s => (s || '').replace(/[`\|\*]/g, '').trim();
        const english = clean(englishMatch ? englishMatch[0].replace(/🇺🇸/, '') : '') || title;
        const chinese = clean(chineseMatch ? chineseMatch[0].replace(/🇨🇳/, '') : '') || title;

        return { title, author, english, chinese };
      }
    } catch (err) {
      // Continue to HTML scrape fallback below
      console.warn(chalk.yellow(`  ⚠️  GitHub API request failed for PR #${pr}: ${err.code || err.message}`));
    }
  }

  // Fallback: fetch PR HTML page and parse (older behavior)
  try {
    const res = await safeFetch(`https://github.com/ant-design-blazor/ant-design-blazor/pull/${pr}`, {}, 15000);
    if (!res || res.url.includes('/issues/')) return null;
    const html = await res.text();
    const $html = $(html);
    const prTitle = $html.find(QUERY_TITLE).text().trim();
    const prAuthor = $html.find(QUERY_AUTHOR).text().trim();
    const prLines = $html.find(QUERY_DESCRIPTION_LINES);

    const lines = [];
    prLines.each(function getDesc() {
      lines.push({
        text: $(this).text().trim(),
        element: $(this),
      });
    });

    const english = getDescription(lines.find(line => line.text.includes('🇺🇸 English')));
    const chinese = getDescription(lines.find(line => line.text.includes('🇨🇳 Chinese')));

    return {
      title: prTitle || prTitle,
      author: prAuthor || prAuthor,
      english: english || chinese || prTitle,
      chinese: chinese || english || prTitle,
    };
  } catch (err) {
    // Let caller handle fallback
    throw err;
  }
}

function getDescription(entity) {
  if (!entity) {
    return '';
  }
  const descEle = entity.element.find('td:last');
  let htmlContent = descEle.html();
  htmlContent = htmlContent.replace(/<code>([^<]*)<\/code>/g, '`$1`');
  return htmlContent.trim();
}

async function printLog() {
  const tags = await git.tags();
  const { fromVersion } = await inquirer.prompt([
    {
      type: 'list',
      name: 'fromVersion',
      message: '🏷  Please choose tag to compare with current branch:',
      choices: tags.all.reverse().slice(0, 10),
    },
  ]);
  let { toVersion } = await inquirer.prompt([
    {
      type: 'list',
      name: 'toVersion',
      message: `🔀 Please choose branch to compare with ${chalk.magenta(fromVersion)}:`,
      choices: ['master', 'feature', 'custom input ⌨️'],
    },
  ]);

  if (toVersion.startsWith('custom input')) {
    const result = await inquirer.prompt([
      {
        type: 'input',
        name: 'toVersion',
        message: `🔀 Please input custom git hash id or branch name to compare with ${chalk.magenta(
          fromVersion,
        )}:`,
        default: 'master',
      },
    ]);
    toVersion = result.toVersion;
  }

  if (!/\d+\.\d+\.\d+/.test(fromVersion)) {
    console.log(chalk.red(`🤪 tag (${chalk.magenta(fromVersion)}) is not valid.`));
  }

  const logs = await git.log({ from: fromVersion, to: toVersion });

  // Quick connectivity check to GitHub: if network unavailable, we will fallback to local-only mode
  let online = true;
  try {
    // lightweight request
    // eslint-disable-next-line no-await-in-loop
    const check = await safeFetch('https://github.com/', { method: 'HEAD' }, 5000);
    if (!check || !check.ok) online = false;
  } catch (e) {
    online = false;
    console.warn(chalk.yellow('⚠️  Network check failed, will use local git data only for changelog.'));
  }

  let prList = [];

  for (let i = 0; i < logs.all.length; i += 1) {
    const { message, body, hash, author_name: author } = logs.all[i];

    const text = `${message} ${body}`;

    const match = text.match(/#\d+/g);
    const prs = (match || []).map(pr => pr.slice(1));
    const validatePRs = [];

    console.log(
      `[${i + 1}/${logs.all.length}]`,
      hash.slice(0, 6),
      '-',
      prs.length ? prs.map(pr => `#${pr}`).join(',') : '?',
    );
    for (let j = 0; j < prs.length; j += 1) {
      const pr = prs[j];

      // Try to fetch PR page to parse title/description. If network fails (offline),
      // fallback to using local git commit info so changelog can still be generated.
      if (online) {
        try {
          // Prefer API or safe fetch parsing via getPRInfo
          const prMeta = await getPRInfo(pr);
          if (!prMeta) {
            console.warn(chalk.yellow(`  ⚠️  PR #${pr} returned no data (maybe redirected). Falling back to local git data.`));
            const fallbackTitle = message;
            const fallbackAuthor = author || 'unknown';
            validatePRs.push({
              pr,
              hash,
              title: fallbackTitle,
              author: fallbackAuthor,
              english: fallbackTitle,
              chinese: fallbackTitle,
            });
          } else {
            const { title: prTitle, author: prAuthor, english, chinese } = prMeta;
            if (english) {
              console.log(`  🇨🇳  ${english}`);
            }
            if (chinese) {
              console.log(`  🇺🇸  ${chinese}`);
            }
            validatePRs.push({
              pr,
              hash,
              title: prTitle,
              author: prAuthor,
              english: english || chinese || prTitle,
              chinese: chinese || english || prTitle,
            });
          }
        } catch (err) {
          // Network/parsing error — fallback to local git information
          console.warn(chalk.yellow(`  ⚠️  Could not fetch PR #${pr} (${err.code || err.message}). Falling back to local git data.`));
          const fallbackTitle = message;
          const fallbackAuthor = author || 'unknown';
          validatePRs.push({
            pr,
            hash,
            title: fallbackTitle,
            author: fallbackAuthor,
            english: fallbackTitle,
            chinese: fallbackTitle,
          });
        }
      } else {
        // Offline mode: use commit info only
        console.log(chalk.gray(`  ↪ offline: using local data for PR #${pr}`));
        const fallbackTitle = message;
        const fallbackAuthor = author || 'unknown';
        validatePRs.push({
          pr,
          hash,
          title: fallbackTitle,
          author: fallbackAuthor,
          english: fallbackTitle,
          chinese: fallbackTitle,
        });
      }
    }

    if (validatePRs.length === 1) {
      console.log(chalk.cyan(' - Match PR:', `#${validatePRs[0].pr}`));
      prList = prList.concat(validatePRs);
    } else if (message.includes('docs:')) {
      console.log(chalk.cyan(' - Skip document!'));
    } else {
      console.log(chalk.yellow(' - Miss match!'));
      prList.push({
        hash,
        title: message,
        author,
        english: message,
        chinese: message,
      });
    }
  }

  console.log('\n', chalk.green('Done. Here is the log:'));

  function printPR(lang, postLang) {
    prList.forEach(entity => {
      const { pr, author, hash, title } = entity;
      if (pr) {
        const str = postLang(entity[lang]);
        let icon = '';
        if (str.toLowerCase().includes('fix') || str.includes('修复')) {
          icon = '🐞';
        }

        let authorText = '';
        if (!MAINTAINERS.includes(author.toLowerCase())) {
          authorText = ` [@${author}](https://github.com/${author})`;
        }

        console.log(
          `- ${icon} ${str}[#${pr}](https://github.com/ant-design-blazor/ant-design-blazor/pull/${pr})${authorText}`,
        );
      } else {
        console.log(
          `🆘 Miss Match: ${title} -> https://github.com/ant-design-blazor/ant-design-blazor/commit/${hash}`,
        );
      }
    });
  }

  // Chinese
  console.log('\n');
  console.log(chalk.yellow('🇨🇳 Chinese changelog:'));
  console.log('\n');
  printPR('chinese', chinese =>
    chinese[chinese.length - 1] === '。' || !chinese ? chinese : `${chinese}。`,
  );

  console.log('\n-----\n');

  // English
  console.log(chalk.yellow('🇺🇸 English changelog:'));
  console.log('\n');
  printPR('english', english => {
    english = english.trim();
    if (english[english.length - 1] !== '.' || !english) {
      english = `${english}.`;
    }
    if (english) {
      return `${english} `;
    }
    return '';
  });

  // Preview editor generate
  // Web source: https://github.com/ant-design/antd-changelog-editor
  let html = fs.readFileSync(path.join(__dirname, 'previewEditor', 'template.html'), 'utf8');
  html = html.replace('// [Replacement]', `window.changelog = ${JSON.stringify(prList)};`);
  fs.writeFileSync(path.join(__dirname, 'previewEditor', 'index.html'), html, 'utf8');

  // Start preview
  const ls = spawn(
    'npx',
    ['http-server', path.join(__dirname, 'previewEditor'), '-c-1', '-p', '2893'],
    {
      shell: true,
    },
  );
  ls.stdout.on('data', data => {
    console.log(data.toString());
  });

  console.log(chalk.green('Start changelog preview editor...'));
  setTimeout(() => {
    open('http://localhost:2893/');
  }, 1000);
}

printLog();

