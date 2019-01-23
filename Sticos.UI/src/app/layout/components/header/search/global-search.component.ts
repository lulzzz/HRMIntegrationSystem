import { Component, OnInit, Input } from '@angular/core';
import { EventService } from 'src/app/core/services';
import { Router } from '@angular/router';

@Component({
  selector: 'app-global-search',
  templateUrl: './global-search.component.html',
  styleUrls: ['./global-search.component.scss'],
})
export class GlobalSearchComponent implements OnInit {
  query = '';

  constructor(private eventService: EventService, private router: Router) {}

  ngOnInit() {}

  quickSearch() {
    if (this.query !== '') {
      this.router.navigateByUrl('/search?query=' + this.query);
      this.eventService.globalSearch.emit(this.query);
    }
  }
}
