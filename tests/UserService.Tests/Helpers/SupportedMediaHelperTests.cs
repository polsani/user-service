using UserService.Helpers;
using Xunit;

namespace UserService.Tests.Helpers
{
    public class SupportedMediaHelperTests
    {
        private readonly SupportedMediaHelper _subject;

        public SupportedMediaHelperTests()
        {
            _subject = new SupportedMediaHelper();
        }

        [Fact]
        public void SupportedMediaIncludeCsv()
        {
            const string csvMediaContentType = "text/csv";
            
            Assert.True(_subject.IsMediaSupported(csvMediaContentType));
        }

        [Fact]
        public void SupportedMediaIncludeExcel()
        {
            const string excelMediaContentType =
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            
            Assert.True(_subject.IsMediaSupported(excelMediaContentType));
        }

        [Theory]
        [InlineData("text/plain")]
        [InlineData("text/html")]
        [InlineData("application/vnd.openxmlformats-officedocument.wordprocessingml.document")]
        [InlineData("application/pdf")]
        public void SupportedMediaExcludesDifferentMediaTypes(string mediaType)
        {
            Assert.False(_subject.IsMediaSupported(mediaType));
        }
    }
}