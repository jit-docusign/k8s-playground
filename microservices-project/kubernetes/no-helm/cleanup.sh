#!/bin/bash

set -e

echo "======================================"
echo "Cleanup Script"
echo "======================================"
echo ""

echo "🗑️  Deleting all resources in microservices namespace..."
kubectl delete -f kubernetes/deployment-order-api.yaml --ignore-not-found=true
kubectl delete -f kubernetes/service-order-api.yaml --ignore-not-found=true
kubectl delete -f kubernetes/deployment-product-api.yaml --ignore-not-found=true
kubectl delete -f kubernetes/service-product-api.yaml --ignore-not-found=true

echo ""
echo "🗑️  Deleting namespace..."
kubectl delete -f kubernetes/namespace.yaml --ignore-not-found=true

echo ""
echo "✅ Cleanup complete!"
