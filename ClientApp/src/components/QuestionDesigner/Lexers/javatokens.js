"use strict";
const { createToken: createTokenOrg, Lexer } = require("chevrotain");
const camelCase = require("lodash/camelCase");

let chars;
// A little mini DSL for easier lexer definition.
const fragments = {};
try {
  chars = require("./unicodesets");
} catch (e) {
  throw Error(
    "unicodesets.js file could not be found. Did you try to run the command: yarn run build ?"
  );
}

function inlineFragments(def) {
  let inlinedDef = def;
  Object.keys(fragments).forEach(prevFragmentName => {
    const prevFragmentDef = fragments[prevFragmentName];
    const templateRegExp = new RegExp(`{{${prevFragmentName}}}`, "g");
    inlinedDef = inlinedDef.replace(templateRegExp, prevFragmentDef);
  });
  return inlinedDef;
}

function FRAGMENT(name, def) {
  fragments[name] = inlineFragments(def);
}

function MAKE_PATTERN(def, flags) {
  const inlinedDef = inlineFragments(def);
  return new RegExp(inlinedDef, flags);
}

// The order of fragments definitions is important
FRAGMENT("Digits", "[0-9]([0-9_]*[0-9])?");
FRAGMENT("ExponentPart", "[eE][+-]?{{Digits}}");
FRAGMENT("HexDigit", "[0-9a-fA-F]");
FRAGMENT("HexDigits", "{{HexDigit}}(({{HexDigit}}|'_')*{{HexDigit}})?");
FRAGMENT("FloatTypeSuffix", "[fFdD]");
FRAGMENT("LineTerminator", "(\\x0A|(\\x0D(\\x0A)?))");
FRAGMENT("UnicodeMarker", "uu*");
FRAGMENT("UnicodeEscape", "\\\\{{UnicodeMarker}}{{HexDigit}}{4}");
FRAGMENT("RawInputCharacter", "\\\\{{UnicodeMarker}}[0-9a-fA-F]{4}");
FRAGMENT("UnicodeInputCharacter", "({{UnicodeEscape}}|{{RawInputCharacter}})");
FRAGMENT("OctalDigit", "[0-7]");
FRAGMENT("ZeroToThree", "[0-3]");
FRAGMENT(
  "OctalEscape",
  "\\\\({{OctalDigit}}|{{ZeroToThree}}?{{OctalDigit}}{2})"
);
FRAGMENT("EscapeSequence", "\\\\[btnfr\"'\\\\]|{{OctalEscape}}");
// Not using InputCharacter terminology there because CR and LF are already captured in EscapeSequence
FRAGMENT(
  "StringCharacter",
  "(?:(?:{{EscapeSequence}})|{{UnicodeInputCharacter}})"
);

function matchJavaIdentifier(text, startOffset) {
  let endOffset = startOffset;
  let charCode = text.codePointAt(endOffset);

  // We verifiy if the first character is from one of these categories
  // Corresponds to the isJavaIdentifierStart function from Java
  if (chars.firstIdentChar.has(charCode)) {
    endOffset++;
    // If we encounter a surrogate pair (something that is beyond 65535/FFFF)
    // We skip another offset because a surrogate pair is of length 2.
    if (charCode > 65535) {
      endOffset++;
    }
    charCode = text.codePointAt(endOffset);
  }

  // We verify if the remaining characters is from one of these categories
  // Corresponds to the isJavaIdentifierPart function from Java
  while (chars.restIdentChar.has(charCode)) {
    endOffset++;
    // See above.
    if (charCode > 65535) {
      endOffset++;
    }
    charCode = text.codePointAt(endOffset);
  }

  // No match, must return null to conform with the RegExp.prototype.exec signature
  if (endOffset === startOffset) {
    return null;
  }
  const matchedString = text.substring(startOffset, endOffset);
  // according to the RegExp.prototype.exec API the first item in the returned array must be the whole matched string.
  return [matchedString];
}

const Identifier = createTokenOrg({
  name: "Identifier",
  pattern: { exec: matchJavaIdentifier },
  line_breaks: false,
  start_chars_hint: Array.from(chars.firstIdentChar, x =>
    String.fromCharCode(x)
  )
});

const allTokens = [];
const tokenDictionary = {};

