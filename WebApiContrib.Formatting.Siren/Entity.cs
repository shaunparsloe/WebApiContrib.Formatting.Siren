using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WebApiContrib.Formatting.Siren
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary> An Entity is a URI-addressable resource that has properties and actions associated with it. It may contain sub-entities and navigational links.
    ///
    /// Root entities and sub-entities that are embedded representations SHOULD contain a links collection with at least one item contain a rel value of self and an href attribute with a value of the entity's URI.
    ///
    /// Sub-entities that are embedded links MUST contain an href attribute with a value of its URI.</summary>
    /// -------------------------------------------------------------------------------------------------
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1309:FieldNamesMustNotBeginWithUnderscore", Justification = "Allowed.")]
    public abstract class Entity : IEntity // , ISerializable
    {
        private List<string> _class;
        private List<ILink> _links;
        private List<Action> _actions;
        private List<object> _entities;
        private Dictionary<string, object> _dict;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>A set of key-value pairs that describe the state of an entity. </summary>
        /// -------------------------------------------------------------------------------------------------
        public Dictionary<string, object> Properties
        {
            get
            {
                if (this._dict == null)
                { 
                    this._dict = new Dictionary<string, object>(); 
                }

                return this._dict;
            }

            set 
            { 
                this._dict = value; 
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>   A collection of related sub-entities. If a sub-entity contains an href value, it should be treated as an embedded link. Clients may choose to optimistically load embedded links. If no href value exists, the sub-entity is an embedded entity representation that contains all the characteristics of a typical entity. One difference is that a sub-entity MUST contain a rel attribute to describe its relationship to the parent entity.
        ///
        /// In JSON Siren, this is represented as an array. Optional. </summary>
        /// -------------------------------------------------------------------------------------------------
        public List<object> Entities
        {
            get
            {
                if (this._entities == null)
                { 
                    this._entities = new List<object>(); 
                }

                return this._entities;
            }

            set 
            { 
                this._entities = value; 
            }
        }

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
        /// <summary>A collection of items that describe navigational links, distinct from entity relationships. Link items should contain a rel attribute to describe the relationship and an href attribute to point to the target URI. Entities should include a link rel to self. In JSON Siren, this is represented as "links": <c>[{ "rel": ["self"], "href": "http://api.x.io/orders/1234" }]</c> Optional. </summary>
        ///
        /// <value> The links. </value>
        /// -------------------------------------------------------------------------------------------------
        public List<ILink> Links
        {
            get
            {
                if (this._links == null)
                { 
                    this._links = new List<ILink>(); 
                }

                return this._links;
            }

            set 
            { 
                this._links = value; 
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>   A collection of action objects, represented in JSON Siren as an array such as <code>{ "actions": [{ ... }] }</code>. See Actions. Optional </summary>
        ///
        /// <value> The actions. </value>
        /// -------------------------------------------------------------------------------------------------
        public List<Action> Actions
        {
            get
            {
                if (this._actions == null)
                { 
                    this._actions = new List<Action>();
                }

                return this._actions;
            }

            set 
            { 
                this._actions = value; 
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>   Descriptive text about the entity. Optional. </summary>
        ///
        /// <value> The title. </value>
        /// -------------------------------------------------------------------------------------------------
        public string Title { get; set; }

        public Entity()
        {
        }
     }
    
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

    /// -------------------------------------------------------------------------------------------------
    /// <summary>  Fields represent controls inside of actions. They may contain these attributes: </summary>
    /// -------------------------------------------------------------------------------------------------
    public class Field
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the WebApiContrib.Formatting.Siren.Field class.
        /// </summary>
        ///
        /// <param name="name">     The name. </param>
        /// <param name="title">    The title. </param>
        /// <param name="value">    The value. </param>
        /// <param name="type">     The type. </param>
        /// -------------------------------------------------------------------------------------------------
        public Field(string name, string title, object value, HTMLInputType type = HTMLInputType.text)
        {
            this.Name = name;
            this.Title = title;
            this.Value = value;
            this.Type = type;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>A name describing the control. Required.</summary>
        ///
        /// <value> The name. </value>
        /// -------------------------------------------------------------------------------------------------
        public string Name { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>The input type of the field. This may include any of the following <a href="http://www.w3.org/TR/html5/single-page.html#the-input-element">element specified in HTML5</a>:
        ///
        /// When missing, the default value is text. Serialization of these fields will depend on the value of the action's type attribute. See type under Actions, above. Optiona </summary>
        ///
        /// <value> The type. </value>
        /// -------------------------------------------------------------------------------------------------
        public HTMLInputType Type { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>A value assigned to the field. Optional. </summary>
        ///
        /// <value> The value. </value>
        /// -------------------------------------------------------------------------------------------------
        public object Value { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Textual annotation of a field. Clients may use this as a label. Optional. </summary>
        ///
        /// <value> The title. </value>
        /// -------------------------------------------------------------------------------------------------
        public string Title { get; set; }
    }

    /// -------------------------------------------------------------------------------------------------
    /// <summary>   Values that represent HTTP methods. </summary>
    /// -------------------------------------------------------------------------------------------------
    [JsonConverter(typeof(StringEnumConverter))]
    public enum HTTP_Method
    {
        /// <summary>   An enum constant representing the get option. </summary>
        GET,

        /// <summary>   An enum constant representing the post option. </summary>
        POST, 

        /// <summary>   An enum constant representing the put option. </summary>
        PUT, 

        /// <summary>   An enum constant representing the patch option. </summary>
        PATCH, 

        /// <summary>   An enum constant representing the delete option. </summary>
        DELETE 
    }

    /// -------------------------------------------------------------------------------------------------
    /// <summary>   Values that represent HTML input types. </summary>
    /// -------------------------------------------------------------------------------------------------
    [JsonConverter(typeof(StringEnumConverter))]
    public enum HTMLInputType
    {
        hidden, 
        text, 
        search, 
        tel, 
        url, 
        email, 
        password, 
        datetime, 
        date, 
        month, 
        week, 
        time, 
        number, 
        range, 
        color, 
        checkbox, 
        radio, 
        file, 
        image, 
        button
    }
}