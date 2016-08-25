﻿#r "../Uncertain/bin/Debug/Uncertain.dll"
#r "bin/Debug/Histogram.dll"
#load "HistogramUncertain.fs"
open Microsoft.Research.Uncertain.Histogram

let someDist = new HistogramUncertain<float> ([ (73.0, 0.8); (42.1, 0.15) ])

// An uncertainty-unaware function, double, which takes an argument, n.
let double n = 2.0 * n

// This won't type-check, of course:
//let doubledDist = double someDist

// But it does if we apply our combinator!
let doubledDist = (Lifting.lift double) someDist

// Dump the distribution.
for weighted in doubledDist.Support() do
    match weighted.Value with
    | Top v -> printfn "%A: %f" v weighted.Probability
    | Other -> printfn "other: %f" weighted.Probability