import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { News } from '../../../core/models/news';

@Component({
  selector: 'app-news-admin-edit-page',
  templateUrl: './news-admin-edit.component.html',
  styleUrls: ['./news-admin-edit.component.scss'],
})
export class NewsAdminEditComponent implements OnInit {
  item$: Observable<News>;

  constructor() {}

  ngOnInit() {}
}
