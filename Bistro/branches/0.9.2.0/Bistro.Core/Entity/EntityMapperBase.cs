using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bistro.Controllers;

namespace Bistro.Entity
{
    public abstract class EntityMapperBase
    {
        protected Dictionary<MemberAccessor, MemberAccessor> mapping = new Dictionary<MemberAccessor, MemberAccessor>();
        public IDictionary<MemberAccessor, MemberAccessor> Mapping { get { return mapping; } }

        /// <summary>
        /// Gets the source type.
        /// </summary>
        /// <value>The source.</value>
        public abstract Type Source { get; }

        /// <summary>
        /// Gets the target type.
        /// </summary>
        /// <value>The target.</value>
        public abstract Type Target { get; }

        /// <summary>
        /// Performs the mapping from the controller to the entity
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="entity">The entity.</param>
        public void Map(IController controller, object entity)
        {
            Perform(controller, entity, true);
        }

        /// <summary>
        /// Performs the mapping from the entity to the controller
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="entity">The entity.</param>
        public void Unmap(IController controller, object entity)
        {
            Perform(controller, entity, false);
        }

        /// <summary>
        /// Performs the specified controller.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="forward">if set to <c>true</c> [forward].</param>
        protected virtual void Perform(IController controller, object entity, bool forward)
        {
            foreach (KeyValuePair<MemberAccessor, MemberAccessor> row in mapping)
                if (forward)
                {
                    if (row.Value.CanWrite && row.Key.CanRead)
                        row.Value.SetValue(entity, row.Key.GetValue((controller)));
                }
                else
                {
                    if (row.Key.CanWrite && row.Value.CanRead)
                        row.Key.SetValue(controller, row.Value.GetValue((entity)));
                }
        }

        /// <summary>
        /// Configures the mapper for an inferred mapping, without using the Builder pattern.
        /// </summary>
        protected internal abstract void InferOnly();

        /// <summary>
        /// Configures the mapper for a strict inferred mapping, without using the Builder pattern.
        /// </summary>
        protected internal abstract void InferStrictOnly();
    }
}
