using System;
using Newtonsoft.Json;

namespace UserService.ViewModels
{
    public class ImportResult
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "createDate")]
        public DateTime CreateDate { get; set; }

        [JsonProperty(PropertyName = "amountToImport")]
        public int AmountRows { get; set; }
        
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