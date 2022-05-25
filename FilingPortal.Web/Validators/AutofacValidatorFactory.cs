using System;
using System.Web.Http;
using FluentValidation;

namespace FilingPortal.Web.Validators
{
    /// <summary>
    /// Factory for validators that using Autofac
    /// </summary>
    public class AutofacValidatorFactory : ValidatorFactoryBase
    {
        /// <summary>
        /// The configuration
        /// </summary>
        private readonly HttpConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacValidatorFactory"/> class
        /// </summary>
        /// <param name="configuration">The configuration</param>
        public AutofacValidatorFactory(HttpConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Instantiates the validator
        /// </summary>
        /// <param name="validatorType">Type of the validator</param>
        public override IValidator CreateInstance(Type validatorType)
        {
            return _configuration.DependencyResolver.GetService(validatorType) as IValidator;
        }
    }
}