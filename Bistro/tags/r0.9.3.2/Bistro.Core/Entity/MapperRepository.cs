using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bistro.Entity
{
    /// <summary>
    /// Repository of all entity mappers defined in the system
    /// </summary>
    public class MapperRepository
    {
        /// <summary>
        /// Instance variable
        /// </summary>
        private static MapperRepository instance = new MapperRepository();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static MapperRepository Instance { get { return instance; } }

        /// <summary>
        /// Mapping of source type to mapper
        /// </summary>
        private Dictionary<Type, List<EntityMapperBase>> sourceMapping = new Dictionary<Type, List<EntityMapperBase>>();

        /// <summary>
        /// Mapping of target type to mapper
        /// </summary>
        private Dictionary<Type, List<EntityMapperBase>> targetMapping = new Dictionary<Type, List<EntityMapperBase>>();

        /// <summary>
        /// Registers the mapper.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        public void RegisterMapper(EntityMapperBase mapper)
        {
            AddToList(sourceMapping, mapper.Source, mapper);
            AddToList(targetMapping, mapper.Target, mapper);
        }

        /// <summary>
        /// Finds the mapper by the source type.
        /// </summary>
        /// <param name="sourceType">Type of the source.</param>
        /// <returns></returns>
        public IList<EntityMapperBase> FindMapperBySource(Type sourceType)
        {
            return FindMapper(sourceType, sourceMapping);
        }

        /// <summary>
        /// Finds the mapper by the source type.
        /// </summary>
        /// <param name="sourceType">Type of the source.</param>
        /// <returns></returns>
        public IList<EntityMapperBase> FindMapperByTarget(Type targetType)
        {
            return FindMapper(targetType, targetMapping);
        }

        /// <summary>
        /// Finds the mapper.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <param name="lookup">The lookup.</param>
        /// <returns></returns>
        private IList<EntityMapperBase> FindMapper(Type t, Dictionary<Type, List<EntityMapperBase>> lookup)
        {
            List<EntityMapperBase> list;
            if (lookup.TryGetValue(t, out list))
                return list;

            return new List<EntityMapperBase>();
        }

        /// <summary>
        /// Adds the mapper to the list in the map, creating a new list if none exists
        /// </summary>
        /// <param name="map">The map.</param>
        /// <param name="key">The key.</param>
        /// <param name="mapper">The mapper.</param>
        private void AddToList(Dictionary<Type, List<EntityMapperBase>> map, Type key, EntityMapperBase mapper)
        {
            List<EntityMapperBase> list;
            if (map.TryGetValue(key, out list))
            {
                list.Add(mapper);
                return;
            }

            list = new List<EntityMapperBase>() {mapper};
            map.Add(key, list);
        }
    }
}