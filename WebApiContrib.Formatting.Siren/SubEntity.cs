using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebApiContrib.Formatting.Siren
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1309:FieldNamesMustNotBeginWithUnderscore", Justification = "Allowed.")]
    public abstract class SubEntity : Entity, ISubEntity // , ISerializable
    {
        private List<string> _rel;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Sub-entities can be expressed as either an embedded link or an embedded representation. In JSON Siren, sub-entities are represented by an entities array, such as { "entities": [{ ... }] }.
        /// </summary>
        /// -------------------------------------------------------------------------------------------------
        public SubEntity()
        {
            this._rel = new List<string>();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Defines the relationship of the sub-entity to its parent, per <a href="http://tools.ietf.org/html/rfc5988">Web Linking (RFC5899)</a>. MUST be an array of strings. Required.</summary>
        /// -------------------------------------------------------------------------------------------------
        public List<string> Rel
        {
            get
            {
                return this._rel;
            }

            set 
            {
                this._rel = value; 
            }
        }

        public ISubEntity AddRel(string rel)
        {
            this.Rel.Add(rel);
            return this;
        }

    }
}