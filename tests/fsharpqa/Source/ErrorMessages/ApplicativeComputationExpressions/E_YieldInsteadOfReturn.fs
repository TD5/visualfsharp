// #ErrorMessages
//<Expects id="FS3245" status="error" span="(11,9)">'yield' is not valid in this position in an applicative computation expression. Did you mean 'return' instead?</Expects>

namespace ApplicativeComputationExpressions

module E_LetBangAndBangNotTerminatedWithReturn =

    eventually {
        let! x = Eventually.NotYetDone (fun () -> Eventually.Done 4)
        and! y = Eventually.Done 6
        yield x + y
    }