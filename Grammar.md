# Lisp BNF Grammar

The following grammar is used by the interpreter:

```
// TODO: Add quoted s_expr

S_EXPR ->  ATOM | DOTTED_LIST | LIST

DOTTED_LIST -> "(" S_EXPR "." S_EXPR ")"
LIST -> "(" S_EXPR < S_EXPR > ")"

ATOM -> SYMBOL | VALUE

SYMBOL -> STRING

VALUE -> INTEGER | FLOAT | RATIO | STRING_LITERAL

RATIO -> INTEGER "/" INTEGER
```

Based on a grammar example provided by [Wil Chung](https://iamwilhelm.github.io/bnf-examples/lisp).