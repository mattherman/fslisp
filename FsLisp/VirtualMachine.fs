module VirtualMachine

open Shared

let execute symbol parameters =
    List parameters

let rec eval lispVal =
    match lispVal with
    | List values -> execute (List.head values) (List.tail values)
    | QuotedExpression exp -> exp
    | DottedList (left, right) -> DottedList (left, right)
    | Symbol s -> Symbol s
    | Integer i -> Integer i
    | Float f -> Float f
    | Ratio (num, denom) -> Ratio (num, denom)
    | StringLiteral str -> StringLiteral str
