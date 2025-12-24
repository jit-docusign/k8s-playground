# Kubernetes Manifests

This directory contains all Kubernetes manifests for deploying the sample-dotnet-app.

## Files

- `namespace.yaml` - Creates the `sample-dotnet-app` namespace
- `deployment-product-api.yaml` - ProductApi deployment configuration
- `service-product-api.yaml` - ProductApi service (ClusterIP)
- `deployment-order-api.yaml` - OrderApi deployment configuration
- `service-order-api.yaml` - OrderApi service (ClusterIP)
- `deploy.sh` - Automated deployment script
- `cleanup.sh` - Cleanup script to remove all resources

## Prerequisites

Before deploying, ensure you have:

1. **Kubernetes cluster running**
   ```bash
   kubectl cluster-info
   kubectl get nodes
   ```

2. **Docker images built and loaded into KIND**
   ```bash
   cd ..
   ./build-images.sh
   kind load docker-image product-api:1.0.0 --name k8s-dev-cluster
   kind load docker-image order-api:1.0.0 --name k8s-dev-cluster
   ```

## Quick Deploy

Use the automated script:

```bash
chmod +x deploy.sh cleanup.sh
./deploy.sh
```

## Manual Deployment

Step-by-step deployment:

```bash
# 1. Create namespace
kubectl apply -f namespace.yaml

# 2. Deploy ProductApi
kubectl apply -f deployment-product-api.yaml
kubectl apply -f service-product-api.yaml

# 3. Deploy OrderApi
kubectl apply -f deployment-order-api.yaml
kubectl apply -f service-order-api.yaml

# 4. Verify deployment
kubectl get all -n sample-dotnet-app

# 5. Wait for pods to be ready
kubectl wait --for=condition=ready pod -l app=product-api -n sample-dotnet-app --timeout=300s
kubectl wait --for=condition=ready pod -l app=order-api -n sample-dotnet-app --timeout=300s
```

## Verify Deployment

```bash
# Check all resources
kubectl get all -n sample-dotnet-app

# Check pod status
kubectl get pods -n sample-dotnet-app

# Check services
kubectl get svc -n sample-dotnet-app

# View pod logs
kubectl logs -f deployment/product-api -n sample-dotnet-app
kubectl logs -f deployment/order-api -n sample-dotnet-app

# Describe deployment
kubectl describe deployment product-api -n sample-dotnet-app
kubectl describe deployment order-api -n sample-dotnet-app
```

## Testing the APIs

### Port Forward to Access Services

```bash
# ProductApi
kubectl port-forward svc/product-api 8080:80 -n sample-dotnet-app

# In another terminal - OrderApi
kubectl port-forward svc/order-api 8081:80 -n sample-dotnet-app
```

### Test Commands

```bash
# Test ProductApi
curl http://localhost:8080/api/product
curl http://localhost:8080/api/product/1
curl http://localhost:8080/api/product/health

# Test OrderApi (calls ProductApi internally)
curl -X POST http://localhost:8081/api/order \
  -H "Content-Type: application/json" \
  -d '{"productId": 1, "quantity": 2}'

curl http://localhost:8081/api/order
curl http://localhost:8081/api/order/health
```

## Scaling

```bash
# Scale ProductApi to 5 replicas
kubectl scale deployment product-api --replicas=5 -n sample-dotnet-app

# Scale OrderApi to 3 replicas
kubectl scale deployment order-api --replicas=3 -n sample-dotnet-app

# Verify scaling
kubectl get pods -n sample-dotnet-app
```

## Rolling Updates

```bash
# Update ProductApi image
kubectl set image deployment/product-api api=product-api:2.0.0 -n sample-dotnet-app

# Watch rollout status
kubectl rollout status deployment/product-api -n sample-dotnet-app

# View rollout history
kubectl rollout history deployment/product-api -n sample-dotnet-app

# Rollback if needed
kubectl rollout undo deployment/product-api -n sample-dotnet-app
```

## Cleanup

Remove all deployed resources:

```bash
./cleanup.sh
```

Or manually:

```bash
kubectl delete namespace sample-dotnet-app
```

## Resource Configuration

### ProductApi Deployment
- **Replicas**: 2
- **CPU Request**: 100m
- **CPU Limit**: 500m
- **Memory Request**: 256Mi
- **Memory Limit**: 512Mi
- **Health Check**: `/api/product/health` on port 8080

### OrderApi Deployment
- **Replicas**: 2
- **CPU Request**: 100m
- **CPU Limit**: 500m
- **Memory Request**: 256Mi
- **Memory Limit**: 512Mi
- **Health Check**: `/api/order/health` on port 8081
- **Environment**: `PRODUCT_API_URL=http://product-api.sample-dotnet-app.svc.cluster.local`

## Troubleshooting

### Pods not starting

```bash
# Check pod status
kubectl get pods -n sample-dotnet-app

# Describe pod for events
kubectl describe pod <pod-name> -n sample-dotnet-app

# Check logs
kubectl logs <pod-name> -n sample-dotnet-app

# Check if images are loaded in KIND
docker exec -it k8s-dev-cluster-control-plane crictl images
```

### ImagePullBackOff error

This means the image is not available in the cluster:

```bash
# Load images into KIND
kind load docker-image product-api:1.0.0 --name k8s-dev-cluster
kind load docker-image order-api:1.0.0 --name k8s-dev-cluster
```

### Service not accessible

```bash
# Check service endpoints
kubectl get endpoints -n sample-dotnet-app

# Check if pods are ready
kubectl get pods -n sample-dotnet-app

# Test service from within cluster
kubectl run -it --rm debug --image=curlimages/curl --restart=Never -n sample-dotnet-app -- \
  curl http://product-api/api/product/health
```

## Using k9s

For a better terminal UI experience:

```bash
# Install k9s
brew install k9s

# Launch k9s in sample-dotnet-app namespace
k9s -n sample-dotnet-app

# Keyboard shortcuts:
# :pods - View pods
# :svc - View services
# :deploy - View deployments
# l - View logs
# d - Describe resource
# s - Shell into pod
```
