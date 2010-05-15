using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using Bistro.Validation;

namespace Bistro.Validation
{
    /// <summary>
    /// A class member to to be validated
    /// </summary>
    /// <typeparam name="T">the type of the owner class</typeparam>
    /// <typeparam name="K">the type of the member to be validated</typeparam>
    public class ValidationSite<T, K> : Validator<T>, IValidationSite where T : IValidatable
    {
        /// <summary>
        /// The member represented by this validation site
        /// </summary>
        MemberInfo member;

        /// <summary>
        /// Gets the member represented by this validation site.
        /// </summary>
        /// <value>The member.</value>
        public MemberInfo Member { get { return member; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationSite&lt;T, K&gt;"/> class.
        /// </summary>
        /// <param name="expr">The expression that defines which member to attach to.</param>
        public ValidationSite(Expression<Func<T, K>> expr)
        {
            var body = expr.Body as MemberExpression;
            member = body.Member;

            this.Name = body.Member.Name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationSite&lt;T, K&gt;"/> class.
        /// </summary>
        /// <param name="member">The member.</param>
        protected ValidationSite(MemberInfo member)
        {
            this.member = member;

            this.Name = member.Name;
        }

        /// <summary>
        /// Aliases this validation site as the supplied name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public ValidationSite<T, K> AliasedAs(string name)
        {
            this.Name = name;

            return this;
        }

        /// <summary>
        /// Determines whether the specified target is valid.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="messages">The messages.</param>
        /// <returns>
        /// 	<c>true</c> if the specified target is valid; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsValid(object target, out List<IValidationResult> messages)
        {
            return base.IsValid(Evaluate(target), out messages);
        }

        /// <summary>
        /// Adds the validation.
        /// </summary>
        /// <param name="validator">The validator.</param>
        public void AddValidation(IValidator validator)
        {
            children.Add(validator);
        }

        /// <summary>
        /// Evaluates the valdiation site on the target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        protected virtual object Evaluate(object target)
        {
            var fInfo = member as FieldInfo;
            if (fInfo == null)
            {
                var pInfo = (PropertyInfo)member;
                return pInfo.GetValue(target, null);
            }

            return fInfo.GetValue(target);
        }

        /// <summary>
        /// Translates this validation to a different target
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public override IValidator Translate(MemberInfo target)
        {
            var site = ValidatorForType(target.DeclaringType, new object[] { target }) as IValidationSite;

            foreach (IValidator child in children)
                site.Add(child.Translate(target));

            return site;
        }
    }
}