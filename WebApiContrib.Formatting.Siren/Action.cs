using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiContrib.Formatting.Siren
{
    /// <summary>
    /// Actions show available behaviors an entity exposes.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1309:FieldNamesMustNotBeginWithUnderscore", Justification = "Allowed.")]
    public class Action
    {
        private string _type = "application/x-www-form-urlencoded";
        private List<string> _class;
        private List<Field> _fields;

        public Action(string name, string title, HTTP_Method method, Uri href, string type = "application/json")
        {
            this.Name = name;
            this.Title = title;
            this.Class.Add(this.Name);
            this.Method = method;
            this.Href = href;
            this.Type = type;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>A string that identifies the action to be performed. Required. </summary>
        /// -------------------------------------------------------------------------------------------------
        public string Name { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Describes the nature of an entity's content based on the current representation. Possible values are implementation-dependent and should be documented. MUST be an array of strings. Optional. </summary>
        ///
        /// <value> The class. </value>
        /// -------------------------------------------------------------------------------------------------
        public List<string> Class
        {
            get
            {
                if (this._class == null)
                {
                    this._class = new List<string>();
                }

                return this._class;
            }

            set
            {
                this._class = value;
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>An enumerated attribute mapping to a protocol method. For HTTP, these values may be GET, PUT, POST, DELETE, or PATCH. As new methods are introduced, this list can be extended. If this attribute is omitted, GET should be assumed. Optional. </summary>
        /// -------------------------------------------------------------------------------------------------
        public HTTP_Method Method { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>The URI of the action. Required.</summary>
        ///
        /// <value> The hRef. </value>
        /// -------------------------------------------------------------------------------------------------
        public Uri Href { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Descriptive text about the action. Optional.</summary>
        /// -------------------------------------------------------------------------------------------------
        public string Title { get; set; }

        /// <summary>
        /// The encoding type for the request. When omitted and the fields attribute exists, the default value is application/x-www-form-urlencoded. Optional.
        /// </summary>
        public string Type
        {
            get
            {
                return this._type;
            }

            set
            {
                this._type = value;
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>A collection of fields, expressed as an array of objects in JSON Siren such as 
        ///          <c>{ "fields" : [{ ... }] }</c>. See Fields. Optional. </summary>
        /// -------------------------------------------------------------------------------------------------
        public List<Field> Fields
        {
            get
            {
                if (this._fields == null)
                {
                    this._fields = new List<Field>();
                }

                return this._fields;
            }

            set
            {
                this._fields = value;
            }
        }
    }

}
