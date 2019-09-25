open System

open Shared

let read = Console.ReadLine

let eval (text: string) =
    let result = text |> Parser.parse
    match result with
    | Ok values -> 
        let evalResult = List.head values |> VirtualMachine.eval
        match evalResult with
        | Ok output -> lispValString output
        | Error msg -> msg
    | Error msg -> sprintf "Failed to parse the input: %s" msg

let print (result: string)=
    Console.WriteLine(result)

let rec loop () =
    Console.Write("* ");
    () |> read
       |> eval
       |> print
       |> loop

[<EntryPoint>]
let main argv =
    printfn "Running FsLisp interpreter..."
    loop()
    0 // return an integer exit code

