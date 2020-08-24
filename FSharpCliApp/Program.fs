open System
open Argu

type CliError =
    | ArgumentsNotSpecified

type Name =
    | [<AltCommandLine("-n")>] Name of name:string
with
    interface IArgParserTemplate with
        member this.Usage =
            match this with
            | Name _ -> "Name of the guest"

let getExitCode result =
    match result with
    | Ok () -> 0
    | Error err ->
        match err with
        | ArgumentsNotSpecified -> 1

let sayName name = 
    printfn "Hello, %s!" name
    Ok ()




[<EntryPoint>]
let main argv = 
    let errorHandler = ProcessExiter(colorizer = function ErrorCode.HelpText -> None | _ -> Some ConsoleColor.Red)
    let parser = ArgumentParser.Create<Name>(programName = "FSharpCliApp", errorHandler = errorHandler)

//    let results = parser.ParseCommandLine argv
//    printfn "Got parse results %A" <| results.GetAllResults()

    match parser.ParseCommandLine argv with
    | p when p.Contains(Name) -> sayName (p.GetResult(Name))
    | _ ->
        printfn "%s" (parser.PrintUsage())
        Error ArgumentsNotSpecified
    |> getExitCode



