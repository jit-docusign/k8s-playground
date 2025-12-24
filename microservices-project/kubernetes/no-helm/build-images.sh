#!/bin/bash

set -e

echo "======================================"
echo "Building Docker Images"
echo "======================================"
echo ""

# Navigate to project root
cd ../..

echo "Building ProductApi Docker image..."
docker build -f ProductApi/Dockerfile -t product-api:1.0.0 .

echo ""
echo "Building OrderApi Docker image..."
docker build -f OrderApi/Dockerfile -t order-api:1.0.0 .

echo ""
echo "======================================"
echo "✅ Docker images built successfully!"
echo "======================================"
echo ""
echo "Loading images into KIND cluster..."
kind load docker-image product-api:1.0.0 --name k8s-dev-cluster
kind load docker-image order-api:1.0.0 --name k8s-dev-cluster

echo ""
echo "✅ Images loaded into KIND cluster"
echo ""
echo "To verify images in cluster:"
echo "  docker exec -it k8s-dev-cluster-control-plane crictl images | grep -E 'product-api|order-api'"
