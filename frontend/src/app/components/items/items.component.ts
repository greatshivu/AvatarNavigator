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
  selectedBrand = '';
  selectedColor = '';
  selectedSize = '';
  inStockOnly = false;
  minPrice = '';
  maxPrice = '';
  startDate = '';
  endDate = '';
  loading = false;

  categories = ['Electronics', 'Clothing', 'Home', 'Sports', 'Books', 'Toys'];
  brands = ['Brand A', 'Brand B', 'Brand C', 'Brand D', 'Brand E'];
  colors = ['Red', 'Blue', 'Green', 'Black', 'White', 'Yellow'];
  sizes = ['S', 'M', 'L', 'XL', 'XXL'];

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

  buildCriteria(): FilterCriteria {
    const criteria: FilterCriteria = {
      searchText: this.searchTerm?.trim() || undefined,
      category: this.selectedCategory || undefined,
      brand: this.selectedBrand || undefined,
      color: this.selectedColor || undefined,
      size: this.selectedSize || undefined,
      inStockOnly: this.inStockOnly || undefined,
      minPrice: this.minPrice ? Number(this.minPrice) : undefined,
      maxPrice: this.maxPrice ? Number(this.maxPrice) : undefined,
      startDate: this.startDate || undefined,
      endDate: this.endDate || undefined
    };

    return Object.fromEntries(
      Object.entries(criteria).filter(([, value]) => value !== undefined && value !== '' && !(typeof value === 'boolean' && value === false))
    ) as FilterCriteria;
  }

  searchItems(): void {
    this.applyAdvancedFilter();
  }

  filterByCategory(): void {
    this.applyAdvancedFilter();
  }

  applyAdvancedFilter(): void {
    const criteria = this.buildCriteria();
    this.filterCriteria = criteria;

    this.loading = true;
    this.itemService.filterItems(criteria).subscribe({
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

  clearFilters(): void {
    this.searchTerm = '';
    this.selectedCategory = '';
    this.selectedBrand = '';
    this.selectedColor = '';
    this.selectedSize = '';
    this.inStockOnly = false;
    this.minPrice = '';
    this.maxPrice = '';
    this.startDate = '';
    this.endDate = '';
    this.filterCriteria = {};
    this.loadItems();
  }
}
