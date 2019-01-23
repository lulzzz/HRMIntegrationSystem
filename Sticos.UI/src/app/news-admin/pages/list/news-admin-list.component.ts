import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { News } from '../../../core/models/news';

@Component({
  selector: 'app-news-admin-list-page',
  templateUrl: './news-admin-list.component.html',
  styleUrls: ['./news-admin-list.component.scss'],
})
export class NewsAdminListComponent implements OnInit {
  items$: Observable<News[]>;

  constructor() {}

  ngOnInit() {}
}
