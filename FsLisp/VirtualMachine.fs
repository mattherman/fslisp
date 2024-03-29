module VirtualMachine

open Shared

module StandardLibrary =
    let T = Symbol "T"
    let NIL = Symbol "NIL"

    let boolToSymbol b =
        if b then T else NIL

    let equal (left: LispVal, right: LispVal) =
        fun () -> left = right |> boolToSymbol

    let car (list: LispVal list) =
        fun () -> List.head list

    let cdr (list: LispVal list) =
        fun () -> List.tail list |> List

let hasParams expectedCount parameters : Result<LispVal list, string> =
    if expectedCount = List.length parameters then
        Ok parameters
    else
        Error "incorrect number of parameters"

let unpackList parameters =
    match parameters with
    | [List x] -> Ok x
    | _ -> Error "expected parameter to be of type list"

let equal parameters =
    let constructEquals p =
        let left = List.item 0 p
        let right = List.item 1 p
        StandardLibrary.equal (left, right)
    parameters 
    |> hasParams 2
    |> Result.map constructEquals

let car parameters =
    let constructCar p =
        StandardLibrary.car p
    parameters
    |> hasParams 1
    |> Result.bind unpackList
    |> Result.map constructCar

let cdr parameters =
    let constructCdr p =
        StandardLibrary.cdr p
    parameters
    |> hasParams 1
    |> Result.bind unpackList
    |> Result.map constructCdr

let functionLookup symbol parameters =
    match symbol with
    | Symbol "equal" -> equal parameters
    | Symbol "car" -> car parameters
    | Symbol "cdr" -> cdr parameters
    | _ -> Error "undefined function"

let execute (list: LispVal list) =
    match list with
    | funcName :: funcParameters -> 
        functionLookup funcName funcParameters 
        |> Result.map (fun funcMatch -> funcMatch()) 
    | [] -> Ok StandardLibrary.NIL

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