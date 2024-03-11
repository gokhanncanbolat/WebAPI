using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFCore.Extensions
{
    public static class OrderQueryBuilder
    {
        public static string CreateOrderQuery<T>(string orderByQueryString)
        {
            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var orderQueryBilder = new StringBuilder();

            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;


                var propertyFromQueryName = param.Split(",")[0];

                //var objectProperty = propertyInfos.FirstOrDefault(p => p.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));
                var objectProperty = propertyInfos.FirstOrDefault(p => string.Equals(p.Name, propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty is null)
                    continue;

                var direction = param.EndsWith(" desc") ? "descending" : "ascending";

                orderQueryBilder.Append($"{objectProperty.Name.ToString()} {direction},");

            }

            var orderQuery = orderQueryBilder.ToString().TrimEnd(',', ' ');

            return orderQuery;
        }
    }
}
