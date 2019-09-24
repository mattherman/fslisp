open System

open Shared

let eval (text: string) =
    let result = text |> Parser.parse
    match result with
    | Success values -> List.head values |> VirtualMachine.eval |> string
    | Failure -> "Failed to parse the input"

let print (result: string)=
    Console.WriteLine(result)

let rec repl () =
    Console.Write("* ");
    () |> Console.ReadLine
       |> eval
       |> print
       |> repl

[<EntryPoint>]
let main argv =
    printfn "Running FsLisp interpreter..."
    repl()
    0 // return an integer exit code

