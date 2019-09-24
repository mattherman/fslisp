module Shared

type LispVal =
    | QuotedExpression of LispVal
    | DottedList of LispVal * LispVal
    | List of LispVal list
    | Symbol of string
    | Integer of int
    | Float of float
    | Ratio of int * int
    | StringLiteral of string