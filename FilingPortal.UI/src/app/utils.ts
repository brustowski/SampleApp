import { IconType } from './common/icon-tooltip';
import { MappingStatus, FilingStatus, JobStatus, HighlightingType } from '@common/models';

const env = {
  backend: 'api'
};

const getBasePath = () => {
  const baseTag = document.getElementsByTagName('base');
  return baseTag.length > 0 ? baseTag[0].href : '/';
};
export const locationPath = getBasePath() + env.backend;
export const controllerPath = getBasePath();

export const dateFormatFull = 'mm/dd/yyyy';
export const dateFormatCompact = 'm/d/yyyy';
export const dateFormatShort = 'mm/dd/yy';

export const paginationInfo = {
  pageSizes: [20, 50, 100, 300],
  pageSize: 50
};

export const globalSettings = {
  isSaveFiltersInStorage: true,
  isSaveGridSettingsInStorage: true,
  isShowErrorsInModal: false,
  isUseCache: false
};

export const gridPathAPIs = {
  clients: 'clients'
};

export const gridNames = {
  clients: 'clients'
};

export const jobStatusTypes: { [index: string]: string } = {
  [JobStatus.Open]: 'open',
  [JobStatus.InReview]: 'inreview',
  [JobStatus.InProgress]: 'inprogress',
  [JobStatus.Created]: 'created',
  [JobStatus.CreatingError]: 'error',
  [JobStatus.MappingError]: 'error',
  [JobStatus.UpdatingError]: 'error',
  [JobStatus.Updated]: 'entry-updated',
  [JobStatus.WaitingUpdate]: 'updated',
};

export const mappingStatusTypes: { [index: string]: string } = {
  Open: 'open',
  'In Review': 'inreview',
  'In Progress': 'inprogress',
  Mapped: 'mapped',
  Error: 'error',
  Updated: 'updated',
  [MappingStatus.Open]: 'open',
  [MappingStatus.InReview]: 'inreview',
  [MappingStatus.InProgress]: 'inprogress',
  [MappingStatus.Mapped]: 'mapped',
  [MappingStatus.Error]: 'error',
  [MappingStatus.Updated]: 'updated'
};

export const filingStatusTypes: { [index: string]: string } = {
  Open: 'open',
  'In Progress': 'inprogress',
  Filed: 'filed',
  Error: 'error',
  Updated: 'updated',
  [FilingStatus.Open]: 'open',
  [FilingStatus.InProgress]: 'inprogress',
  [FilingStatus.Filed]: 'filed',
  [FilingStatus.Error]: 'error',
  [FilingStatus.Updated]: 'updated'
};

export const updateStatusTypes: { [index: string]: string } = {
  Open: 'open',
  Updated: 'entry-updated',
  Error: 'error',
  [0]: 'open',
  [1]: 'entry-updated',
  [2]: 'error'
};

export const ftaReconStatusTypes: { [index: string]: string } = {
  Open: 'open',
  'In Progress': 'inprogress',
  Updated: 'updated',
  'Updating Error': 'updating-error',
  [0]: 'open',
  [1]: 'inprogress',
  [2]: 'updated',
  [3]: 'updating-error'
}

export const valueReconStatusTypes: { [index: string]: string } = {
  Open: 'open',
  Processed: 'processed',
  Error: 'error',
  [0]: 'open',
  [1]: 'processed',
  [2]: 'error'
}

export const typeMappHighlightingIcon = [
  { highlightingType: HighlightingType.Error, iconType: IconType.Error },
  { highlightingType: HighlightingType.Warning, iconType: IconType.Warning },
  { highlightingType: HighlightingType.Success, iconType: IconType.Success },
  { highlightingType: HighlightingType.Info, iconType: IconType.Info },
  { highlightingType: HighlightingType.NoHighlighting, iconType: IconType.None }
];

export const styleMappHighlightingType: { [index: number]: string } = {
  [HighlightingType.Error]: 'row-error',
  [HighlightingType.Warning]: 'row-warning',
  [HighlightingType.Success]: 'row-success',
  [HighlightingType.Info]: 'row-info',
  [HighlightingType.NoHighlighting]: ''
};

export enum KeyCodes {
  NumpadAdd = 43,
  NumpadSubstruct = 45,
  Period = 46,
  Digit0 = 48,
  Digit9 = 57,
  Numpad0 = 96,
  Numpad9 = 105,
  Add = 107,
  Substruct = 109,
}
