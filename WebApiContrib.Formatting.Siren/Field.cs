using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiContrib.Formatting.Siren
{
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

}
