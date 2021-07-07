namespace ncwsapp.Models
{
    public class Api
    {
        public string BaseUrl { get; set; }
        public int RequestTimeout { get; set; }
        public Credentials Credentials { get; set; }
    }
}