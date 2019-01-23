import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-news-list-page-skeleton',
  templateUrl: './news-list-page-skeleton.component.html',
  styleUrls: ['./news-list-page-skeleton.component.scss'],
})
export class NewsListPageSkeletonComponent implements OnInit {
  fakePosts: any[] = [{}, {}, {}, {}, {}];
  constructor() {}

  ngOnInit() {}
}
