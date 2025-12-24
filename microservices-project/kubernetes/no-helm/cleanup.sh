#!/bin/bash

set -e

echo "======================================"
echo "Cleanup Script"
echo "======================================"
echo ""

echo "🗑️  Deleting all resources in microservices namespace..."
kubectl delete -f deployment-order-api.yaml --ignore-not-found=true
kubectl delete -f service-order-api.yaml --ignore-not-found=true
kubectl delete -f deployment-product-api.yaml --ignore-not-found=true
kubectl delete -f service-product-api.yaml --ignore-not-found=true
kubectl delete -f statefulset-postgres.yaml --ignore-not-found=true
kubectl delete -f service-postgres.yaml --ignore-not-found=true
kubectl delete -f configmap-order-api.yaml --ignore-not-found=true
kubectl delete -f configmap-product-api.yaml --ignore-not-found=true
kubectl delete -f secret-api-auth.yaml --ignore-not-found=true
kubectl delete -f secret-postgres.yaml --ignore-not-found=true

echo ""
echo "🗑️  Deleting PVCs..."
kubectl delete pvc -n microservices --all --ignore-not-found=true

echo ""
echo "🗑️  Deleting namespace..."
kubectl delete -f namespace.yaml --ignore-not-found=true

echo ""
echo "✅ Cleanup complete!"
