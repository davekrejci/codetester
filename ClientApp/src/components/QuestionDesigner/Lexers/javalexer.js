"use strict";
const chevrotain = require("chevrotain");
const { allTokens } = require("./javatokens");

const Lexer = chevrotain.Lexer;
const JavaLexer = new Lexer(allTokens, {
  ensureOptimizations: true,
  skipValidations: getSkipValidations()
});

/**
 * Should Parser / Lexer Validations be skipped?
 *
 * By default (productive mode) the validations would be skipped to reduce parser initialization time.
 * But during development flows (e.g testing/CI) they should be enabled to detect possible issues.
 *
 * @returns {boolean}
 */
function getSkipValidations() {
    return (
      (process && // (not every runtime has a global `process` object
        process.env &&
        process.env["development"] === "enabled") === false
    );
  }

module.exports = JavaLexer;