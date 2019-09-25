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

type ParseResult =
    | Success of LispVal list
    | Failure

let rec lispValString lispVal =
    match lispVal with
    | QuotedExpression exp -> sprintf "'%s" (lispValString exp)
    | DottedList (left, right) -> sprintf "%s . %s" (lispValString left) (lispValString right)
    | List values -> sprintf "(%s)" (values |> List.map lispValString |> String.concat " ")
    | Symbol s -> s
    | Integer i -> string i
    | Float f -> string f
    | Ratio (num, denom) -> sprintf "%i/%i" num denom
    | StringLiteral s -> s