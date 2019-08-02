# Lisp BNF Grammar

The following grammar is used by the interpreter:

```
// TODO: Add quoted s_expr

S_EXPR ->  ATOM | DOTTED_LIST | LIST | QUOTED_S_EXPR

ATOM -> SYMBOL | VALUE
DOTTED_LIST -> "(" S_EXPR "." S_EXPR ")"
LIST -> "(" S_EXPR < S_EXPR > ")"
QUOTED_S_EXPR -> "'" S_EXPR

SYMBOL -> STRING

VALUE -> INTEGER | FLOAT | RATIO | STRING_LITERAL

RATIO -> INTEGER "/" INTEGER
```

Based on a grammar example provided by [Wil Chung](https://iamwilhelm.github.io/bnf-examples/lisp).