function createToken(options) {
  // TODO create a test to check all the tokenbs have a label defined
  if (!options.label) {
    // simple token (e.g operator)
    if (typeof options.pattern === "string") {
      options.label = `'${options.pattern}'`;
    }
    // Complex token (e.g literal)
    else if (options.pattern instanceof RegExp) {
      options.label = `'${options.name}'`;
    }
  }

  const newTokenType = createTokenOrg(options);
  allTokens.push(newTokenType);
  tokenDictionary[options.name] = newTokenType;
  return newTokenType;
}

function createKeywordLikeToken(options) {
  // A keyword 'like' token uses the "longer_alt" config option
  // to resolve ambiguities, see: http://sap.github.io/chevrotain/docs/features/token_alternative_matches.html
  options.longer_alt = Identifier;
  return createToken(options);
}

// Token Categories
// Used a Token Category to mark all restricted keywords.
// This could be used in syntax highlights implementation.
const RestrictedKeyword = createToken({
  name: "RestrictedKeyword",
  pattern: Lexer.NA
});

// Used a Token Category to mark all keywords.
// This could be used in syntax highlights implementation.
const Keyword = createToken({
  name: "Keyword",
  pattern: Lexer.NA
});

// subcategory for keywords
const DataType = createToken({
  name: "DataType",
  pattern: Lexer.NA
});

// subcategory for keywords
const ReservedLiteralValues = createToken({
  name: "ReservedLiteralValues",
  pattern: Lexer.NA
});

// subcategory for keywords
const Modifier = createToken({
  name: "Modifier",
  pattern: Lexer.NA
});

// subcategory for keywords
const AccessModifier = createToken({
  name: "AccessModifier",
  pattern: Lexer.NA
});

// subcategory for keywords
const FlowControl = createToken({
  name: "FlowControl",
  pattern: Lexer.NA
});

// subcategory for keywords
const ErrorHandling = createToken({
  name: "ErrorHandling",
  pattern: Lexer.NA
});

// subcategory for keywords
const OtherKeyword = createToken({
  name: "OtherKeyword",
  pattern: Lexer.NA
});

const Operator = createToken({
  name: "Operator",
  pattern: Lexer.NA
});

const AssignmentOperator = createToken({
  name: "AssignmentOperator",
  pattern: Lexer.NA
});

const BinaryOperator = createToken({
  name: "BinaryOperator",
  pattern: Lexer.NA
});

const UnaryPrefixOperator = createToken({
  name: "UnaryPrefixOperator",
  pattern: Lexer.NA
});
const UnaryPrefixOperatorNotPlusMinus = createToken({
  name: "UnaryPrefixOperatorNotPlusMinus",
  pattern: Lexer.NA
});

const UnarySuffixOperator = createToken({
  name: "UnarySuffixOperator",
  pattern: Lexer.NA
});

const Literal = createToken({
  name: "Literal",
  pattern: Lexer.NA
});

// https://docs.oracle.com/javase/specs/jls/se11/html/jls-3.html#jls-3.11
const Separators = createToken({
  name: "Separators",
  pattern: Lexer.NA
});

