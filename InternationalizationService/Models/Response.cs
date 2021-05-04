using Newtonsoft.Json.Linq;

namespace InternationalizationService.Models
{
    public class ErrorResponse
    {
        public string error { get; set; }
    }
    public class SuccessResponse
    {
        public string message { get; set; }
    }
    public class GetResponse
    {
        public Pagination pagination { get; set; }
        public JArray data { get; set; }
    }
}
