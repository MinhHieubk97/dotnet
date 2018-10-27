using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace brechtbaekelandt.enableODataQueryOnComputedProperties.Extensions
{
    public static class UriExtensions
    {
        public static Uri RemoveFieldFromODataOrderBy(this Uri input, string fieldName)
        {
            var url = input.ToString();

            var orderByIndex = url.IndexOf("$orderby=", StringComparison.OrdinalIgnoreCase);

            if (orderByIndex == -1)
            {
                return input;
            }

            var orderByQuery = url.Substring(orderByIndex);

            var firstAmpersandIndex = orderByQuery.IndexOf("&", StringComparison.OrdinalIgnoreCase);

            orderByQuery = orderByQuery.Substring(0,
                firstAmpersandIndex > -1 ? firstAmpersandIndex : orderByQuery.Length);

            if (!orderByQuery.Contains(fieldName))
            {
                return input;
            }

            var newOrderByQuery = orderByQuery

                // first check if the fieldName $orderby is descending and remove if found
                .Replace($",{fieldName} desc,",
                    ",") // if fieldName desc is in not in the beginning or end of the $orderby clause, then remove it
                .Replace($",{fieldName} desc",
                    string.Empty) // if fieldName desc is at the end of the $orderby clause, then remove it
                .Replace($"{fieldName} desc,",
                    string.Empty) // if fieldName desc is at the beginning of the $orderby clause, then remove it
                .Replace($"{fieldName} desc",
                    string.Empty) // otherwise fieldName desc is the only query field and thus at the beginning of the $orderby clause, so remove it too

                // if no descending $orderby is found remove the (ascending) fieldName
                .Replace($",{fieldName},",
                    ",") // if fieldName is in not in the beginning or end of the $orderby clause, then remove it
                .Replace($",{fieldName}",
                    string.Empty) // if fieldName is at the end of the $orderby clause, then remove it
                .Replace($"{fieldName},",
                    string.Empty) // if fieldName is at the beginning of the $orderby clause, then remove it
                .Replace($"{fieldName}",
                    string.Empty); // otherwise fieldName is the only query field and thus at the beginning of the $orderby clause, so remove it too


            // means that there is an empty query
            if (newOrderByQuery != "$orderby=")
            {
                return new Uri(url.Replace(orderByQuery, newOrderByQuery));
            }

            return url[orderByIndex - 1] == '&'
                ? new Uri(url
                    .Replace($"&$orderby={fieldName} desc", string.Empty)
                    .Replace($"&$orderby={fieldName}", string.Empty))
                : new Uri(url
                    .Replace($"$orderby={fieldName} desc", string.Empty)
                    .Replace($"$orderby={fieldName}", string.Empty));
        }

        public static Uri RemoveFieldFromODataSelect(this Uri input, string fieldName)
        {
            var url = input.ToString();

            var selectIndex = url.IndexOf("$select=", StringComparison.OrdinalIgnoreCase);

            if (selectIndex == -1)
            {
                return input;
            }

            var selectQuery = url.Substring(selectIndex);

            var firstAmpersandIndex = selectQuery.IndexOf("&", StringComparison.OrdinalIgnoreCase);

            selectQuery =
                selectQuery.Substring(0, firstAmpersandIndex > -1 ? firstAmpersandIndex : selectQuery.Length);

            if (!selectQuery.Contains(fieldName))
            {
                return input;
            }

            var newSelectQuery = selectQuery

                .Replace($",{fieldName},",
                    ",") // if fieldName is in not in the beginning or end of the $select clause, then remove it
                .Replace($",{fieldName}",
                    string.Empty) // if fieldName is at the end of the $select clause, then remove it
                .Replace($"{fieldName},",
                    string.Empty) // if fieldName is at the beginning of the $select clause, then remove it
                .Replace($"{fieldName}",
                    string.Empty); // otherwise fieldName is the only query field and thus at the beginning of the $select clause, so remove it too


            // means that there is an empty query
            if (newSelectQuery != "$select=")
            {
                return new Uri(url.Replace(selectQuery, newSelectQuery));
            }


            var newUrl = url[selectIndex - 1] == '&'
                ? url
                    .Replace($"&$select={fieldName}", string.Empty)
                : url
                    .Replace($"$select={fieldName}", string.Empty);

            newUrl = newUrl.Replace("?&", "?");

            return new Uri(newUrl);
        }

        public static Uri RemoveODataOrderBy(this Uri input)
        {
            var url = input.ToString();

            var orderByIndex = url.IndexOf("$orderby=", StringComparison.OrdinalIgnoreCase);

            if (orderByIndex == -1)
            {
                return input;
            }

            var orderByQuery = url.Substring(orderByIndex);

            var firstAmpersandIndex = orderByQuery.IndexOf("&", StringComparison.OrdinalIgnoreCase);

            orderByQuery = orderByQuery.Substring(0,
                firstAmpersandIndex > -1 ? firstAmpersandIndex : orderByQuery.Length);

            var newUrl = url[orderByIndex - 1] == '&'
                ? url.Replace($"&{orderByQuery}", string.Empty)
                : url.Replace(orderByQuery, string.Empty);

            newUrl = newUrl.Replace("?&", "?");

            return new Uri(newUrl);
        }

        public static Uri RemoveODataSelect(this Uri input)
        {
            var url = input.ToString();

            var selectIndex = url.IndexOf("$select=", StringComparison.OrdinalIgnoreCase);

            if (selectIndex == -1)
            {
                return input;
            }

            var selectQuery = url.Substring(selectIndex);

            var firstAmpersandIndex = selectQuery.IndexOf("&", StringComparison.OrdinalIgnoreCase);

            selectQuery =
                selectQuery.Substring(0, firstAmpersandIndex > -1 ? firstAmpersandIndex : selectQuery.Length);

            var newUrl = url[selectIndex - 1] == '&'
                ? url.Replace($"&{selectQuery}", string.Empty)
                : url.Replace(selectQuery, string.Empty);

            newUrl = newUrl.Replace("?&", "?");

            return new Uri(newUrl);
        }

        public static Uri RemoveODataFilter(this Uri input)
        {
            var url = input.ToString();

            var filterIndex = url.IndexOf("$filter=", StringComparison.OrdinalIgnoreCase);

            if (filterIndex == -1)
            {
                return input;
            }

            var filterQuery = url.Substring(filterIndex);

            var firstAmpersandIndex = filterQuery.IndexOf("&", StringComparison.OrdinalIgnoreCase);

            filterQuery =
                filterQuery.Substring(0, firstAmpersandIndex > -1 ? firstAmpersandIndex : filterQuery.Length);

            var newUrl = url[filterIndex - 1] == '&'
                ? url.Replace($"&{filterQuery}", string.Empty)
                : url.Replace(filterQuery, string.Empty);

            newUrl = newUrl.Replace("?&", "?");

            return new Uri(newUrl);
        }
    }

}