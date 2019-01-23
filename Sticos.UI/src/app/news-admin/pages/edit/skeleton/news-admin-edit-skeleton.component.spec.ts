import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewsAdminEditSkeletonComponent } from './news-admin-edit-skeleton.component';

describe('NewsAdminEditSkeletonComponent', () => {
  let component: NewsAdminEditSkeletonComponent;
  let fixture: ComponentFixture<NewsAdminEditSkeletonComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [NewsAdminEditSkeletonComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewsAdminEditSkeletonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
