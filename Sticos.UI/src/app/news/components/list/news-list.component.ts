import { Component, OnInit, Input } from '@angular/core';
import { News } from '../../../core/models/news';

@Component({
  selector: 'app-news-list',
  templateUrl: './news-list.component.html',
  styleUrls: ['./news-list.component.scss'],
})
export class NewsListComponent implements OnInit {
  @Input()
  items: News[];
  @Input()
  options: any;

  constructor() {}

  ngOnInit() {}
}
