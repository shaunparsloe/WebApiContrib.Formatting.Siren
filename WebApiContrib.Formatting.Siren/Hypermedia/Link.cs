using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiContrib.MediaType.Hypermedia
{
    /// <summary>
    /// A base class for relation links
    /// </summary>
    public class Link : WebApiContrib.MediaType.Hypermedia.ILink
    {
        public List<string> Rel { get; set; }

        public string Href { get; set; }

        public string Method { get; set; }

        public string Title { get;  set; }

        public string Type { get; set; }

        public Link()
        {
            this.Rel = new List<string>();
            this.Method = HTTP_Method.GET.ToString();
        }

        public Link(string relation, string href, string method = "GET", string title = null)
        {
            this.Rel = new List<string>();
            this.Rel.Add(relation);
            this.Href = href;
            this.Method = method;
            this.Title = title;
        }
    }
}