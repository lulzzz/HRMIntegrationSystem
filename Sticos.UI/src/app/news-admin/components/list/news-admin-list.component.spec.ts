import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewsAdminListComponent } from './news-admin-list.component';

describe('NewsAdminListComponent', () => {
  let component: NewsAdminListComponent;
  let fixture: ComponentFixture<NewsAdminListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [NewsAdminListComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewsAdminListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
