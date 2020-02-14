using System;
using Newtonsoft.Json;

namespace UserService.ViewModels
{
    public class PreviousImportResult
    {
        [JsonProperty(PropertyName = "importId")]
        public Guid ImportId { get; set; }

        [JsonProperty(PropertyName = "createDate")]
        public DateTime CreateDate { get; set; }

        [JsonProperty(PropertyName = "amountToImport")]
        public int AmountRows { get; set; }
    }
}