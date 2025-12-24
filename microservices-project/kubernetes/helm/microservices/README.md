# Microservices Helm Chart

This Helm chart deploys the ProductApi and OrderApi microservices to Kubernetes.

## Prerequisites

- Kubernetes cluster (KIND, minikube, or cloud provider)
- Helm 3.12+
- Docker images built and loaded into the cluster

## Installation

### 1. Build and Load Docker Images

```bash
# From the microservices-project directory
./build-images.sh

# Load images into KIND cluster
kind load docker-image product-api:1.0.0 --name k8s-dev-cluster
kind load docker-image order-api:1.0.0 --name k8s-dev-cluster
```

### 2. Install the Chart

```bash
# Install with default values
helm install microservices ./helm/microservices

# Install with custom values
helm install microservices ./helm/microservices -f custom-values.yaml

# Install to a specific namespace
helm install microservices ./helm/microservices --create-namespace --namespace my-namespace
```

### 3. Verify Deployment

```bash
# Check Helm release status
helm status microservices

# List all resources
kubectl get all -n microservices

# Check pod logs
kubectl logs -n microservices -l app=product-api
kubectl logs -n microservices -l app=order-api
```

## Configuration

The following table lists the configurable parameters and their default values:

| Parameter | Description | Default |
|-----------|-------------|---------|
| `namespace.name` | Namespace name | `microservices` |
| `productApi.replicaCount` | Number of ProductApi replicas | `2` |
| `productApi.image.repository` | ProductApi image repository | `product-api` |
| `productApi.image.tag` | ProductApi image tag | `1.0.0` |
| `productApi.service.port` | ProductApi service port | `80` |
| `orderApi.replicaCount` | Number of OrderApi replicas | `2` |
| `orderApi.image.repository` | OrderApi image repository | `order-api` |
| `orderApi.image.tag` | OrderApi image tag | `1.0.0` |
| `orderApi.service.port` | OrderApi service port | `80` |

## Customization

Create a `custom-values.yaml` file to override default values:

```yaml
productApi:
  replicaCount: 3
  resources:
    limits:
      memory: "1Gi"
      cpu: "1000m"

orderApi:
  replicaCount: 3
  env:
    productApiUrl: "http://product-api.microservices.svc.cluster.local"
```

Then install with:
```bash
helm install microservices ./helm/microservices -f custom-values.yaml
```

## Upgrading

```bash
# Upgrade with new values
helm upgrade microservices ./helm/microservices

# Upgrade with custom values file
helm upgrade microservices ./helm/microservices -f custom-values.yaml
```

## Uninstallation

```bash
# Uninstall the release
helm uninstall microservices

# Uninstall and delete the namespace
helm uninstall microservices
kubectl delete namespace microservices
```

## Testing

```bash
# Port forward to ProductApi
kubectl port-forward -n microservices svc/product-api 8080:80

# Port forward to OrderApi
kubectl port-forward -n microservices svc/order-api 8081:80

# Test ProductApi
curl http://localhost:8080/api/products
curl http://localhost:8080/api/products/health

# Test OrderApi
curl http://localhost:8081/api/orders
curl http://localhost:8081/api/orders/health
curl -X POST http://localhost:8081/api/orders -H "Content-Type: application/json" -d '{"productId":1,"quantity":2}'
```

## Chart Structure

```
helm/microservices/
├── Chart.yaml                           # Chart metadata
├── values.yaml                          # Default configuration values
├── .helmignore                          # Files to ignore when packaging
├── README.md                            # This file
└── templates/                           # Kubernetes manifest templates
    ├── namespace.yaml                   # Namespace definition
    ├── product-api-deployment.yaml      # ProductApi deployment
    ├── product-api-service.yaml         # ProductApi service
    ├── order-api-deployment.yaml        # OrderApi deployment
    └── order-api-service.yaml           # OrderApi service
```

## Troubleshooting

### Images not found
```bash
# Check if images are loaded in KIND
docker exec -it k8s-dev-cluster-control-plane crictl images | grep api

# Reload images if needed
kind load docker-image product-api:1.0.0 order-api:1.0.0 --name k8s-dev-cluster
```

### Pods not starting
```bash
# Describe pods to see events
kubectl describe pod -n microservices -l app=product-api
kubectl describe pod -n microservices -l app=order-api

# Check pod logs
kubectl logs -n microservices -l app=product-api
kubectl logs -n microservices -l app=order-api
```

### Service communication issues
```bash
# Test DNS resolution from OrderApi pod
kubectl exec -n microservices -it $(kubectl get pod -n microservices -l app=order-api -o jsonpath='{.items[0].metadata.name}') -- nslookup product-api

# Test connectivity
kubectl exec -n microservices -it $(kubectl get pod -n microservices -l app=order-api -o jsonpath='{.items[0].metadata.name}') -- curl http://product-api/api/products/health
```
