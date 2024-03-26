/**
 * .eslint.js
 *
 * ESLint configuration file.
 */

module.exports = {
  root: true,
  parser: "vue-eslint-parser",
  parserOptions: {
    parser: "@typescript-eslint/parser",
  },
  env: {
    node: true,
  },
  extends: [
    "plugin:vue/vue3-recommended",
    "eslint:recommended",
    "@vue/eslint-config-typescript/recommended",
    "@vue/typescript/recommended",
    "plugin:prettier/recommended",
    "prettier",
  ],
  plugins: ["@typescript-eslint", "prettier"],
  rules: {
    "prettier/prettier": "error",
    "vue/multi-word-component-names": "off",
    "@typescript-eslint/explicit-module-boundary-types": "error",
    "@typescript-eslint/no-explicit-any": "warn",
    "@typescript-eslint/no-unused-vars": "error",
    "@typescript-eslint/no-var-requires": "error",
    "@typescript-eslint/no-empty-function": "error",
  },
};
