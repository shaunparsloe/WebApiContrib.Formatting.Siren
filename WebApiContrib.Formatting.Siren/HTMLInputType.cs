using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiContrib.Formatting.Siren
{

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
