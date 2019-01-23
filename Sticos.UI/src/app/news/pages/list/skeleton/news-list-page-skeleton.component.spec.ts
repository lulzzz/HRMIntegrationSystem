import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewsListPageSkeletonComponent } from './news-list-page-skeleton.component';

describe('SkeletonComponent', () => {
  let component: NewsListPageSkeletonComponent;
  let fixture: ComponentFixture<NewsListPageSkeletonComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [NewsListPageSkeletonComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewsListPageSkeletonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
