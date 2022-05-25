using FilingPortal.Domain;
using FilingPortal.Domain.Common;
using FilingPortal.PluginEngine.Models;
using Framework.Domain.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using Framework.Infrastructure;
using HttpPostedFile = FilingPortal.PluginEngine.Models.HttpPostedFile;

namespace FilingPortal.PluginEngine.Controllers
{
    /// <summary>
    /// Base Web API controller
    /// </summary>
    public abstract class ApiControllerBase : ApiController
    {
        /// <summary>
        /// Gets or sets Current user 
        /// </summary>
        public AppUsersModel CurrentUser { get; set; }

        /// <summary>
        /// Creates result model from command result
        /// </summary>
        /// <param name="commandResult">The command result</param>
        public IHttpActionResult Result(CommandResult commandResult)
        {
            return commandResult.IsValid
                ? Ok()
                : InvalidResult(commandResult);
        }

        /// <summary>
        /// Creates result model from command result with the specified result type
        /// </summary>
        /// <param name="commandResult">The command result</param>
        public IHttpActionResult Result<TValue>(CommandResult<TValue> commandResult)
        {
            return commandResult.IsValid
                ? Ok(commandResult.Value)
                : InvalidResult(commandResult);
        }

        /// <summary>
        /// Creates result model with errors from command result
        /// </summary>
        /// <param name="commandResult">The command result</param>
        protected IHttpActionResult InvalidResult(CommandResult commandResult)
        {
            return BadRequest(commandResult);
        }

        /// <summary>
        /// Creates result model with errors added to model state dictionary from command result
        /// </summary>
        /// <param name="commandResult">The command result</param>
        public IHttpActionResult BadRequest(CommandResult commandResult)
        {
            AddToModelState(commandResult, ModelState, "dbr");
            return BadRequest(ModelState);
        }


        /// <summary>
        /// Copies errors from the command result to specified model state dictionary with the naming prefix
        /// </summary>
        /// <param name="result">The command result</param>
        /// <param name="modelState">Current model state</param>
        /// <param name="prefix">The naming prefix</param>
        private void AddToModelState(CommandResult result, ModelStateDictionary modelState, string prefix)
        {
            foreach (CommandResultError error in result.Errors)
            {
                var key = string.IsNullOrEmpty(prefix) ? error.PropertyName : prefix + "." + error.PropertyName;
                modelState.AddModelError(key, error.ErrorMessage);
                var rawAttemptedValue = error.Data;
                var attemptedValue = rawAttemptedValue?.ToString() ?? string.Empty;
                modelState.SetModelValue(key, new ValueProviderResult(rawAttemptedValue, attemptedValue, CultureInfo.CurrentCulture));
            }
        }

        /// <summary>
        /// Gets the file response based on specified binary file model
        /// </summary>
        /// <param name="file">The binary file model</param>
        protected HttpResponseMessage GetFileResponse(BinaryFileModel file)
        {
            return GetFileResponse(file.Content, file.FileName);
        }

        /// <summary>
        /// Returns File as response
        /// </summary>
        /// <param name="fileContent">Content of the file</param>
        /// <param name="fileName">Name of the file</param>
        protected HttpResponseMessage GetFileResponse(byte[] fileContent, string fileName)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(new MemoryStream(fileContent))
            };
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = fileName };
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(fileName));
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="filePathOnDisk"></param>
        /// <param name="contentType"></param>
        protected HttpResponseMessage SendAsFileStream(string filename, string filePathOnDisk, string contentType = null)
        {
            var sourceFile = new FileInfo(filePathOnDisk);
            var sourceFileSize = sourceFile.Length;
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(sourceFile.OpenRead())
            };
            HttpContentHeaders header = result.Content.Headers;
            header.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = filename,
                FileNameStar = filename,
                Size = sourceFileSize
            };
            if (contentType == null)
            {
                contentType = MimeMapping.GetMimeMapping(filePathOnDisk);
            }
            header.ContentType = new MediaTypeHeaderValue(contentType);
            header.ContentLength = sourceFileSize;
            return result;
        }

        /// <summary>
        /// Deserializes specified Base64 string to object of specified type
        /// </summary>
        /// <param name="data">The Base64 string</param>
        /// <returns>T.</returns>
        protected T FromBase64String<T>(string data)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(Convert.FromBase64String(data)));
            }
            catch (Exception ex)
            {
                throw new ArgumentException("An error occured while deserializing the object from Base64 string", ex);
            }
        }

        /// <summary>
        /// Gets the validation result using the specified model name and state
        /// </summary>
        /// <param name="modelName">Name of the model</param>
        /// <param name="modelState">Model state</param>
        public IEnumerable<FieldErrorViewModel> GetValidationResultForFields(string modelName, ModelStateDictionary modelState)
        {
            var result = new List<FieldErrorViewModel>();
            foreach (KeyValuePair<string, ModelState> state in ModelState)
            {
                foreach (ModelError error in state.Value.Errors)
                {
                    var fieldName = state.Key.Replace(modelName + ".", string.Empty);
                    result.Add(new FieldErrorViewModel { FieldName = fieldName, Message = error.ErrorMessage });
                }
            }
            return result;
        }

        /// <summary>
        /// Parses Multipart request
        /// </summary>
        /// <param name="request">Multipart request message</param>
        /// <param name="folder">Folder to save posted files</param>
        protected async Task<HttpPostedData> ParseMultipartRequest(HttpRequestMessage request, string folder)
        {
            if (request.Content == null || !request.Content.IsMimeMultipartContent())
            {
                throw CreateResponseException(HttpStatusCode.UnsupportedMediaType, ErrorMessages.UnknownRequestContent);
            }

            if (string.IsNullOrEmpty(folder))
            {
                folder = HttpContext.Current.Server.MapPath("~/App_Data");
            }

            var provider = new MultipartFormDataStreamProvider(folder);
            var files = new Dictionary<string, HttpPostedFile>(StringComparer.InvariantCultureIgnoreCase);
            var fields = new Dictionary<string, HttpPostedField>(StringComparer.InvariantCultureIgnoreCase);

            // Read the form data.
            await request.Content.ReadAsMultipartAsync(provider);

            foreach (MultipartFileData file in provider.FileData)
            {
                var fieldName = TrimFieldValue(file.Headers.ContentDisposition.Name);
                var fileName = TrimFieldValue(file.Headers.ContentDisposition.FileName);
                files.Add(fieldName, new HttpPostedFile(fileName, file.LocalFileName));
            }

            foreach (var key in provider.FormData.AllKeys)
            {
                var data = string.Join("; ", provider.FormData.GetValues(key));
                fields.Add(key, new HttpPostedField(key, data));
            }

            return new HttpPostedData(fields, files);
        }

        private static string TrimFieldValue(string value) => value?.Trim(' ', '"');

        /// <summary>
        /// Creates responce exception with specified http status code and error message
        /// </summary>
        /// <param name="code">Http status code</param>
        /// <param name="error">Error message</param>
        protected HttpResponseException CreateResponseException(HttpStatusCode code, string error)
        {
            return new HttpResponseException(Request.CreateResponse(code, error));
        }

        protected void CheckPermissions(int permission)
        {
            if (!CurrentUser.HasPermissions(new[] { permission }))
            {
                AppLogger.Error(ErrorMessages.InsufficientPermissions);
                throw CreateResponseException(HttpStatusCode.Forbidden, ErrorMessages.InsufficientPermissions);
            }
        }
    }
}