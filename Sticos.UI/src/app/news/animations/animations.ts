import {
  animate,
  state,
  style,
  transition,
  trigger,
} from '@angular/animations';

// image fullsize transition
export const imageTransition = trigger('imageTransition', [
  state(
    'small',
    style({ objectFit: 'cover', height: '400px', cursor: 'zoom-in' }),
  ),
  transition('small => large', animate('200ms ease-in')),
  state('large', style({ cursor: 'zoom-out' })),
  transition('large => small', animate('200ms ease-in')),
]);
