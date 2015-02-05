using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiContrib.MediaType.Hypermedia
{
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
}
