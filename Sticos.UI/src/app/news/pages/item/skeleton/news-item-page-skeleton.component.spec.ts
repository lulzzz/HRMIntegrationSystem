import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewsItemPageSkeletonComponent } from './news-item-page-skeleton.component';

describe('NewsItemPageSkeletonComponent', () => {
  let component: NewsItemPageSkeletonComponent;
  let fixture: ComponentFixture<NewsItemPageSkeletonComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [NewsItemPageSkeletonComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewsItemPageSkeletonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