// https://docs.oracle.com/javase/specs/jls/se11/html/jls-3.html#jls-3.6
// Note [\\x09\\x20\\x0C] is equivalent to [\\t\\x20\\f] and that \\x20 represents
// space character
createToken({
  name: "WhiteSpace",
  pattern: MAKE_PATTERN("[\\x09\\x20\\x0C]|{{LineTerminator}}"),
  group: Lexer.SKIPPED
});
createToken({
  name: "LineComment",
  pattern: /\/\/[^\n\r]*/,
  group: "comments"
});
createToken({
  name: "TraditionalComment",
  pattern: /\/\*([^*]|\*(?!\/))*\*\//,
  group: "comments"
});
createToken({ name: "BinaryLiteral", pattern: /0[bB][01]([01_]*[01])?[lL]?/, categories: [Literal] });
createToken({
  name: "FloatLiteral",
  pattern: MAKE_PATTERN(
    "{{Digits}}\\.({{Digits}})?({{ExponentPart}})?({{FloatTypeSuffix}})?|" +
      "\\.{{Digits}}({{ExponentPart}})?({{FloatTypeSuffix}})?|" +
      "{{Digits}}{{ExponentPart}}({{FloatTypeSuffix}})?|" +
      "{{Digits}}({{ExponentPart}})?{{FloatTypeSuffix}}"
  ),
  categories: [Literal]
});
createToken({ name: "OctalLiteral", pattern: /0_*[0-7]([0-7_]*[0-7])?[lL]?/, categories: [Literal] });
createToken({
  name: "HexFloatLiteral",
  pattern: MAKE_PATTERN(
    "0[xX]({{HexDigits}}\\.?|({{HexDigits}})?\\.{{HexDigits}})[pP][+-]?{{Digits}}[fFdD]?"
  ),
  categories: [Literal]
});
createToken({
  name: "HexLiteral",
  pattern: /0[xX][0-9a-fA-F]([0-9a-fA-F_]*[0-9a-fA-F])?[lL]?/,
  categories: [Literal]
});
createToken({
  name: "DecimalLiteral",
  pattern: MAKE_PATTERN("(0|[1-9](_+{{Digits}}|({{Digits}})?))[lL]?"),
  categories: [Literal]
});
// https://docs.oracle.com/javase/specs/jls/se11/html/jls-3.html#jls-3.10.4
createToken({
  name: "CharLiteral",
  // Not using SingleCharacter Terminology because ' and \ are captured in EscapeSequence
  pattern: MAKE_PATTERN(
    "'(?:[^\\\\']|(?:(?:{{EscapeSequence}})|{{UnicodeInputCharacter}}))'"
  ),
  categories: [Literal]
});

createToken({
  name: "TextBlock",
  pattern: /"""\s*\n(\\"|\s|.)*?"""/,
});

createToken({
  name: "StringLiteral",
  pattern: MAKE_PATTERN('"(?:[^\\\\"]|{{StringCharacter}})*"'),
  categories: [Literal]
});

// https://docs.oracle.com/javase/specs/jls/se11/html/jls-3.html#jls-3.9
// TODO: how to handle the special rule (see spec above) for "requires" and "transitive"
const restrictedKeywords = [
  "open",
  "module",
  "requires",
  "transitive",
  "exports",
  "opens",
  "to",
  "uses",
  "provides",
  "with",
  "sealed",
  "non-sealed",
  "permits"
];

// By sorting the keywords in descending order we avoid ambiguities
// of common prefixes.
sortDescLength(restrictedKeywords).forEach(word => {
  createKeywordLikeToken({
    name: word[0].toUpperCase() + camelCase(word.substr(1)),
    pattern: word,
    // restricted keywords can also be used as an Identifiers according to the spec.
    // TODO: inspect this causes no ambiguities
    categories: [Identifier, RestrictedKeyword]
  });
});

// https://docs.oracle.com/javase/specs/jls/se11/html/jls-3.html#jls-3.9
const keywords = [
  ["abstract", "abstract", [Modifier]],
  ["continue", "continue", [FlowControl]],
  ["for", "for", [FlowControl]],
  ["new", "new", [OtherKeyword]],
  ["switch", "switch", [FlowControl]],
  ["assert", "assert", [ErrorHandling]],
  ["default", "default", [FlowControl]],
  ["if", "if", [FlowControl]],
  ["package", "package", [OtherKeyword]],
  ["synchronized", "synchronized", [Modifier]],
  ["boolean", "boolean", [DataType]],
  ["do", "do", [FlowControl]],
  ["goto", "goto", [OtherKeyword]],
  ["private", "private", [AccessModifier]],
  ["this", "this", [OtherKeyword]],
  ["break", "break", [FlowControl]],
  ["double", "double", [DataType]],
  ["implements", "implements", [OtherKeyword]],
  ["protected", "protected", [AccessModifier]],
  ["throw", "throw", [ErrorHandling]],
  ["byte", "byte", [DataType]],
  ["else", "else", [FlowControl]],
  ["import", "import", [OtherKeyword]],
  ["public", "public", [AccessModifier]],
  ["throws", "throws", [ErrorHandling]],
  ["case", "case", [FlowControl]],
  ["enum", "enum", [OtherKeyword]],
  // "instanceof", // special handling for "instanceof" operator below
  ["return", "return", [FlowControl]],
  ["transient", "transient", [Modifier]],
  ["catch", "catch", [ErrorHandling]],
  ["extends", "extends", [OtherKeyword]],
  ["int", "int", [DataType]],
  ["short", "short", [DataType]],
  ["try", "try", [ErrorHandling]],
  ["char", "char", [DataType]],
  ["final", "final", [Modifier]],
  ["interface", "interface", [OtherKeyword]],
  ["static", "static", [Modifier]],
  ["void", "void", [OtherKeyword]],
  ["class", "class", [OtherKeyword]],
  ["finally", "finally", [ErrorHandling]],
  ["long", "long", [DataType]],
  ["strictfp", "strictfp", [OtherKeyword]],
  ["volatile", "volatile", [Modifier]],
  ["const", "const", [OtherKeyword]],
  ["float", "float", [DataType]],
  ["native", "native", [Modifier]],
  ["super", "super", [OtherKeyword]],
  ["while", "while", [FlowControl]],
  ["_", "underscore", [OtherKeyword]]
];

