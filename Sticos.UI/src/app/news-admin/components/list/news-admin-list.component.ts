import { Component, OnInit, Input } from '@angular/core';
import { News } from '../../../core/models/news';

@Component({
  selector: 'app-news-admin-list',
  templateUrl: './news-admin-list.component.html',
  styleUrls: ['./news-admin-list.component.scss'],
})
export class NewsAdminListComponent implements OnInit {
  @Input()
  items: News[];

  constructor() {}

  ngOnInit() {}
}
