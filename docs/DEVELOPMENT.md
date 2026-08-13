## AvatarNavigator Development

This document outlines the development setup and contribution guidelines.

### Code Standards

#### C# / .NET Backend
- Follow Microsoft C# naming conventions
- Use Entity Framework Core for data access
- Async/await for all I/O operations
- Dependency injection for services
- XML documentation comments for public APIs

#### Angular Frontend
- Use standalone components (Angular 17+)
- Follow Google Angular Style Guide
- Use typed services with RxJS
- Implement proper error handling
- Use BEM methodology for CSS

### Git Workflow

1. **Main Branch** - Production-ready code
2. **Develop Branch** - Development integration
3. **Feature Branches** - `feature/feature-name`
4. **Bug Branches** - `bug/bug-description`

### Commit Message Format

```
<type>: <subject>

<body>

<footer>
```

Types: feat, fix, docs, style, refactor, perf, test

Example:
```
feat: Add voice command filtering for items

- Implement voice to text processing
- Add filter criteria extraction
- Update item search to accept voice filters

Closes #123
```

### Testing

#### Backend Testing
```bash
cd backend
dotnet test
```

#### Frontend Testing
```bash
cd frontend
npm test
```

### Building for Production

#### Backend
```bash
cd backend
dotnet publish -c Release
```

#### Frontend
```bash
cd frontend
npm run build -- --configuration production
```

### Debugging

#### Backend (Visual Studio Code)
Create `.vscode/launch.json`:
```json
{
  "version": "0.2.0",
  "configurations": [
    {
      "name": ".NET Core Attach",
      "type": "coreclr",
      "request": "attach",
      "processId": "${command:pickProcess}"
    }
  ]
}
```

#### Frontend (Browser DevTools)
- Use Angular DevTools extension
- Check Network tab for API calls
- Use RxJS DevTools for subscription debugging

## Performance Optimization

### Backend
- Implement pagination for large datasets
- Use EF Core query optimization
- Add caching for frequently accessed data
- Monitor query performance

### Frontend
- Lazy load routes
- Implement virtual scrolling for large lists
- Optimize change detection
- Use OnPush strategy where possible

## Security Considerations

- [ ] Input validation on both frontend and backend
- [ ] SQL injection prevention (using EF Core)
- [ ] XSS prevention (Angular sanitization)
- [ ] CORS configuration
- [ ] Azure credentials management
- [ ] HTTPS in production
- [ ] Database encryption
- [ ] API rate limiting

## Database Migrations

```bash
cd backend

# Add migration
dotnet ef migrations add AddNewFeature

# Update database
dotnet ef database update

# Revert last migration
dotnet ef migrations remove
```

## Monitoring and Logging

### Backend Logging
Configure in `appsettings.json`:
```json
"Logging": {
  "LogLevel": {
    "Default": "Information",
    "Microsoft": "Warning"
  }
}
```

### Application Insights
Enable in backend for production monitoring and diagnostics.

## Useful Commands

### Docker
```bash
# Build and run
docker-compose up --build

# View logs
docker-compose logs -f

# Stop services
docker-compose down

# Remove volumes
docker-compose down -v
```

### Backend
```bash
# Run tests
dotnet test

# Run with specific configuration
dotnet run --configuration Release

# Generate NuGet packages
dotnet pack
```

### Frontend
```bash
# Run tests
npm test

# Run with specific environment
npm start -- --configuration=production

# Generate coverage report
npm run test -- --code-coverage

# Lint code
npm run lint
```
