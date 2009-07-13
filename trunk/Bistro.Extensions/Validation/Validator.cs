﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bistro.Validation;
using System.Linq.Expressions;

namespace Bistro.Extensions.Validation
{
    /// <summary>
    /// Build class for validation rules
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Validator<T>: IValidator
    {
        protected List<IValidator> children = new List<IValidator>();

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
        /// Generates a <see cref="ValidationSite"/> validation site for the specified member
        /// </summary>
        /// <typeparam name="K">the type of the member</typeparam>
        /// <param name="expr">The expression tree for the member.</param>
        /// <returns></returns>
        public ValidationSite<T,K> Value<K>(Expression<Func<T, K>> expr)
        {
            var site = new ValidationSite<T, K>(expr);
            children.Add(site);

            return site;
        }

        /// <summary>
        /// Incorporates rules from within the target into this validator. This operation will
        /// place the validation rules of the target as children within the namespace of the
        /// instance. Given a rule on the target with a name of "a.b", and a validator with a name of
        /// "c", a.b will become "c.a.b".
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns>a validator which is the composition of the instance with the target</returns>
        public Validator<T> WithRulesFrom(IValidatable target)
        {
            children.Add(target.Validator);
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
        public virtual bool IsValid(object target, out List<string> messages)
        {
            messages = new List<string>();
            var valid = DoValidate(target, out messages);

            foreach (IValidator child in children)
            {
                var newMessages = new List<string>();
                valid = valid && child.IsValid(target, out newMessages);

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
        public virtual bool DoValidate(object target, out List<string> messages)
        {
            messages = new List<string>();
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
    }
}