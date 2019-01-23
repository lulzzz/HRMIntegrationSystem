import { Directive, OnInit, ElementRef } from '@angular/core';
import { Input } from '@angular/core';

@Directive({
  selector: '[appMatchedEntitiesColor]',
})
export class MatchedEntitiesColorDirective implements OnInit {
  @Input()
  value: boolean;

  constructor(private el: ElementRef) {}

  ngOnInit() {
    this.el.nativeElement.style.opacity = '0.8';
    if (!this.value) {
      this.el.nativeElement.style.backgroundColor = 'var(--success)';
    } else {
      this.el.nativeElement.style.backgroundColor = 'var(--danger)';
    }
  }
}
