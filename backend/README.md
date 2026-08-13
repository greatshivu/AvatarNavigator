# .NET Backend

**AvatarNavigator.API** is an ASP.NET Core 8 Web API that provides RESTful endpoints for order management, item catalog, and Azure Avatar voice interaction.

## Project Structure

```
backend/
├── Controllers/          # API endpoints
│   ├── OrdersController.cs
│   ├── ItemsController.cs
│   └── AvatarController.cs
├── Models/              # Data models
│   ├── Order.cs
│   ├── Item.cs
│   └── OrderStatus.cs
├── Services/            # Business logic
│   ├── OrderService.cs
│   ├── ItemService.cs
│   └── AvatarService.cs
├── Data/                # Database context & migrations
│   ├── ApplicationDbContext.cs
│   └── SeedData.cs
├── Program.cs           # Application startup
├── appsettings.json     # Configuration
└── AvatarNavigator.API.csproj
```

## Features

### Order Management
- CRUD operations for orders
- Search and filter by status
- Relationship with order items

### Item Catalog
- Browse 10,000+ items
- Advanced filtering (category, brand, color, price)
- Item inventory management

### Azure Avatar Integration
- Voice command recognition
- Text-to-speech synthesis
- Real-time avatar responses

## API Endpoints

### Orders
- `GET /api/orders` - Get all orders
- `GET /api/orders/{id}` - Get order details
- `GET /api/orders/search?term=` - Search orders
- `GET /api/orders/status/{status}` - Filter by status
- `POST /api/orders` - Create order
- `PUT /api/orders/{id}` - Update order
- `DELETE /api/orders/{id}` - Delete order

### Items
- `GET /api/items` - Get all items
- `GET /api/items/{id}` - Get item details
- `GET /api/items/search?term=` - Search items
- `GET /api/items/category/{category}` - Get by category
- `POST /api/items/filter` - Advanced filter
- `POST /api/items` - Create item
- `PUT /api/items/{id}` - Update item
- `DELETE /api/items/{id}` - Delete item

### Avatar
- `POST /api/avatar/voice-command` - Process voice
- `POST /api/avatar/synthesize` - Text to speech
- `GET /api/avatar/health` - Health check

## Database

### Technologies
- SQL Server 2022
- Entity Framework Core 8
- Migration-based schema management

### Seeding
On first run, the database is automatically seeded with:
- 10,000 orders with realistic data
- 10,000 items across 6 categories
- Order items with random quantities

## Configuration

Update `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=AvatarNavigatorDB;User Id=sa;Password=YourPassword;"
  },
  "AzureAvatar": {
    "SubscriptionKey": "your-key-here",
    "Region": "eastus"
  }
}
```

## Running the Backend

### Development
```bash
cd backend
dotnet restore
dotnet run
```

### With Docker
```bash
docker-compose up backend
```

### Building for Production
```bash
dotnet publish -c Release -o ./release
```

## Dependencies

- **Microsoft.EntityFrameworkCore** - ORM
- **Microsoft.EntityFrameworkCore.SqlServer** - SQL Server provider
- **Microsoft.CognitiveServices.Speech** - Azure Speech Services
- **Swashbuckle.AspNetCore** - Swagger/OpenAPI

## Testing

```bash
dotnet test
```

## Database Migrations

```bash
# Add migration
dotnet ef migrations add MigrationName

# Apply migrations
dotnet ef database update

# Revert migration
dotnet ef migrations remove
```

## Logging

Configured in `appsettings.json`:
```json
"Logging": {
  "LogLevel": {
    "Default": "Information",
    "Microsoft.EntityFrameworkCore": "Debug"
  }
}
```

## CORS Configuration

CORS is enabled for Angular frontend development. Update `Program.cs` for production domains.

## Swagger Documentation

Available at `http://localhost:5000/swagger` when running locally.

## Error Handling

The API uses standard HTTP status codes:
- `200 OK` - Successful request
- `201 Created` - Resource created
- `204 No Content` - Successful deletion
- `400 Bad Request` - Invalid input
- `404 Not Found` - Resource not found
- `500 Internal Server Error` - Server error
