﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using WebApiContrib.MediaType.Hypermedia;
using System.Collections;

namespace WebApiContrib.Formatting.Siren
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1309:FieldNamesMustNotBeginWithUnderscore", Justification = "Allowed.")]
    public sealed class SirenMediaTypeFormatter : JsonMediaTypeFormatter
    {
        private const string _MediaType = "application/vnd.siren+json";

        public SirenMediaTypeFormatter()
        {
            //Clear Supported media types because base class constructor has modified it to support Json. We only support siren format here.
            SupportedMediaTypes.Clear();
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(_MediaType));
        }

        public override bool CanReadType(Type type)
        {
            return typeof(IEntity).IsAssignableFrom(type);
        }

        public override bool CanWriteType(Type type)
        {
            return typeof(IEntity).IsAssignableFrom(type);
        }

        // Set the content headers for the type of document being returned
        public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
        {
            base.SetDefaultContentHeaders(type, headers, new MediaTypeHeaderValue(_MediaType));
        }

        // By the time we get to WriteToStreamAsync, it's too late to set the Content Headers
        public override Task WriteToStreamAsync(Type type, object value, Stream stream, System.Net.Http.HttpContent content, System.Net.TransportContext transportContext)
        {

            if (typeof(IEntity).IsAssignableFrom(type))
            {
                var objectToSerialize = SerializeSirenEntity((IEntity)value);
                return JsonHelpers.WriteJsonToStreamAsync(stream, objectToSerialize);
            }

            //We need to return something anyways
            return Task.Delay(0);
        }

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
                        bool ignore = false;
                        foreach (var attrib in prop.CustomAttributes)
                        {
                            if (attrib.AttributeType.FullName == "WebApiContrib.Formatting.Siren.SirenIgnoreAttribute")
                            {
                                ignore = true;
                            }
                        }
                        if (!ignore)
                        {
                            entityProperties.Add(prop.Name, prop.GetValue(entityItem, null));
                        }
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

                    if (typeof(SubEntity).IsAssignableFrom(embeddedSirenSubEntityObject.GetType()))
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
                if (prop.GetValue(subEntityItem, null) != null)
                {
                    if (!ReservedWords.ListOfWords().Contains(prop.Name))
                    {
                        bool ignore = false;
                        foreach (var attrib in prop.CustomAttributes)
                        {
                            if (attrib.AttributeType.FullName == "WebApiContrib.Formatting.Siren.SirenIgnoreAttribute")
                            {
                                ignore = true;
                            }
                        }
                        if (!ignore)
                        {
                            entityProperties.Add(prop.Name, prop.GetValue(subEntityItem, null));
                        }
                    }
                }
            }

            if (entityProperties.Count > 0)
            {
                resultantSubEntity.Add("properties", entityProperties);
            }

            if (subEntityItem.Entities != null)
            {
                List<object> subEntities = new List<object>();
                foreach (object embeddedSirenSubEntityObject in subEntityItem.Entities)
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

                resultantSubEntity.Add("entities", subEntities);
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

        public override sealed Task<Object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            using (var strdr = new StreamReader(readStream))
            using (var jtr = new JsonTextReader(strdr))
            {                                
                return strdr.ReadToEndAsync()
                    .ContinueWith(result =>
                    {
                        var jsonString = string.Empty;
                        if (result.IsFaulted || string.IsNullOrEmpty(jsonString = result.Result))
                            return null;

                        JObject jobject;
                        try
                        {
                            jobject = JObject.Parse(jsonString);
                        }
                        catch (JsonReaderException)
                        {
                            //We shouldn't go silent...
                            return null;
                        }
                        return DeSerializeSirenEntity(type, jobject);
                    }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private object DeSerializeSirenEntity(Type type, JObject deserialized)
        {
            dynamic entity = Activator.CreateInstance(type);

            foreach (JProperty property in deserialized.Properties())
            {
                switch (property.Name)
                {
                    case "class":
                        foreach (JValue jvalue in property.Value)
                        {
                            string stringValue = jvalue.Value.ToString();
                            entity.Class.Add(stringValue);
                        }
                        break;

                    case "title":
                        entity.Title = property.Value.ToString();
                        break;


                    case "rel":
                        foreach (JValue jvalue in property.Value)
                        {
                            entity.Rel.Add(jvalue.Value.ToString());
                        }
                        break;

                    case "properties":
                        foreach (JProperty objectProperty in property.Value)
                        {
                            string x = objectProperty.Value.ToString();
                            string propertyName = objectProperty.Name.ToString();

                            PropertyInfo prop = entity.GetType()
                                .GetProperty(propertyName,
                                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                            if (null != prop && prop.CanWrite)
                            {
                                bool ignore = false;
                                foreach (var attrib in prop.CustomAttributes)
                                {
                                    if (attrib.AttributeType.FullName == "WebApiContrib.Formatting.Siren.SirenIgnoreAttribute")
                                    {
                                        ignore = true;
                                    }
                                }
                                if (!ignore)
                                {
                                    var val = Convert.ChangeType(objectProperty.Value, prop.PropertyType);
                                    prop.SetValue(entity, val, null);
                                }
                            }

                            // entity.Class.Add(propertyName);
                        }
                        break;

                    case "entities":
                        foreach (JObject subEntityJObject in property.Value)
                        {
                            // TODO: Get the correct type to pass into deserialize
                            if (subEntityJObject.Children().Contains("rel"))
                            {
                                DeSerializeSirenEntity(typeof(SubEntity), subEntityJObject);
                            }
                            else
                            {
                                // DeSerializeSirenEntity(typeof(EmbeddedLink), subEntityJObject);
                                WebApiContrib.MediaType.Hypermedia.EmbeddedLink link =
                                    subEntityJObject.ToObject<WebApiContrib.MediaType.Hypermedia.EmbeddedLink>();
                                entity.AddSubEntity(link);
                            }

                        }
                        break;

                    case "actions":
                        foreach (JObject jObject in property.Value)
                        {
                            WebApiContrib.MediaType.Hypermedia.Action action =
                                jObject.ToObject<WebApiContrib.MediaType.Hypermedia.Action>();
                            entity.AddAction(action);
                        }
                        break;

                    case "links":
                        foreach (JObject jObject in property.Value)
                        {
                            WebApiContrib.MediaType.Hypermedia.Link link =
                                jObject.ToObject<WebApiContrib.MediaType.Hypermedia.Link>();
                            entity.AddLink(link);
                        }
                        break;

                    default:
                        Debug.WriteLine("Key " + property.ToString() + " is not supported by the Siren Deserializer");
                        break;
                }
            }
            return entity;
        }
    }
}