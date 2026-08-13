import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ItemService } from '../../services/item.service';
import { Item, FilterCriteria } from '../../models';

@Component({
  selector: 'app-items',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './items.component.html',
  styleUrls: ['./items.component.css']
})
export class ItemsComponent implements OnInit {
  items: Item[] = [];
  searchTerm = '';
  selectedCategory = '';
  loading = false;
  categories = ['Electronics', 'Clothing', 'Home', 'Sports', 'Books', 'Toys'];

  filterCriteria: FilterCriteria = {};

  constructor(private itemService: ItemService) { }

  ngOnInit(): void {
    this.loadItems();
  }

  loadItems(): void {
    this.loading = true;
    this.itemService.getAllItems().subscribe({
      next: (data: Item[]) => {
        this.items = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading items:', err);
        this.loading = false;
      }
    });
  }

  searchItems(): void {
    if (!this.searchTerm) {
      this.loadItems();
      return;
    }
    this.loading = true;
    this.itemService.searchItems(this.searchTerm).subscribe({
      next: (data: Item[]) => {
        this.items = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error searching items:', err);
        this.loading = false;
      }
    });
  }

  filterByCategory(): void {
    if (!this.selectedCategory) {
      this.loadItems();
      return;
    }
    this.loading = true;
    this.itemService.getItemsByCategory(this.selectedCategory).subscribe({
      next: (data: Item[]) => {
        this.items = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error filtering items:', err);
        this.loading = false;
      }
    });
  }

  applyAdvancedFilter(): void {
    this.loading = true;
    this.itemService.filterItems(this.filterCriteria).subscribe({
      next: (data: Item[]) => {
        this.items = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error applying filter:', err);
        this.loading = false;
      }
    });
  }
}
