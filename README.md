# AvatarNavigator

> A web application tool that renders the Azure Avatar and communicates with users through voice-based commands for seamless web navigation and interaction.

![Version](https://img.shields.io/badge/version-1.0.0-blue)
![License](https://img.shields.io/badge/license-MIT-green)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-purple)
![Angular](https://img.shields.io/badge/Angular-17-red)

## 🎯 Overview

AvatarNavigator is an innovative web application that combines:

- **Azure Avatar Integration** - Realistic, lifelike avatars for user interaction
- **Voice Recognition** - Natural language voice command processing
- **Smart Navigation** - Voice-controlled website navigation
- **Interactive Catalog** - Browse 10,000+ products with voice filtering
- **Order Management** - Complete order tracking and management system

Users can interact with the application naturally using voice commands like:
- "Navigate to orders page"
- "Filter items by price"
- "Show electronics under $100"
- "Go back to previous page"

## 🚀 Quick Start

### Prerequisites

- Docker and Docker Compose (recommended)
- OR: .NET 8 SDK, Node.js 20+, SQL Server 2022

### 1. Using Docker Compose (Easiest)

```bash
git clone https://github.com/greatshivu/AvatarNavigator.git
cd AvatarNavigator
docker-compose up --build
```

Then open:
- **Frontend**: http://localhost:4200
- **Backend API**: http://localhost:5000
- **Swagger Docs**: http://localhost:5000/swagger

### 2. Manual Setup

#### Backend Setup
```bash
cd backend
dotnet restore
dotnet ef database update
dotnet run
```

#### Frontend Setup
```bash
cd frontend
npm install
npm start
```

## 📁 Project Structure

```
AvatarNavigator/
├── backend/                    # ASP.NET Core 8 API
│   ├── Controllers/           # API endpoints
│   ├── Models/                # Data models
│   ├── Services/              # Business logic
│   ├── Data/                  # Database & EF Core
│   └── appsettings.json       # Configuration
│
├── frontend/                   # Angular 17 SPA
│   ├── src/app/
│   │   ├── components/        # Dashboard, Orders, Items, Avatar
│   │   ├── services/          # API integration
│   │   └── models/            # TypeScript interfaces
│   └── package.json
│
├── database/                   # Database scripts (optional)
├── docs/                       # Documentation
└── docker-compose.yml         # Container orchestration
```

## 🎨 Features

### 🏠 Dashboard
- Order statistics and metrics
- Revenue tracking
- Order status summary
- Quick overview of key metrics

### 📦 Order Management
- View all orders with details
- Search by order number or customer name
- Filter by order status
- Manage order items
- Track shipping information

### 🛍️ Item Catalog
- Browse 10,000+ products
- Category filtering
- Advanced search with filters
  - Price range filtering
  - Brand selection
  - Color and size filters
- Product details and availability
- Inventory status tracking

### 🤖 Avatar Interaction
- Record voice commands
- Real-time voice-to-text conversion
- Text-to-speech avatar responses
- Microphone input management
- Natural language command processing

## 🔌 API Endpoints

### Orders
```
GET    /api/orders              # List all
GET    /api/orders/{id}         # Get details
GET    /api/orders/search       # Search
GET    /api/orders/status/{s}   # Filter by status
POST   /api/orders              # Create
PUT    /api/orders/{id}         # Update
DELETE /api/orders/{id}         # Delete
```

### Items
```
GET    /api/items               # List all
GET    /api/items/{id}          # Get details
GET    /api/items/search        # Search
GET    /api/items/category/{c}  # Get by category
POST   /api/items/filter        # Advanced filter
POST   /api/items               # Create
PUT    /api/items/{id}          # Update
DELETE /api/items/{id}          # Delete
```

### Avatar
```
POST   /api/avatar/voice-command    # Process voice
POST   /api/avatar/synthesize       # Text to speech
GET    /api/avatar/health           # Health check
```

## 🛠️ Technologies

### Backend
- **ASP.NET Core 8** - Web framework
- **Entity Framework Core** - ORM
- **SQL Server** - Database
- **Azure Speech Services** - Voice processing
- **Swagger/OpenAPI** - API documentation

### Frontend
- **Angular 17** - Framework
- **TypeScript 5.2** - Language
- **RxJS 7.8** - Reactive programming
- **CSS3** - Styling with BEM methodology

### DevOps
- **Docker** - Containerization
- **Docker Compose** - Orchestration

## 🗄️ Database

### Schema
- **Orders** - Order information and status
- **Items** - Product catalog
- **OrderItems** - Order line items
- **ItemFilters** - Product attributes

### Initial Data
- 10,000 orders with realistic data
- 10,000 items across 6 categories
- Random order relationships
- Filter attributes for each item

## 🔐 Configuration

### Azure Avatar Setup

Update `backend/appsettings.json`:

```json
{
  "AzureAvatar": {
    "SubscriptionKey": "your-subscription-key",
    "Region": "eastus"
  }
}
```

### Database Connection

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Initial Catalog=AvatarNavigatorDB;User Id=sa;Password=AvatarNav@123;"
  }
}
```

## 📖 Documentation

- [Setup Guide](./docs/SETUP.md) - Detailed installation instructions
- [Development Guide](./docs/DEVELOPMENT.md) - Development workflow
- [Architecture](./docs/ARCHITECTURE.md) - System design and structure
- [Project Structure](./docs/PROJECT_STRUCTURE.md) - Directory organization
- [Backend README](./backend/README.md) - API documentation
- [Frontend README](./frontend/README.md) - UI component documentation

## 🧪 Testing

### Backend
```bash
cd backend
dotnet test
```

### Frontend
```bash
cd frontend
npm test
```

## 🚢 Deployment

### Docker Image Build
```bash
docker-compose build
docker-compose up -d
```

### Production Checklist
- [ ] Update Azure credentials
- [ ] Configure production database
- [ ] Enable HTTPS
- [ ] Set up reverse proxy (nginx/IIS)
- [ ] Configure CORS for production domain
- [ ] Enable logging and monitoring
- [ ] Set up backup strategy

## 🐛 Troubleshooting

### Port Already in Use
```bash
# Find process using port
lsof -i :5000
# Kill process
kill -9 <PID>
```

### Database Connection Error
- Verify SQL Server is running
- Check connection string in appsettings.json
- Ensure database user permissions

### Frontend Build Issues
```bash
rm -rf node_modules
npm install
ng cache clean
```

### Docker Issues
```bash
# View logs
docker-compose logs -f

# Restart services
docker-compose restart

# Clean up
docker-compose down -v
```

## 📝 Development

### Create Feature Branch
```bash
git checkout -b feature/feature-name
```

### Commit Message Format
```
<type>: <subject>

<body>

<footer>
```

Types: `feat`, `fix`, `docs`, `style`, `refactor`, `perf`, `test`

### Pull Request Process
1. Create feature branch
2. Make changes with tests
3. Commit with clear messages
4. Submit PR with description
5. Address review comments
6. Merge after approval

## 🤝 Contributing

Contributions are welcome! Please:

1. Fork the repository
2. Create your feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## 📄 License

This project is licensed under the MIT License - see [LICENSE](LICENSE) file for details.

## 🙋 Support

For issues, questions, or suggestions:

- Create an [GitHub Issue](https://github.com/greatshivu/AvatarNavigator/issues)
- Contact the development team
- Check existing documentation

## 👨‍💻 Author

**Shiva** - [@greatshivu](https://github.com/greatshivu)

## 🎓 Acknowledgments

- Azure Avatar Service
- Microsoft .NET Foundation
- Angular Team
- Community contributors

## 📚 Resources

- [Azure Speech Services](https://azure.microsoft.com/en-us/services/cognitive-services/speech-services/)
- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [Angular Documentation](https://angular.io)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)

---

**Made with ❤️ by greatshivu**