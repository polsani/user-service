using Newtonsoft.Json;

namespace UserService.ViewModels
{
    public class ImportResult
    {
        [JsonProperty(PropertyName = "inserted")]
        public int Inserted { get; set; }
        
        [JsonProperty(PropertyName = "updated")]
        public int Updated { get; set; }
        
        [JsonProperty(PropertyName = "ignored")]
        public int Ignored { get; set; }
        
        [JsonProperty(PropertyName = "failed")]
        public int Failed { get; set; }
    }
}