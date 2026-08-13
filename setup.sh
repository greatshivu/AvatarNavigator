#!/bin/bash

# AvatarNavigator Development Setup Script

echo "🚀 AvatarNavigator Setup Script"
echo "================================"
echo ""

# Colors for output
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
NC='\033[0m' # No Color

# Check for Docker
if command -v docker &> /dev/null; then
    echo -e "${GREEN}✓ Docker is installed${NC}"
    DOCKER_VERSION=$(docker --version)
    echo "  $DOCKER_VERSION"
else
    echo -e "${YELLOW}✗ Docker is not installed${NC}"
    echo "  Please install Docker from https://www.docker.com/products/docker-desktop"
fi

echo ""

# Check for .NET SDK
if command -v dotnet &> /dev/null; then
    echo -e "${GREEN}✓ .NET SDK is installed${NC}"
    DOTNET_VERSION=$(dotnet --version)
    echo "  Version: $DOTNET_VERSION"
else
    echo -e "${YELLOW}✗ .NET SDK is not installed${NC}"
    echo "  Please install from https://dotnet.microsoft.com/download"
fi

echo ""

# Check for Node.js
if command -v node &> /dev/null; then
    echo -e "${GREEN}✓ Node.js is installed${NC}"
    NODE_VERSION=$(node --version)
    echo "  Version: $NODE_VERSION"
else
    echo -e "${YELLOW}✗ Node.js is not installed${NC}"
    echo "  Please install from https://nodejs.org/"
fi

echo ""

# Check for npm
if command -v npm &> /dev/null; then
    echo -e "${GREEN}✓ npm is installed${NC}"
    NPM_VERSION=$(npm --version)
    echo "  Version: $NPM_VERSION"
else
    echo -e "${YELLOW}✗ npm is not installed${NC}"
    echo "  Please install Node.js which includes npm"
fi

echo ""
echo "================================"
echo "Setup Instructions:"
echo "================================"
echo ""
echo "Option 1: Using Docker Compose (Recommended)"
echo "  docker-compose up --build"
echo ""
echo "Option 2: Manual Setup"
echo ""
echo "  Backend:"
echo "    cd backend"
echo "    dotnet restore"
echo "    dotnet ef database update"
echo "    dotnet run"
echo ""
echo "  Frontend (in another terminal):"
echo "    cd frontend"
echo "    npm install"
echo "    npm start"
echo ""
echo "After startup:"
echo "  - Frontend: http://localhost:4200"
echo "  - Backend: http://localhost:5000"
echo "  - API Docs: http://localhost:5000/swagger"
echo ""
echo "================================"
