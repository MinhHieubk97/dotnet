using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Microsoft.AspNet.OData.Query;

namespace brechtbaekelandt.enableODataQueryOnComputedProperties.Extensions
{
    public static class IQueryableExtensions
    {
        ///// <summary>
        ///// Enables querying on computed fields (field that is not supported by LINQ-to-Entities), this method must be used after you build your LINQ-to-Entities query!
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="opts">The opts.</param>
        ///// <param name="query">The query.</param>
        ///// <returns></returns>
        ///// <exception cref="Exception"></exception>
        public static IQueryable<T> EnableODataQueryOnComputedProperties<T>(this IQueryable<T> query, ODataQueryOptions opts) where T : class
        {
            if (opts == null)
            {
                return query;
            }

            var originalOrderBy = opts.OrderBy?.RawValue ?? string.Empty;
            var originalOrderByFields = originalOrderBy.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var originalSelect = opts.SelectExpand?.RawSelect ?? string.Empty;
            var originalSelectFields = originalSelect.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            var shouldSelectOnComputedProperties = false;
            var shouldOrderByOnComputedProperties = false;

            var originalRequestUri = opts.Request.RequestUri;

            try
            {
                opts.Request.RequestUri = originalRequestUri.RemoveODataSelect();

                // Rebuild the ODataQueryOptions
                opts = new ODataQueryOptions(opts.Context, opts.Request);

                // This will throw an exception if it should order by a computed property
                var result = opts.ApplyTo(query.Take(1)).Cast<T>().ToList();

                // If not, then apply the query
                query = opts.ApplyTo(query).Cast<T>();
            }
            catch (NotSupportedException e)
            {
                shouldOrderByOnComputedProperties = true;
            }

            try
            {
                opts.Request.RequestUri = originalRequestUri.RemoveODataOrderBy();

                // Rebuild the ODataQueryOptions
                opts = new ODataQueryOptions(opts.Context, opts.Request);

                // This will throw an exception if it should select at least one computed property
                var result = opts.ApplyTo(query.Take(1)).Cast<T>().ToList();

                // If not, then apply the query
                query = opts.ApplyTo(query).Cast<T>();
            }
            catch (NotSupportedException e)
            {
                shouldSelectOnComputedProperties = true;
            }

            IQueryable<T> newQuery;

            if (shouldSelectOnComputedProperties)
            {
                // Get all properties of type T
                var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(prop => prop.GetGetMethod() != null).ToList();

                // If there is a $select, then only get the properties of type T that are necessary (eg that are in the $select clause), otherwise get all properties
                var selectProperties = originalSelectFields.Count != 0 ? properties.Where(prop => originalSelectFields.Any(select => select == prop.Name) || originalOrderByFields.Any(orderby => orderby == prop.Name)).ToList() : properties;

                // Remove the select and orderby because otherwise the next two lines are going to throw an exception
                opts.Request.RequestUri = originalRequestUri.RemoveODataOrderBy().RemoveODataSelect();

                // Rebuild the ODataQueryOptions
                opts = new ODataQueryOptions(opts.Context, opts.Request);

                // Apply the new query
                query = opts.ApplyTo(query).Cast<T>();

                // Remove the filter, otherwise it wil filter again (and the filter is already applied in the previous two lines)
                opts.Request.RequestUri = opts.Request.RequestUri.RemoveODataFilter();

                // Init result list
                var result = new List<T>();

                // I know, I know, reflection is not the fastest..., will need to check article https://vagifabilov.wordpress.com/2010/04/02/dont-use-activator-createinstance-or-constructorinfo-invoke-use-compiled-lambda-expressions/
                query.ToList().ForEach(queryObject =>
                {
                    // Create an instance of type T
                    var obj = Activator.CreateInstance(typeof(T)) as T;

                    // Set the properties that are needed
                    selectProperties.ForEach(prop =>
                    {
                        if (prop.GetSetMethod() != null)
                        {
                            prop.SetValue(obj, prop.GetValue(queryObject));
                        }
                    });

                    // Add to the result
                    result.Add(obj);
                });

                newQuery = result.AsQueryable();
            }
            else
            {
                newQuery = query;
            }

            if (!shouldOrderByOnComputedProperties)
            {
                return newQuery;
            }

            newQuery = newQuery.ToList().AsQueryable();

            var orderByIndex = 0;

            // Do the orderby
            originalOrderByFields.ForEach(field =>
            {
                var orderByFieldName = field.Split(' ')[0];

                var orderByDirection = string.Empty;

                if (field.Split(' ').Length > 1)
                {
                    orderByDirection = field.Split(' ')[1];
                }

                if (orderByIndex == 0)
                {
                    // First OrderBy(Descending)
                    newQuery = orderByDirection == "desc" ?
                        newQuery.OrderByDescending(orderByFieldName) :
                        newQuery.OrderBy(orderByFieldName);
                }
                else
                {
                    // Then (if necessary) ThenBy(Descending)
                    newQuery = orderByDirection == "desc" ?
                        ((IOrderedQueryable<T>)newQuery).ThenByDescending(orderByFieldName) :
                        ((IOrderedQueryable<T>)newQuery).ThenBy(orderByFieldName);
                }

                orderByIndex++;
            });

            return newQuery;
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string propertyName)
        {
            var property = typeof(T).GetProperty(propertyName);

            return query.OrderBy(p => property.GetValue(p));
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> query, string propertyName)
        {
            var property = typeof(T).GetProperty(propertyName);

            return query.OrderByDescending(p => property.GetValue(p));
        }

        public static IOrderedQueryable<T>ThenBy<T>(this IOrderedQueryable<T> query, string propertyName)
        {
            var property = typeof(T).GetProperty(propertyName);

            return query.ThenBy(p => property.GetValue(p));
        }

        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> query, string propertyName)
        {
            var property = typeof(T).GetProperty(propertyName);

            return query.ThenByDescending(p => property.GetValue(p));
        }
    }
}