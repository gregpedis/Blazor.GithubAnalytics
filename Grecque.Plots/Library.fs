namespace Grecque.Plots

open Plotly.NET

module public Visualization =

    let lineChart xs ys name = Chart.Line(x = xs, y = ys, Name= name, ShowLegend = true)

    let namedLineChart xs ys name = 
        lineChart xs ys name 
        |> Chart.withXAxisStyle "Timeline" 
        |> Chart.withYAxisStyle "Issues" 
        |> Chart.withSize(2000, 1200) 

    let combinedChart xs ys1 ys2 name1 name2 =
        [   
            namedLineChart xs ys1 name1
            namedLineChart xs ys2 name2
        ]
        |> Chart.combine

