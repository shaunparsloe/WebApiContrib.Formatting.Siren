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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the WebApiContrib.Formatting.Siren.Entity class.
        /// </summary>
        /// 
        /// <param name="className">    Class name of the object. </param>
        ///-------------------------------------------------------------------------------------------------
        public Entity(string className)
        {
            this.Class.Add(className);
        }

        public IEntity AddClass(string className)
        {
            this.Class.Add(className);
            return this;
        }

        public IEntity AddProperty(string propertyName, object propertyValue)
        {
            this.Properties.Add(propertyName, propertyValue);
            return this;
        }

        public IEntity AddSubEntity(ISubEntity subEntity)
        {
            this.Entities.Add(subEntity);
            return this;
        }

        public IEntity AddLink(ILink link)
        {
            this.Links.Add(link);
            return this;
        }

        public IEntity AddAction(Action action)
        {
            this.Actions.Add(action);
            return this;
        }


     }
}