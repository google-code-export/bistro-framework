using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bistro.Entity;

namespace Bistro.Validation
{
    /// <summary>
    /// A repository of all known controller validation rules
    /// </summary>
    public class ValidationRepository
    {
        private static ValidationRepository instance = new ValidationRepository();

        /// <summary>
        /// Gets an instance of the repository.
        /// </summary>
        /// <value>The instance.</value>
        public static ValidationRepository Instance { get { return instance; } }

        /// <summary>
        /// Map of validators by namespace
        /// </summary>
        private Dictionary<string, IValidator> validatorsByNamespace = new Dictionary<string, IValidator>();

        private Dictionary<Type, IValidator> validatorsByType = new Dictionary<Type, IValidator>();

        /// <summary>
        /// Registers a type marked with the <see cref="ValidateWithAttribute"/> attribute.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The list of validators generated</returns>
        public ICollection<IValidator> RegisterValidatable(Type type)
        {
            var attribs = type.GetCustomAttributes(typeof(ValidateWithAttribute), false);
            var validators = new List<IValidator>();

            foreach (ValidateWithAttribute attrib in attribs)
            {
                var v = (IValidator)Activator.CreateInstance(attrib.TargetType);
                RegisterValidator(type, v);
                validators.Add(v);
            }

            validators.InsertRange(0, CheckAndRegisterInferred(type));
            return validators;
        }

        /// <summary>
        /// Checks for the presence of an <see cref="InferValidationFromAttribute"/>, and creates validators if present
        /// </summary>
        /// <param name="type">The type.</param>
        protected virtual IList<IValidator> CheckAndRegisterInferred(Type type)
        {
            var attribs = type.GetCustomAttributes(typeof (InferValidationFromAttribute), false) as InferValidationFromAttribute[];
            var validators = new List<IValidator>();

            foreach (var attrib in attribs)
            {
                // the 'source' here is the controller. we use the target of the infer attribute 
                // to make sure that we bring in validations from the correct mapper
                IList<EntityMapperBase> mappers = MapperRepository.Instance.FindMapperBySource(type);

                foreach (var mapper in mappers)
                {
                    if (mapper.Target != attrib.TargetType)
                        continue;
                    
                    Type validatorType = typeof (Validator<>).MakeGenericType(new Type[] {mapper.Source});
                    var validator = Activator.CreateInstance(validatorType) as IValidator;
                    validator.ByMappingOnly();
                    validators.Add(validator);
                }
            }

            return validators;
        }

        /// <summary>
        /// Registers the validator.
        /// </summary>
        /// <param name="validator">The validator.</param>
        public void RegisterValidator(Type target, IValidator validator)
        {
            IValidator current = null;
            if (validatorsByNamespace.TryGetValue(validator.Name, out current))
                validatorsByNamespace[validator.Name] = validator.Merge(current);
            else
                validatorsByNamespace.Add(validator.Name, validator);

            if (validatorsByType.TryGetValue(target, out current))
                validatorsByType[target] = validator.Merge(current);
            else
                validatorsByType.Add(target, validator);
        }

        /// <summary>
        /// Gets the validator for namespace.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public IValidator GetValidatorForNamespace(string name)
        {
            IValidator validator = null;
            if (!validatorsByNamespace.TryGetValue(name, out validator))
                return null;

            return validator;
        }

        public IValidator GetValidatorForType(Type type)
        {
            IValidator validator = null;
            if (!validatorsByType.TryGetValue(type, out validator))
            {
                // if we can't find it, maybe we haven't loaded it yet...
                if (RegisterValidatable(type).Count > 0)
                    return GetValidatorForType(type);

                return null;
            }

            return validator;
        }
    }
}
