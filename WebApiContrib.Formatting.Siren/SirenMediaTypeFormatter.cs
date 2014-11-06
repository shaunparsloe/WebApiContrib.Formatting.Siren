using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace WebApiContrib.Formatting.Siren
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1309:FieldNamesMustNotBeginWithUnderscore", Justification = "Allowed.")]
    public class SirenMediaTypeFormatter : JsonMediaTypeFormatter 
    {
        private const string _MediaType = "application/vnd.siren+json";

        public SirenMediaTypeFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(_MediaType));

            // Put this line in so that it does not crash if is is being browsed by a standard browser.
            // By supporting text/html we can 
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html")); 
        }

        public override bool CanReadType(Type type)
        {
            bool blnRetval = typeof(IEntity).IsAssignableFrom(type);
            return blnRetval;
        }

        public override bool CanWriteType(Type type)
        {
            bool blnRetval = typeof(IEntity).IsAssignableFrom(type);
            return blnRetval;
        }

        // Set the content headers for the type of document being returned
        public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
        {
            base.SetDefaultContentHeaders(type, headers, new MediaTypeHeaderValue(_MediaType));
        }

        // By the time we get to WriteToStreamAsync, it's too late to set the Content Headers
        public override Task WriteToStreamAsync(Type type, object value, Stream stream, System.Net.Http.HttpContent content, System.Net.TransportContext transportContext)
        {
            return Task.Factory.StartNew(() =>
            {
                if (typeof(IEntity).IsAssignableFrom(type))
                {
                    var objectToSerialize = SerializeSirenEntity((IEntity)value);
                    JsonHelpers.WriteJsonToStream(stream, objectToSerialize);
                }
            });
        }

        //private object BuildSirenDocument(object models, Stream stream, string contenttype)
        //{
        //    List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();

        //    items.Add(this.FormatSirenEntity((Entity)models));

        //    return items;
        //}

        private Dictionary<string, object> SerializeSirenEntity(IEntity entityItem)
        {
            Dictionary<string, object> resultantEntity = new Dictionary<string, object>();
            Dictionary<string, object> entityProperties = new Dictionary<string, object>();

            if (entityItem.Class != null)
            {
                resultantEntity.Add("class", entityItem.Class);
            }

            if (entityItem.Title != null)
            {
                resultantEntity.Add("title", entityItem.Title);
            }

            System.Reflection.PropertyInfo[] properties = entityItem.GetType().GetProperties();

            // Any property that is not one of the "reserved" names
            foreach (System.Reflection.PropertyInfo prop in properties)
            {
                if (prop.GetValue(entityItem, null) != null)
                {
                    if (!ReservedWords.ListOfWords().Contains(prop.Name))
                    {
                        entityProperties.Add(prop.Name, prop.GetValue(entityItem, null));
                    }
                }
            }

            if (entityProperties.Count > 0)
            {
                resultantEntity.Add("properties", entityProperties);
            }

            if (entityItem.Entities != null)
            {
                List<object> subEntities = new List<object>();
                foreach (object embeddedSirenSubEntityObject in entityItem.Entities)
                {
                    if (embeddedSirenSubEntityObject.GetType().IsSubclassOf(typeof(EmbeddedLink)) ||
                        embeddedSirenSubEntityObject.GetType() == typeof(EmbeddedLink))
                    {
                        subEntities.Add(this.SerializeSirenEmbeddedLink((EmbeddedLink)embeddedSirenSubEntityObject));
                    }

                    if (embeddedSirenSubEntityObject.GetType().IsSubclassOf(typeof(SubEntity)))
                    {
                        subEntities.Add(this.SerializeSirenSubEntity((SubEntity)embeddedSirenSubEntityObject));
                    }
                }

                resultantEntity.Add("entities", subEntities);
            }

            if (entityItem.Actions.Count > 0)
            {
                resultantEntity.Add("actions", entityItem.Actions);
            }

            if (entityItem.Links.Count > 0)
            {
                resultantEntity.Add("links", entityItem.Links);
            }

            return resultantEntity;
        }

        private Dictionary<string, object> SerializeSirenSubEntity(SubEntity subEntityItem)
        {
            Dictionary<string, object> resultantSubEntity = new Dictionary<string, object>();
            Dictionary<string, object> entityProperties = new Dictionary<string, object>();

            if (subEntityItem.Class != null)
            {
                resultantSubEntity.Add("class", subEntityItem.Class);
            }

            if (subEntityItem.Title != null)
            {
                resultantSubEntity.Add("title", subEntityItem.Title);
            }

            resultantSubEntity.Add("rel", subEntityItem.Rel);

            System.Reflection.PropertyInfo[] properties = subEntityItem.GetType().GetProperties();

            // Any property that is not one of the "reserved" names
            foreach (System.Reflection.PropertyInfo prop in properties)
            {
                if (!ReservedWords.ListOfWords().Contains(prop.Name))
                {
                    entityProperties.Add(prop.Name, prop.GetValue(subEntityItem, null));
                }
            }

            if (entityProperties.Count > 0)
            {
                resultantSubEntity.Add("properties", entityProperties);
            }

            if (subEntityItem.Entities != null)
            {
                resultantSubEntity.Add("entities", subEntityItem.Entities);
            }

            if (subEntityItem.Actions.Count > 0)
            {
                resultantSubEntity.Add("actions", subEntityItem.Actions);
            }

            if (subEntityItem.Links.Count > 0)
            {
                resultantSubEntity.Add("links", subEntityItem.Links);
            }

            return resultantSubEntity;
        }

        private Dictionary<string, object> SerializeSirenEmbeddedLink(EmbeddedLink embeddedLink)
        {
            Dictionary<string, object> retval = new Dictionary<string, object>();

            retval.Add("Class", embeddedLink.Class);
            retval.Add("Rel", embeddedLink.Rel);
            retval.Add("Href", embeddedLink.Href);

            return retval;
        }
    }
}