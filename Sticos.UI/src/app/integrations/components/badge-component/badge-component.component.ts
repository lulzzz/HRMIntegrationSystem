import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-badge-component',
  templateUrl: './badge-component.component.html',
  styleUrls: ['./badge-component.component.scss'],
})
export class BadgeComponentComponent implements OnInit {
  @Input()
  messageType: string;
  @Input()
  type = '';
  constructor() {}

  ngOnInit() {}
}
