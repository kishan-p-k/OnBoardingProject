import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { LoginPageService } from './Services/login-page-service';

export const authGuard: CanActivateFn = (route, state) => {

  const loginService = inject(LoginPageService);
  const router = inject(Router);

  const user = loginService.getCurrentUser();

  if (!user) {
    return router.createUrlTree(['/login']);
  }

  if (route.routeConfig?.path === 'userbugs/:ref_id') {
    const refId = route.paramMap.get('ref_id');

    if (refId !== user.reference_id) {
      return router.createUrlTree(['/bug']);
    }
  }
  if (route.routeConfig?.path === 'admin') {
    if (user.role !== 'Admin') {
      return router.createUrlTree(['/login']);
    }
  }

  return true;
};
