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
    public class Siren_Serializer_Tests
    {
        private SirenMediaTypeFormatter formatter = new SirenMediaTypeFormatter();
        private const string SirenMediaType = "application/vnd.siren+json";
        private MediaTypeHeaderValue SirenMediaTypeHeader = new MediaTypeHeaderValue(SirenMediaType);

        [Fact]
        public void WriteToStreamAsync_Serializes_Class_Correctly()
        {
            // Arrange
            Car myCar = new Car();
            myCar.Class.Add("Car Class");

            // Act
            IEntity entityAfter = GetIEntityResultsFromWriteToStreamAsync(myCar);

            // Assert - Have properties
            entityAfter.Class.Contains("Car Class").ShouldBeTrue();
        }

        [Fact]
        public void WriteToStreamAsync_Serializes_Title_Correctly()
        {
            // Arrange
            Car myCar = new Car();
            myCar.Title = "Car Title";

            // Act
            IEntity entityAfter = GetIEntityResultsFromWriteToStreamAsync(myCar);

            // Assert - Have properties
            Assert.Equal("Car Title", entityAfter.Title);
        }

        [Fact]
        public void WriteToStreamAsync_Serializes_Properties_Correctly()
        {
            // Arrange
            string colourString = "Black";
            int numberOfWheels = 5;
            Car myCar = new Car();
            myCar.Colour = colourString;
            myCar.NumberOfWheels = numberOfWheels;

            // Act
            IEntity entityAfter = GetIEntityResultsFromWriteToStreamAsync(myCar);

            // Assert - Have properties
            entityAfter.Properties.ContainsKey("colour").ShouldBeTrue();
            entityAfter.Properties.ContainsKey("numberOfWheels").ShouldBeTrue();
            // Asset - property values
            Assert.Equal(colourString, entityAfter.Properties["colour"].ToString());
            Assert.Equal(numberOfWheels, Convert.ToInt32(entityAfter.Properties["numberOfWheels"]));
        }

        [Fact]
        public void WriteToStreamAsync_Serializes_EmbeddedLinks_Correctly()
        {
            // Arrange
            Car myCar = new Car();
            EmbeddedLink embeddedLink = new EmbeddedLink(new Uri("https://www.test.com"), "Embedded Link Class", "Embedded Link Rel");
            myCar.AddSubEntity(embeddedLink);

            // Act
            IEntity entityAfter = GetIEntityResultsFromWriteToStreamAsync(myCar);

            // Assert - Have properties
            string entityString = entityAfter.Entities[0].ToString();
            string expectedString = @"{
  ""class"": [
    ""Embedded Link Class""
  ],
  ""rel"": [
    ""Embedded Link Rel""
  ],
  ""href"": ""https://www.test.com""
}";
            Assert.Equal(expectedString, entityString);


        }

        [Fact]
        public void WriteToStreamAsync_Serializes_SubEntities_Correctly()
        {
            // Arrange
            Car myCar = new Car();
            Wheel myWheel = new Wheel();

            myWheel.id = 1;
            myWheel.Size = "124x55x18";

            myWheel.Title = "My Car Wheel";
            myWheel.AddRel("/rels/car/wheel");

            myWheel.AddClass("Wheel")
                .AddAction(new WebApiContrib.MediaType.Hypermedia.Action("Inflate", "Inflate the wheel", HTTP_Method.PUT, new Uri("https://api.test.com/wheel/inflate")))
                .AddLink(new SelfLink(new Uri("https://api.test.com/wheel/1")));

            myCar.Entities.Add(myWheel);

            // Act
            IEntity entityAfter = GetIEntityResultsFromWriteToStreamAsync(myCar);

            // Assert - Have properties
            string entityString = entityAfter.Entities[0].ToString();
            string expectedString = TestJSON.WheelClass();
            Assert.Equal(expectedString, entityString);
        }

        [Fact]
        public void WriteToStreamAsync_Serializes_SubEntities_Of_SubEntitiess_Correctly()
        {
            // Arrange
            Car myCar = new Car();
            Wheel myWheel = new Wheel();

            WheelNut myNut = new WheelNut();
            myNut.IsLockNut = true;
            myNut.AddClass("WheelNut");
            myNut.AddRel("/rels/wheelnut");

            // Add the subentity to the subentity
            myWheel.AddSubEntity(myNut);

            myWheel.id = 1;
            myWheel.Size = "124x55x18";

            myWheel.Title = "My Car Wheel";
            myWheel.AddRel("/rels/car/wheel");

            myWheel.AddClass("Wheel")
                .AddAction(new WebApiContrib.MediaType.Hypermedia.Action("Inflate", "Inflate the wheel", HTTP_Method.PUT, new Uri("https://api.test.com/wheel/inflate")))
                .AddLink(new SelfLink(new Uri("https://api.test.com/wheel/1")));



            myCar.Entities.Add(myWheel);

            // Act
            IEntity entityAfter = GetIEntityResultsFromWriteToStreamAsync(myCar);

            // Assert - Have properties
            string entityString = entityAfter.Entities[0].ToString();

            string expectedString = @"{
  ""class"": [
    ""Wheel""
  ],
  ""title"": ""My Car Wheel"",
  ""rel"": [
    ""/rels/car/wheel""
  ],
  ""properties"": {
    ""id"": 1,
    ""size"": ""124x55x18""
  },
  ""entities"": [
    {
      ""class"": [
        ""WheelNut""
      ],
      ""rel"": [
        ""/rels/wheelnut""
      ],
      ""properties"": {
        ""id"": 0,
        ""isLockNut"": true
      },
      ""entities"": []
    }
  ],
  ""actions"": [
    {
      ""name"": ""Inflate"",
      ""class"": [
        ""Inflate""
      ],
      ""method"": ""PUT"",
      ""href"": ""https://api.test.com/wheel/inflate"",
      ""title"": ""Inflate the wheel"",
      ""type"": ""application/json"",
      ""fields"": []
    }
  ],
  ""links"": [
    {
      ""rel"": [
        ""self""
      ],
      ""href"": ""https://api.test.com/wheel/1"",
      ""title"": null,
      ""type"": null
    }
  ]
}";
            Assert.Equal(expectedString, entityString);
        }

        [Fact]
        public void WriteToStreamAsync_Serializes_Actions_Correctly()
        {
            // Arrange
            Car myCar = new Car();
            Uri actionUri = new Uri("https://api.test.com/car/start");
            myCar.Actions.Add(new WebApiContrib.MediaType.Hypermedia.Action("Start", "Start the car", HTTP_Method.POST, actionUri));

            // Act
            string entityAfter = GetStringResultsFromWriteToStreamAsync(myCar);

            // Assert - Have properties
            string expectedEntity = @"{""class"":[],""properties"":{""numberOfWheels"":0},""entities"":[],""actions"":[{""name"":""Start"",""class"":[""Start""],""method"":""POST"",""href"":""https://api.test.com/car/start"",""title"":""Start the car"",""type"":""application/json"",""fields"":[]}]}";

            Assert.Equal(expectedEntity, entityAfter);
        }

        [Fact]
        public void WriteToStreamAsync_Serializes_Links_Correctly()
        {
            // Arrange
            Car myCar = new Car();
            Uri linkUri = new Uri("https://api.test.com/home");
            SelfLink selfLink = new SelfLink(linkUri);

            myCar.Links.Add(selfLink);

            // Act
            string entityAfter = GetStringResultsFromWriteToStreamAsync(myCar);

            // Assert - Have properties
            string expectedEntity = @"{""class"":[],""properties"":{""numberOfWheels"":0},""entities"":[],""links"":[{""rel"":[""self""],""href"":""https://api.test.com/home"",""title"":null,""type"":null}]}";
            Assert.Equal(expectedEntity, entityAfter);
        }
        
        public IEntity GetIEntityResultsFromWriteToStreamAsync(IEntity myCar)
        {
            MemoryStream memStream = new MemoryStream();

            Task writeTask = formatter.WriteToStreamAsync(typeof(Car), myCar, memStream, null, null);
            writeTask.Wait();

            memStream.Seek(0, SeekOrigin.Begin);
            string resultString = new StreamReader(memStream).ReadToEnd();

            IEntity entityAfter = JsonConvert.DeserializeObject<Car>(resultString);

            return entityAfter;

        }

        public string GetStringResultsFromWriteToStreamAsync(IEntity myCar)
        {
            MemoryStream memStream = new MemoryStream();

            Task writeTask = formatter.WriteToStreamAsync(typeof(Car), myCar, memStream, null, null);
            writeTask.Wait();

            memStream.Seek(0, SeekOrigin.Begin);
            string resultString = new StreamReader(memStream).ReadToEnd();

            return resultString;
        }

    }
}
