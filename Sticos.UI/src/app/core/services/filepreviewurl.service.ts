import { Injectable } from '@angular/core';
import { FilePreviewOptions } from '../models/filePreviewSettings';
import { FilePreviewType } from '../models/enums/fileType';
import { URLSearchParams } from '@angular/http';
import { AppConfig } from '../../core/services/app-config-service';

@Injectable({
  providedIn: 'root',
})
export class FilePreviewUrlService {
  constructor() {}

  getUrl(
    id: string,
    fileType: FilePreviewType,
    settings?: FilePreviewOptions,
  ): string {
    if (!id || fileType === FilePreviewType.None) {
      return '';
    }

    let prefix = '';
    switch (fileType) {
      case FilePreviewType.Profile:
        prefix = 'profile';
        break;
      case FilePreviewType.Datasheet:
        prefix = 'datasheet';
        break;
      case FilePreviewType.Archive:
      case FilePreviewType.News:
      case FilePreviewType.OrgUnitLogo:
      default:
        prefix = 'F';
        break;
    }

    const url = `${
      AppConfig.settings.apiUrls.filePreview
    }/api/archive/${prefix}_${id}/preview`;
    const urlParams: URLSearchParams = new URLSearchParams();

    if (settings) {
      for (const key in settings) {
        if (key) {
          urlParams.set(key, settings[key]);
        }
      }
    }

    const urlQuery = urlParams.toString();

    return url + (urlQuery.length > 0 ? '?' + urlQuery : '');
  }
}
