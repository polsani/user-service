using System;
using Newtonsoft.Json;

namespace UserService.ViewModels
{
    public class UserImportRequest
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
        
        [JsonProperty(PropertyName = "birthDate")]
        public string BirthDate { get; set; }
        
        [JsonProperty(PropertyName = "gender")]
        public string Gender { get; set; }

        [JsonProperty(PropertyName = "importId")]
        public Guid ImportId { get; set; }
    }
}