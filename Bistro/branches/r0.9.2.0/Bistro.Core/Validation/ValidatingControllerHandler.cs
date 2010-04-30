using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bistro.Controllers;
using Bistro.Controllers.Descriptor;
using Bistro.Configuration.Logging;

namespace Bistro.Validation
{
    /// <summary>
    /// Controller handler that incorporates validation functionality
    /// </summary>
    public class ValidatingControllerHandler: ControllerHandler
    {
        enum Exceptions
        {
            [DefaultMessage("{0} does not implement IValidatable")]
            InvalidCast,
            [DefaultMessage("'{0}' occured while attempting to query {1} for validation rules. Validation rule discovery occurs outside of normal controller lifecycle, and cannot rely on any controller state.")]
            RuntimeError
        }

        List<IValidator> validators = new List<IValidator>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatingControllerHandler"/> class.
        /// </summary>
        /// <param name="descriptor"></param>
        /// <param name="logger"></param>
        protected internal ValidatingControllerHandler(Application application, ControllerDescriptor descriptor, ILogger logger)
            : base(application, descriptor, logger)
        {
            try
            {
                validators.InsertRange(0, ValidationRepository.Instance.RegisterValidatable(descriptor.ControllerType as Type));
            }
            catch (InvalidCastException)
            {
                logger.Report(Exceptions.InvalidCast, descriptor.ControllerTypeName);
            }
            catch (Exception ex)
            {
                logger.Report(Exceptions.RuntimeError, ex, ex.Message, descriptor.ControllerTypeName);
            }
        }

        /// <summary>
        /// Gets the controller instance.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        /// <param name="requestContext">The request context.</param>
        /// <returns></returns>
        public override IController GetControllerInstance(ControllerInvocationInfo info, System.Web.HttpContextBase context, IContext requestContext)
        {
            var instance = base.GetControllerInstance(info, context, requestContext);
            var validatable = (IValidatable)instance;

            validatable.Messages = new List<IValidationResult>();
            validatable.IsValid = true;

            foreach (IValidator validator in validators)
            {
                var messages = new List<IValidationResult>();
                validatable.IsValid = validator.IsValid(instance, out messages) && validatable.IsValid;
                validatable.Messages.InsertRange(0, messages);

                //validatable.IsValid = validator.IsValid(instance, out messages) && validatable.IsValid;
                //validatable.Messages.InsertRange(0, messages);

            }

            return instance;
        }
    }
}
