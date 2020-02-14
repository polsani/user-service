using System;
using UserService.Domain.Mappers;
using UserService.Factories;
using UserService.Mappers;
using Xunit;

namespace UserService.Tests.Factories
{
    public class MediaMapperFactoryTests
    {
        private readonly MediaMapperFactory _subject;

        public MediaMapperFactoryTests()
        {
            _subject = new MediaMapperFactory(new IMediaMapper[]
            {
                new CsvMediaMapper(),
                new ExcelMediaMapper()
            });
        }

        [Theory]
        [InlineData("text/csv", typeof(CsvMediaMapper))]
        [InlineData("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", typeof(ExcelMediaMapper))]
        public void MediaMapperFactoryReturnsCorrectMapper(string mediaType, Type type)
        {
            var mapper = _subject.GetMediaMapper(mediaType);
            
            Assert.True(mapper.GetType() == type);
        }
    }
}