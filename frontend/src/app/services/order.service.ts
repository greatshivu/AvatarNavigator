import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Order, OrderStatus } from '../models';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private apiUrl = '/api/orders';

  constructor(private http: HttpClient) { }

  getAllOrders(): Observable<Order[]> {
    return this.http.get<Order[]>(this.apiUrl);
  }

  getOrderById(id: number): Observable<Order> {
    return this.http.get<Order>(`${this.apiUrl}/${id}`);
  }

  searchOrders(term: string): Observable<Order[]> {
    return this.http.get<Order[]>(`${this.apiUrl}/search`, {
      params: { term }
    });
  }

  getOrdersByStatus(status: OrderStatus): Observable<Order[]> {
    return this.http.get<Order[]>(`${this.apiUrl}/status/${status}`);
  }

  createOrder(order: Order): Observable<Order> {
    return this.http.post<Order>(this.apiUrl, order);
  }

  updateOrder(id: number, order: Order): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, order);
  }

  deleteOrder(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
