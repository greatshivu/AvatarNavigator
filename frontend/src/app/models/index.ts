export interface Order {
  id: number;
  orderNumber: string;
  orderDate: Date;
  customerName: string;
  customerEmail: string;
  totalAmount: number;
  status: OrderStatus;
  shippingAddress: string;
  orderItems: OrderItem[];
}

export interface OrderItem {
  id: number;
  orderId: number;
  itemId: number;
  quantity: number;
  unitPrice: number;
  item?: Item;
}

export enum OrderStatus {
  Pending = 0,
  Processing = 1,
  Shipped = 2,
  Delivered = 3,
  Cancelled = 4
}

export interface Item {
  id: number;
  name: string;
  description: string;
  price: number;
  stock: number;
  category: string;
  imageUrl?: string;
  filter?: ItemFilter;
}

export interface ItemFilter {
  id: number;
  itemId: number;
  brand?: string;
  color?: string;
  size?: string;
  rating?: number;
}

export interface FilterCriteria {
  searchText?: string;
  category?: string;
  brand?: string;
  color?: string;
  size?: string;
  inStockOnly?: boolean;
  minPrice?: number;
  maxPrice?: number;
  startDate?: string;
  endDate?: string;
}
