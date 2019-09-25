module VirtualMachine

open Shared

module StandardLibrary =
    let T = Symbol "T"
    let NIL = Symbol "NIL"

    let boolToSymbol b =
        if b then T else NIL

    let equal (parameters: LispVal list) =
        let rec listEqual l =
            match l with
                | [_] -> true
                | x::xs -> if x = (List.head xs) then listEqual xs else false
                | _ -> true
        parameters |> listEqual |> boolToSymbol

let functionLookup symbol =
    match symbol with
    | Symbol "equal" -> StandardLibrary.equal
    | _ -> fun parameters -> List (symbol :: parameters)

let execute symbol parameters =
    functionLookup symbol parameters

let rec eval lispVal =
    match lispVal with
    | List values -> execute (List.head values) (List.tail values)
    | QuotedExpression exp -> execute (Symbol "quote") [exp]
    | DottedList (left, right) -> DottedList (left, right)
    | Symbol s -> Symbol s
    | Integer i -> Integer i
    | Float f -> Float f
    | Ratio (num, denom) -> Ratio (num, denom)
    | StringLiteral str -> StringLiteral str
