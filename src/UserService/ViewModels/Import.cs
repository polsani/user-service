using System;
using Newtonsoft.Json;

namespace UserService.ViewModels
{
    public class Import
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        
        [JsonProperty(PropertyName = "createDate")]
        public DateTime CreateDate { get; set; }
        
        [JsonProperty(PropertyName = "approved")]
        public bool Approved { get; set; }
        
        [JsonProperty(PropertyName = "importDate", NullValueHandling=NullValueHandling.Ignore)]
        public DateTime? ImportDate { get; set; }
        
        [JsonProperty(PropertyName = "amountRows")]
        public int AmountRows { get; set; }
    }
}