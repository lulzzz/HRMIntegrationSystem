import { Component, OnInit, Input } from '@angular/core';
import { News } from '../../../core/models/news';

@Component({
  selector: 'app-news-admin-edit',
  templateUrl: './news-edit.component.html',
  styleUrls: ['./news-edit.component.scss'],
})
export class NewsEditComponent implements OnInit {
  @Input()
  item: News;

  constructor() {}

  ngOnInit() {}
}
