import { Component, OnInit, Input } from '@angular/core';
import { News } from '../../../core/models/news';
import { imageTransition } from '../../animations/animations';
import { FilePreviewUrlService } from 'src/app/core/services/filepreviewurl.service';
import {
  FilePreviewType,
  ImageResizeMode,
} from 'src/app/core/models/enums/fileType';

@Component({
  selector: 'app-news-item',
  templateUrl: './news-item.component.html',
  styleUrls: ['./news-item.component.scss'],
  animations: [imageTransition],
})
export class NewsItemComponent implements OnInit {
  @Input('size')
  size: 'full' | 'main' | 'small' = 'full';

  @Input()
  post: News;

  type: FilePreviewType = FilePreviewType.News;
  toggleImage: 'small' | 'large' = 'small';

  constructor(private filePreviewUrlService: FilePreviewUrlService) {}

  ngOnInit() {}

  resizeImage() {
    if (this.toggleImage === 'small') {
      this.toggleImage = 'large';
    } else {
      this.toggleImage = 'small';
    }
  }

  getImageUrl(imageId: number, size: 'small' | 'main' | 'full') {
    let height = 200;
    let width = 800; // do some sort of screen check to optimize file download

    switch (size) {
      case 'small':
        width = 400;
        height = 200;
        break;
      case 'main':
        width = 800;
        height = 400;
        break;
      case 'full':
        height = 400;
        width = 800;
        break;
    }
    const settings = {
      width: width,
      height: height,
      imageResizeMode: ImageResizeMode.Fill,
    };

    return this.filePreviewUrlService.getUrl(
      imageId.toString(),
      FilePreviewType.News,
      settings,
    );
  }
}
