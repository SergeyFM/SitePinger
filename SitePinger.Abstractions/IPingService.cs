public interface IPingService {
    Task<long?> MeasureRequestTimeAsync(string address, int port, int timeout);
}