using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace UserService.ViewModels
{
    public class Import
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        
        [JsonProperty(PropertyName = "importDate")]
        public DateTime ImportDateTime { get; set; }

        [JsonProperty(PropertyName = "users")]
        public IEnumerable<User> Users { get; set; }
    }
}