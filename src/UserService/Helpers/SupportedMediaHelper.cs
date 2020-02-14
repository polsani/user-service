using System.Collections.Generic;
using System.Linq;
using UserService.Domain.Helpers;

namespace UserService.Helpers
{
    public class SupportedMediaHelper : ISupportedMediaHelper
    {
        private readonly IEnumerable<string> _supportedMedia = new[]
        {
            "text/csv",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        };

        public bool IsMediaSupported(string contentType)
        {
            return _supportedMedia.Any(x => x.Equals(contentType));
        }
    }
}