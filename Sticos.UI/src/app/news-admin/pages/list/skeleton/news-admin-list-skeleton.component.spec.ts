import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewsAdminListSkeletonComponent } from './news-admin-list-skeleton.component';

describe('NewsAdminListSkeletonComponent', () => {
  let component: NewsAdminListSkeletonComponent;
  let fixture: ComponentFixture<NewsAdminListSkeletonComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [NewsAdminListSkeletonComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewsAdminListSkeletonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
