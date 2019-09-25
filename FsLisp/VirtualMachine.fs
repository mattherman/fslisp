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
    | Symbol "equal" -> Some StandardLibrary.equal
    | _ -> None

let execute (list: LispVal list) =
    match list with
    | funcName :: funcParameters -> 
        let funcMatch = functionLookup funcName
        match funcMatch with
        | Some func -> Ok (func funcParameters)
        | None -> Error "undefined function"
    | [] -> Ok (StandardLibrary.NIL)

let elevateToListResult (listOfResults: Result<LispVal, string> list) : Result<LispVal list, string> =
    let addToResultListIfOk acc element =
        match acc, element with
        // Switched from `el :: currentList` since it messed with the order, not sure of the negatives that come along with List.append...
        | Ok currentList, Ok el -> Ok (List.append currentList [el])
        | Ok _, Error msg -> Error msg
        | Error msg, _ -> Error msg
    listOfResults |> List.fold addToResultListIfOk (Ok List.empty)

let rec eval lispVal =
    match lispVal with
    | List values -> values |> List.map eval |> elevateToListResult |> Result.bind execute
    | QuotedExpression exp -> Ok exp
    | DottedList (left, right) -> Ok (DottedList (left, right))
    | Symbol s -> Ok (Symbol s)
    | Integer i -> Ok (Integer i)
    | Float f -> Ok (Float f)
    | Ratio (num, denom) -> Ok (Ratio (num, denom))
    | StringLiteral str -> Ok (StringLiteral str)