#!/bin/bash

set -e

echo "======================================"
echo "Kubernetes Deployment Script"
echo "======================================"
echo ""

# Check if kubectl is available
if ! command -v kubectl &> /dev/null; then
    echo "❌ kubectl not found. Please install kubectl first."
    exit 1
fi

# Check if cluster is running
if ! kubectl cluster-info &> /dev/null; then
    echo "❌ Kubernetes cluster not accessible. Please start your cluster first."
    exit 1
fi

echo "✅ kubectl found"
echo "✅ Cluster accessible"
echo ""

# Build and load Docker images
echo "🔨 Building Docker images..."
bash build-images.sh

echo ""
# Create namespace
echo "📦 Creating namespace..."
kubectl apply -f namespace.yaml

echo ""
echo "🔐 Creating Secrets..."
kubectl apply -f secret-postgres.yaml
kubectl apply -f secret-api-auth.yaml

echo ""
echo "⚙️  Creating ConfigMaps..."
kubectl apply -f configmap-product-api.yaml
kubectl apply -f configmap-order-api.yaml

echo ""
echo "🗄️  Deploying PostgreSQL..."
kubectl apply -f statefulset-postgres.yaml
kubectl apply -f service-postgres.yaml

echo ""
echo "⏳ Waiting for PostgreSQL to be ready..."
kubectl wait --for=condition=ready pod -l app=postgres -n microservices --timeout=120s

echo ""
echo "🚀 Deploying ProductApi..."
kubectl apply -f deployment-product-api.yaml
kubectl apply -f service-product-api.yaml

echo ""
echo "🚀 Deploying OrderApi..."
kubectl apply -f deployment-order-api.yaml
kubectl apply -f service-order-api.yaml

echo ""
echo "⏳ Waiting for deployments to be ready..."
kubectl wait --for=condition=available --timeout=300s deployment/product-api -n microservices
kubectl wait --for=condition=available --timeout=300s deployment/order-api -n microservices

echo ""
echo "======================================"
echo "✅ Deployment Complete!"
echo "======================================"
echo ""
echo "To view the deployed resources:"
echo "  kubectl get all -n microservices"
echo ""
echo "To view pod logs:"
echo "  kubectl logs -f deployment/product-api -n microservices"
echo "  kubectl logs -f deployment/order-api -n microservices"
echo ""
echo "To access ProductApi:"
echo "  kubectl port-forward svc/product-api 8080:80 -n microservices"
echo "  curl http://localhost:8080/api/products"
echo ""
echo "To access OrderApi:"
echo "  kubectl port-forward svc/order-api 8081:80 -n microservices"
echo "  curl http://localhost:8081/api/orders"
echo ""
