using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiContrib.Formatting.Siren
{
    /// <summary>   Interface for entity. </summary>
    public interface IEntity
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Describes the nature of an entity's content based on the current representation. Possible
        /// values are implementation-dependent and should be documented. MUST be an array of strings.
        /// Optional.
        /// </summary>
        ///
        /// <value> The class. </value>
        /// -------------------------------------------------------------------------------------------------
        List<string> Class { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>   A set of key-value pairs that describe the state of an entity. </summary>
        ///
        /// <value> The properties. </value>
        /// -------------------------------------------------------------------------------------------------
        Dictionary<string, object> Properties { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// A collection of related sub-entities. If a sub-entity contains an href value, it should be
        /// treated as an embedded link. Clients may choose to optimistically load embedded links. If no
        /// href value exists, the sub-entity is an embedded entity representation that contains all the
        /// characteristics of a typical entity. One difference is that a sub-entity MUST contain a rel
        /// attribute to describe its relationship to the parent entity.
        /// 
        /// In JSON Siren, this is represented as an array. Optional.
        /// </summary>
        ///
        /// <value> The entities. </value>
        /// -------------------------------------------------------------------------------------------------
        List<object> Entities { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// A collection of items that describe navigational links, distinct from entity relationships.
        /// Link items should contain a rel attribute to describe the relationship and an href attribute
        /// to point to the target URI. Entities should include a link rel to self. In JSON Siren, this
        /// is represented as "links": <c>[{ "rel": ["self"], "href": "http://api.x.io/orders/1234"
        /// }]</c> Optional.
        /// </summary>
        ///
        /// <value> The links. </value>
        /// -------------------------------------------------------------------------------------------------
        List<ILink> Links { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// A collection of action objects, represented in JSON Siren as an array such as <code>{
        /// "actions": [{ ... }] }</code>. See Actions. Optional.
        /// </summary>
        ///
        /// <value> The actions. </value>
        /// -------------------------------------------------------------------------------------------------
        List<Action> Actions { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>   Descriptive text about the entity. Optional. </summary>
        ///
        /// <value> The title. </value>
        /// -------------------------------------------------------------------------------------------------
        string Title { get; set; }


        IEntity AddClass(string className);


        IEntity AddProperty(string propertyName, object propertyValue);

        IEntity AddSubEntity(ISubEntity subEntity);

        IEntity AddLink(ILink link);

        IEntity AddAction(Action action);
    }

    /// <summary>   Interface for sub entity. </summary>
    public interface ISubEntity
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the relative. </summary>
        ///
        /// <value> The relative. </value>
        /// -------------------------------------------------------------------------------------------------
        List<string> Rel { get; set; }

        ISubEntity AddRel(string rel);
    }

    /// <summary>Links represent navigational transitions. In JSON Siren, links are represented as an array
    /// inside the entity, such as
    /// { "links": [{ "rel": [ "self" ], "href": "http:// api.x.io/orders/42"}] }
    /// </summary>
    public interface ILink
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>Defines the relationship of the link to its entity, per <a href="http://tools.ietf.org/html/rfc5988">Web Linking (RFC5899)</a>. MUST be an array of strings. Required.</summary>
        ///
        /// <value> The relative. </value>
        /// -------------------------------------------------------------------------------------------------
        List<string> Rel { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>The URI of the linked resource. Required</summary>
        ///
        /// <value> The hRef. </value>
        /// -------------------------------------------------------------------------------------------------
        string Href { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Text describing the nature of a link. Optional.</summary>
        ///
        /// <value> The title. </value>
        /// -------------------------------------------------------------------------------------------------
        string Title { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Defines media type of the linked resource, per <a href="http://tools.ietf.org/html/rfc5988">Web Linking (RFC5899)</a>. Optional.</summary>
        ///
        /// <value> The type. </value>
        /// -------------------------------------------------------------------------------------------------
        string Type { get; set; }
    }


}
