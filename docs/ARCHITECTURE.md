# AvatarNavigator - Architecture and Design

## System Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                     Frontend (Angular)                       │
│  Dashboard | Orders | Items | Avatar Interface             │
└──────────────────────┬──────────────────────────────────────┘
                       │ HTTP/REST API
                       ▼
┌──────────────────────────────────────────────────────────────┐
│                  Backend (ASP.NET Core)                      │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐       │
│  │   Orders     │  │    Items     │  │    Avatar    │       │
│  │ Controllers  │  │ Controllers  │  │ Controllers  │       │
│  └──────┬───────┘  └──────┬───────┘  └──────┬───────┘       │
│         │                 │                 │                │
│  ┌──────▼──────────────────▼────────────────▼──────┐        │
│  │          Services Layer (Business Logic)        │        │
│  │  • OrderService  • ItemService  • AvatarService │        │
│  └──────┬────────────────────────────────────────┘         │
│         │                                                   │
│  ┌──────▼──────────────────────────────────┐              │
│  │   Entity Framework Core (ORM)           │              │
│  │  • ApplicationDbContext                 │              │
│  │  • Models • Migrations                  │              │
│  └──────┬──────────────────────────────────┘              │
└─────────┼──────────────────────────────────────────────────┘
          │
          ▼
    ┌──────────────┐
    │  SQL Server  │
    │  Database    │
    └──────────────┘
```

## Data Model

### Order Entity
```
Order
├── Id (PK)
├── OrderNumber (unique)
├── OrderDate
├── CustomerName
├── CustomerEmail
├── TotalAmount (decimal)
├── Status (enum: Pending, Processing, Shipped, Delivered, Cancelled)
├── ShippingAddress
├── CreatedAt
├── UpdatedAt
└── OrderItems (collection)
```

### Item Entity
```
Item
├── Id (PK)
├── Name
├── Description
├── Price (decimal)
├── Stock (int)
├── Category
├── ImageUrl (optional)
├── CreatedAt
├── UpdatedAt
├── Filter (one-to-one)
└── OrderItems (collection)
```

### ItemFilter Entity
```
ItemFilter
├── Id (PK)
├── ItemId (FK)
├── Brand
├── Color
├── Size
├── Rating (1-5)
└── Item (navigation)
```

### OrderItem Entity
```
OrderItem
├── Id (PK)
├── OrderId (FK)
├── ItemId (FK)
├── Quantity
├── UnitPrice
├── Order (navigation)
└── Item (navigation)
```

## API Design

### RESTful Endpoints

#### Orders API
```
GET    /api/orders              - List all orders
GET    /api/orders/{id}         - Get order details
GET    /api/orders/search       - Search orders
GET    /api/orders/status/{s}   - Filter by status
POST   /api/orders              - Create order
PUT    /api/orders/{id}         - Update order
DELETE /api/orders/{id}         - Delete order
```

#### Items API
```
GET    /api/items               - List all items
GET    /api/items/{id}          - Get item details
GET    /api/items/search        - Search items
GET    /api/items/category/{c}  - Filter by category
POST   /api/items/filter        - Advanced filtering
POST   /api/items               - Create item
PUT    /api/items/{id}          - Update item
DELETE /api/items/{id}          - Delete item
```

#### Avatar API
```
POST   /api/avatar/voice-command    - Process voice
POST   /api/avatar/synthesize       - Text to speech
GET    /api/avatar/health           - Health check
```

## Frontend Architecture

### Component Hierarchy
```
AppComponent (root)
├── DashboardComponent
├── OrdersComponent
│   └── OrderListComponent (future)
├── ItemsComponent
│   └── ItemCardComponent (future)
└── AvatarComponent
```

### Service Architecture
```
HttpClient
├── OrderService
├── ItemService
└── AvatarService
```

### State Management
Currently using component state with RxJS. Future enhancement: NgRx for complex state management.

## Database Schema Relationships

```
Orders (1) ──────────────────────── (N) OrderItems
  │                                    │
  └────────────────────────────────────┘
                                       │
                                       │
                          (N)          │         (1)
                  OrderItems ──────────┴────── Items
                                               │
                                               │
                          (1-1)                │
                  ItemFilters ─────────────────┘
```

## Security Architecture

### Authentication & Authorization
- Future: JWT token-based authentication
- Current: CORS enabled for development

### Data Protection
- SQL Server encryption at rest
- HTTPS for data in transit (production)
- Input validation on all endpoints
- SQL injection prevention via EF Core

### Azure Integration
- Azure Speech Services for voice processing
- Subscription key management
- API key rotation (production)

## Scalability Considerations

### Database
- Indexing on frequently queried fields
- Pagination for large datasets
- Query optimization
- Connection pooling

### Backend
- Stateless API design
- Horizontal scaling via containers
- Caching layer (Redis) for frequently accessed data
- Load balancing

### Frontend
- Lazy loading of routes
- Code splitting
- Virtual scrolling for large lists
- Service worker for offline capability

## Deployment Architecture

### Container Stack
```
Docker Network
├── SQL Server Container
├── Backend Container (ASP.NET Core)
└── Frontend Container (Nginx)
```

### Production Deployment
- Kubernetes orchestration
- Load balancer
- Database replication
- CDN for static assets
- Monitoring and logging (Application Insights)

## Error Handling Strategy

### Backend
- Centralized exception handling middleware
- Meaningful HTTP status codes
- Structured error responses
- Logging all errors

### Frontend
- Global HTTP error interceptor
- User-friendly error messages
- Retry logic for transient failures
- Error tracking/analytics

## Performance Optimization

### Backend
- Database query optimization
- Async/await for non-blocking I/O
- Caching strategies
- Compression middleware

### Frontend
- Change detection optimization
- Bundle size analysis
- Tree shaking for unused code
- Image optimization
- HTTP/2 server push

## Monitoring and Observability

- Application Insights for backend
- Error tracking (Sentry)
- Performance monitoring
- User analytics
- Distributed tracing
