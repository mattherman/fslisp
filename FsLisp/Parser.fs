module Parser

open FParsec
open System

module StringUtility =
    let charListToStr charList =
        String(List.toArray charList)

type LispVal =
    | Symbol of string
    | QuotedSymbol of int * string
    | Integer of int
    | Float of float
    | Ratio of int * int
    | StringLiteral of string

//let singleQuote = pchar '''
let doubleQuote = pchar '"'

let symbol =
    many1Satisfy2 isLetter (fun c -> isLetter c || isDigit c) |>> Symbol

let stringLiteral =
    between doubleQuote doubleQuote (manySatisfy (function '"'|'\n' -> false | _ -> true)) |>> StringLiteral

let ratio =
    let slash = pstring "/"
    // Need to backtrack if "/" not found in case we're parsing integer/float
    (pint32 .>>? slash) .>>. pint32 |>> fun (n, d) -> Ratio(n, d)

let numberFormat = 
    NumberLiteralOptions.AllowMinusSign ||| 
    NumberLiteralOptions.AllowFraction

let number =
    numberLiteral numberFormat "number"
    |>> fun nl ->
            if nl.IsInteger then Integer (int32 nl.String)
            else Float (float nl.String)

let value =
    (ratio <|> number <|> stringLiteral) .>> spaces

let atom =
    (symbol <|> value)

let parse (input: string) =
    run (many1 atom) input