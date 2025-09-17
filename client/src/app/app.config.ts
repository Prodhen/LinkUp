import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideToastr } from 'ngx-toastr'
import { TimeagoModule } from 'ngx-timeago';
import { errorsInterceptor } from './_interceptors/errors.interceptor';
import { jwtInterceptor } from './_interceptors/jwt.interceptor';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ModalModule } from 'ngx-bootstrap/modal';
import { loadingInterceptor } from './_interceptors/loading.interceptor';


export const appConfig: ApplicationConfig = {
  providers: [provideZoneChangeDetection({ eventCoalescing: true }),
  provideRouter(routes),
  provideHttpClient(withInterceptors([errorsInterceptor, jwtInterceptor, loadingInterceptor])),
  provideAnimations(),
  provideToastr({
    positionClass: 'toast-bottom-right'
  }),
  importProvidersFrom(NgxSpinnerModule.forRoot({
    type: 'line-scale-party'
  })),
  importProvidersFrom(TimeagoModule.forRoot(), ModalModule.forRoot())

  ]
};
