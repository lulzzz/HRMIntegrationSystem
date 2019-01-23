import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { slideTopBottom, slideBottomTop, slideLeftRight } from '@sticos/ui';
import { LayoutService } from '../../services/layout.service';
import { HostListener } from '@angular/core';

@Component({
  selector: 'app-side-menu',
  templateUrl: './side-menu.component.html',
  styleUrls: ['./side-menu.component.scss'],
  animations: [slideTopBottom, slideBottomTop, slideLeftRight],
})
export class SideMenuComponent implements OnInit {
  isAdminPanel = false;
  mobileSideMenu = false;

  // Side menu close on mobile
  @HostListener('window:resize', ['$event'])
  onResize() {
    if (
      window.innerWidth < 767 &&
      this.layoutService.isOpen &&
      !this.mobileSideMenu
    ) {
      this.layoutService.sidebarToggle();
      this.mobileSideMenu = true;
    } else if (window.innerWidth > 767 && this.mobileSideMenu) {
      this.mobileSideMenu = false;
    }
  }

  constructor(
    private location: Location,
    private layoutService: LayoutService,
  ) {}

  ngOnInit() {
    this.checkCurrentLocation();
  }

  checkCurrentLocation() {
    const url = this.location.path();

    if (url.includes('admin')) {
      this.isAdminPanel = true;
      return true;
    }
    return false;
  }
}
