// @ts-check
const eslint = require('@eslint/js');
const { defineConfig } = require('eslint/config');
const tseslint = require('typescript-eslint');
const angular = require('angular-eslint');
const unusedImports = require('eslint-plugin-unused-imports');
const simpleImportSort = require('eslint-plugin-simple-import-sort');

module.exports = defineConfig([
    {
        files: ['**/*.ts'],
        plugins: {
            'unused-imports': unusedImports,
            'simple-import-sort': simpleImportSort
        },
        extends: [
            eslint.configs.recommended,
            tseslint.configs.recommended,
            tseslint.configs.stylistic,
            angular.configs.tsRecommended,
        ],
        processor: angular.processInlineTemplates,
        rules: {
            curly: 'error',
            eqeqeq: ['error', 'always'],
            quotes: ['error', 'single', { allowTemplateLiterals: true }],
            'max-classes-per-file': ['error', 1],
            'prefer-const': 'error',
            'padding-line-between-statements': [
                'error',
                {
                    blankLine: 'always',
                    prev: '*',
                    next: 'return',
                },
            ],
            'unused-imports/no-unused-imports': 'warn',
            'unused-imports/no-unused-vars': [
                'warn',
                {
                    vars: 'all',
                    varsIgnorePattern: '^_',
                    args: 'after-used',
                    argsIgnorePattern: '^_',
                },
            ],
            'simple-import-sort/imports': 'error',
            'simple-import-sort/exports': 'error',
            '@angular-eslint/directive-selector': [
                'error',
                {
                    type: 'attribute',
                    prefix: '',
                    style: 'camelCase',
                },
            ],
            '@angular-eslint/component-selector': [
                'error',
                {
                    type: 'element',
                    prefix: '',
                    style: 'kebab-case',
                },
            ],
            '@typescript-eslint/member-ordering': [
                'error',
                {
                    default: [
                        ['public-static-get', 'public-static-set'],
                        ['protected-static-get', 'protected-static-set'],
                        ['private-static-get', 'private-static-set'],
                        'public-static-field',
                        'protected-static-field',
                        'private-static-field',
                        'public-static-method',
                        'protected-static-method',
                        'private-static-method',
                        ['public-abstract-get', 'public-abstract-set'],
                        ['protected-abstract-get', 'protected-abstract-set'],
                        'public-abstract-field',
                        'protected-abstract-field',
                        ['public-decorated-get', 'public-decorated-set'],
                        'public-decorated-field',
                        ['protected-decorated-get', 'protected-decorated-set'],
                        'protected-decorated-field',
                        ['private-decorated-get', 'private-decorated-set'],
                        'private-decorated-field',
                        ['public-get', 'public-set'],
                        ['protected-get', 'protected-set'],
                        ['private-get', 'private-set'],
                        'public-field',
                        'protected-field',
                        'private-field',
                        'constructor',
                        'public-abstract-method',
                        'protected-abstract-method',
                        'public-decorated-method',
                        'protected-decorated-method',
                        'private-decorated-method',
                        'public-method',
                        'protected-method',
                        'private-method',
                    ],
                },
            ],
            '@typescript-eslint/naming-convention': [
                'error',
                {
                    selector: 'enumMember',
                    format: ['PascalCase'],
                    custom: {
                        regex: '^[^А-ЯЁа-яё]*$',
                        match: true,
                    },
                },
                {
                    selector: 'default',
                    format: ['camelCase'],
                    custom: {
                        regex: '^[^А-ЯЁа-яё]*$',
                        match: true,
                    },
                },
                {
                    selector: ['classProperty', 'parameterProperty'],
                    format: ['camelCase'],
                    modifiers: ['private'],
                    prefix: ['_'],
                    custom: {
                        regex: '^[^А-ЯЁа-яё]*$',
                        match: true,
                    },
                },
                {
                    selector: ['classProperty'],
                    modifiers: ['public', 'static', 'readonly'],
                    format: ['camelCase', 'UPPER_CASE'],
                    custom: {
                        regex: '^[^А-ЯЁа-яё]*$',
                        match: true,
                    },
                },
                {
                    selector: 'interface',
                    format: ['PascalCase'],
                    custom: {
                        regex: '^I[A-Z][^А-Яа-я]*$',
                        match: true,
                    },
                },
                {
                    selector: 'objectLiteralProperty',
                    format: null,
                    custom: {
                        regex: '^[^А-ЯЁа-яё]*$',
                        match: true,
                    },
                },
                {
                    selector: 'typeLike',
                    format: ['PascalCase'],
                    custom: {
                        regex: '^[^А-ЯЁа-яё]*$',
                        match: true,
                    },
                },
                {
                    selector: ['variable'],
                    modifiers: ['const', 'exported'],
                    format: ['camelCase', 'UPPER_CASE'],
                    custom: {
                        regex: '^[^А-ЯЁа-яё]*$',
                        match: true,
                    },
                },
            ],
            '@typescript-eslint/no-inferrable-types': [
                'error',
                {
                    ignoreProperties: true,
                    ignoreParameters: true,
                },
            ],
            '@typescript-eslint/no-shadow': 'error',
            '@typescript-eslint/explicit-member-accessibility': [
                'error',
                {
                    accessibility: 'explicit',
                    overrides: {
                        constructors: 'no-public',
                    },
                },
            ],
            '@typescript-eslint/array-type': [
                'error',
                {
                    default: 'array-simple',
                },
            ],
            '@typescript-eslint/explicit-function-return-type': 'error',
            '@angular-eslint/no-conflicting-lifecycle': 'error',
            '@angular-eslint/no-input-rename': 'error',
            '@angular-eslint/no-inputs-metadata-property': 'error',
            '@angular-eslint/no-output-native': 'error',
            '@angular-eslint/no-output-on-prefix': 'error',
            '@angular-eslint/no-output-rename': 'error',
            '@angular-eslint/no-outputs-metadata-property': 'error',
            '@angular-eslint/use-lifecycle-interface': 'error',
            '@angular-eslint/use-pipe-transform-interface': 'error',
            '@angular-eslint/prefer-on-push-component-change-detection': 'error',
        },
    },
    {
        files: ['**/*.html'],
        extends: [angular.configs.templateRecommended, angular.configs.templateAccessibility],
        rules: {},
    },
]);
