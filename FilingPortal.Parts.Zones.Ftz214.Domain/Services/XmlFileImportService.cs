using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using AutoMapper;
using FilingPortal.Domain;
using FilingPortal.Domain.Common.Import;
using FilingPortal.Domain.Common.Import.Models;
using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Parts.Common.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.Parts.Zones.Ftz214.Domain.Entities;
using FilingPortal.Parts.Zones.Ftz214.Domain.Repositories;
using Framework.Infrastructure;

namespace FilingPortal.Parts.Zones.Ftz214.Domain.Services
{
    internal class XmlFileImportService : FileImportService<FileProcessingDetailedResult>, IXmlFileImportService
    {
        private readonly IParsingDataValidationService<FTZ_214FTZ_ADMISSION> _validationService;
        private readonly IInboundRecordsRepository _repository;
        private readonly IFilingHeadersRepository _filingHeadersRepository;
        private readonly IValidationRepository<InboundRecord> _validationRepository;

        /// <summary>
        /// Locker is used to prevent multiple file processing at the same time
        /// </summary>
        private static readonly object Locker = new object();

        /// <summary>
        /// Creates a new instance of <see cref="XmlFileImportService"/>
        /// </summary>
        /// <param name="validationService">Validation service</param>
        /// <param name="repository">Inbound records repository</param>
        /// <param name="filingHeadersRepository">Filing Headers repository</param>
        /// <param name="validationRepository">Validation repository</param>
        public XmlFileImportService(IParsingDataValidationService<FTZ_214FTZ_ADMISSION> validationService,
            IInboundRecordsRepository repository,
            IFilingHeadersRepository filingHeadersRepository,
            IValidationRepository<InboundRecord> validationRepository)
        {
            _validationService = validationService;
            _repository = repository;
            _filingHeadersRepository = filingHeadersRepository;
            _validationRepository = validationRepository;
        }

        /// <summary>
        /// Processing the file
        /// </summary>
        /// <param name="fileName">Import file Name</param>
        /// <param name="fileStream">File stream</param>
        /// <param name="userName">Current user name</param>
        public override FileProcessingDetailedResult Process(string fileName, Stream fileStream, string userName = null)
        {
            lock (Locker)
            {
                byte[] file = ReadFully(fileStream);

                var processingResult = new FileProcessingDetailedResult(fileName);

                ParsingResult<FTZ_214FTZ_ADMISSION> parsingResult = GetParsingResult(file);
                processingResult.AddCommonErrors(parsingResult.Errors);
                processingResult.AddParsingErrors(parsingResult.RowErrors);

                ParsedDataValidationResult<FTZ_214FTZ_ADMISSION> validationResult = Validate(parsingResult.ParsedData);
                processingResult.AddValidationErrors(validationResult.Errors);

                (List<InboundRecord> created, List<InboundRecord> updated) = Save(validationResult.ValidData.ToList(), file, fileName, userName);

                UpdateFilingHeaders(updated, userName);

                processingResult.Inserted = created.Count;
                processingResult.Updated = updated.Count;
                processingResult.Count = created.Count + updated.Count;

                return processingResult;
            }
        }

        private static byte[] ReadFully(Stream input)
        {
            using (var ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        private ParsedDataValidationResult<FTZ_214FTZ_ADMISSION> Validate(List<FTZ_214FTZ_ADMISSION> items)
        {
            ParsedDataValidationResult<FTZ_214FTZ_ADMISSION> validationResult;
            try
            {
                validationResult = _validationService.Validate(items);
            }
            catch (Exception ex)
            {
                AppLogger.Error(ex, ErrorMessages.UnexpectedValidationError);
                validationResult = new ParsedDataValidationResult<FTZ_214FTZ_ADMISSION>(new[] {
                    new RowError(-1,"", ErrorLevel.Critical,"", ErrorMessages.UnexpectedValidationError)
                });
            }
            return validationResult;
        }

        private ParsingResult<FTZ_214FTZ_ADMISSION> GetParsingResult(byte[] fileContents)
        {
            var ser = new XmlSerializer(typeof(FTZ_214));
            using (var reader = new XmlTextReader(new MemoryStream(fileContents)))
            {
                reader.Namespaces = false;

                var result = new ParsingResult<FTZ_214FTZ_ADMISSION>();
                if (ser.Deserialize(reader) is FTZ_214 parsedRecord)
                {
                    result.ParsedData.AddRange(parsedRecord.Items);
                }
                else
                {
                    result.AddError("Wrong XML format");
                }

                return result;
            }
        }

        /// <summary>
        /// Save parsed and valid data
        /// </summary>
        /// <param name="items"><see cref="ParsingResult{T}"/></param>
        /// <param name="userName">User name</param>
        private (List<InboundRecord> created, List<InboundRecord> updated) Save(IReadOnlyCollection<FTZ_214FTZ_ADMISSION> items, byte[] fileContents, string fileName, string userName)
        {
            var created = new List<InboundRecord>(items.Count);
            var updated = new List<InboundRecord>(items.Count);

            if (!items.Any())
            {
                return (created, updated);
            }

            DateTime currentDate = DateTime.Now;

            foreach (FTZ_214FTZ_ADMISSION item in items)
            {
                InboundRecord matchedEntity = _repository.GetMatchedEntity(item);

                if (matchedEntity is null)
                {
                    InboundRecord entity = item.Map<FTZ_214FTZ_ADMISSION, InboundRecord>();

                    entity.CreatedUser = userName;
                    entity.CreatedDate = currentDate;
                    entity.ModifiedUser = userName;
                    entity.ModifiedDate = currentDate;
                    entity.XmlFile = fileContents;
                    entity.XmlFileName = Path.GetFileName(fileName);

                    _repository.Add(entity);
                    created.Add(entity);
                }
                else
                {
                    _repository.ClearParsedLines(matchedEntity);
                    _repository.ClearParsedData(matchedEntity);

                    Mapper.Map(item, matchedEntity);
                    matchedEntity.ModifiedUser = userName;
                    matchedEntity.ModifiedDate = currentDate;
                    matchedEntity.IsUpdate = true;
                    matchedEntity.IsAuto = false;
                    matchedEntity.IsAutoProcessed = false;

                    matchedEntity.XmlFile = fileContents;
                    matchedEntity.XmlFileName = Path.GetFileName(fileName);

                    _repository.Update(matchedEntity);
                    updated.Add(matchedEntity);
                }
            }

            _repository.Save();

            _validationRepository.Validate(created.Union(updated).ToList());

            return (created, updated);
        }

        private void UpdateFilingHeaders(List<InboundRecord> updated, string user)
        {
            if (!updated.Any())
            {
                return;
            }

            IEnumerable<FilingHeader> filingHeaders = _filingHeadersRepository.FindByInboundRecordIds(updated.Select(x => x.Id))
                .Where(x => x.JobStatus != JobStatus.Open);

            DateTime lastModifiedDate = DateTime.Now;
            foreach (FilingHeader filingHeader in filingHeaders)
            {
                filingHeader.LastModifiedDate = lastModifiedDate;
                filingHeader.LastModifiedUser = user;
                filingHeader.JobStatus = JobStatus.WaitingUpdate;

                _filingHeadersRepository.Update(filingHeader);
            }

            _filingHeadersRepository.Save();
        }
    }
}