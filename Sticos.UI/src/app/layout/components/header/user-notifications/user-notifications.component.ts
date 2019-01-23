import { Component, OnInit } from '@angular/core';
import { Notification, NotificationService } from '@sticos/apis/common';
import { UserCacheService } from 'src/app/core/services';

@Component({
  selector: 'app-user-notifications',
  templateUrl: './user-notifications.component.html',
  styleUrls: ['./user-notifications.component.scss'],
})
export class UserNotificationsComponent implements OnInit {
  notifications: Notification[];
  customerId: string;

  constructor(
    private notificationService: NotificationService,
    private userCacheService: UserCacheService,
  ) {
    this.userCacheService.ClaimsUser().subscribe(claimsUser => {
      this.customerId = claimsUser.customerId.toString();
    });
  }

  ngOnInit() {
    this.notificationService
      .Search({ customerId: this.customerId })
      .subscribe(data => {
        this.notifications = data;
      });
  }
}
