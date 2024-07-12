import { convert } from "./TsConverter";
import * as fs from 'fs';
import * as path from 'path';
import { components } from "./Components";
import { writeAllText } from "./Util";
import { simpleGit, SimpleGit } from 'simple-git';
import { genComponentStyleTests } from "./StyleTests";
import { genToken } from "./Token";

const config = {
    version: '5.11.4',
    gitHash: '3fbed04e4b03ef1754a78a9245a2bb59f8b72fd1',
    remotePath: 'https://github.com/ant-design/ant-design.git',
    localPath: '../ant-design'
}

/**
 * git clone ant-design code to local dir, if local dir not exists.
 */
async function clone(): Promise<void> {
    const localPath = path.resolve(config.localPath);
    let git: SimpleGit;
    if (!fs.existsSync(localPath)) {
        fs.mkdirSync(localPath, { recursive: true });
        git = simpleGit(localPath);
        console.log(`git clone: ${config.remotePath}`);
        await git.clone(config.remotePath, localPath);
    } else {
        git = simpleGit(localPath);
    }
    const isRepo = await git.checkIsRepo();
    if (!isRepo) return;
    const commitHash = await git.revparse('HEAD');
    console.log(`Current commithash: ${commitHash}`);
    if (commitHash !== config.gitHash) {
        // checkout specify commithash
        console.log(`Checkout commithash: ${config.gitHash}`);
        await git.checkout(config.gitHash);
    }
    console.log(`clone ant-design ${config.version} code ok.`);
}

/**
 * remove style files.
 */
function clean() {
    // clean generated style file.
    components.forEach(x => {
        try {
            console.log(`remove style file: ${x.dist}`);
            fs.rmSync(x.dist);
        } catch (error) {
        }
    });
}

/**
 * convert ts style code to cs code.
 */
function converTsStyle() {
    components.forEach(component => {
        console.log(`Migrate component ${component.name} style.`);
        const src = component.src.map(x => path.resolve(path.join(config.localPath, x)));
        const dist = path.resolve(component.dist);
        const content = convert(src, component.csOptions);
        writeAllText(dist, content);
    });
}

async function migrate() {
    // await clone();
    // converTsStyle();
    // genComponentStyleTests();
    genToken();
}

migrate();