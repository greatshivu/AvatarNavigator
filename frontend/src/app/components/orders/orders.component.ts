import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { OrderService } from '../../services/order.service';
import { Order } from '../../models';

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css']
})
export class OrdersComponent implements OnInit {
  orders: Order[] = [];
  searchTerm = '';
  loading = false;

  constructor(private orderService: OrderService) { }

  ngOnInit(): void {
    this.loadOrders();
  }

  loadOrders(): void {
    this.loading = true;
    this.orderService.getAllOrders().subscribe({
      next: (data: Order[]) => {
        this.orders = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading orders:', err);
        this.loading = false;
      }
    });
  }

  searchOrders(): void {
    if (!this.searchTerm) {
      this.loadOrders();
      return;
    }
    this.loading = true;
    this.orderService.searchOrders(this.searchTerm).subscribe({
      next: (data: Order[]) => {
        this.orders = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error searching orders:', err);
        this.loading = false;
      }
    });
  }

  getStatusLabel(status: number): string {
    const statusLabels: { [key: number]: string } = {
      0: 'Pending',
      1: 'Processing',
      2: 'Shipped',
      3: 'Delivered',
      4: 'Cancelled'
    };
    return statusLabels[status] || 'Unknown';
  }
}
