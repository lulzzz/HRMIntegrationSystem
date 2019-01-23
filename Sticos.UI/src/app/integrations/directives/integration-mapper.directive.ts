import {
  Directive,
  OnInit,
  OnDestroy,
  ViewContainerRef,
  ComponentFactoryResolver,
  Type,
  Input,
} from '@angular/core';
import { IntegrationMapperNotFoundComponent } from '../components';
import { Integration } from '@sticos/apis/integrations';

@Directive({
  selector: '[appIntegrationMapper]',
})
export class IntegrationMapperDirective implements OnInit, OnDestroy {
  componentRef;
  init = false;

  @Input()
  category: string;
  @Input()
  integration: Integration;

  constructor(
    public viewContainerRef: ViewContainerRef,
    private resolver: ComponentFactoryResolver,
  ) {}

  ngOnInit(): void {
    const factories = Array.from(this.resolver['_factories'].keys());
    let factoryClass = <Type<any>>(
      factories.find(
        (x: any) =>
          x.sticosIntegrationMapper &&
          x.sticosIntegrationMapper.category === +this.category,
      )
    );
    factoryClass = factoryClass || IntegrationMapperNotFoundComponent;
    const factory = this.resolver.resolveComponentFactory(factoryClass);
    const cref = this.viewContainerRef.createComponent(factory);

    cref.instance.integration = this.integration;

    if (this.componentRef) {
      this.componentRef.destroy();
    }

    this.componentRef = cref;
    this.init = true;
  }

  ngOnDestroy(): void {
    if (this.componentRef) {
      this.componentRef.destroy();
      this.componentRef = null;
    }
  }
}
