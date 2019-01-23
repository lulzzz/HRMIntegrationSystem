import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Observable } from 'rxjs';
import { News } from '../../../core/models/news';
import { ActivatedRoute } from '@angular/router';
import { NewsService } from 'src/app/core/services/news.service';

@Component({
  selector: 'app-newsitem-page',
  templateUrl: './news-item-page.component.html',
  styleUrls: ['./news-item-page.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class NewsItemPageComponent implements OnInit {
  item$: Observable<News>;
  id: number;

  constructor(private route: ActivatedRoute, private newsService: NewsService) {
    if (this.route.snapshot.params['id']) {
      this.id = +this.route.snapshot.params['id'];
    }
  }

  ngOnInit() {
    if (this.id > 0) {
      this.item$ = this.newsService.get(this.id);
    }
  }
}
