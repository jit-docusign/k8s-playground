#!/bin/bash

echo "Building ProductApi Docker image..."
docker build -f ProductApi/Dockerfile -t product-api:1.0.0 .

echo ""
echo "Building OrderApi Docker image..."
docker build -f OrderApi/Dockerfile -t order-api:1.0.0 .

echo ""
echo "Docker images built successfully!"
echo ""
echo "To load into KIND cluster, run:"
echo "  kind load docker-image product-api:1.0.0 --name k8s-dev-cluster"
echo "  kind load docker-image order-api:1.0.0 --name k8s-dev-cluster"
