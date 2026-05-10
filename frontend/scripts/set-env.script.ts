import fs from 'node:fs';
import path from 'node:path';

import chalk from 'chalk';
import dotenv from 'dotenv';

dotenv.config();

const envFileContent = `
export const environment = {
    production: false,
    apiHost: '${process.env.API_HOST}'
};
`;
const targetPath = path.join(
    process.cwd(),
    'src/environments/environment.ts'
)

fs.writeFileSync(targetPath, envFileContent);
console.log(chalk.green('environment.ts generated'));
