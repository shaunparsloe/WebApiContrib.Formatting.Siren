using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiContrib.Formatting.Siren
{
    public class EmbeddedLink : ISubEntity
    {
        public List<string> Class { get; set; }

        public List<string> Rel { get; set; }
        
        public Uri Href { get; set; }
        
        public string Type { get; set; }

        private void Initialise()
        {
            this.Class = new List<string>();
            this.Rel = new List<string>();
        }

        public EmbeddedLink(Uri href, string paramClass, string rel)
        {
            this.Initialise();

            this.Href = href;
            this.Class.Add(paramClass);
            this.Rel.Add(rel);
        }

        public EmbeddedLink(Uri href, List<string> paramClass, List<string> rel)
        {
            this.Href = href;
            this.Class = paramClass;
            this.Rel = rel;
        }

        public ISubEntity AddRel(string rel)
        {
            this.Rel.Add(rel);
            return this;
        }
    }
}