sortDescLength(keywords).forEach(word => {
  // For handling symbols keywords (underscore)
  const isPair = Array.isArray(word);
  const actualName = isPair ? word[1] : word;
  const actualPattern = isPair ? word[0] : word;
  let wordCategories = word[2];
  let categories = [Keyword].concat(wordCategories);

  const options = {
    name: actualName[0].toUpperCase() + actualName.substr(1),
    pattern: actualPattern,
    categories: categories
  };

  if (isPair) {
    options.label = `'${actualName}'`;
  }
  createKeywordLikeToken(options);
});

createKeywordLikeToken({
  name: "Instanceof",
  pattern: "instanceof",
  categories: [Keyword, BinaryOperator]
});

// added with DataType category for simple filtering along with primitive data types
createKeywordLikeToken({
  name: "String",
  pattern: "String",
  categories: [DataType]
});

createKeywordLikeToken({
  name: "Var",
  pattern: "var",
  // https://docs.oracle.com/javase/specs/jls/se16/html/jls-3.html#jls-3.9
  // "var is not a keyword, but rather an identifier with special meaning as the type of a local variable declaration"
  categories: Identifier
});
createKeywordLikeToken({
  name: "Yield",
  pattern: "yield",
  // https://docs.oracle.com/javase/specs/jls/se16/html/jls-3.html#jls-3.9
  // "yield is not a keyword, but rather an identifier with special meaning as the type of a local variable declaration"
  categories: Identifier
});
createKeywordLikeToken({
  name: "Record",
  pattern: "record",
  // https://docs.oracle.com/javase/specs/jls/se16/html/jls-3.html#jls-3.9
  // "record is not a keyword, but rather an identifier with special meaning as the type of a local variable declaration"
  categories: Identifier
});
createKeywordLikeToken({ name: "True", pattern: "true", categories: ReservedLiteralValues });
createKeywordLikeToken({ name: "False", pattern: "false", categories: ReservedLiteralValues });
createKeywordLikeToken({ name: "Null", pattern: "null", categories: ReservedLiteralValues });

// punctuation and symbols
createToken({ name: "At", pattern: "@", categories: [Separators] });
createToken({ name: "Arrow", pattern: "->" });
createToken({ name: "DotDotDot", pattern: "...", categories: [Separators] });
createToken({ name: "Dot", pattern: ".", categories: [Separators] });
createToken({ name: "Comma", pattern: ",", categories: [Separators] });
createToken({ name: "Semicolon", pattern: ";", categories: [Separators] });
createToken({ name: "ColonColon", pattern: "::", categories: [Separators] });
createToken({ name: "Colon", pattern: ":" });
createToken({ name: "QuestionMark", pattern: "?" });
createToken({ name: "LBrace", pattern: "(", categories: [Separators] });
createToken({ name: "RBrace", pattern: ")", categories: [Separators] });
createToken({ name: "LCurly", pattern: "{", categories: [Separators] });
createToken({ name: "RCurly", pattern: "}", categories: [Separators] });
createToken({ name: "LSquare", pattern: "[", categories: [Separators] });
createToken({ name: "RSquare", pattern: "]", categories: [Separators] });

