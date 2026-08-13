import { Routes } from '@angular/router';
import { OrdersComponent } from './components/orders/orders.component';
import { ItemsComponent } from './components/items/items.component';
import { AvatarComponent } from './components/avatar/avatar.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';

export const appRoutes: Routes = [
  { path: '', component: DashboardComponent },
  { path: 'orders', component: OrdersComponent },
  { path: 'items', component: ItemsComponent },
  { path: 'avatar', component: AvatarComponent },
  { path: '**', redirectTo: '' }
];
