using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bistro.Validation;
using System.Linq.Expressions;
using Bistro.Entity;
using System.Reflection;

namespace Bistro.Validation
{
    /// <summary>
    /// Build class for validation rules
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Validator<T>: IValidator where T : IValidatable
    {
        protected List<IValidator> children = new List<IValidator>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Validator&lt;T&gt;"/> class.
        /// </summary>
        public Validator()
        {
            DefiningParams = new Dictionary<string, object>();
            Name = typeof(T).Name;

#pragma warning disable 618,612
            Define();
#pragma warning restore 618,612
        }

        /// <summary>
        /// Sets a validation namespace for this, and all child validations.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public virtual Validator<T> As(string name)
        {
            Name = name;

            return this;
        }

        /// <summary>
        /// Adds the specified rule to the rule set
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <returns></returns>
        public virtual Validator<T> Define(Validator<T> rule)
        {
            children.Add(rule);
            return this;
        }

        /// <summary>
        /// Adds the specified rule to the rule set
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <returns></returns>
        public virtual Validator<T> And(Validator<T> rule)
        {
            children.Add(rule);
            return this;
        }

        /// <summary>
        /// Generates a <see cref="ValidationSite"/> validation site for the specified member
        /// </summary>
        /// <typeparam name="K">the type of the member</typeparam>
        /// <param name="expr">The expression tree for the member.</param>
        /// <returns></returns>
        public ValidationSite<T,K> Value<K>(Expression<Func<T, K>> expr)
        {
            return new ValidationSite<T, K>(expr);
        }

        /// <summary>
        /// Incorporates rules from within the target into this validator. This operation will
        /// place the validation rules of the target as children within the namespace of the
        /// instance. Given a rule on the target with a name of "a.b", and a validator with a name of
        /// "c", a.b will become "c.a.b".
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns>
        /// a validator which is the composition of the instance with the target
        /// </returns>
        public Validator<T> WithRulesFrom(Type target)
        {
            children.Add(ValidationRepository.Instance.GetValidatorForType(target));
            return this;
        }

        /// <summary>
        /// Adds the specified validator.
        /// </summary>
        /// <param name="validator">The validator.</param>
        /// <returns></returns>
        public IValidator Add(IValidator validator)
        {
            children.Add(validator);
            return this;
        }

        /// <summary>
        /// Determines whether the specified target, and all of its children, is valid.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="messages">The messages.</param>
        /// <returns>
        /// 	<c>true</c> if the specified target is valid; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool IsValid(object target, out List<IValidationResult> messages)
        {
            messages = new List<IValidationResult>();
            var valid = DoValidate(target, out messages);

            foreach (IValidator child in children)
            {
                var newMessages = new List<IValidationResult>();
                valid = child.IsValid(target, out newMessages) && valid;

                messages.AddRange(newMessages);
            }

            return valid;
        }

        /// <summary>
        /// Evaluates this validator only.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="messages">The messages.</param>
        /// <returns></returns>
        public virtual bool DoValidate(object target, out List<IValidationResult> messages)
        {
            messages = new List<IValidationResult>();
            return true;
        }

        /// <summary>
        /// Gets the name of the validation rule defined by this validator.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the child validators of this instance
        /// </summary>
        /// <value>The children.</value>
        public IEnumerable<IValidator> Children
        {
            get { return children; }
        }

        /// <summary>
        /// Merges the validator with the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public IValidator Merge(IValidator target)
        {
            var validator = new Validator<T>();
            validator.children.AddRange(children);
            validator.children.AddRange(target.Children);

            return validator;
        }

        /// <summary>
        /// Defines this instance.
        /// </summary>
        [ObsoleteAttribute("This method is deprecated and will be removed in future releases. Please move initialization logic into the constructor.")]
        protected virtual void Define() { }

        /// <summary>
        /// Gets the parameters that define this validator
        /// </summary>
        /// <value>The defining params.</value>
        public virtual Dictionary<string, object> DefiningParams
        {
            get;
            protected set;
        }

        /// <summary>
        /// Builds a validator based on the mapping of the target object
        /// </summary>
        /// <returns></returns>
        public virtual IValidator ByMappingOnly()
        {
            return ByMapping();
        }

        /// <summary>
        /// Builds a validator by cloning rules on mapped fields
        /// </summary>
        /// <returns></returns>
        public virtual Validator<T> ByMapping()
        {
            IList<EntityMapperBase> mappers = MapperRepository.Instance.FindMapperBySource(typeof (T));
            if (mappers.Count < 1)
                throw new InvalidOperationException(String.Format("{0} is not mappable, and cannot be used in this context", typeof(T).Name));

            // the entity type is the second type parameter to EntityMapper<T,K>
            var entityType = mappers[0].Target;
            var mapper = mappers[0];

            // the mapping the mapper will have is controller field -> entity field. the validation
            // will have rules for the entity, and so we'll need to get the controller field by
            // the entity field. reverse the mapping, and also strip off the memberaccessor - easier
            // to work with that here, and this is metadata, so not worried about performance.
            Dictionary<MemberInfo, MemberInfo> reversedMapping = new Dictionary<MemberInfo, MemberInfo>();

            // little f# trick :)
            mapper.Mapping
                .ToList()
                .ForEach(
                    x => { reversedMapping.Add(x.Value.Member, x.Key.Member); }
                );

            var validator = ValidationRepository.Instance.GetValidatorForType(entityType);

            if (validator == null)
                return this;

            foreach (IValidator child in validator.Children)
            {
                var validationSite = child as IValidationSite;
                if (validationSite == null)
                    continue;

                MemberInfo controllerField;
                if (reversedMapping.TryGetValue(validationSite.Member, out controllerField))
                    children.Add(validationSite.Translate(controllerField));
            }

            return this;
        }

        /// <summary>
        /// Translates this validation to a different target
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public virtual IValidator Translate(MemberInfo target)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Validators for type.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        protected IValidator ValidatorForType(Type target, params object[] ctrParams)
        {
            Type[] typeSignature = GetType().GetGenericArguments();
            typeSignature[0] = target;

            return
                Activator.CreateInstance(
                    GetType()
                        .GetGenericTypeDefinition()
                        .MakeGenericType(typeSignature),
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                    null,
                    ctrParams,
                    null
                    ) as IValidator;
        }
    }
}
