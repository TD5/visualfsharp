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
    
let opt = OptionalBuilder()

let x = Some 1
let y = Some "A"
let z : float option = None

let foo : string option =
    opt {
        let! x' = x
        let! y' = y
        let! z' = z
        return sprintf "x = %d, y = %s, z = %f" x' y' z'
    }

match foo with
| Some s -> printf "Some (%s)" s
| None -> printf "None"

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