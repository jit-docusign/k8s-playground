#!/bin/bash

set -e

echo "======================================"
echo "Cleanup Script"
echo "======================================"
echo ""

echo "🗑️  Deleting all resources in sample-dotnet-app namespace..."
kubectl delete -f deployment-order-api.yaml --ignore-not-found=true
kubectl delete -f service-order-api.yaml --ignore-not-found=true
kubectl delete -f deployment-product-api.yaml --ignore-not-found=true
kubectl delete -f service-product-api.yaml --ignore-not-found=true

echo ""
echo "🗑️  Deleting namespace..."
kubectl delete -f namespace.yaml --ignore-not-found=true

echo ""
echo "✅ Cleanup complete!"