// prefix and suffix operators
// must be defined before "-"
createToken({
  name: "MinusMinus",
  pattern: "--",
  categories: [
    Operator,
    UnaryPrefixOperator,
    UnarySuffixOperator,
    UnaryPrefixOperatorNotPlusMinus
  ]
});
// must be defined before "+"
createToken({
  name: "PlusPlus",
  pattern: "++",
  categories: [
    Operator,
    UnaryPrefixOperator,
    UnarySuffixOperator,
    UnaryPrefixOperatorNotPlusMinus
  ]
});
createToken({
  name: "Complement",
  pattern: "~",
  categories: [Operator, UnaryPrefixOperator, UnaryPrefixOperatorNotPlusMinus]
});

createToken({
  name: "LessEquals",
  pattern: "<=",
  categories: [Operator, BinaryOperator]
});
createToken({
  name: "LessLessEquals",
  pattern: "<<=",
  categories: [Operator, AssignmentOperator]
});
createToken({ name: "Less", pattern: "<", categories: [Operator, BinaryOperator] });
createToken({
  name: "GreaterEquals",
  pattern: ">=",
  categories: [Operator, BinaryOperator]
});
createToken({
  name: "GreaterGreaterEquals",
  pattern: ">>=",
  categories: [Operator, AssignmentOperator]
});
createToken({
  name: "GreaterGreaterGreaterEquals",
  pattern: ">>>=",
  categories: [Operator, AssignmentOperator]
});
createToken({ name: "Greater", pattern: ">", categories: [Operator, BinaryOperator] });
createToken({
  name: "EqualsEquals",
  pattern: "==",
  categories: [Operator, BinaryOperator]
});
createToken({
  name: "Equals",
  pattern: "=",
  categories: [Operator, BinaryOperator, AssignmentOperator]
});
createToken({
  name: "MinusEquals",
  pattern: "-=",
  categories: [Operator, AssignmentOperator]
});
createToken({
  name: "Minus",
  pattern: "-",
  categories: [Operator, BinaryOperator, UnaryPrefixOperator]
});
createToken({
  name: "PlusEquals",
  pattern: "+=",
  categories: [Operator, AssignmentOperator]
});
createToken({
  name: "Plus",
  pattern: "+",
  categories: [Operator, BinaryOperator, UnaryPrefixOperator]
});
createToken({ name: "AndAnd", pattern: "&&", categories: [Operator, BinaryOperator] });
createToken({
  name: "AndEquals",
  pattern: "&=",
  categories: [AssignmentOperator]
});
createToken({ name: "And", pattern: "&", categories: [Operator, BinaryOperator] });
createToken({
  name: "XorEquals",
  pattern: "^=",
  categories: [Operator, AssignmentOperator]
});
createToken({ name: "Xor", pattern: "^", categories: [Operator, BinaryOperator] });
createToken({ name: "NotEquals", pattern: "!=", categories: [Operator, BinaryOperator] });
createToken({ name: "OrOr", pattern: "||", categories: [Operator, BinaryOperator] });
createToken({
  name: "OrEquals",
  pattern: "|=",
  categories: [Operator, AssignmentOperator]
});
createToken({ name: "Or", pattern: "|", categories: [Operator, BinaryOperator] });
createToken({
  name: "MultiplyEquals",
  pattern: "*=",
  categories: [Operator, AssignmentOperator]
});
createToken({ name: "Star", pattern: "*", categories: [Operator, BinaryOperator] });
createToken({
  name: "DivideEquals",
  pattern: "/=",
  categories: [Operator, AssignmentOperator]
});
createToken({ name: "Divide", pattern: "/", categories: [Operator, BinaryOperator] });
createToken({
  name: "ModuloEquals",
  pattern: "%=",
  categories: [Operator, AssignmentOperator]
});
createToken({ name: "Modulo", pattern: "%", categories: [Operator, BinaryOperator] });

// must be defined after "!="
createToken({
  name: "Not",
  pattern: "!",
  categories: [Operator, UnaryPrefixOperator, UnaryPrefixOperatorNotPlusMinus]
});

// Identifier must appear AFTER all the keywords to avoid ambiguities.
// See: https://github.com/SAP/chevrotain/blob/master/examples/lexer/keywords_vs_identifiers/keywords_vs_identifiers.js
allTokens.push(Identifier);
tokenDictionary["Identifier"] = Identifier;

function sortDescLength(arr) {
  // sort is not stable, but that will not affect the lexing results.
  return arr.sort((a, b) => {
    return b.length - a.length;
  });
}
module.exports = {
  allTokens,
  tokens: tokenDictionary
};