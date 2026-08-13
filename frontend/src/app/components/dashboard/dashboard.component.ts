import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrderService } from '../../services/order.service';
import { Order } from '../../models';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  totalOrders = 0;
  totalRevenue = 0;
  pendingOrders = 0;
  shippedOrders = 0;

  constructor(private orderService: OrderService) { }

  ngOnInit(): void {
    this.loadDashboardData();
  }

  loadDashboardData(): void {
    this.orderService.getAllOrders().subscribe({
      next: (orders: Order[]) => {
        this.totalOrders = orders.length;
        this.totalRevenue = orders.reduce((sum, order) => sum + order.totalAmount, 0);
        this.pendingOrders = orders.filter(o => o.status === 0).length;
        this.shippedOrders = orders.filter(o => o.status === 2).length;
      },
      error: (err) => console.error('Error loading dashboard data:', err)
    });
  }
}
