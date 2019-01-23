import { Directive, ElementRef, OnInit } from '@angular/core';
import { Input } from '@angular/core';

@Directive({
  selector: '[appExternalEntities]',
})
export class ExternalEntitiesColorDirective implements OnInit {
  @Input()
  value: number;

  constructor(private el: ElementRef) {}

  ngOnInit() {
    if (this.value === 100) {
      this.el.nativeElement.style.boxShadow =
        'rgb(17, 125, 17) 0px 0px 8px 0px';
    } else if (this.value === 50 || this.value === 25) {
      this.el.nativeElement.style.boxShadow =
        'rgba(244, 204, 0, 0.93) 0px 0px 3px 1px';
    } else {
      this.el.nativeElement.style.boxShadow = 'rgb(239, 0, 0) 0px 0px 6px 0px';
    }
  }
}
