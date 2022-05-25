using System;
using Autofac;
using FluentValidation;

namespace FilingPortal.Domain.Common.Parsing
{
    /// <summary>
    /// Provides Parsing Model Validators for specified parsing data model
    /// </summary>
    class ParsingModelValidatorFactory : IParsingDataModelValidatorFactory
    {
        /// <summary>
        /// <see cref="ILifetimeScope"/>
        /// </summary>
        private readonly ILifetimeScope _scope;
        /// <summary>
        /// Initializes a new instance of the <see cref="ParsingModelValidatorFactory"/> class
        /// </summary>
        /// <param name="scope"><see cref="ILifetimeScope"/> object</param>
        public ParsingModelValidatorFactory(ILifetimeScope scope)
        {
            _scope = scope;
        }
        /// <summary>
        /// Creates validator for specified parsing data model type
        /// </summary>
        /// <typeparam name="T">Parsing data model type</typeparam>
        public IValidator<T> Create<T>() where T : IParsingDataModel
        {
            var parsingModelType = typeof(T);


            var validator = _scope.ResolveOptional<IValidator<T>>();
            if (validator == null)
            {
                throw new ArgumentOutOfRangeException("T", $"Unsupported generic type[{parsingModelType}]");
            }

            return validator;
        }
    }
}
