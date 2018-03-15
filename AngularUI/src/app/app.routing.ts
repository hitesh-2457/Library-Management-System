import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { CheckinComponent } from './checkin/checkin.component';
import { BorrowersComponent } from './borrowers/borrowers.component';
import { FinesComponent } from './fines/fines.component';

const APP_ROUTES: Routes = [

  { path: '', component: HomeComponent },
  { path: 'checkin', component: CheckinComponent },
  { path: 'fines', component: FinesComponent },
  { path: 'borrowers', component: BorrowersComponent }
  // { path: '**', redirectTo: ''},s
  // {path:'', redirectTo:'/home', pathMatch:'full'}
];

export const AppRoutingModule = RouterModule.forRoot(APP_ROUTES);
