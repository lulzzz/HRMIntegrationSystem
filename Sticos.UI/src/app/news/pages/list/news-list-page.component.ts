import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Observable } from 'rxjs';
import { News } from '../../../core/models/news';
import { NewsService } from 'src/app/core/services/news.service';

@Component({
  selector: 'app-news-list-page',
  templateUrl: './news-list-page.component.html',
  styleUrls: ['./news-list-page.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class NewsListPageComponent implements OnInit {
  items$: Observable<News[]>;
  options: any;

  constructor(private newsService: NewsService) {
    this.options = {};
  }

  ngOnInit() {
    this.items$ = this.newsService.getLatest();
  }
}
