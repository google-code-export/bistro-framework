using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;
using Bistro.Controllers;

namespace Bistro.Entity
{
    /// <summary>
    /// Defines an endpoint 
    /// </summary>
    /// <typeparam name="TController">The type of the controller.</typeparam>
    public class ControllerEndpoint<TController, TEntity, T>
    {
        private EntityMapper<TController, TEntity> parent;
        private MemberInfo controllerMember;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerEndpoint&lt;TController, TEntity&gt;"/> class.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="parent">The parent.</param>
        public ControllerEndpoint(Expression<Func<TController, T>> expr, EntityMapper<TController, TEntity> parent)
        {
            var body = expr.Body as MemberExpression;
            controllerMember = body.Member;

            this.parent = parent;
        }

        /// <summary>
        /// Sets the target of the mapping expression to the field identified by <c>expr</c>
        /// </summary>
        /// <param name="expr">The expr.</param>
        /// <returns></returns>
        public EntityMapper<TController, TEntity> To(Expression<Func<TEntity, T>> expr)
        {
            var body = expr.Body as MemberExpression;
            var entityMember = body.Member;

            parent.AddMapping(new MemberAccessor(controllerMember), new MemberAccessor(entityMember));
            return parent;
        }
    }

    /// <summary>
    /// Class for defining mapping a mapping between a controller and an entity
    /// </summary>
    /// <typeparam name="TController">The type of the controller.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class EntityMapper<TController, TEntity> : EntityMapperBase
    {
        /// <summary>
        /// Gets the source type.
        /// </summary>
        /// <value>The source.</value>
        public override Type Source { get { return typeof(TController); } }

        /// <summary>
        /// Gets the target type.
        /// </summary>
        /// <value>The target.</value>
        public override Type Target { get { return typeof(TEntity); } }

        /// <summary>
        /// Adds the mapping.
        /// </summary>
        /// <param name="controllerMember">The controller member.</param>
        /// <param name="entityMember">The entity member.</param>
        protected internal void AddMapping(MemberAccessor controllerMember, MemberAccessor entityMember)
        {
            if (!CanMap(controllerMember, entityMember))
                throw new InvalidOperationException(
                    String.Format(
                        "Can't map {0}.{1} to {2}.{3}. The types {4} and {5} are not compatible.",
                        controllerMember.Member.DeclaringType.Name,
                        controllerMember.Member.Name,
                        entityMember.Member.DeclaringType.Name,
                        entityMember.Member.Name,
                        controllerMember.TargetType.Name,
                        entityMember.TargetType.Name
                        ));

            mapping.Add(controllerMember, entityMember);
        }

        /// <summary>
        /// Configures the mapper for an inferred mapping, without using the Builder pattern.
        /// </summary>
        protected internal override void InferOnly()
        {
            Infer();
        }

        /// <summary>
        /// Configures the mapper for a strict inferred mapping, without using the Builder pattern.
        /// </summary>
        protected internal override void InferStrictOnly()
        {
            InferStrict();
        }

        /// <summary>
        /// Builds the mapping based on field names and types only
        /// </summary>
        /// <returns>Updated entity mapper</returns>
        public EntityMapper<TController, TEntity> Infer()
        {
            return Infer(BindingFlags.Public | BindingFlags.IgnoreCase);
        }

        /// <summary>
        /// Builds the mapping based on field names and types only
        /// </summary>
        /// <returns>Updated entity mapper</returns>
        public EntityMapper<TController, TEntity> InferStrict()
        {
            return Infer(BindingFlags.Public);
        }

        /// <summary>
        /// Determines whether the source can be mapped to the target
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns>
        /// 	<c>true</c> if the members are compatible, <c>false</c>.
        /// </returns>
        public bool CanMap(MemberAccessor source, MemberAccessor target)
        {
            return (source.TargetType.IsAssignableFrom(target.TargetType) &&
                    target.TargetType.IsAssignableFrom(source.TargetType));
        }

        /// <summary>
        /// Builds the mapping based on field names and types only
        /// </summary>
        /// <param name="bindingFlags">The binding flags.</param>
        /// <returns>Updated entity mapper</returns>
        public EntityMapper<TController, TEntity> Infer(BindingFlags bindingFlags)
        {
            var finalFlags = bindingFlags | BindingFlags.Instance;

            var aaa = typeof(TController).GetMembers(finalFlags)
                .ToList();
            aaa.ForEach(
                ambControllerMember =>
                {
                    if (ambControllerMember.MemberType != MemberTypes.Field && ambControllerMember.MemberType != MemberTypes.Property)
                        return;
                    
                    var ambEntityMember = typeof(TEntity).GetMember(ambControllerMember.Name, finalFlags);

                    if (ambEntityMember == null || ambEntityMember.Length != 1 ||
                        (ambEntityMember[0].MemberType != MemberTypes.Field && ambEntityMember[0].MemberType != MemberTypes.Property))
                        return;

                    var entityMember = new MemberAccessor(ambEntityMember[0]);
                    var controllerMember = new MemberAccessor(ambControllerMember);

                    if (!CanMap(controllerMember, entityMember))
                        return;

                    AddMapping(controllerMember, entityMember);
                }
            );

            return this;
        }

        /// <summary>
        /// Starts a mapping expression.
        /// </summary>
        /// <param name="expr">Expression identifying the controller field.</param>
        /// <returns></returns>
        public ControllerEndpoint<TController, TEntity, T> Map<T>(Expression<Func<TController, T>> expr)
        {
            return new ControllerEndpoint<TController, TEntity, T>(expr, this);
        }

        /// <summary>
        /// Removes a mapping expression. This is used to remove fields from an inferred mapping.
        /// </summary>
        /// <param name="expr">Expression identifying the controller field.</param>
        /// <returns></returns>
        public EntityMapper<TController, TEntity> Except<T>(Expression<Func<TController, T>> expr)
        {
            var body = expr.Body as MemberExpression;
            var controllerMember = body.Member;

            mapping.Remove(new MemberAccessor(controllerMember));
            return this;
        }
    }
}