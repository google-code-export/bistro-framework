using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bistro.Controllers;
using Bistro.Configuration;

namespace Bistro.Validation
{
    /// <summary>
    /// Validation-aware handler factory
    /// </summary>
    public class ValidatingHandlerFactory : HandlerFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationHandlerFactory"/> class.
        /// </summary>
        /// <param name="application">The application.</param>
        public ValidatingHandlerFactory(Application application, SectionHandler configuration) : base(application, configuration) { }

        /// <summary>
        /// Creates the controller handler for the given descriptor.
        /// </summary>
        /// <param name="descriptor">The descriptor.</param>
        /// <returns></returns>
        public override IControllerHandler CreateControllerHandler(Bistro.Controllers.Descriptor.ControllerDescriptor descriptor)
        {
            var type = descriptor.ControllerType as Type;
            if (type != null && typeof(IValidatable).IsAssignableFrom(type))
                return new ValidatingControllerHandler(application, descriptor, application.LoggerFactory.GetLogger(typeof(ControllerHandler)));

            return base.CreateControllerHandler(descriptor);
        }
    }
}
