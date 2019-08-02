# FsLisp

FsLisp is a Lisp interpreter written in F#. Currently, all it does is parse Lisp code into an AST using [FParsec](https://github.com/stephan-tolksdorf/fparsec).

Parsing example:
```
Running FsLisp interpreter...
* (defun fair-coin () (let ((toss (random 101))) (cond ((< toss 50) 'heads) ((> toss 50) 'tails) (t 'edge))))
Success: [List
   [Symbol "defun"; Symbol "fair-coin"; List [];
    List
      [Symbol "let";
       List [List [Symbol "toss"; List [Symbol "random"; Symbol "101"]]];
       List
         [Symbol "cond";
          List
            [List [Symbol "<"; Symbol "toss"; Symbol "50"];
             QuotedExpression (Symbol "heads")];
          List
            [List [Symbol ">"; Symbol "toss"; Symbol "50"];
             QuotedExpression (Symbol "tails")];
          List [Symbol "t"; QuotedExpression (Symbol "edge")]]]]]
```

## Grammar

The following grammar is used by the interpreter:

```
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