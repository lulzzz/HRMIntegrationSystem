import { ImageFormat, ImageResizeMode } from './enums/fileType';

export interface FilePreviewOptions {
  width?: number;
  height?: number;
  opacity?: number;
  maintainAspectRatio?: boolean;
  brightness?: number;
  contrast?: number;
  imageResizeMode?: ImageResizeMode;
  format?: ImageFormat;
  zoom?: number;
}
