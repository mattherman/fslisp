module VirtualMachine

open Shared

type ConsValue = string
type Cons = {
    Car: Cons
    Cdr: Cons option
}

let rec cons (lispVal: LispVal) =
    match lispVal with
    | List values -> { Car = cons (List.head values); Cdr = None }
    | QuotedExpression exp -> { Car = ""; Cdr = None }
    | DottedList (left, right) -> { Car = ""; Cdr = None }
    | Symbol s -> { Car = s; Cdr = None }
    | Integer i -> { Car = string i; Cdr = None }
    | Float f -> { Car = string f; Cdr = None }
    | Ratio (num, denom) -> { Car = sprintf "%i/%i" num denom; Cdr = None }
    | StringLiteral str -> { Car = str; Cdr = None }

let evaluate ast = 
    cons ast