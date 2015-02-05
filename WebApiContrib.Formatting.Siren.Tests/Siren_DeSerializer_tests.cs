using System;
using WebApiContrib.Formatting.Siren;
using Xunit;
using Should;
using Xunit.Extensions;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebApiContrib.MediaType.Hypermedia;

namespace WebApiContrib.Formatting.Siren.Tests
{
    public class Siren_DeSerializer_tests
    {
        private SirenMediaTypeFormatter formatter = new SirenMediaTypeFormatter();
        private const string SirenMediaType = "application/vnd.siren+json";
        private MediaTypeHeaderValue SirenMediaTypeHeader = new MediaTypeHeaderValue(SirenMediaType);

        [Fact]
        public void ReadFromStreamAsync_Serializes_Class_Correctly()
        {
            // Arrange
            string inputString = @"
{
  ""class"": [
    ""Car""
  ]
}";

            using (MemoryStream stream = new MemoryStream(System.Text.Encoding.Default.GetBytes(inputString)))
            {
                var content = new StreamContent(stream);

                var task = formatter.ReadFromStreamAsync(typeof(Car), stream, content, null);
                Car car = task.Result as Car;
                Assert.Equal("Car", car.Class[0]);
                //Assert.AreEqual("1", car.Value);
            }
        }

        [Fact]
        public void ReadFromStreamAsync_Serializes_Entity_Correctly()
        {
            // Arrange
            string inputString = TestJSON.WheelClass();

            using (MemoryStream stream = new MemoryStream(System.Text.Encoding.Default.GetBytes(inputString)))
            {
                var content = new StreamContent(stream);

                var task = formatter.ReadFromStreamAsync(typeof(Wheel), stream, content, null);
                Wheel wheel = task.Result as Wheel;
                Assert.Equal("Wheel", wheel.Class[0]);
                Assert.Equal("My Car Wheel", wheel.Title);
                Assert.Equal(1, wheel.id);
                Assert.Equal("124x55x18", wheel.Size);
                Assert.Equal(1, wheel.Actions.Count);
                Assert.Equal("Inflate", wheel.Actions[0].Class[0]);
                Assert.Equal("PUT", wheel.Actions[0].Method.ToString());
                Assert.Equal("https://api.test.com/wheel/inflate", wheel.Actions[0].Href.ToString());
                Assert.Equal("Inflate the wheel", wheel.Actions[0].Title);
                Assert.Equal("application/json", wheel.Actions[0].Type);
                

                //Assert.AreEqual("1", car.Value);
            }
        }
    }
}
