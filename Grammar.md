# Lisp BNF Grammar

The following grammar is used by the interpreter:

```
s_expression = atomic_symbol | "(" s_expression "."s_expression ")" | list 
   
list = "(" s_expression < s_expression > ")"

atomic_symbol = letter atom_part

atom_part = empty / letter atom_part / number atom_part

letter = "a" / "b" / " ..." / "z"

number = "1" / "2" / " ..." / "9"

empty = " "
```

Based on a grammar example provided by [Wil Chung](https://iamwilhelm.github.io/bnf-examples/lisp).