using System;
using WebApiContrib.Formatting.Siren;
using Xunit;
using Should;
using Xunit.Extensions;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebApiContrib.MediaType.Hypermedia;

namespace WebApiContrib.Formatting.Siren.Tests
{
    public class Siren_General_Tests
    {
        private SirenMediaTypeFormatter formatter = new SirenMediaTypeFormatter();
        private const string SirenMediaType = "application/vnd.siren+json";
        private MediaTypeHeaderValue SirenMediaTypeHeader = new MediaTypeHeaderValue(SirenMediaType);

        [Fact]
        public void DefaultMediaType_ReturnsApplicationJson()
        {
            MediaTypeHeaderValue mediaType = SirenMediaTypeFormatter.DefaultMediaType;
            Assert.NotNull(mediaType);
            Assert.Equal("application/json", mediaType.MediaType);
        }

        [Theory]
        [InlineData(typeof(Entity))]
        [InlineData(typeof(SubEntity))]
        public void CanReadType_ReturnsTrue_On_Entity(Type type)
        {
            formatter.CanReadType(type).ShouldBeTrue();
        }

        [Theory]
        [InlineData(typeof(List<object>))]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        public void CanReadType_ReturnsFalse_On_Non_Entity(Type type)
        {
            formatter.CanReadType(type).ShouldBeFalse();
        }


        [Theory]
        [InlineData(typeof(Entity))]
        [InlineData(typeof(SubEntity))]
        public void CanWriteType_ReturnsTrue_On_Entity(Type type)
        {
            formatter.CanWriteType(type).ShouldBeTrue();
        }

        [Theory]
        [InlineData(typeof(List<object>))]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        public void CanWriteType_ReturnsFalse_On_Non_IEntity(Type type)
        {
            formatter.CanWriteType(type).ShouldBeFalse();
        }

        [Fact]
        public void SetDefaultContentHeaders_Should_Set_Siren_As_Content_Header()
        {
            Type type = typeof(object);
            HttpContentHeaders contentHeaders = FormattingUtilities.CreateEmptyContentHeaders();

            // Act
            formatter.SetDefaultContentHeaders(type, contentHeaders, SirenMediaTypeHeader);

            // Assert
            Assert.NotSame(SirenMediaTypeHeader, contentHeaders.ContentType);
            Assert.Equal(SirenMediaType, contentHeaders.ContentType.MediaType);
        }


    }
}
