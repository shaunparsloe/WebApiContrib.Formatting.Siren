using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Routing;

namespace WebApiContrib.MediaType.Hypermedia
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1309:FieldNamesMustNotBeginWithUnderscore", Justification = "Allowed.")]
    public abstract class LinkFactory
    {
        private readonly UrlHelper _urlHelper;
        private readonly string _controllerName;

        protected LinkFactory(HttpRequestMessage request, Type controllerType)
        {
            this._urlHelper = new UrlHelper(request);
            this._controllerName = this.GetControllerName(controllerType);
        }

        private string GetControllerName(Type controllerType)
        {
            var name = controllerType.Name;
            return name.Substring(0, name.Length - "controller".Length).ToLower();
        }

        protected Link GetLink<TController>(string rel, object id, string action, string route)
        {
            var uri = this.GetUri(new { controller = this.GetControllerName(typeof(TController)), id, action }, route);
            return new Link { Href = uri.ToString(), Rel = new List<string>() { rel } };
        }

        protected Uri GetUri(object routeValues, string route)
        {
            return new Uri(this._urlHelper.Link(route, routeValues));
        }

        public class Rels
        {
            public const string Self = "self";
        }
    }

    public abstract class LinkFactory<TController> : LinkFactory
    {
        public LinkFactory(HttpRequestMessage request) : base(request, typeof(TController)) 
        { 
        }
    }
}