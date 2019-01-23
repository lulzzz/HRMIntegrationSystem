import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { LayoutService } from './modules/layout/services/layout.service';
import { TranslateService } from '@ngx-translate/core';
import { UserSettingsService } from './services/user-settings.service';
import { AppComponent } from './app.component';

describe('AppComponent', () => {
  let comp: AppComponent;
  let fixture: ComponentFixture<AppComponent>;

  beforeEach(() => {
    const layoutServiceStub = {
      isOpen: {},
      sidebarToggled: {
        subscribe: () => ({}),
      },
    };
    const translateServiceStub = {
      setDefaultLang: () => ({}),
      use: () => ({}),
    };
    const userSettingsServiceStub = {
      getLanguage: () => ({}),
      setLanguage: () => ({}),
    };
    TestBed.configureTestingModule({
      declarations: [AppComponent],
      schemas: [NO_ERRORS_SCHEMA],
      providers: [
        { provide: LayoutService, useValue: layoutServiceStub },
        { provide: TranslateService, useValue: translateServiceStub },
        { provide: UserSettingsService, useValue: userSettingsServiceStub },
      ],
    });
    fixture = TestBed.createComponent(AppComponent);
    comp = fixture.componentInstance;
  });

  it('can load instance', () => {
    expect(comp).toBeTruthy();
  });
});
