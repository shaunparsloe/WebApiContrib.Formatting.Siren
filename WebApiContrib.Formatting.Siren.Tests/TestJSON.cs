using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiContrib.Formatting.Siren.Tests
{
    public class TestJSON
    {
        public static string WheelClass()
        {
            return @"{
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
  ""entities"": [],
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
        }
    }
}
