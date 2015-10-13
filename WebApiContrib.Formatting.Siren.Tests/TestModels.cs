using System;
using System.Collections.Generic;
using WebApiContrib.MediaType.Hypermedia;

namespace WebApiContrib.Formatting.Siren.Tests
{
    public class Car : Entity
    {
        public string Colour { get; set; }
        public int NumberOfWheels { get; set; }
    }

    public class Wheel : SubEntity
    {
        public int id { get; set; }
        public string Size { get; set; }
    }

    public class WheelNut : SubEntity
    {
        public int id { get; set; }
        public bool IsLockNut { get; set; }
    }

    public class SelfLink : ILink
    {
        public List<string> Rel { get; set; }
        public string Href { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }

        public SelfLink(Uri href, string title = "Self")
        {
            this.Rel = new List<string>();
            this.Rel.Add("self");
            this.Href = href.ToString();
        }
    }

    public class Engine
    {
        public int Cylinders { get; set; }
        public double Size { get; set; }
        public bool Turbocharged { get; set; }
    }
}
