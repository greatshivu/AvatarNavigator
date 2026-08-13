# Angular Frontend

**AvatarNavigator Frontend** is an Angular 17 single-page application that provides a modern interface for order management, item catalog browsing, and avatar-based voice interaction.

## Project Structure

```
frontend/
├── src/
│   ├── app/
│   │   ├── components/
│   │   │   ├── dashboard/        # Dashboard statistics
│   │   │   ├── orders/           # Order management
│   │   │   ├── items/            # Item catalog
│   │   │   └── avatar/           # Voice interaction
│   │   ├── services/
│   │   │   ├── order.service.ts
│   │   │   ├── item.service.ts
│   │   │   └── avatar.service.ts
│   │   ├── models/
│   │   │   └── index.ts          # TypeScript interfaces
│   │   ├── app.routes.ts         # Route configuration
│   │   └── app.component.ts      # Root component
│   ├── main.ts                   # Application entry point
│   ├── index.html                # HTML template
│   └── styles.css                # Global styles
├── package.json
├── angular.json
└── tsconfig.json
```

## Features

### Dashboard
- Order statistics overview
- Revenue tracking
- Order status summary

### Orders Page
- List all orders with pagination
- Search orders by number or customer name
- Filter by status
- View order details and items

### Items Catalog
- Browse 10,000+ products
- Search by name or description
- Filter by category, brand, color, price range
- Product grid with details
- Inventory status

### Avatar Interface
- Record and process voice commands
- Real-time voice-to-text conversion
- Text-to-speech synthesis
- Avatar response display
- Microphone input management

## Technologies

- **Angular 17** - Frontend framework
- **TypeScript 5.2** - Language
- **RxJS 7.8** - Reactive programming
- **Standalone Components** - Modern Angular pattern

## Installation

```bash
cd frontend
npm install
```

## Development Server

```bash
npm start
```

Navigate to `http://localhost:4200/`. The application will automatically reload if you change any source files.

## Build for Production

```bash
npm run build
```

The build artifacts will be stored in the `dist/avatar-navigator/` directory.

## Running Tests

```bash
npm test
```

## Code Linting

```bash
npm run lint
```

## Project Structure Details

### Components

Each component follows Angular best practices with standalone pattern:

- **DashboardComponent** - Statistics and metrics display
- **OrdersComponent** - Order listing and search
- **ItemsComponent** - Item catalog with filters
- **AvatarComponent** - Voice interaction interface

### Services

Services handle all HTTP communication and business logic:

- **OrderService** - Order CRUD operations
- **ItemService** - Item management and filtering
- **AvatarService** - Avatar API integration

### Models

TypeScript interfaces defined in `src/app/models/index.ts`:

```typescript
interface Order { ... }
interface Item { ... }
interface OrderItem { ... }
interface ItemFilter { ... }
interface FilterCriteria { ... }
```

## API Integration

Services communicate with backend API at:
- Development: `http://localhost:5000/api/`
- Production: Configure in environment files

## Routing

Routes configured in `app.routes.ts`:

```
/ → DashboardComponent
/orders → OrdersComponent
/items → ItemsComponent
/avatar → AvatarComponent
```

## Styling

- **BEM methodology** for CSS class naming
- **CSS custom properties** for theming
- **Responsive design** with flexbox and CSS Grid
- **Mobile-first** approach

### Color Scheme

- Primary: `#0366d6` (Blue)
- Secondary: `#6f42c1` (Purple)
- Success: `#28a745` (Green)
- Danger: `#dc3545` (Red)
- Warning: `#ffc107` (Yellow)
- Info: `#17a2b8` (Cyan)

## Environment Configuration

Create `src/environments/environment.ts`:

```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000/api'
};
```

## Performance Optimization

- Lazy loading of routes
- OnPush change detection strategy
- CSS optimization
- Image lazy loading
- Bundle size optimization

## Browser Support

- Chrome (latest)
- Firefox (latest)
- Safari (latest)
- Edge (latest)

## Common Tasks

### Add New Component
```bash
ng generate component components/component-name
```

### Add New Service
```bash
ng generate service services/service-name
```

### Generate Module
```bash
ng generate module modules/module-name
```

## Troubleshooting

### Port 4200 already in use
```bash
ng serve --port 4300
```

### Clear Angular cache
```bash
ng cache clean
```

### Module not found
```bash
rm -rf node_modules package-lock.json
npm install
```

## Deployment

### Docker
```bash
docker build -t avatarnavigator-frontend .
docker run -p 80:80 avatarnavigator-frontend
```

### Static Hosting
Upload contents of `dist/avatar-navigator/` to your hosting provider.

## Documentation

- [Angular Documentation](https://angular.io)
- [TypeScript Handbook](https://www.typescriptlang.org)
- [RxJS Guide](https://rxjs.dev)

## Contributing

1. Create a feature branch
2. Make your changes
3. Test thoroughly
4. Submit a pull request

## License

This project is licensed under the MIT License.
