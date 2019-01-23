import { Component, OnInit, Input } from '@angular/core';
import { FilePreviewUrlService } from '../../services/filepreviewurl.service';
import { FilePreviewOptions } from '../../models/filePreviewSettings';
import { FilePreviewType, ImageResizeMode } from '../../models/enums/fileType';

@Component({
  selector: 'app-image',
  templateUrl: './image.component.html',
  styleUrls: ['./image.component.scss'],
})
export class ImageComponent implements OnInit {
  sources: any[] = [];
  altText = '';
  src = '';
  style = '';

  @Input('options')
  options: FilePreviewOptions = {};

  @Input()
  id = '';

  @Input()
  width = '';

  @Input()
  height = '';

  @Input()
  type: FilePreviewType = FilePreviewType.None;

  @Input()
  title = '';

  @Input()
  cssClass = '';

  constructor(private filePreviewUrlService: FilePreviewUrlService) {}

  ngOnInit() {
    this.altText = this.title;
    this.sources = this.generateSrcSet();
    this.src = this.filePreviewUrlService.getUrl(
      this.id,
      this.type,
      this.options,
    );

    if (this.options) {
      if (
        this.width &&
        this.height &&
        this.options.imageResizeMode === ImageResizeMode.Fill
      ) {
        this.style += `width: ${this.width}; height: ${
          this.height
        }; object-fit: cover;`;
      }
    }
  }

  private generateSrcSet(): any[] {
    const id = this.id || '';

    if (id === '' || this.type === FilePreviewType.None) {
      return [];
    }

    return [];
  }
}
