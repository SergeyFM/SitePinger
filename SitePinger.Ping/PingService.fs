module PingService

open System
open System.Net.Http
open System.Diagnostics

open System.Threading.Tasks


// Define an asynchronous F# function to measure request time
let measureRequestTime (address: string) (port: int) =
    async {
        printf "start pinging..."
        
        // Initialize the HttpClient
        use client = new HttpClient()

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
                return Some stopwatch.ElapsedMilliseconds
            else
                // Return None if the request was unsuccessful
                return None
        with
        | :? HttpRequestException as ex ->
            printfn "HttpRequestException: %s" ex.Message
            return None
        | ex ->
            printfn "Exception: %s" ex.Message
            return None
    }

// Example usage
let address = "example.com"
let port = 80

async {
    let! timeTaken = measureRequestTime address port
    match timeTaken with
    | Some time -> printfn "Time taken: %d ms" time
    | None -> printfn "Failed to get response."
} |> Async.RunSynchronously



let measureRequestTimeTask (address: string) (port: int) : Task<Option<int64>> =
    measureRequestTime address port
    |> Async.StartAsTask