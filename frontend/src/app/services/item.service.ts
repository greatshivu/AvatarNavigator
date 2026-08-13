import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Item, FilterCriteria } from '../models';

@Injectable({
  providedIn: 'root'
})
export class ItemService {
  private apiUrl = '/api/items';

  constructor(private http: HttpClient) { }

  getAllItems(): Observable<Item[]> {
    return this.http.get<Item[]>(this.apiUrl);
  }

  getItemById(id: number): Observable<Item> {
    return this.http.get<Item>(`${this.apiUrl}/${id}`);
  }

  searchItems(term: string): Observable<Item[]> {
    return this.http.get<Item[]>(`${this.apiUrl}/search`, {
      params: { term }
    });
  }

  getItemsByCategory(category: string): Observable<Item[]> {
    return this.http.get<Item[]>(`${this.apiUrl}/category/${category}`);
  }

  filterItems(criteria: FilterCriteria): Observable<Item[]> {
    return this.http.post<Item[]>(`${this.apiUrl}/filter`, criteria);
  }

  createItem(item: Item): Observable<Item> {
    return this.http.post<Item>(this.apiUrl, item);
  }

  updateItem(id: number, item: Item): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, item);
  }

  deleteItem(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
