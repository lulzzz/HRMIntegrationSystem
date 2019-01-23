import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { pages } from './pages';
import { components } from './components';
import { TranslateModule } from '@ngx-translate/core';
import { translateSettings } from '../translate.settings';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CoreModule } from '../core/core.module';
import { Routing } from './search.routing';
import { SearchComponent } from './components/search/search.component';
import { SearchEventService } from './search-event.service';
import { SearchLoadingComponent } from './components/search-loading/search-loading.component';

@NgModule({
  imports: [
    CommonModule,
    CoreModule.forRoot(),
    TranslateModule.forChild(translateSettings),
    ReactiveFormsModule,
    FormsModule,
    Routing,
  ],
  providers: [SearchEventService],
  declarations: [
    ...pages,
    ...components,
    SearchComponent,
    SearchLoadingComponent,
  ],
})
export class SearchModule {}
