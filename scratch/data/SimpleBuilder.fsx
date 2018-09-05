// TODO Work towards defining an option applicative builder here

[<Struct>]
type OptionalBuilder =

    member __.Bind(opt, f) =
        match opt with
        | Some x -> f x
        | None -> None
    
    // TODO Make this actually get called when `let! ... and! ...` syntax is used (and automagically if RHSs allow it?)
    member __.Apply(xOpt : 'a option, fOpt : ('a -> 'b) option) : 'b option =
        match fOpt, xOpt with
        | Some f, Some x -> Some (f x)
        | _ -> None

    // TODO Not needed, but for maximum efficiency, we want to use this if it is defined
    member __.Map(xOpt : 'a option, f : 'a -> 'b) : 'b option =
        match xOpt with
        | Some x -> Some (f x)
        | None -> None

    member __.Return(x) =
        Some x

    member __.Delay(thunk : unit -> 'a option) : 'a option =
        thunk ()

    member __.Combine(x : unit option, y : 'a option) : 'a option =
        match x with
        | Some () -> y
        | None -> None

    member __.Zero() : unit option =
        Some ()
    
let opt = OptionalBuilder()

let x = Some 1
let y = opt { return "A" }
let z = Some 3.5

let foo : string option =
    opt {
        let! x' = x
        letmatch! y with
        | yValue -> printfn "y was set to %s!" yValue
        let! z' = z
        return sprintf "x = %d, z = %f" x' z'
    }

match foo with
| Some s -> printfn "Some (%s)" s
| None -> printfn "None"

System.Threading.Thread.Sleep(10000)

(*
let foo' =
    opt {
        let! x' = x
        and! y' = y
        and! z' = z
        return sprintf "x = %d, y = %s, z = %f" x y z
    }
*)