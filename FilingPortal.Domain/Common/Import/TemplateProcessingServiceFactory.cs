using Autofac;
using FilingPortal.Domain.Services;

namespace FilingPortal.Domain.Common.Import
{
    /// <summary>
    /// Represents the Template processing service factory
    /// </summary>
    public class TemplateProcessingServiceFactory : ITemplateProcessingServiceFactory
    {
        /// <summary>
        /// A lifetime scope
        /// </summary>
        private readonly ILifetimeScope _scope;
        /// <summary>
        /// Initialize the new instance of the <see cref="TemplateProcessingServiceFactory"/> class.
        /// </summary>
        /// <param name="scope">The lifetime scope</param>
        public TemplateProcessingServiceFactory(ILifetimeScope scope)
        {
            _scope = scope;
        }

        /// <summary>
        /// Create an instance of the Template processing service specified by configuration
        /// </summary>
        /// <param name="configuration"><see cref="IImportConfiguration"/></param>
        public ITemplateProcessingService Create(IImportConfiguration configuration)
        {
            var instance = _scope.Resolve(
                    typeof(ITemplateProcessingService<,>)
                        .MakeGenericType(configuration.ModelType,
                            configuration.ResultType)) as ITemplateProcessingService;

            return instance;
        }

        /// <summary>
        /// Create an instance of the form data Template processing service specified by configuration
        /// </summary>
        /// <param name="configuration"><see cref="IFormImportConfiguration"/></param>
        public IFormDataTemplateProcessingService Create(IFormImportConfiguration configuration)
        {
            var instance = _scope.Resolve(
                typeof(IFormDataTemplateProcessingService<,>)
                    .MakeGenericType(configuration.ModelType,
                        configuration.ResultType)) as IFormDataTemplateProcessingService;

            return instance;
        }
    }
}
