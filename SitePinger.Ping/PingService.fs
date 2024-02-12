module PingService

open System
open System.Net.Http
open System.Diagnostics

open System.Threading.Tasks


// Define an asynchronous F# function to measure request time
let measureRequestTime (address: string) (port: int) (timeout: int) =
    async {
        
        printf "%s:%d..." address port
        
        // Initialize the HttpClient
        use client = new HttpClient()
        client.Timeout <- TimeSpan.FromSeconds(timeout)


        // Construct the full URL using the provided address and port
        let url = sprintf "http://%s:%d" address port

        // Create and start a stopwatch for measuring elapsed time
        let stopwatch = Stopwatch.StartNew()

        try
            // Asynchronously send a GET request to the server
            let! response = client.GetAsync(url) |> Async.AwaitTask
            
            // Stop the stopwatch after receiving the response
            stopwatch.Stop()

            

            // Check if the request was successful
            if response.IsSuccessStatusCode then
                // Return the elapsed time in milliseconds
                printf "%dms\n" stopwatch.ElapsedMilliseconds
            else
                // Return None if the request was unsuccessful
                printf "NOPE\n"

            return None
        with
        | :? HttpRequestException as ex ->
            printfn "%s" ex.Message
            return None
        | ex ->
            printfn "%s" ex.Message
            return None
    }


let measureRequestTimeTask (address: string) (port: int) (timeout: int) : Task<Nullable<int64>> =
    async {
        let! result = measureRequestTime address port timeout
        match result with
        | Some time -> return Nullable(time)
        | None -> return Nullable()
    } |> Async.StartAsTask

type PingService() =
    interface IPingService with
        member this.MeasureRequestTimeAsync(address, port, timeout) =
            measureRequestTimeTask address port timeout
