# AvatarNavigator - Setup and Getting Started Guide

## Prerequisites
- Docker and Docker Compose (for containerized setup)
- OR:
  - .NET 8 SDK
  - Node.js 20+ and npm
  - SQL Server 2022

## Quick Start with Docker

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd AvatarNavigator
   ```

2. **Start all services**
   ```bash
   docker-compose up --build
   ```

3. **Access the application**
   - Frontend: http://localhost:4200
   - Backend API: http://localhost:5000
   - Swagger UI: http://localhost:5000/swagger

## Manual Setup

### Backend Setup

1. **Navigate to backend directory**
   ```bash
   cd backend
   ```

2. **Install dependencies**
   ```bash
   dotnet restore
   ```

3. **Update database connection string** (in `appsettings.json` if needed)

4. **Create and seed database**
   ```bash
   dotnet ef database update
   ```

5. **Run the application**
   ```bash
   dotnet run
   ```

### Frontend Setup

1. **Navigate to frontend directory**
   ```bash
   cd frontend
   ```

2. **Install dependencies**
   ```bash
   npm install
   ```

3. **Start development server**
   ```bash
   npm start
   ```

4. **Open browser**
   Navigate to http://localhost:4200

## Configuration

### Azure Avatar Settings
Update `backend/appsettings.json` with your Azure credentials:
```json
"AzureAvatar": {
  "SubscriptionKey": "your-subscription-key",
  "Region": "eastus"
}
```

### Database Connection
Update connection string in `backend/appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Initial Catalog=AvatarNavigatorDB;User Id=sa;Password=YourPassword;"
}
```

## Project Features

### Dashboard
- View order statistics
- Track total revenue
- Monitor pending and shipped orders

### Orders Page
- Search orders by order number or customer name
- View order details
- Manage order status

### Items Catalog
- Browse 10,000+ items
- Search by name or description
- Filter by category, brand, color, and price range
- View item details and availability

### Avatar Interface
- Record voice commands
- Process voice input to text
- Synthesize text-to-speech responses
- Real-time avatar interaction

## API Documentation

Full API documentation is available at `http://localhost:5000/swagger` when the backend is running.

### Sample API Calls

**Get all orders:**
```bash
curl http://localhost:5000/api/orders
```

**Search for items:**
```bash
curl "http://localhost:5000/api/items/search?term=electronics"
```

**Filter items:**
```bash
curl -X POST http://localhost:5000/api/items/filter \
  -H "Content-Type: application/json" \
  -d '{"category":"Electronics","minPrice":10,"maxPrice":100}'
```

## Troubleshooting

### Docker Issues
- Ensure Docker daemon is running
- Check port availability (1433, 5000, 4200)
- View logs: `docker-compose logs -f`

### Database Connection Issues
- Verify SQL Server is running
- Check connection string in appsettings.json
- Ensure database user has proper permissions

### Frontend Build Issues
- Clear node_modules: `rm -rf node_modules && npm install`
- Clear Angular cache: `ng cache clean`

### Port Already in Use
- Find process using port: `lsof -i :5000` (or other port)
- Kill process: `kill -9 <PID>`

## Development Workflow

1. **Backend Development**
   - Models are in `backend/Models/`
   - Services in `backend/Services/`
   - Controllers in `backend/Controllers/`
   - Database migrations: `dotnet ef migrations add MigrationName`

2. **Frontend Development**
   - Components in `frontend/src/app/components/`
   - Services in `frontend/src/app/services/`
   - Models in `frontend/src/app/models/`
   - Styles follow BEM naming convention

## Deployment

### Docker Image Build
```bash
# Build images
docker-compose build

# Push to registry
docker tag avatarnavigator-backend:latest yourregistry/avatarnavigator-backend:latest
docker push yourregistry/avatarnavigator-backend:latest
```

### Production Checklist
- [ ] Update Azure Avatar credentials
- [ ] Configure production database
- [ ] Enable HTTPS
- [ ] Set up reverse proxy (nginx/IIS)
- [ ] Configure CORS for production domain
- [ ] Enable logging and monitoring
- [ ] Set up backup strategy

## Contributing

1. Create a feature branch
2. Make your changes
3. Test thoroughly
4. Submit a pull request

## Support

For issues and questions, please create an issue in the repository.

## License

This project is licensed under the MIT License.
