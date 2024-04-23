using Entities.Models;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class DataShaper<T> : IDataShaper<T> where T : class
    {
        public PropertyInfo[] Properties { get; set; }
        public DataShaper()
        {
            Properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }
        IEnumerable<ShapedEntity> IDataShaper<T>.ShapeData(IEnumerable<T> entities, string fieldString)
        {
            var requiredProperties = GetRequiredFields(fieldString);
            return FetchDataForEntities(requiredProperties, entities);
        }

        ShapedEntity IDataShaper<T>.ShapeData(T entity, string fieldString)
        {
            var requiredProperties = GetRequiredFields(fieldString);
            return FetchDataForEntity(requiredProperties, entity);
        }

        private IEnumerable<PropertyInfo> GetRequiredFields(string fieldsString)
        {
            var requiredFields = new List<PropertyInfo>();
            if (!string.IsNullOrWhiteSpace(fieldsString))
            {
                var fields = fieldsString.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (var field in fields)
                {
                    var property = Properties.FirstOrDefault(p => p.Name.Equals(field, StringComparison.InvariantCultureIgnoreCase));

                    if (property is null)
                        continue;

                    requiredFields.Add(property);
                }
            }
            else
            {
                requiredFields = Properties.ToList();
            }

            return requiredFields;
        }

        private ShapedEntity FetchDataForEntity(IEnumerable<PropertyInfo> requiredProperties, T entity)
        {
            var shappedEntity = new ShapedEntity();

            foreach (var property in requiredProperties)
            {
                var propertyValue = property.GetValue(entity);
                shappedEntity.Entity.TryAdd(property.Name,propertyValue);

            }

            var objectProperty = entity.GetType().GetProperty("Id");
            shappedEntity.Id = (int)objectProperty.GetValue(entity);

            return shappedEntity;
        }

        private IEnumerable<ShapedEntity> FetchDataForEntities(IEnumerable<PropertyInfo> requiredProperties, IEnumerable<T> entities)
        {
            var shappedData = new List<ShapedEntity>();

            foreach (var entity in entities)
            {
                var shappedObjects = FetchDataForEntity(requiredProperties, entity);
                shappedData.Add(shappedObjects);
            }

            return shappedData;
        }
    }
}
