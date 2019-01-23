import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class LoaderSpinnerService {
  public isLoading = new BehaviorSubject<boolean>(false);
}
