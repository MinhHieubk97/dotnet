using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using brechtbaekelandt.enableODataQueryOnComputedProperties.Data.Entities;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.OData.Edm;


namespace brechtbaekelandt.enableODataQueryOnComputedProperties
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapODataServiceRoute("odata", "odata", GetModel());
            config.Count().Filter().OrderBy().Expand().Select().MaxTop(null);
            config.EnsureInitialized();
        }
        public static IEdmModel GetModel()
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();

            var persons = builder.EntitySet<Person>("Persons");
            var animals = builder.EntitySet<Animal>("Animals");

            return builder.GetEdmModel();
        }

    }
}
