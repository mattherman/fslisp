open System

let eval (text: string) =
    text

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

