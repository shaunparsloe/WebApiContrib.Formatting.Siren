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
        public void ReadFromStreamAsync_DeSerializes_Class_Correctly()
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
            }
        }

        [Fact]
        public void ReadFromStreamAsync_Deserializes_BlankBody_Correctly()
        {
            // Arrange
            Car car = new Car();
            string inputString =
                "";

            // Act
            using (MemoryStream stream = new MemoryStream(System.Text.Encoding.Default.GetBytes(inputString)))
            {
                var content = new StreamContent(stream);

                var task = formatter.ReadFromStreamAsync(typeof(Car), stream, content, null);
                car = task.Result as Car;
            }

            // Assert
            Assert.Null(car);
          
        }

        [Fact]
        public void ReadFromStreamAsync_Deserializes_Title_Correctly()
        {
            // Arrange
            Car car = new Car();
            string inputString =
                "{\"class\":[],\"title\":\"Car Title\",\"properties\":{\"numberOfWheels\":0},\"entities\":[]}";

            // Act
            using (MemoryStream stream = new MemoryStream(System.Text.Encoding.Default.GetBytes(inputString)))
            {
                var content = new StreamContent(stream);

                var task = formatter.ReadFromStreamAsync(typeof(Car), stream, content, null);
                car = task.Result as Car;
            }

            // Assert
            Assert.Equal("Car Title", car.Title);

        }

        [Fact]
        public void ReadFromStreamAsync_Deserializes_Actions_Correctly()
        {
            // Arrange
            Car car = new Car();
            string inputString =
                "{\"class\":[],\"properties\":{\"numberOfWheels\":0},\"entities\":[],\"actions\":[{\"name\":\"Start\",\"class\":[\"StartAction\"],\"method\":\"POST\",\"href\":\"https://api.test.com/car/start\",\"title\":\"Start the car\",\"type\":\"application/json\",\"fields\":[]}]}";

            // Act
            using (MemoryStream stream = new MemoryStream(System.Text.Encoding.Default.GetBytes(inputString)))
            {
                var content = new StreamContent(stream);

                var task = formatter.ReadFromStreamAsync(typeof(Car), stream, content, null);
                car = task.Result as Car;
            }

            WebApiContrib.MediaType.Hypermedia.Action startTheCarAction = car.Actions[0];

            // Assert
            Assert.Equal("Start", startTheCarAction.Name);
            Assert.Equal("StartAction", startTheCarAction.Class[0]);
            Assert.Equal(HTTP_Method.POST, startTheCarAction.Method);
            Assert.Equal("https://api.test.com/car/start", startTheCarAction.Href.ToString());
            Assert.Equal("Start the car", startTheCarAction.Title);
            Assert.Equal("application/json", startTheCarAction.Type);
          
        }

        [Fact]
        public void ReadFromStreamAsync_Deserializes_Properties_Correctly()
        {
            // Arrange
            Car car = new Car();
            string inputString =
                "{\"class\":[],\"properties\":{\"colour\":\"Black\",\"numberOfWheels\":5},\"entities\":[]}";

            // Act
            using (MemoryStream stream = new MemoryStream(System.Text.Encoding.Default.GetBytes(inputString)))
            {
                var content = new StreamContent(stream);

                var task = formatter.ReadFromStreamAsync(typeof(Car), stream, content, null);
                car = task.Result as Car;
            }

            // Assert
            Assert.Equal("Black", car.Colour);
            Assert.Equal(5, car.NumberOfWheels);
        }

        [Fact]
        public void ReadFromStreamAsync_Deserializes_EmbeddedLinks_Correctly()
        {
            // Arrange
            Car car = new Car();
            EmbeddedLink embeddedLink = new EmbeddedLink(new Uri("https://www.test.com"), "Embedded Link Class", "Embedded Link Rel");
            string inputString =
                "{\"class\":[],\"properties\":{\"numberOfWheels\":0},\"entities\":[{\"class\":[\"Embedded Link Class\"],\"rel\":[\"Embedded Link Rel\"],\"href\":\"https://www.test.com\"}]}";

            // Act
            using (MemoryStream stream = new MemoryStream(System.Text.Encoding.Default.GetBytes(inputString)))
            {
                var content = new StreamContent(stream);

                var task = formatter.ReadFromStreamAsync(typeof(Car), stream, content, null);
                car = task.Result as Car;
            }

            EmbeddedLink deserialisedEmbeddedLink = (EmbeddedLink)car.Entities[0];

            // Assert
            Assert.Equal(embeddedLink.Class[0], deserialisedEmbeddedLink.Class[0]);
            Assert.Equal(embeddedLink.Rel[0], deserialisedEmbeddedLink.Rel[0]);
            Assert.Equal(embeddedLink.Href, deserialisedEmbeddedLink.Href);

        }

        [Fact]
        public void ReadFromStreamAsync_Deserializes_Links_Correctly()
        {
            // Arrange
            Car car = new Car();
            string inputString =
                @"{""class"":[],""properties"":{""numberOfWheels"":0},""entities"":[],""links"":[{""rel"":[""self""],""href"":""https://api.test.com/home"",""title"":""LinkTitle"",""type"":null}]}";

            // Act
            using (MemoryStream stream = new MemoryStream(System.Text.Encoding.Default.GetBytes(inputString)))
            {
                var content = new StreamContent(stream);

                var task = formatter.ReadFromStreamAsync(typeof(Car), stream, content, null);
                car = task.Result as Car;
            }

            Link deserializedLink = (Link)car.Links[0];

            // Assert
            Assert.Equal("https://api.test.com/home", deserializedLink.Href.ToString());
            Assert.Equal("self", deserializedLink.Rel[0]);
            Assert.Equal("LinkTitle", deserializedLink.Title);
            Assert.Equal("GET", deserializedLink.Method);

        }

        [Fact]
        public void ReadFromStreamAsync_DeSerializes_Entity_Correctly()
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
