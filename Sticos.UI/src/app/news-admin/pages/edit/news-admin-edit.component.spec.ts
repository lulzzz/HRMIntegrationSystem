import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewsAdminEditComponent } from './news-admin-edit.component';

describe('NewsAdminEditComponent', () => {
  let component: NewsAdminEditComponent;
  let fixture: ComponentFixture<NewsAdminEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [NewsAdminEditComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewsAdminEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
