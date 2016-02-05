﻿namespace Microsoft.Research.Uncertain.Histogram
open Microsoft.Research.Uncertain

module Histogram =
    let rec getScore entries value =
        match entries with
        | (v, p)::es -> (if v = value then p else getScore es value)
        | _ -> 0.0

type HistogramUncertain<'a> when 'a : equality (topk: seq< 'a * float >) =
    inherit RandomPrimitive<'a>()

    // A list of object/probability pairs.
    let entries =
        List.ofSeq topk

    // The "non-top" probability.
    member this.otherProbability () =
        let allProbs = seq { for value, probability in entries -> probability } in
        let totalProb = Seq.fold (+) 0.0 allProbs in
        let topkProb = 0.0 in
        1.0 - topkProb
    
    member this.getEntries () = entries
    

    // The Uncertain<T> interface.

    override this.GetSupport () =
        raise (System.Exception("infinite support"))
    
    override this.GetSample () =
        raise (System.Exception("unimplemented")) // TODO

    override this.StructuralEquals other =
        match other with
        | :? HistogramUncertain<'a> as hu ->
            entries = hu.getEntries ()  // TODO Not really structural.
        | _ -> false

    override this.GetStructuralHash () =
        hash entries

    override this.Score value =
        Histogram.getScore entries value