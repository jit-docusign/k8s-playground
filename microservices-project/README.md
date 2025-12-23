# Microservices Project

This project contains two .NET 8 microservices for the Kubernetes Masterclass:

## Services

### ProductApi (Port 8080)
- **Purpose**: Product catalog management
- **Endpoints**:
  - `GET /api/product` - Get all products
  - `GET /api/product/{id}` - Get product by ID
  - `POST /api/product` - Create new product
  - `GET /api/product/health` - Health check

### OrderApi (Port 8081)
- **Purpose**: Order management (calls ProductApi)
- **Endpoints**:
  - `GET /api/order` - Get all orders
  - `GET /api/order/{id}` - Get order by ID
  - `POST /api/order` - Create new order (calls ProductApi)
  - `GET /api/order/health` - Health check

## Running Locally

### Option 1: Using Docker Compose (Recommended)

```bash
# Build and start both services
docker-compose up --build

# Or run in detached mode
docker-compose up -d --build

# View logs
docker-compose logs -f

# Stop services
docker-compose down
```

### Option 2: Using .NET CLI

#### ProductApi
```bash
cd ProductApi
dotnet run
# Or with custom port:
PORT=8080 dotnet run
```

#### OrderApi
```bash
cd OrderApi
export PRODUCT_API_URL=http://localhost:8080
dotnet run
# Or with custom port:
PORT=8081 dotnet run
```

## Building Docker Images
the APIs

Once the services are running (via Docker Compose or .NET CLI):

```bash
# Test ProductApi - Get all products
curl http://localhost:8080/api/product

# Test ProductApi - Get product by ID
curl http://localhost:8080/api/product/1

# Test ProductApi - Create a new product
curl -X POST http://localhost:8080/api/product \
  -H "Content-Type: application/json" \
  -d '{"name": "Monitor", "price": 299.99, "description": "4K Monitor"}'

# Test ProductApi - Health check
curl http://localhost:8080/api/product/health

# Test OrderApi - Get all orders
curl http://localhost:8081/api/order

# Test OrderApi - Create order (calls ProductApi internally)
curl -X POST http://localhost:8081/api/order \
  -H "Content-Type: application/json" \
  -d '{"productId": 1, "quantity": 2}'

# Test OrderApi - Health check
curl http://localhost:8081/api/order/health
```

## Testing Locally with Docker (Alternative)

```bash
# Run ProductApi
docker run -p 8080:8080 product-api:1.0.0

# Run OrderApi (in another terminal)
docker run -p 8081:8081 -e PRODUCT_API_URL=http://host.docker.internal:8080 order-api:1.0.0 --name k8s-dev-cluster
```

## Testing Locally with Docker

```bash
# Run ProductApi
docker run -p 8080:8080 product-api:1.0.0

# Run OrderApi (in another terminal)
docker run -p 8081:8081 -e PRODUCT_API_URL=http://host.docker.internal:8080 order-api:1.0.0

# Test ProductApi
curl http://localhost:8080/api/product

# Test OrderApi (creates order for product ID 1)
curl -X POST http://localhost:8081/api/order \
  -H "Content-Type: application/json" \
├── docker-compose.yml
├── build-images.sh
├── .gitignore
  -d '{"productId": 1, "quantity": 2}'
```

## Project Structure

```
microservices-project/
├── ProductApi/
│   ├── Controllers/
│   │   └── ProductController.cs
│   ├── Services/
│   │   └── ProductService.cs
│   ├── Program.cs
│   ├── ProductApi.csproj
│   └── Dockerfile
├── OrderApi/
│   ├── Controllers/
│   │   └── OrderController.cs
│   ├── Services/
│   │   └── OrderService.cs
│   ├── Program.cs
│   ├── OrderApi.csproj
│   └── Dockerfile
└── README.md
```

## Technologies

- .NET 8
- ASP.NET Core Web API
- Docker
- Kubernetes
