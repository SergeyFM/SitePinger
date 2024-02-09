namespace SitePinger.Ping

type PingSettings() =
    let mutable webServer = "http://localhost"
    let mutable port = 80
    let mutable intervalSec = 60
    let mutable timeoutSec = 30

    member _.WebServer
        with get() = webServer
        and set(value) = webServer <- value
    member _.Port
        with get() = port
        and set(value) = port <- value
    member _.IntervalSec
        with get() = intervalSec
        and set(value) = intervalSec <- value
    member _.TimeoutSec
        with get() = timeoutSec
        and set(value) = timeoutSec <- value

    override _.ToString() =
        sprintf "WebServer: %s, Port: %d, IntervalSec: %d, TimeoutSec: %d" webServer port intervalSec timeoutSec



