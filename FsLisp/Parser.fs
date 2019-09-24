module Parser

open FParsec
open System

open Shared

let singleQuote = pchar '''
let doubleQuote = pchar '"'
let openParenthesis = pchar '(' .>> spaces
let closeParenthesis = pchar ')' .>> spaces
let dot = pchar '.' .>> spaces

let symbol =
    // This is by no means exhaustive, will be improved upon later
    let isValidIdentifier c =
        not (Char.IsControl(c) || Char.IsWhiteSpace(c) || c = '(' || c = ')' || c = '.')
    many1Satisfy isValidIdentifier |>> Symbol

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
    (ratio <|> number <|> stringLiteral)

let atom =
    (value <|> symbol) .>> spaces

let expression, expressionRef = createParserForwardedToRef<LispVal, unit>()

let list =
    between openParenthesis closeParenthesis (many expression) |>> List

let dotExpression =
    (expression .>> dot) .>>. expression |>> fun (e1, e2) -> DottedList(e1, e2)

let dottedList =
    // Need to backtrack if "." not found in case we're parsing a regular list
    attempt (between openParenthesis closeParenthesis dotExpression)

let quotedExpression =
    singleQuote >>. expression |>> QuotedExpression

expressionRef := choice 
    [
        quotedExpression
        dottedList
        list
        atom
    ]

let parse (input: string) =
    run (spaces >>. many expression) input