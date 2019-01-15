namespace Vinance.Api.Helpers
{
    public class VinanceApiResponseExample<T> where T : class 
    {
        public int StatusCode { get; set; }
        public T Data { get; set; }
    }
}