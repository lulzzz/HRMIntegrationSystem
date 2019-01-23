import { NewsAdminModule } from './news-admin.module';

describe('NewsAdminModule', () => {
  let newsadminModule: NewsAdminModule;

  beforeEach(() => {
    newsadminModule = new NewsAdminModule();
  });

  it('should create an instance', () => {
    expect(newsadminModule).toBeTruthy();
  });
});
