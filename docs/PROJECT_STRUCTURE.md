# AvatarNavigator Project Structure

## Backend (ASP.NET Core)
- `backend/` - .NET 8 Web API project
  - `Controllers/` - API endpoints for Orders, Items, and Avatar services
  - `Models/` - Data models (Order, Item, OrderItem, ItemFilter)
  - `Services/` - Business logic services
  - `Data/` - Entity Framework Core DbContext and seed data
  - `Program.cs` - Application startup and configuration

## Frontend (Angular)
- `frontend/` - Angular 17 web application
  - `src/app/components/` - Reusable Angular components
    - `dashboard/` - Dashboard with statistics
    - `orders/` - Order management page
    - `items/` - Item catalog with filtering
    - `avatar/` - Avatar voice interaction interface
  - `src/app/services/` - HTTP services for API communication
  - `src/app/models/` - TypeScript interfaces and types

## Database
- Configured for SQL Server
- Initial seed: 10,000 orders and 10,000 items with filters
- Entity relationships between Orders, OrderItems, and Items

## Configuration Files
- `docker-compose.yml` - Multi-container setup for SQL Server, Backend, and Frontend
- `.editorconfig` - Code style configuration
- `.gitignore` - Git ignore rules

## API Endpoints

### Orders API
- `GET /api/orders` - Get all orders
- `GET /api/orders/{id}` - Get order by ID
- `GET /api/orders/search?term=` - Search orders
- `GET /api/orders/status/{status}` - Get orders by status
- `POST /api/orders` - Create order
- `PUT /api/orders/{id}` - Update order
- `DELETE /api/orders/{id}` - Delete order

### Items API
- `GET /api/items` - Get all items
- `GET /api/items/{id}` - Get item by ID
- `GET /api/items/search?term=` - Search items
- `GET /api/items/category/{category}` - Get items by category
- `POST /api/items/filter` - Filter items
- `POST /api/items` - Create item
- `PUT /api/items/{id}` - Update item
- `DELETE /api/items/{id}` - Delete item

### Avatar API
- `POST /api/avatar/voice-command` - Process voice command from audio file
- `POST /api/avatar/synthesize` - Synthesize text to speech
- `GET /api/avatar/health` - Health check

## Running the Project

### Using Docker Compose
```bash
docker-compose up --build
```

### Manual Setup
1. Install SQL Server and create `AvatarNavigatorDB` database
2. Install .NET 8 SDK
3. Build and run backend: `dotnet run` in `backend/` directory
4. Install Node.js and npm
5. Install Angular CLI: `npm install -g @angular/cli`
6. Install dependencies and run frontend: `npm install && npm start` in `frontend/` directory

## Features
- ✅ Order management with search and filtering by status
- ✅ Item catalog with category filtering and advanced search
- ✅ Voice command processing via Azure Speech Services
- ✅ Text-to-speech synthesis with Avatar
- ✅ Dashboard with order statistics
- ✅ Responsive web design
- ✅ RESTful API with CORS support
- ✅ Entity Framework Core ORM with migrations
- ✅ 10,000 seed records for demonstration

## Future Enhancements
- Azure Avatar SDK integration for realistic avatar rendering
- Advanced voice command processing with NLP
- Real-time updates with SignalR
- Authentication and authorization
- User profiles and preferences
- Order history and tracking
- Item recommendations based on search history
