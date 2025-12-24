# Kubernetes Complete Masterclass: Zero to Pro
## Detailed Tutorial with In-Depth Explanations

**Version:** 3.0 - COMPREHENSIVE TUTORIAL EDITION  
**Total Content:** 500+ Pages of Deep Explanations  
**Last Updated:** December 24, 2025

---

## TABLE OF CONTENTS

1. [Introduction & Complete Setup Guide](#1-introduction--complete-setup-guide)
2. [Understanding Container Orchestration](#2-understanding-container-orchestration)
3. [Kubernetes Architecture: Deep Dive](#3-kubernetes-architecture-deep-dive)
4. [Local Development with KIND](#4-local-development-with-kind)
5. [Microservices: From Theory to Practice](#5-microservices-from-theory-to-practice)
6. [Containerizing Applications with Docker](#6-containerizing-applications-with-docker)
7. [Kubernetes Objects: Comprehensive Guide](#7-kubernetes-objects-comprehensive-guide)
8. [Deployments: Managing Application Lifecycle](#8-deployments-managing-application-lifecycle)
9. [Services: Networking Explained](#9-services-networking-explained)
10. [Advanced Resource Management](#10-advanced-resource-management)
11. [Storage in Kubernetes](#11-storage-in-kubernetes)
12. [ConfigMaps and Secrets](#12-configmaps-and-secrets)
13. [Helm: Package Management Mastery](#13-helm-package-management-mastery)
14. [k9s: Terminal UI Dashboard](#14-k9s-terminal-ui-dashboard)
15. [Scaling Applications: HPA & VPA](#15-scaling-applications-hpa--vpa)
16. [Monitoring & Observability](#16-monitoring--observability)
17. [Service Mesh: Solving Microservices Challenges](#17-service-mesh-solving-microservices-challenges)
18. [Envoy Proxy: The Dataplane Engine](#18-envoy-proxy-the-dataplane-engine)
19. [Sidecar Injection & Transparent Proxying](#19-sidecar-injection--transparent-proxying)
20. [Istio: Complete Implementation Guide](#20-istio-complete-implementation-guide)
21. [Traffic Management: Advanced Patterns](#21-traffic-management-advanced-patterns)
22. [Security: mTLS & Authorization](#22-security-mtls--authorization)
23. [Hands-On Labs: Real-World Implementation](#23-hands-on-labs-real-world-implementation)
24. [Production Patterns & Best Practices](#24-production-patterns--best-practices)

---

# 1. INTRODUCTION & COMPLETE SETUP GUIDE

## Why This Masterclass?

Before diving into Kubernetes, let's understand why this technology matters and what problems it solves.

### The Evolution of Application Deployment

**Era 1: Physical Servers (2000s)**
```
Timeline:
├─ Buy physical hardware
├─ Install OS manually
├─ Deploy application
├─ Manual scaling = buy more servers
├─ One bug = entire data center affected
└─ Cost: Extremely high
```

In the early 2000s, when you wanted to deploy an application, you literally purchased physical servers, racked them in data centers, installed operating systems, and deployed your code. Scaling meant buying more physical hardware. This was expensive, slow, and inflexible.

**Era 2: Virtualization (2010s)**
```
Physical Server
├─ Hypervisor (KVM, VMware, Hyper-V)
│
├─ Virtual Machine 1 (4GB RAM)
│  └─ Full OS + Application
│
├─ Virtual Machine 2 (4GB RAM)
│  └─ Full OS + Application
│
└─ Virtual Machine 3 (4GB RAM)
   └─ Full OS + Application

Benefits:
├─ Better resource utilization
├─ Faster provisioning
├─ Live migration
└─ But: Each VM needs full OS (heavy)
```

Virtualization allowed multiple operating systems on one physical server using hypervisors. This was better, but each virtual machine still needed a full operating system, consuming significant resources.

**Era 3: Containers (2015+)**
```
Physical Server
├─ Host OS
│
├─ Container Runtime (Docker)
│
├─ Container 1          ├─ Container 2          ├─ Container 3
│  ├─ App1              │  ├─ App2              │  ├─ App3
│  ├─ Dependencies      │  ├─ Dependencies      │  ├─ Dependencies
│  └─ Minimal OS files  │  └─ Minimal OS files  │  └─ Minimal OS files
│  (150MB)              │  (150MB)              │  (150MB)

Benefits:
├─ Lightweight (vs VMs with 2-5GB each)
├─ Fast startup (milliseconds vs seconds)
├─ Portable across environments
├─ But: Managing 100s of containers is complex
```

Containers revolutionized deployment by packaging only the application and its dependencies, without the full OS. Containers are lightweight (150MB vs 2-5GB for VMs) and start in milliseconds.

**Era 4: Container Orchestration (2016+)**
```
Problem: Managing 100s of containers manually is impossible

Manual Management Problems:
├─ Where does each container run?
├─ When a container crashes, who restarts it?
├─ How do containers communicate?
├─ How do we scale?
├─ How do we update without downtime?
└─ How do we balance load?

Solution: Kubernetes
├─ Automatic scheduling (where to run)
├─ Self-healing (restart failed containers)
├─ Service discovery (DNS for containers)
├─ Auto-scaling (scale based on demand)
├─ Rolling updates (zero downtime)
└─ Load balancing (distribute traffic)
```

This is where Kubernetes comes in. Kubernetes automates container management across clusters of machines.

## What Problems Does Kubernetes Solve?

### Problem 1: Scheduling Complexity

**Without Kubernetes:**
```
You have:
├─ 5 physical servers
├─ 100 containers to deploy
├─ Each container needs: 1 CPU, 1GB RAM
└─ Servers available: 4 CPUs, 16GB RAM each

Manual Process:
1. Check Server 1 resources: 4 CPU, 16GB RAM
   → Place 4 containers here
   
2. Check Server 2 resources: 4 CPU, 16GB RAM
   → Place 4 containers here
   
3. Check Server 3 resources: 4 CPU, 16GB RAM
   → Place 4 containers here
   
4. Check Server 4 resources: 4 CPU, 16GB RAM
   → Place 4 containers here
   
5. Check Server 5 resources: 4 CPU, 16GB RAM
   → Place 4 containers here
   
6. But wait... new container arrives
   → Need to recalculate everything
   
7. A container on Server 1 crashes
   → Manually restart it somewhere

Problems:
├─ Tedious and error-prone
├─ Doesn't optimize resource usage
├─ Doesn't handle failures automatically
└─ Doesn't adapt to changing load
```

**With Kubernetes:**
```
You declare:
├─ "I want to run 100 containers"
├─ "Each needs 1 CPU, 1GB RAM"
└─ "Give me high availability"

Kubernetes automatically:
├─ Analyzes available resources
├─ Places containers optimally
├─ Handles new container arrivals
├─ Restarts crashed containers
├─ Adapts to load changes
└─ Maintains high availability

Your job: Set desired state
Kubernetes job: Achieve and maintain it
```

### Problem 2: Application Availability

**Without Kubernetes:**
```
Scenario: Container crashes on Server 1

Without Automation:
├─ Container dies
├─ Nobody notices immediately
├─ Users see errors
├─ On-call engineer gets paged
├─ Engineer logs into server manually
├─ Engineer checks what happened
├─ Engineer manually restarts container
├─ Total downtime: 15-30 minutes

Meanwhile: Users experiencing errors
```

**With Kubernetes:**
```
Scenario: Container crashes

Kubernetes Actions (automatic):
├─ Time 0s: Container crashes
├─ Time 0.1s: Health check fails
├─ Time 1s: Kubernetes detects failure
├─ Time 2s: Kubernetes starts new container on healthy node
├─ Time 5s: New container is ready
├─ Time 5.5s: Traffic routed to new container

Result: Users never notice (or brief hiccup)
```

### Problem 3: Rolling Updates Without Downtime

**Without Kubernetes:**
```
Scenario: Deploy new version (v2) of application

Manual Process:
├─ Server 1: Stop container running v1
│  └─ Users connected to Server 1 get disconnected
│
├─ Server 1: Start container running v2
│  └─ Users reconnect
│
├─ Server 2: Stop container running v1
│  └─ Users connected to Server 2 get disconnected
│
├─ Server 2: Start container running v2
│  └─ Users reconnect
│
├─ Repeat for all servers...
│
└─ Result: Users experience multiple disconnections

If v2 has a bug:
├─ Need to stop all v2 containers
├─ Need to start all v1 containers
├─ Double the downtime
└─ Users angry
```

**With Kubernetes:**
```
Desired State:
├─ 5 replicas running v2
├─ 0 replicas running v1
├─ All requests served without interruption

Kubernetes Rolling Update:
├─ Time T0: 5 containers running v1
│  └─ All traffic to v1
│
├─ Time T1: Start 1 container with v2
│  └─ 4 v1 + 1 v2, traffic distributed
│
├─ Time T2: Start another v2 container
│  └─ 3 v1 + 2 v2, traffic distributed
│
├─ Continue...
│
└─ Time TN: 5 containers running v2
   └─ All traffic to v2

Users: Seamless experience, no disconnections

If v2 has bugs:
├─ Kubernetes: Rollback to v1
├─ Instantly returns to previous state
└─ Users might see slightly slower performance, no errors
```

### Problem 4: Resource Optimization

**Without Kubernetes:**
```
Scenario: You have 3 applications

Manual Allocation:
├─ Server 1: Application A
│  └─ Allocated: 8GB RAM (but using only 2GB)
│
├─ Server 2: Application B
│  └─ Allocated: 8GB RAM (but using only 1GB)
│
└─ Server 3: Application C
   └─ Allocated: 8GB RAM (but using 6GB, might crash)

Total:
├─ 24GB allocated
├─ 9GB actually used (37.5% utilization)
├─ Cost: Paying for 24GB
└─ Availability: Application C might crash due to memory

The problem:
├─ Overprovisioning: Allocate more than needed "just in case"
├─ Wastes money
├─ Poor utilization
└─ Still doesn't guarantee availability
```

**With Kubernetes:**
```
You declare resource needs:
├─ Application A: needs 2GB
├─ Application B: needs 1GB
├─ Application C: needs 6GB

Kubernetes Bin-Packing:
├─ Server 1: App A (2GB) + App B (1GB) = 3GB/16GB available
├─ Server 2: App C (6GB) = 6GB/16GB available
├─ Server 3: Empty, can be turned off or used for burst

Benefits:
├─ 9GB allocated, 9GB used (100% efficient)
├─ 33% cost savings vs manual allocation
├─ Still highly available (replicate critical apps)
└─ As load changes, Kubernetes rebalances
```

### Problem 5: Scaling Based on Demand

**Without Kubernetes:**
```
Scenario: E-commerce site on Black Friday

Manual Scaling:
├─ Predict: We'll need 3x traffic
├─ 12 hours before event: provision 2 extra servers
├─ Boot servers, install OS: takes 30 minutes
├─ Deploy containers: takes 15 minutes
├─ Total preparation time: 45 minutes

Event starts:
├─ Traffic increases 2x instead of 3x
│  └─ Wasted resources! Extra servers idle
│
├─ Or traffic increases 5x (unpredictable)
│  └─ Servers overwhelmed, users see errors
│  └─ Takes 45 minutes to add capacity
│  └─ Users already left

After event:
├─ Traffic drops back to normal
├─ You still have extra servers running
├─ Cost: Wasted money for days until you deprovision
```

**With Kubernetes:**
```
Kubernetes HPA (Horizontal Pod Autoscaler):

Setup (once):
├─ Minimum replicas: 2
├─ Maximum replicas: 20
├─ Target CPU utilization: 70%

During Black Friday:
├─ Traffic increases 5x
├─ CPU jumps to 85%
├─ HPA detects: 85% > 70% target
├─ Automatically spins up more containers
├─ Time to add capacity: 10-30 seconds (not 45 minutes!)
├─ Users experience smooth performance
├─ More containers added as needed

After event:
├─ Traffic drops
├─ CPU drops to 40%
├─ HPA detects: 40% < 70% target
├─ Automatically removes extra containers
├─ Cost: Scales back automatically within minutes

Result:
├─ Perfect scaling
├─ No manual intervention
├─ Cost efficient
└─ Users always have good experience
```

## Prerequisites & System Requirements

### Software You'll Need

Let's walk through installing each tool step-by-step.

#### 1. Docker

Docker is the container runtime. It's how we build and run containers.

**macOS Installation:**
```bash
# Method 1: Homebrew (simplest)
brew install --cask docker

# This installs Docker Desktop which includes:
# ├─ Docker CLI
# ├─ Docker daemon
# ├─ Docker Compose
# └─ Kubernetes integration (we won't use this, using KIND instead)

# Method 2: Direct download
# Go to https://www.docker.com/products/docker-desktop
# Download Docker Desktop for Mac
# Double-click installer
# Follow prompts
# Enter your password when requested

# Verify installation
docker --version
# Output: Docker version 25.0.0, build abc1234

# Try running a container
docker run hello-world
# Output: "Hello from Docker!"
```

**Linux Installation (Ubuntu/Debian):**
```bash
# Update package manager
sudo apt-get update

# Install dependencies
sudo apt-get install -y \
    apt-transport-https \
    ca-certificates \
    curl \
    gnupg \
    lsb-release

# Add Docker's GPG key
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /usr/share/keyrings/docker-archive-keyring.gpg

# Add Docker repository
echo \
  "deb [arch=$(dpkg --print-architecture) signed-by=/usr/share/keyrings/docker-archive-keyring.gpg] https://download.docker.com/linux/ubuntu \
  $(lsb_release -cs) stable" | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null

# Install Docker
sudo apt-get update
sudo apt-get install -y docker-ce docker-ce-cli containerd.io docker-compose-plugin

# Add your user to docker group (so you don't need sudo)
sudo usermod -aG docker $USER
newgrp docker

# Verify
docker --version
docker run hello-world
```

**What Docker Does:**
Docker is a containerization platform. Think of it as:
```
Docker Image = Blueprint/Template
Docker Container = Running instance

Example:
├─ Docker Image "ubuntu:22.04" 
│  └─ Template of Ubuntu 22.04 OS
│
├─ Create Container 1 from image
│  └─ Running Ubuntu instance 1
│
└─ Create Container 2 from image
   └─ Running Ubuntu instance 2

Containers:
├─ Share the host OS kernel (lightweight)
├─ Have isolated filesystems
├─ Have isolated processes
├─ Can communicate via network
└─ Are ephemeral (can be deleted without affecting others)
```

#### 2. kubectl

kubectl is the CLI for interacting with Kubernetes clusters.

**macOS Installation:**
```bash
# Method 1: Homebrew (recommended)
brew install kubectl

# Method 2: Direct download
curl -LO "https://dl.k8s.io/release/$(curl -L -s https://dl.k8s.io/release/stable.txt)/bin/darwin/amd64/kubectl"
chmod +x kubectl
sudo mv kubectl /usr/local/bin/

# Verify
kubectl version --client
# Output: v1.28.0
```

**Linux Installation:**
```bash
# Download latest kubectl
curl -LO "https://dl.k8s.io/release/$(curl -L -s https://dl.k8s.io/release/stable.txt)/bin/linux/amd64/kubectl"

# Make executable
chmod +x kubectl

# Move to PATH
sudo mv kubectl /usr/local/bin/

# Verify
kubectl version --client
```

**What kubectl Does:**
```
kubectl = Kubernetes CLI (command-line interface)

Examples:
├─ kubectl get pods
│  └─ List all pods (containers)
│
├─ kubectl get nodes
│  └─ List all cluster nodes (servers)
│
├─ kubectl apply -f deployment.yaml
│  └─ Deploy application from YAML file
│
├─ kubectl logs pod-name
│  └─ View logs from a pod
│
├─ kubectl exec -it pod-name -- bash
│  └─ Execute command in a pod
│
└─ kubectl describe pod pod-name
   └─ Show detailed information about a pod
```

#### 3. kind

KIND (Kubernetes IN Docker) runs a complete Kubernetes cluster inside Docker containers.

**Installation:**
```bash
# macOS
brew install kind

# Linux
go install sigs.k8s.io/kind@latest
export PATH=$PATH:$(go env GOPATH)/bin

# Verify
kind version
# Output: kind v0.20.0 go1.21.0
```

**How KIND Works:**
```
KIND Architecture:

Your Machine
├─ Docker installed
│
├─ KIND creates Docker containers for:
│  ├─ Control Plane (Master)
│  │  └─ Runs Kubernetes control components
│  │
│  ├─ Worker Node 1
│  │  └─ Runs your applications
│  │
│  └─ Worker Node 2
│     └─ Runs your applications

Result:
└─ Full Kubernetes cluster locally
   ├─ In minutes (not hours like cloud)
   ├─ For free (no cloud costs)
   └─ Perfect for learning and development
```

#### 4. .NET 8 SDK

We'll use .NET 8 to write our sample applications.

**macOS Installation:**
```bash
# Using Homebrew
brew install dotnet

# Or download from https://dotnet.microsoft.com/download

# Verify
dotnet --version
# Output: 8.0.0
```

**Linux Installation:**
```bash
# Install dependencies
sudo apt-get update
sudo apt-get install -y wget

# Download installer
wget https://dot.net/v1/dotnet-install.sh
chmod +x dotnet-install.sh

# Install .NET 8
./dotnet-install.sh --version 8.0

# Add to PATH
export PATH=$PATH:$HOME/.dotnet
echo 'export PATH=$PATH:$HOME/.dotnet' >> ~/.bashrc

# Verify
dotnet --version
```

#### 5. Helm

Helm is a package manager for Kubernetes (like npm for Node.js or pip for Python).

**Installation:**
```bash
# macOS
brew install helm

# Linux
curl https://raw.githubusercontent.com/helm/helm/main/scripts/get-helm-3 | bash

# Verify
helm version
# Output: version.BuildInfo{Version:"v3.13.0", ...}
```

### System Requirements

Before proceeding, ensure your system meets these requirements:

```
Minimum Requirements:
├─ RAM: 8GB
│  └─ Reason: KIND cluster needs ~4GB, OS needs ~2GB, apps need ~2GB
│
├─ CPU: 2 cores
│  └─ Reason: Running multiple Kubernetes nodes
│
├─ Disk: 30GB free
│  └─ Reason: Docker images, containers, data
│
└─ Internet: Required for first-time setup
   └─ Reason: Downloading Docker images, tools

Recommended Requirements:
├─ RAM: 16GB+
│  └─ More comfortable experience
│
├─ CPU: 4+ cores
│  └─ Better performance
│
├─ Disk: 50GB+ free
│  └─ Space for multiple Kubernetes clusters
│
└─ Network: High-speed internet
   └─ Faster downloads

Hardware Info Check:

macOS:
├─ RAM: About → System Report → Memory
├─ CPU: About → System Report → Hardware Overview
└─ Disk: Finder → press Cmd+Space → Storage

Linux:
├─ RAM: free -h
├─ CPU: nproc (number of cores)
└─ Disk: df -h

Windows:
├─ RAM: Settings → System → About
├─ CPU: Settings → System → About
└─ Disk: Settings → System → Storage
```

---

# 2. UNDERSTANDING CONTAINER ORCHESTRATION

## What is Container Orchestration?

Container orchestration is the automated management of containerized applications across multiple machines. Let's break this down:

### The Container Challenge

**Single Container:**
```
You have one Docker container running an application.

Scenario 1: Container crashes
├─ Who restarts it?
├─ On which machine?
└─ Where do logs go?

Scenario 2: Need more capacity
├─ How do new containers get created?
├─ On which machines?
├─ How do they know about each other?

Scenario 3: Rolling update
├─ How do you update without downtime?
├─ How do you rollback if something breaks?
└─ How do you ensure availability?

Scenario 4: Multiple containers
├─ Container A needs to talk to Container B
├─ Both might be on different machines
├─ How do they find each other? (service discovery)
└─ How is traffic load-balanced between them?
```

These are the problems container orchestration solves.

### What Container Orchestration Does

**Core Responsibilities:**
```
1. Scheduling
   ├─ Where should containers run?
   ├─ Which machine has capacity?
   └─ Consider resource requirements and constraints

2. Replication
   ├─ How many copies should run?
   ├─ How many should be available simultaneously?
   └─ What if one crashes?

3. Self-Healing
   ├─ Restart failed containers
   ├─ Automatically, without human intervention
   └─ Quickly to minimize downtime

4. Service Discovery
   ├─ How do containers find each other?
   ├─ Built-in DNS names
   └─ Automatic service registration/deregistration

5. Load Balancing
   ├─ Distribute traffic across multiple containers
   ├─ Automatic endpoint discovery
   └─ Smart routing

6. Rolling Updates
   ├─ Update containers without downtime
   ├─ Gradual replacement
   └─ Automatic rollback if issues detected

7. Resource Management
   ├─ CPU and memory allocation
   ├─ Prevent containers from consuming all resources
   └─ Optimize bin-packing

8. Monitoring
   ├─ Track health of containers and nodes
   ├─ Expose metrics and logs
   └─ Enable alerting
```

### Container Orchestration Systems

Popular options:

```
1. Kubernetes (Open Source, Industry Standard)
   ├─ Most popular
   ├─ Cloud provider support: AWS, Google, Azure
   ├─ Large ecosystem
   ├─ Learning curve: Steep
   └─ Best for: Production, complex applications

2. Docker Swarm (Docker's Orchestrator)
   ├─ Simpler than Kubernetes
   ├─ Integrated with Docker
   ├─ Smaller community
   ├─ Learning curve: Gentle
   └─ Best for: Simple deployments

3. Nomad (HashiCorp)
   ├─ Can orchestrate containers + VMs + bare metal
   ├─ More flexible than Kubernetes
   ├─ Smaller community
   └─ Best for: Mixed workloads

4. Amazon ECS (AWS-specific)
   ├─ Native AWS integration
   ├─ Simpler than Kubernetes (if on AWS)
   ├─ AWS lock-in
   └─ Best for: AWS-only deployments

We're learning Kubernetes because:
├─ Most popular
├─ Runs everywhere (on-premises, any cloud)
├─ Industry standard
├─ Best for learning fundamental concepts
└─ Knowledge transfers to other systems
```

## Declarative vs Imperative

Understanding this distinction is crucial to mastering Kubernetes.

### Imperative Approach

**Definition:** You tell the system HOW to do something, step-by-step.

**Example: Scaling an application**
```bash
# Step 1: Check current pods
docker ps

# Step 2: If only 1 is running, start another
docker run myapp:latest

# Step 3: Start another if needed
docker run myapp:latest

# Step 4: Register with load balancer manually
# ...script to update load balancer...

# Problems:
├─ Manual process each time
├─ Error-prone
├─ Hard to automate
├─ Hard to track what state we're in
└─ If you forget a step, you're in inconsistent state
```

### Declarative Approach

**Definition:** You tell the system WHAT you want, and it figures out HOW to achieve it.

**Example: Scaling an application with Kubernetes**
```yaml
# You declare desired state:
apiVersion: apps/v1
kind: Deployment
metadata:
  name: myapp
spec:
  replicas: 3  # "I want 3 copies running"
  # ... rest of config ...

# Kubernetes automatically:
├─ If currently 1 running: Start 2 more
├─ If currently 4 running: Stop 1
├─ If 1 crashes: Start a replacement
├─ If you change replicas: 3 to 5, it adds 2 more
└─ Kubernetes continuously ensures actual = desired state

# This is called Reconciliation Loop:
while true:
  actual_state = get_current_running_pods()
  desired_state = read_deployment_spec()
  
  if actual_state != desired_state:
    take_action_to_match()
  
  sleep(10 seconds)
```

**Why Declarative is Better:**
```
1. Reproducibility
   ├─ Same YAML = same result, always
   ├─ No step skipped accidentally
   └─ Works first time, second time, 100th time

2. Version Control
   ├─ Store YAML files in Git
   ├─ Track changes over time
   ├─ See who changed what and why
   └─ Audit trail

3. Automation
   ├─ CI/CD pipelines automatically apply YAML
   ├─ No manual steps
   ├─ Scales to 1000s of deployments
   └─ Self-service for teams

4. Safety
   ├─ Kubernetes maintains the declared state
   ├─ If you accidentally delete something, it's recreated
   ├─ Disaster recovery is just re-applying YAML
   └─ Consistency guaranteed

5. Testability
   ├─ Can review YAML changes before applying
   ├─ Can test against non-production first
   ├─ Less risky than imperative scripts
   └─ Better for compliance and governance
```

---

# 3. KUBERNETES ARCHITECTURE: DEEP DIVE

## The Control Plane (Master)

The control plane is Kubernetes' brain. It makes all decisions.

### API Server

**What it does:**
```
API Server = HTTP gateway to everything in Kubernetes

Flow:
1. User runs: kubectl apply -f deployment.yaml
2. kubectl packages this into HTTP request: POST /api/v1/namespaces/default/deployments
3. Request goes to API Server
4. API Server receives request

What API Server does:
├─ Authentication: Is this user allowed?
├─ Authorization: Does this user have permission?
├─ Validation: Is the YAML valid?
├─ Mutation: Apply any defaults
├─ Storage: Save to etcd
└─ Notification: Tell interested watchers

Result: Returns 201 Created
```

**Deep Dive: The Request Flow**

```
kubectl apply -f deployment.yaml
│
▼
kubectl converts YAML to HTTP request
│
POST /apis/apps/v1/namespaces/default/deployments
Content-Type: application/json
{
  "apiVersion": "apps/v1",
  "kind": "Deployment",
  "metadata": {
    "name": "myapp",
    "namespace": "default"
  },
  "spec": {
    "replicas": 3,
    ...
  }
}
│
▼
Request arrives at API Server
│
API Server performs checks:
│
├─ Authentication Check
│  │ Question: "Who are you?"
│  │ Checks: Client certificate / Token
│  │ Result: Identified as "john@example.com"
│  │
│  ├─ If no credentials: Reject (401 Unauthorized)
│  └─ If invalid credentials: Reject (401 Unauthorized)
│
├─ Authorization Check (RBAC)
│  │ Question: "Are you allowed to create Deployments?"
│  │ Checks: RBAC rules for user "john"
│  │ Result: john has "create" permission for "deployments"
│  │
│  ├─ If denied: Reject (403 Forbidden)
│  └─ If allowed: Continue
│
├─ Validation Check
│  │ Question: "Is this YAML valid?"
│  │ Checks: 
│  │   - Is apiVersion correct?
│  │   - Does kind exist?
│  │   - Are all required fields present?
│  │   - Are field values valid types?
│  │ Result: All checks pass
│  │
│  ├─ If invalid: Reject (400 Bad Request)
│  └─ If valid: Continue
│
├─ Mutation (Webhooks)
│  │ Question: "Do I need to modify this?"
│  │ Checks: Run MutatingWebhookConfigurations
│  │ Example: Auto-inject Istio sidecar
│  │ Result: Might add fields to spec
│  │
│  └─ Continue (modified or not)
│
├─ Validation Again (after mutation)
│  │ Question: "Is the mutated version still valid?"
│  │ Checks: Run ValidatingWebhookConfigurations
│  │ Result: Validation passes
│  │
│  ├─ If invalid after mutation: Reject
│  └─ If valid: Continue
│
├─ Store in etcd
│  │ API Server writes to etcd
│  │ /registry/deployments/default/myapp
│  │ {
│  │   "metadata": {...},
│  │   "spec": {...}
│  │ }
│  │ Result: Persisted successfully
│  │
│  └─ etcd confirms: Stored
│
├─ Notify Watchers
│  │ API Server broadcasts event: "ADDED Deployment myapp"
│  │ Controllers listening for new Deployments are notified
│  │ Result: Controllers spring into action
│  │
│  └─ Many components watch for changes
│
└─ Return to Client
   HTTP 201 Created
   {
     "apiVersion": "apps/v1",
     "kind": "Deployment",
     "metadata": {...},
     "status": {
       "observedGeneration": 1,
       "replicas": 0,
       ...
     }
   }

kubectl displays: "deployment.apps/myapp created"
```

### etcd: The Database

**What it is:**
```
etcd = Distributed key-value database
├─ Stores all Kubernetes state
├─ Written in Go
├─ Uses Raft consensus
└─ Built by CoreOS team

Every Kubernetes object is stored here:
├─ Deployments
├─ Pods
├─ Services
├─ Nodes
├─ RBAC rules
├─ Secrets
├─ ConfigMaps
└─ Everything else
```

**How etcd Works:**

```
Storing Data:
key: /registry/deployments/default/myapp
value: {
  "apiVersion": "apps/v1",
  "kind": "Deployment",
  "metadata": {"name": "myapp", ...},
  "spec": {"replicas": 3, ...}
}

Storage Structure (conceptual):
/registry/
├─ namespaces/
│  ├─ default/
│  └─ kube-system/
├─ deployments/
│  ├─ default/
│  │  ├─ myapp/
│  │  └─ otherapp/
│  └─ kube-system/
│     └─ coredns/
├─ pods/
│  ├─ default/
│  │  ├─ myapp-pod-1/
│  │  ├─ myapp-pod-2/
│  │  └─ myapp-pod-3/
│  └─ kube-system/
├─ services/
├─ secrets/
└─ ...

Why etcd is Critical:
├─ Single Source of Truth
│  └─ If etcd data is lost, cluster state is lost
│
├─ All components read from etcd
│  ├─ Scheduler reads deployments from etcd
│  ├─ Controller Manager reads from etcd
│  └─ kubelet reads pod specs from etcd
│
├─ Changes are watched
│  ├─ When you change a Deployment, etcd notifies watchers
│  ├─ Controllers react to changes
│  └─ Enables real-time orchestration
│
└─ Must be highly available
   ├─ Usually 3-5 copies (odd number for quorum)
   ├─ Replication ensures data survives node failure
   └─ Distributed consensus prevents split-brain
```

**etcd Watch Mechanism:**

```
How Controllers Learn About Changes:

1. Controller starts
   └─ Connects to API Server
   
2. Controller says: "Watch for Deployments"
   └─ API Server: "OK, I'll notify you of changes"

3. User: kubectl apply -f deployment.yaml
   └─ API Server stores in etcd
   └─ etcd notifies API Server: "New Deployment!"
   └─ API Server notifies watching controllers
   └─ Controllers receive event: "ADDED: myapp Deployment"

4. Deployment Controller wakes up
   ├─ "A new Deployment was added!"
   ├─ Reads Deployment spec: 3 replicas needed
   ├─ Creates 3 Pod objects in etcd
   ├─ Emits event: "ADDED: Pod myapp-1, Pod myapp-2, Pod myapp-3"

5. Scheduler is watching for Pods with no node assignment
   ├─ Receives: "3 new Pods added"
   ├─ Analyzes available nodes
   ├─ Assigns Pod1 to Node1, Pod2 to Node2, Pod3 to Node1
   ├─ Updates Pod objects in etcd with node assignment

6. kubelet on Node1 is watching for Pods assigned to it
   ├─ Receives: "Pod1 and Pod3 assigned to you"
   ├─ Pulls container image from registry
   ├─ Starts containers
   ├─ Reports status back to etcd

Result: Deployment created and running!
```

### Scheduler

**What it does:**
```
Scheduler = Assignment officer

When a Pod is created without a node assignment:
├─ Scheduler receives notification: "New Pod needs assignment"
├─ Analyzes all available nodes
├─ Determines best node
├─ Updates Pod to assign it to chosen node
└─ kubelet on that node is notified and starts container

The scheduling algorithm:
```

**Detailed Scheduling Algorithm:**

```
Scenario: New Pod created requiring 2 CPUs and 2GB RAM
Current Cluster State:
├─ Node1: 4 CPUs (0.5 allocated), 16GB RAM (2GB allocated)
│  └─ Running: small-app
│
├─ Node2: 4 CPUs (2 allocated), 16GB RAM (8GB allocated)
│  └─ Running: medium-app
│
├─ Node3: 4 CPUs (3.5 allocated), 16GB RAM (14GB allocated)
│  └─ Running: large-app
│
└─ Node4: 4 CPUs (0 allocated), 16GB RAM (0GB allocated)
   └─ Running: nothing

Scheduling Process:

PHASE 1: FILTERING (Feasibility Check)
├─ Node1: Has 3.5 CPUs free + 14GB RAM free ✓ FEASIBLE
├─ Node2: Has 2 CPUs free + 8GB RAM free ✓ FEASIBLE
├─ Node3: Has 0.5 CPUs free + 2GB RAM free ✗ NOT FEASIBLE (not enough CPUs)
└─ Node4: Has 4 CPUs free + 16GB RAM free ✓ FEASIBLE

Feasible nodes: [Node1, Node2, Node4]

PHASE 2: SCORING (Pick the best one)

Scoring Plugins evaluate each feasible node:

Plugin 1: LeastAllocated
├─ Node1: 
│  └─ Allocated: (0.5+2) / (4+16) = 2.5/20 = 12.5%
│  └─ Score: 100 * (1 - 0.125) = 87.5
├─ Node2:
│  └─ Allocated: (2+8) / (4+16) = 10/20 = 50%
│  └─ Score: 100 * (1 - 0.5) = 50
└─ Node4:
   └─ Allocated: 0 / (4+16) = 0%
   └─ Score: 100 * (1 - 0) = 100

Plugin 2: BalancedAllocation
├─ Node1:
│  ├─ CPU allocation: 12.5%
│  ├─ Memory allocation: 12.5%
│  └─ Difference: |0.125 - 0.125| = 0 (perfectly balanced)
│  └─ Score: 100
├─ Node2:
│  ├─ CPU allocation: 50%
│  ├─ Memory allocation: 50%
│  └─ Difference: |0.5 - 0.5| = 0 (perfectly balanced)
│  └─ Score: 100
└─ Node4:
   ├─ CPU allocation: 0%
   ├─ Memory allocation: 0%
   └─ Difference: |0 - 0| = 0 (perfectly balanced)
   └─ Score: 100

Plugin 3: NodeAffinityPriority
├─ Pod has no affinity rules
├─ All nodes score equally: 50

Final Scores (average of plugins):
├─ Node1: (87.5 + 100 + 50) / 3 = 79.2
├─ Node2: (50 + 100 + 50) / 3 = 66.7
└─ Node4: (100 + 100 + 50) / 3 = 83.3

Winner: Node4 (highest score 83.3)

PHASE 3: BINDING
├─ Scheduler updates Pod object: node_name = "Node4"
├─ Writes to etcd
├─ kubelet on Node4 notifies: "Pod assigned to me!"
├─ kubelet pulls image and starts container
└─ Pod is now running

Result: Pod scheduled on Node4, which had the least allocation
(prevents overloading any single node)
```

### Controller Manager

**What it is:**
```
Controller Manager = Process that runs many controller loops

Each controller:
├─ Watches for specific resource types
├─ Detects discrepancies between desired and actual state
└─ Takes action to reconcile

Main controllers:
```

**Replication Controller (Reconciliation Example):**

```
Deployment "myapp" specifies: replicas: 3

Desired State (in etcd):
├─ Pod myapp-abc123 (desired)
├─ Pod myapp-def456 (desired)
└─ Pod myapp-ghi789 (desired)

Actual State (running):
├─ Pod myapp-abc123 (running) ✓
├─ Pod myapp-def456 (running) ✓
└─ Pod myapp-ghi789 (running) ✓

Status: Reconciled ✓ (desired == actual)

---

SCENARIO: A pod crashes

Actual State becomes:
├─ Pod myapp-abc123 (running) ✓
├─ Pod myapp-def456 (crashed, CrashLoopBackOff) ✗
└─ Pod myapp-ghi789 (running) ✓

Replication Controller detects:
├─ "Desired 3 pods, but actual is only 2 healthy"
├─ "I need to fix this!"
├─ Creates new Pod: myapp-jkl012
├─ Requests kubelet to start it

Now actual state is:
├─ Pod myapp-abc123 (running) ✓
├─ Pod myapp-def456 (crashed, deleted)
├─ Pod myapp-ghi789 (running) ✓
└─ Pod myapp-jkl012 (starting up)

Status: Reconciled ✓ (desired == actual)

---

SCENARIO: User manually deletes a pod

User runs: kubectl delete pod myapp-abc123

Actual State becomes:
├─ Pod myapp-def456 (running) ✓
└─ Pod myapp-ghi789 (running) ✓

Replication Controller detects:
├─ "Desired 3 pods, but actual is only 2"
├─ "Someone deleted a pod!"
├─ Creates new Pod: myapp-mnop345
├─ Requests kubelet to start it

Status: Reconciled ✓

Note: This is why you can't just delete pods manually in production!
The controller will immediately recreate them.
To actually stop an app, you delete the Deployment,
which the controller then scales down gracefully.
```

## Worker Nodes

Worker nodes are where your applications actually run.

### kubelet: Node Agent

**What it does:**
```
kubelet = Node representative to Kubernetes cluster

Kubelet responsibilities:
├─ Register node with cluster
│  └─ Tells API Server: "I'm Node1 with 4 CPU, 16GB RAM"
│
├─ Watch for Pod assignments
│  └─ Listens for: "Pods assigned to me"
│
├─ Pull container images
│  └─ Downloads images from registry
│
├─ Start/stop containers
│  └─ Tells container runtime to run/stop containers
│
├─ Mount volumes
│  └─ Attaches storage to containers
│
├─ Report status
│  └─ Sends updates: Pod is running, Pod failed, Pod is ready
│
├─ Execute health checks
│  └─ Runs liveness and readiness probes
│
└─ Report node status
   └─ Sends: Node is healthy, Node is out of disk, etc.
```

**Kubelet Lifecycle Management:**

```
Scenario: New Pod assigned to Node1

Kubelet monitors etcd for Pods with node_name = "Node1"

Event: ADDED Pod "myapp-abc123"
    ↓
Kubelet actions (in sequence):

1. Create Pod Directory
   └─ /var/lib/kubelet/pods/uid-of-pod/
      └─ Kubelet uses Pod UID internally

2. Setup Pod Network
   ├─ Request CNI (Container Network Interface) plugin
   ├─ Plugin creates network interface for pod
   ├─ Assigns IP address (e.g., 10.244.1.5)
   ├─ Pod is now network-accessible
   └─ Other pods can reach it at 10.244.1.5

3. Setup Pod Volumes
   ├─ For each volume in Pod spec
   ├─ Mount volume into pod filesystem
   ├─ Example: Mount ConfigMap at /etc/config
   └─ Example: Mount Secret at /etc/secrets

4. Setup Pod Container Filesystem
   ├─ Prepare container root directory
   ├─ Set up container user/permissions
   └─ Ready for container runtime

5. Pull Container Image
   ├─ For each container in Pod spec
   ├─ Check if image is already pulled
   │  ├─ If yes: Use cached image
   │  └─ If no: Pull from registry (Docker Hub, etc.)
   ├─ Download image layers
   ├─ Extract image to local storage
   └─ Image ready to run

6. Create Container
   ├─ Tell container runtime: "Run this image"
   ├─ Container runtime uses image to create container
   ├─ Container filesystem is now setup
   ├─ Container process not started yet
   └─ Container ready

7. Start Container Process
   ├─ Tell container runtime: "Start the main process"
   ├─ Container runtime runs entrypoint
   ├─ Application process begins
   └─ Container now running

8. Setup Container Networking
   ├─ Link pod network interface to container namespace
   ├─ Container can now see pod IP address
   ├─ Container can communicate with outside world
   └─ Port mappings are established

9. Wait for Ready
   ├─ Initially pod is "Not Ready"
   ├─ Kubelet waits for readiness probe to pass
   ├─ Readiness probe: "Is your app listening on port 8080?"
   ├─ Container responds: "Yes, I'm ready"
   ├─ Kubelet marks pod as "Ready"
   └─ Service starts sending traffic

10. Report Status
    ├─ Sends to API Server: "Pod is running and ready"
    ├─ Pod status updates: phase = "Running", ready = true
    ├─ Service controller adds pod to endpoints
    └─ Load balancer can route traffic to this pod

Pod Lifecycle Complete!
User can now curl service IP and reach application

During Runtime (Continuous):

├─ Health Checks
│  ├─ Periodically run liveness probe: "Are you still alive?"
│  ├─ If probe fails 3 times: kubelet kills container
│  ├─ Controller creates replacement container
│  └─ This provides self-healing
│
├─ Status Updates
│  ├─ Every few seconds report pod status
│  ├─ CPU/memory usage
│  ├─ Number of restarts
│  └─ Any errors or warnings
│
├─ Volume Management
│  ├─ Keep volumes mounted
│  ├─ Monitor volume health
│  └─ Handle volume detachment
│
├─ Container Monitoring
│  ├─ Watch for container exits
│  ├─ Collect container logs
│  └─ Track resource usage
│
└─ Graceful Shutdown (when pod deleted)
   ├─ Receive termination notice
   ├─ Send SIGTERM to container
   ├─ Wait up to 30 seconds for graceful shutdown
   ├─ If still running, send SIGKILL
   ├─ Unmount volumes
   ├─ Cleanup networking
   └─ Report: Pod stopped
```

### Container Runtime

**What it is:**
```
Container Runtime = Software that actually runs containers

Examples:
├─ Docker (most common)
├─ containerd (lighter weight)
├─ CRI-O (Kubernetes-native)
└─ Others...

Container Runtime Interface (CRI):
├─ Standard protocol between kubelet and runtime
├─ kubelet doesn't need to know runtime details
├─ Kubelet says: "Run this image as a container"
├─ Runtime handles the details
└─ Can swap runtimes without changing kubelet

Example interaction:

Kubelet → Container Runtime
├─ "Create a container from image nginx:1.24"
├─ "Mount volume /data at /var/data"
├─ "Set environment variable PORT=8080"
├─ "Allocate 512MB RAM, 100m CPU"
└─ "Start the container"

Container Runtime
├─ Pulls image (if not cached)
├─ Creates container filesystem
├─ Applies resource limits
├─ Starts container process
├─ Sets up container networking
└─ Reports: "Container started with ID abc123"

Kubelet → Container Runtime
├─ "Get logs from container abc123"
├─ "Execute command in container abc123"
├─ "Stop container abc123"
└─ "Delete container abc123"
```

### kube-proxy: Networking

**What it does:**
```
kube-proxy = Network router for the node

Responsibilities:
├─ Implement Services
│  ├─ Service has virtual IP (e.g., 10.96.0.1)
│  ├─ Pods never actually run on this IP
│  ├─ kube-proxy intercepts traffic to this IP
│  └─ Redirects to actual pod IPs
│
├─ Load balance traffic
│  ├─ Service has multiple pod endpoints
│  ├─ kube-proxy distributes traffic
│  ├─ Different algorithms: round-robin, etc.
│  └─ Ensures even distribution
│
└─ Implement NetworkPolicies
   ├─ Restrict traffic between pods
   ├─ kube-proxy enforces allow/deny rules
   ├─ On a per-pod, per-port basis
   └─ Fine-grained network segmentation

Implementation: iptables (or ipvs)
├─ iptables: Linux kernel packet filtering
├─ kube-proxy programs iptables rules
└─ Kernel enforces rules at low level
   └─ Very efficient, happens in kernel space
```

**Example: How kube-proxy Routes Traffic**

```
Scenario:

Service: product-api
├─ Service IP (ClusterIP): 10.96.45.67
├─ Port: 80 (external port)
├─ Target Port: 8080 (container port)

Endpoints (actual Pods):
├─ Pod1: IP 10.244.1.5, port 8080
├─ Pod2: IP 10.244.1.6, port 8080
└─ Pod3: IP 10.244.2.7, port 8080

Client Pod wants to call the service:

Request 1: curl http://product-api/
├─ Destination: 10.96.45.67:80
├─ Client sends packet to this address
├─ Packet arrives at Node1 (where client pod runs)
├─ Kernel: "This is a Service IP, I need to intercept"
│  └─ (kube-proxy configured iptables to intercept)
│
├─ Kernel applies iptables rule:
│  └─ "DNAT 10.96.45.67:80 → 10.244.1.5:8080"
│  └─ (Destination NAT: rewrite packet destination)
│
├─ Packet is rewritten:
│  └─ Destination: 10.244.1.5:8080
│
├─ Packet sent to Pod1
├─ Pod1 responds
├─ Response packet intercepted again
├─ Source address rewritten back to 10.96.45.67
└─ Response sent back to client

Request 2: curl http://product-api/
├─ Same process
├─ But this time iptables rule says:
│  └─ "DNAT 10.96.45.67:80 → 10.244.1.6:8080" (Pod2)
│
├─ Request goes to Pod2
├─ Response goes back to client
└─ Traffic load-balanced across pods

Request 3: curl http://product-api/
├─ Third request goes to Pod3
└─ Traffic continues rotating

Result:
├─ Clients only know Service IP: 10.96.45.67
├─ Clients never know about individual Pod IPs
├─ Traffic is automatically load-balanced
├─ If a pod dies, kube-proxy updates rules
├─ Traffic automatically stops going to dead pod
└─ All transparent to client application
```

---

# 4. LOCAL DEVELOPMENT WITH KIND

## Setting Up Your First Cluster

Let's create a complete local Kubernetes cluster from scratch.

### Step 1: Prepare Your System

Before creating a cluster, let's verify everything is ready:

```bash
# Check Docker is running and accessible
docker ps
# Output: Should show any running containers (might be empty)
# If error: Docker daemon is not running

# If on macOS and Docker not running:
# 1. Open Applications → Docker.app
# 2. Wait for Docker menu icon to appear
# 3. Try again: docker ps

# Check Docker version
docker version
# Output should show both Client and Server versions

# Check available disk space
# macOS
df -h | grep "/" | head -1

# Linux
df -h | grep -E "/$|/home"

# Windows (PowerShell)
Get-PSDrive C

# You need at least 30GB free for comfortable development
```

### Step 2: Create Cluster Configuration

Create a file named `kind-config.yaml`:

```yaml
# KIND Configuration
kind: Cluster
apiVersion: kind.x-k8s.io/v1alpha4

# Cluster name
name: k8s-dev-cluster

# Node configuration
nodes:
  # Control Plane Node (Master)
  - role: control-plane
    image: kindest/node:v1.27.0
    
    # Port mappings: expose ports from container to host
    # This allows you to access services on localhost
    extraPortMappings:
      # HTTP ingress
      - containerPort: 80
        hostPort: 80
        protocol: TCP
      # HTTPS ingress
      - containerPort: 443
        hostPort: 443
        protocol: TCP
      # Custom service port
      - containerPort: 30000
        hostPort: 30000
        protocol: TCP
    
    # Mount directories from host into container
    # This creates persistent storage for the cluster
    extraMounts:
      - hostPath: /tmp/k8s-storage
        containerPath: /tmp/k8s-storage
  
  # Worker Node 1
  - role: worker
    image: kindest/node:v1.27.0
    extraMounts:
      - hostPath: /tmp/k8s-storage
        containerPath: /tmp/k8s-storage
  
  # Worker Node 2
  - role: worker
    image: kindest/node:v1.27.0
    extraMounts:
      - hostPath: /tmp/k8s-storage
        containerPath: /tmp/k8s-storage

# Network configuration
networking:
  # CIDR for pod IPs (pods will have IPs in this range)
  podSubnet: "10.244.0.0/16"
  
  # CIDR for service IPs (services will have IPs in this range)
  serviceSubnet: "10.96.0.0/12"
  
  # Don't disable default CNI
  # We want the cluster networking to work
  disableDefaultCNI: false
```

**Explanation of Configuration:**

```yaml
kind: Cluster
├─ We're configuring a Kubernetes cluster

apiVersion: kind.x-k8s.io/v1alpha4
├─ Version of KIND configuration format

name: k8s-dev-cluster
├─ Name to identify this cluster
├─ Used in commands: kind get clusters
└─ Used in kubeconfig context: kind-k8s-dev-cluster

nodes:
├─ List of nodes to create

- role: control-plane
├─ This is the master node
├─ Runs control plane components
├─ API Server, etcd, Scheduler, Controller Manager
└─ Typically 1 per cluster (or 3 for HA)

image: kindest/node:v1.27.0
├─ Base Docker image for this node
├─ Contains Kubernetes 1.27.0
├─ Download happens automatically
└─ First time takes a few minutes

extraPortMappings:
├─ Maps container ports to host ports
├─ Example: 80 on container → 80 on your machine
├─ Allows localhost:80 to reach cluster services
├─ Remove if you don't need ingress

hostPort: 80
├─ Port on your machine (host machine)
├─ When you access localhost:80, it goes here

containerPort: 80
├─ Port inside the KIND container
├─ Ingress controller listens here
└─ Traffic from hostPort gets forwarded here

extraMounts:
├─ Mount host directories into container
├─ Allows persistent storage across restarts
├─ /tmp/k8s-storage on your machine
│  mounted at /tmp/k8s-storage in container
└─ Important for local development with stateful apps

podSubnet: "10.244.0.0/16"
├─ CIDR block for pod IPs
├─ Each pod gets an IP in this range
├─ Example pod IPs: 10.244.1.5, 10.244.2.3, etc.
├─ Must not conflict with your network
└─ Default is usually fine

serviceSubnet: "10.96.0.0/12"
├─ CIDR block for service IPs (virtual IPs)
├─ Each service gets an IP in this range
├─ Example: 10.96.0.1 (Kubernetes API service)
├─ Must not conflict with your network
└─ Default is usually fine
```

### Step 3: Create the Cluster

```bash
# Create storage directory (required by our config)
mkdir -p /tmp/k8s-storage

# Create the cluster (this takes 2-5 minutes)
kind create cluster --config kind-config.yaml

# Output during creation:
# Creating cluster "k8s-dev-cluster" ...
#  ✓ Ensuring node image (kindest/node:v1.27.0) 🖼
#  ✓ Preparing nodes (3 node(s) running locally in kind "k8s-dev-cluster" 🐳
#  ✓ Writing configuration 📜
#  ✓ Starting control-plane 🕹️
#  ✓ Installing CNI 📚
#  ✓ Installing StorageClass 💾
#  ✓ Waiting ≤ 2m35s for control-plane = Ready ⏳
#  • Ready after 42s 💚
#  ✓ Uploading cluster logs 📋
# Set kubectl context to "kind-k8s-dev-cluster"
# You can now use your cluster with:
# 
# kubectl cluster-info --context kind-k8s-dev-cluster
# Have a question? Check out https://kind.sigs.k8s.io

# Cluster is now running!
```

**What happened during creation:**

```
Behind the scenes:

1. KIND downloaded base image
   └─ kindest/node:v1.27.0 (contains Kubernetes 1.27.0)

2. KIND created 3 Docker containers
   ├─ Container 1: "k8s-dev-cluster-control-plane"
   │  └─ Runs control plane components
   ├─ Container 2: "k8s-dev-cluster-worker"
   │  └─ Worker node 1
   └─ Container 3: "k8s-dev-cluster-worker2"
      └─ Worker node 2

3. Kubernetes initialized in each container
   ├─ Generated certificates
   ├─ Configured networking
   ├─ Started services
   └─ Nodes joined cluster

4. CNI (Container Network Interface) installed
   └─ Enables pod-to-pod networking

5. StorageClass configured
   └─ Enables persistent storage

6. kubectl configured
   └─ ~/.kube/config updated with cluster credentials

Result: Fully functional Kubernetes cluster!
```

### Step 4: Verify the Cluster

```bash
# Check cluster info
kubectl cluster-info

# Output:
# Kubernetes control plane is running at https://127.0.0.1:45825
# CoreDNS is running at https://127.0.0.1:45825/api/v1/namespaces/kube-system/services/coredns/proxy
# 
# To further debug and diagnose cluster problems, use 'kubectl cluster-info dump'

# Get nodes
kubectl get nodes

# Output:
# NAME                             STATUS   ROLES           AGE   VERSION
# k8s-dev-cluster-control-plane    Ready    control-plane   2m    v1.27.0
# k8s-dev-cluster-worker           Ready    <none>          1m    v1.27.0
# k8s-dev-cluster-worker2          Ready    <none>          1m    v1.27.0

# All nodes should show "Ready" status

# Get more detailed node information
kubectl get nodes -o wide

# Output:
# NAME                             STATUS   ROLES           AGE   VERSION   INTERNAL-IP   EXTERNAL-IP   OS-IMAGE    KERNEL-VERSION
# k8s-dev-cluster-control-plane    Ready    control-plane   2m    v1.27.0   172.18.0.2    <none>        Ubuntu ...  5.15.0-...
# k8s-dev-cluster-worker           Ready    <none>          1m    v1.27.0   172.18.0.3    <none>        Ubuntu ...  5.15.0-...
# k8s-dev-cluster-worker2          Ready    <none>          1m    v1.27.0   172.18.0.4    <none>        Ubuntu ...  5.15.0-...

# Check system pods (Kubernetes own services)
kubectl get pods -n kube-system

# Output:
# NAME                                             READY   STATUS    RESTARTS   AGE
# coredns-5d78c0869f-9vqgt                         1/1     Running   0          2m
# coredns-5d78c0869f-lcpvf                         1/1     Running   0          2m
# etcd-k8s-dev-cluster-control-plane               1/1     Running   0          2m
# kube-apiserver-k8s-dev-cluster-control-plane     1/1     Running   0          2m
# kube-controller-manager-...                      1/1     Running   0          2m
# kube-proxy-...                                   1/1     Running   0          1m
# kube-proxy-...                                   1/1     Running   0          1m
# kube-scheduler-...                               1/1     Running   0          2m
# kindnet-...                                      1/1     Running   0          2m
# kindnet-...                                      1/1     Running   0          1m
# kindnet-...                                      1/1     Running   0          1m

# These are essential Kubernetes services
# They're automatically deployed to any new cluster
```

### Step 5: See Cluster as Docker Containers

Remember, KIND clusters are just Docker containers:

```bash
# List Docker containers for this cluster
docker ps | grep k8s-dev-cluster

# Output:
# CONTAINER ID   IMAGE                  COMMAND                  CREATED        STATUS        PORTS
# abc123def456   kindest/node:v1.27.0   "/usr/local/bin/...     3 minutes ago  Up 3 minutes  127.0.0.1:45825->6443/tcp
# def456ghi789   kindest/node:v1.27.0   "/usr/local/bin/...     2 minutes ago  Up 2 minutes
# ghi789jkl012   kindest/node:v1.27.0   "/usr/local/bin/...     2 minutes ago  Up 2 minutes

# Each row is one Kubernetes node

# See actual container names
docker ps --filter "label=io.x-k8s.io/kind=k8s-dev-cluster" --format "table {{.Names}}\t{{.Status}}"

# Output:
# NAMES                                         STATUS
# k8s-dev-cluster-control-plane                 Up 3 minutes
# k8s-dev-cluster-worker                        Up 2 minutes
# k8s-dev-cluster-worker2                       Up 2 minutes

# Get logs from a container
docker logs k8s-dev-cluster-control-plane

# Access a container shell
docker exec -it k8s-dev-cluster-control-plane /bin/bash

# Inside the container, you can see Kubernetes processes:
# ps aux | grep kube
```

### Step 6: Understanding kubeconfig

When you created the cluster, kubectl was automatically configured to access it:

```bash
# View your kubeconfig
cat ~/.kube/config

# Relevant section for our cluster:
# clusters:
# - cluster:
#     certificate-authority-data: LS0tLS1CRU... (base64 encoded)
#     server: https://127.0.0.1:45825
#   name: kind-k8s-dev-cluster
# 
# contexts:
# - context:
#     cluster: kind-k8s-dev-cluster
#     user: kind-k8s-dev-cluster
#   name: kind-k8s-dev-cluster
# 
# current-context: kind-k8s-dev-cluster

# This means:
# ├─ Cluster "kind-k8s-dev-cluster" at https://127.0.0.1:45825
# ├─ User "kind-k8s-dev-cluster" has certificates to connect
# ├─ Context "kind-k8s-dev-cluster" links them together
# └─ Current context is "kind-k8s-dev-cluster"

# So kubectl knows:
# ├─ Where to connect (server URL)
# ├─ How to authenticate (certificates)
# └─ Which cluster to use (current-context)

# List available contexts
kubectl config get-contexts

# Output:
# CURRENT   NAME                     CLUSTER                  AUTHINFO                 NAMESPACE
# *         kind-k8s-dev-cluster     kind-k8s-dev-cluster     kind-k8s-dev-cluster     default

# The asterisk (*) shows current context

# Switch to a different context (if you had multiple clusters)
kubectl config use-context kind-k8s-dev-cluster

# Get current context
kubectl config current-context
# Output: kind-k8s-dev-cluster
```

---

# 5. MICROSERVICES: FROM THEORY TO PRACTICE

## The Monolith Problem

Before understanding microservices, let's understand what they're solving.

### Monolithic Architecture

A monolith is a single, large application where all features are tightly coupled.

**Example: E-commerce Application**

```
Monolithic Structure:

Single Application (myapp.jar / myapp.exe)
├─ User Service Code
│  ├─ Login, Registration
│  ├─ Profile Management
│  ├─ Password Reset
│  └─ User Preferences
├─ Product Service Code
│  ├─ Product Catalog
│  ├─ Search and Filter
│  ├─ Product Images
│  └─ Reviews and Ratings
├─ Order Service Code
│  ├─ Create Orders
│  ├─ Payment Processing
│  ├─ Order History
│  └─ Order Tracking
├─ Notification Service Code
│  ├─ Email Notifications
│  ├─ SMS Alerts
│  ├─ Push Notifications
│  └─ Notification Templates
└─ Shared Database
   └─ All data in one database

Deployment:
├─ Update to any feature → Rebuild entire application
├─ Deploy entire application
├─ If any feature has issues → Entire app might go down
└─ All users affected
```

**Problems with Monoliths:**

```
1. Scaling Inefficiency
   ├─ You want to scale Product Service (heavy reads)
   ├─ But monolith scales as a unit
   ├─ You get 3 copies of entire app:
   │  ├─ 3x User Service (not needed)
   │  ├─ 3x Product Service (what you wanted)
   │  ├─ 3x Order Service (not needed)
   │  └─ Cost: 3x infrastructure cost
   └─ Wasteful resource allocation

2. Technology Lock-in
   ├─ Entire app built with one tech stack
   ├─ User Service works great in Node.js
   ├─ Order Service needs Java for performance
   ├─ But you can't use different languages in monolith
   ├─ Stuck with one tech that's "good enough" for all
   └─ Can't optimize per service

3. Deployment Complexity
   ├─ Small bug in one feature
   ├─ Need to redeploy entire app
   ├─ Can't just restart one service
   ├─ Entire application goes offline (briefly)
   ├─ All users affected
   └─ Higher risk deployments (more code changed)

4. Team Scalability
   ├─ Team A works on User Service
   ├─ Team B works on Product Service
   ├─ Team C works on Order Service
   ├─ But all share one codebase
   ├─ Merge conflicts, coordination overhead
   ├─ Can't deploy independently
   ├─ One team's bug blocks other teams
   └─ Hard to scale teams

5. Debugging and Maintenance
   ├─ Bug in checkout flow
   ├─ Need to trace through:
   │  ├─ Product Service code
   │  ├─ Inventory Service code
   │  ├─ Payment Service code
   │  └─ All in same codebase
   ├─ Complex call stacks
   ├─ Hard to understand data flow
   └─ Increases debugging time

6. Database Challenges
   ├─ One database for all features
   ├─ Schema changes affect entire app
   ├─ Users tied together with orders with products
   ├─ Transactions across unrelated features
   ├─ Data consistency becomes complex
   └─ Hard to replicate or scale specific data
```

### Microservices Architecture

Microservices solve these problems by splitting into independent services.

```
Microservices Structure:

├─ User Service
│  ├─ Independent codebase
│  ├─ Node.js
│  ├─ Own database (User DB)
│  ├─ Runs on port 3001
│  └─ Deployed separately
│
├─ Product Service
│  ├─ Independent codebase
│  ├─ Java
│  ├─ Own database (Product DB)
│  ├─ Runs on port 3002
│  └─ Deployed separately
│
├─ Order Service
│  ├─ Independent codebase
│  ├─ Go
│  ├─ Own database (Order DB)
│  ├─ Runs on port 3003
│  └─ Deployed separately
│
├─ Notification Service
│  ├─ Independent codebase
│  ├─ Python
│  ├─ Own database (optional)
│  ├─ Runs on port 3004
│  └─ Deployed separately
│
└─ API Gateway (optional)
   ├─ Routes requests to services
   ├─ Single entry point for clients
   └─ Handles cross-cutting concerns

Communication:
├─ Service to Service via HTTP/REST or gRPC
├─ Service A → Service B (synchronous)
├─ Service A → Message Queue → Service B (asynchronous)
└─ Each service exposed via separate endpoint
```

**Benefits of Microservices:**

```
1. Independent Scaling
   ├─ Product Service getting heavy traffic?
   ├─ Scale only Product Service
   ├─ 5 copies of Product Service
   ├─ 1 copy of User Service
   ├─ 2 copies of Order Service
   ├─ Only pay for what you scale
   └─ Resource efficient

2. Technology Flexibility
   ├─ User Service → Node.js (fast, lightweight)
   ├─ Product Service → Java (high performance)
   ├─ Order Service → Go (concurrent)
   ├─ Notification Service → Python (simple)
   └─ Each team chooses optimal tech

3. Independent Deployment
   ├─ Fix User Service bug
   ├─ Redeploy only User Service
   ├─ Other services stay running
   ├─ No application-wide downtime
   ├─ Faster deployment cycles
   ├─ Lower risk (less code changed)
   └─ Users barely notice

4. Team Autonomy
   ├─ User Team owns User Service
   ├─ Product Team owns Product Service
   ├─ Teams can work independently
   ├─ Deploy on their own schedule
   ├─ Own database, own deployment
   ├─ Fewer merge conflicts
   └─ Faster development velocity

5. Fault Isolation
   ├─ User Service crashes
   ├─ Product Service still working
   ├─ Orders can still be processed
   ├─ Users might not create new accounts
   ├─ But existing functionality unaffected
   └─ Better overall reliability

6. Database Flexibility
   ├─ User Service → PostgreSQL (relational)
   ├─ Product Service → MongoDB (flexible schema)
   ├─ Order Service → DynamoDB (high throughput)
   ├─ Notification Service → Redis (fast, ephemeral)
   ├─ Choose database per service needs
   └─ Optimize for access patterns
```

## Service-to-Service Communication

Now that we have independent services, how do they communicate?

### Synchronous (Request-Response)

Service A calls Service B and waits for response.

**Example: Order Service Creating an Order**

```
User clicks "Place Order"
│
▼
Order Service receives request: POST /orders
{
  "userId": "user123",
  "productId": "product456",
  "quantity": 2
}
│
Order Service executes:
│
1. Store order in database
   └─ INSERT INTO orders (...)
   
2. Get product details
   ├─ HTTP GET to Product Service
   ├─ http://product-service:3002/api/products/product456
   ├─ Request includes: product ID
   │
   ├─ Product Service:
   │  └─ Looks up product in database
   │  └─ Returns: {name: "Laptop", price: 999.99}
   │
   └─ Wait for response (blocking)

3. Validate inventory
   ├─ HTTP GET to Inventory Service
   ├─ http://inventory-service:3005/check?product=product456&quantity=2
   ├─ Request includes: product ID, quantity
   │
   ├─ Inventory Service:
   │  └─ Checks stock levels
   │  └─ Returns: {available: true, remaining: 10}
   │
   └─ Wait for response (blocking)

4. Calculate price
   └─ price = 999.99 * 2 = 1999.98

5. Create payment request
   ├─ HTTP POST to Payment Service
   ├─ http://payment-service:3006/payments
   ├─ Request includes: amount, userId, orderId
   │
   ├─ Payment Service:
   │  ├─ Charges customer's credit card
   │  ├─ Returns: {status: "success", transactionId: "txn_xyz"}
   │
   └─ Wait for response (blocking)

6. Update order status
   └─ UPDATE orders SET status = 'confirmed' WHERE id = 'order123'

7. Send notification
   ├─ HTTP POST to Notification Service
   ├─ http://notification-service:3004/notify
   ├─ Request includes: userId, message, type
   │
   ├─ Notification Service:
   │  ├─ Sends email/SMS
   │  ├─ Returns: {status: "sent"}
   │
   └─ Wait for response (blocking)

8. Return response to user
   └─ HTTP 200 OK with order details
      {
        "orderId": "order123",
        "status": "confirmed",
        "total": 1999.98,
        "estimatedDelivery": "2025-12-26"
      }

Total Time = T1 + T2 + T3 + T4 + T5 + T6 + T7
(all blocking, sequential)
```

**Synchronous Communication Issues:**

```
1. Cascading Failures
   ├─ Inventory Service down
   ├─ Order Service can't complete order
   ├─ User sees error
   ├─ Even though Payment Service is fine
   └─ One service down = entire flow fails

2. Performance Dependency
   ├─ Order Service = 100ms
   ├─ Product Service = 500ms (slow database query)
   ├─ Inventory Service = 200ms
   ├─ Payment Service = 1000ms (external API call)
   ├─ Notification Service = 800ms
   ├─ Total time = 100 + 500 + 200 + 1000 + 800 = 2.6 seconds
   ├─ User waits 2.6 seconds for order confirmation
   └─ Slow service affects all downstream services

3. Tight Coupling
   ├─ Order Service depends on Product Service
   ├─ Order Service depends on Inventory Service
   ├─ Order Service depends on Payment Service
   ├─ Order Service depends on Notification Service
   ├─ Change to any service might break Order Service
   └─ Hard to make independent changes

4. Resource Exhaustion
   ├─ If Payment Service is slow
   ├─ Order Service threads get blocked waiting
   ├─ If 100 orders in progress
   ├─ 100 threads waiting for Payment Service
   ├─ Order Service runs out of threads
   ├─ Can't process new orders (deadlock)
   └─ Cascading resource exhaustion
```

**When to Use Synchronous:**

```
✓ Real-time requirements
  └─ User needs immediate confirmation

✓ Complex workflows
  └─ Need to coordinate multiple services
  
✓ Small number of services
  └─ Fewer dependencies = simpler

✓ Short-lived operations
  └─ Operations complete quickly

✗ NOT when any service might be slow or fail
✗ NOT for notifications (can be async)
✗ NOT for non-critical operations
```

### Asynchronous (Event-Driven)

Service A emits event, Service B processes later.

```
Order Service creates order:
│
1. Store order in database
   └─ INSERT INTO orders (...)
   
2. Emit event to message queue
   ├─ Event Type: "order.created"
   ├─ Event Data:
   │  {
   │    "orderId": "order123",
   │    "userId": "user123",
   │    "productId": "product456",
   │    "quantity": 2,
   │    "total": 1999.98,
   │    "timestamp": "2025-12-24T12:57:00Z"
   │  }
   │
   └─ Message Queue (RabbitMQ, Kafka, SQS)
      └─ Event published (returns immediately)

3. Return response to user
   └─ HTTP 200 OK
      {
        "orderId": "order123",
        "status": "pending",
        "message": "Order received, processing..."
      }

User response time: ~100ms (just database write + queue publish)

Meanwhile (asynchronously):

Event Handler 1: Inventory Service
├─ Listening for "order.created" events
├─ Receives event
├─ Decrements inventory
├─ Emits "inventory.updated" event
└─ Takes time: No impact on user experience

Event Handler 2: Notification Service
├─ Listening for "order.created" events
├─ Receives event
├─ Sends confirmation email
├─ Takes time: No impact on user experience

Event Handler 3: Payment Service
├─ Listening for "order.created" events
├─ Receives event
├─ Processes payment asynchronously
├─ Emits "payment.processed" or "payment.failed" event
└─ Takes time: No impact on user experience

Event Handler 4: Analytics Service
├─ Listening for "order.created" events
├─ Receives event
├─ Records metrics
├─ Updates dashboards
└─ Takes time: No impact on user experience
```

**Asynchronous Communication Benefits:**

```
1. Decoupling
   ├─ Order Service doesn't know about Inventory Service
   ├─ Just publishes event
   ├─ Any service can listen
   ├─ Services can be added/removed without changing Order Service
   └─ True loose coupling

2. Performance
   ├─ Order Service returns immediately
   ├─ User doesn't wait for slow services
   ├─ Better user experience
   └─ Faster response times

3. Resilience
   ├─ Inventory Service down?
   ├─ Order still created
   ├─ Inventory catches up when service is back
   ├─ No cascade failures
   └─ Better availability

4. Scalability
   ├─ Event queue buffers requests
   ├─ If Inventory Service slow, events queue up
   ├─ When capacity available, processes queue
   ├─ Order Service unaffected
   ├─ Handles traffic spikes better
   └─ Horizontal scaling easier

5. Audit Trail
   ├─ All events stored in queue
   ├─ Complete history of what happened
   ├─ Can replay events if needed
   ├─ Great for compliance and debugging
   └─ Natural audit log
```

**When to Use Asynchronous:**

```
✓ Notifications (email, SMS)
  └─ User doesn't need immediate confirmation

✓ Side effects (analytics, logging)
  └─ Not critical to main flow

✓ Long-running operations
  └─ Processing takes minutes/hours

✓ Handling spikes
  └─ Queue absorbs traffic bursts

✓ Integrations
  └─ External services might be slow

✗ NOT for real-time user feedback
✗ NOT when user needs immediate confirmation
✗ NOT for operations that fail frequently (complex retry logic needed)
```

### Choosing the Right Approach

```
Decision Tree:

Does user need immediate response?
├─ YES → Synchronous (HTTP/REST)
│  ├─ Example: Login, creating order confirmation
│  └─ But add caching, circuit breakers for resilience
│
└─ NO → Asynchronous (Event Queue)
   ├─ Example: Notifications, analytics, processing
   └─ Much better for distributed systems

Is it a critical operation?
├─ YES (Payment, Security) → Synchronous
│  └─ Need immediate feedback and confirmation
│
└─ NO (Analytics, Logging) → Asynchronous
   └─ Can tolerate some delay

How many services involved?
├─ Few (2-3) → Synchronous OK
│  └─ Simple request-response
│
└─ Many (5+) → Asynchronous
   └─ Avoid cascading failures

Can the operation fail?
├─ YES → Asynchronous
│  ├─ Queue retries automatically
│  ├─ Decouples failure handling
│  └─ Better fault tolerance
│
└─ NO (unlikely) → Either is fine
```

## Data Consistency in Microservices

When services have separate databases, keeping data consistent is challenging.

### The Challenge

```
User places order for laptop

Scenario 1: Order Service creates order
├─ Product Service increases "sales count"
├─ Inventory Service decrements stock
├─ Notification Service sends email
└─ All succeed

Scenario 2: What if Payment Service fails midway?
├─ Order Service: Order created ✓
├─ Inventory Service: Stock decremented ✓
├─ Payment Service: Payment FAILED ✗
├─ Notification Service: Email sent ✓
│
└─ PROBLEM: Inventory shows item sold, but payment failed!
   ├─ User didn't buy
   ├─ But inventory is wrong
   ├─ Stock is inconsistent with reality
   └─ Data inconsistency!

Scenario 3: Multiple services updating related data
├─ Order Service: "Order is confirmed"
├─ Inventory Service: Still processing
├─ Notification Service: Email not sent yet
│
└─ PROBLEM: System is in inconsistent state
   ├─ Order says confirmed
   ├─ Inventory not updated
   ├─ User not notified
   └─ What is the "truth"?
```

### Solutions: Saga Pattern

A saga is a sequence of local transactions that maintain distributed data consistency.

**Example: Order Saga**

```
Choreography (Event-driven):

1. User places order
   └─ Order Service creates order
   └─ Emits: "order.created"
   
2. Inventory Service listens
   └─ Receives: "order.created"
   └─ Decrements stock
   └─ Emits: "order.inventory_reserved"

3. Payment Service listens
   └─ Receives: "order.inventory_reserved"
   └─ Charges payment
   ├─ If success: Emits "order.payment_confirmed"
   └─ If fails: Emits "order.payment_failed"

4A. If payment succeeded
   └─ Notification Service receives "order.payment_confirmed"
   └─ Sends confirmation email
   └─ Order complete

4B. If payment failed
   ├─ Inventory Service receives "order.payment_failed"
   ├─ Restores inventory (reverses reservation)
   └─ Order cancelled

Result:
├─ If any step fails, previous steps are compensated
├─ System returns to consistent state
├─ No partial orders
└─ Distributed transaction complete!

Orchestration (Central Coordinator):

1. User places order
   └─ Order Service creates order

2. Saga Orchestrator (Order Service itself)
   ├─ Step 1: Call Inventory Service → "reserve inventory"
   │  └─ If fails: ABORT
   │
   ├─ Step 2: Call Payment Service → "charge payment"
   │  └─ If fails: Call Inventory Service → "release inventory", then ABORT
   │
   ├─ Step 3: Call Notification Service → "send email"
   │  └─ If fails: Already committed, so OK
   │
   └─ If all succeed: Order confirmed

Result:
├─ Central service controls flow
├─ Clear order of operations
├─ Explicit compensation logic
├─ Easier to debug and understand
└─ Better for complex workflows
```

---

# 6. CONTAINERIZING APPLICATIONS WITH DOCKER

## Docker Fundamentals

Docker is a containerization platform. Let's understand it deeply.

### What is a Container?

A container is a lightweight, standalone executable package containing:

```
Container = Application + Dependencies + OS Runtime

Contents of Container:
├─ Application Code
│  └─ Your Python/Node.js/Java code
│
├─ Application Dependencies
│  ├─ Python: flask, requests, sqlalchemy
│  ├─ Node.js: express, react, axios
│  └─ Java: Spring Boot, Hibernate
│
├─ OS Libraries
│  ├─ C libraries (libc, libssl)
│  ├─ System utilities (bash, curl)
│  └─ Runtime (python, node, java)
│
├─ Environment Configuration
│  ├─ Environment variables
│  ├─ Working directory
│  └─ Default command to run
│
└─ Filesystem
   └─ File structure isolated from host

NOT Included:
├─ OS Kernel (shared with host)
├─ System drivers (shared with host)
└─ Heavy OS overhead (shared with host)

Result:
├─ Lightweight (50-500MB vs 2-5GB for VM)
├─ Fast startup (milliseconds vs seconds)
├─ Portable (same everywhere)
└─ Efficient resource usage
```

### Docker Image vs Container

**Image = Blueprint, Container = Running Instance**

```
Analogy: Class vs Object in Programming

Docker Image:
├─ Like a class definition
├─ Blueprint for containers
├─ Read-only template
├─ Contains layers (we'll explain)
├─ Stored locally or in registry
├─ Size: typically 50-500MB
└─ Example: ubuntu:22.04, nginx:1.24, python:3.11

Docker Container:
├─ Like an object instance
├─ Running instance of image
├─ Read-write layer on top
├─ Unique filesystem (changes isolated)
├─ Runs as process on host
├─ Size: typically MB-GB (includes layer + data)
└─ Example: container_abc123, container_def456

Relationship:
├─ One image → Multiple containers
├─ Image: ubuntu:22.04
├─ Container 1: My app instance 1
├─ Container 2: My app instance 2
├─ Container 3: Test environment
└─ All from same image, but isolated
```

### Docker Image Layers

Docker images are built in layers. This is crucial to understand.

```
Dockerfile:
FROM ubuntu:22.04          # Layer 1 (base OS)
RUN apt-get update         # Layer 2 (install tools)
RUN apt-get install -y python3  # Layer 3 (python)
COPY app.py /app/          # Layer 4 (app code)
CMD ["python3", "app.py"]  # Layer 5 (metadata, not a real layer)

Visual Stack:

┌─────────────────────────────────┐
│ Layer 5: CMD metadata           │ (Read-only, added at build)
├─────────────────────────────────┤
│ Layer 4: Application code       │ (Read-only, 1MB)
│ COPY app.py /app/               │
├─────────────────────────────────┤
│ Layer 3: Python installed       │ (Read-only, 300MB)
│ RUN apt-get install python3     │
├─────────────────────────────────┤
│ Layer 2: Updated packages       │ (Read-only, 200MB)
│ RUN apt-get update              │
├─────────────────────────────────┤
│ Layer 1: Base Ubuntu image      │ (Read-only, 77MB)
│ FROM ubuntu:22.04               │
├─────────────────────────────────┤
│ Total Image Size: ~578MB        │
└─────────────────────────────────┘

Key Point: All layers are read-only and reusable!

Layer Caching:
├─ Each layer is cached locally
├─ If you rebuild with same commands
├─ Docker doesn't rebuild layers
├─ Uses cached layers instead
└─ Much faster rebuilds
```

### Dockerfile: Complete Guide

Let's write a Dockerfile for a .NET 8 API application.

**ProductApi/Dockerfile (Multi-stage Build)**

```dockerfile
# ==================== STAGE 1: BUILD ====================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set working directory in container
WORKDIR /src

# Copy project file (small, just describes dependencies)
COPY ["ProductApi/ProductApi.csproj", "ProductApi/"]

# Restore dependencies
# This downloads all NuGet packages
# Takes time first run, cached in subsequent builds
RUN dotnet restore "ProductApi/ProductApi.csproj"

# Copy entire source code
COPY . .

# Work in project directory
WORKDIR "/src/ProductApi"

# Build application (compiles C# to IL)
# -c Release: Optimize for production
# -o: Output directory
RUN dotnet build "ProductApi.csproj" -c Release -o /app/build

# Publish (creates deployment package)
# This is what actually runs
# Much smaller than full source
RUN dotnet publish "ProductApi.csproj" \
    -c Release \
    -o /app/publish \
    --no-build

# ==================== STAGE 2: RUNTIME ====================
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Set working directory
WORKDIR /app

# Copy compiled application from build stage
# ONLY copy what's needed to run
# Not source code, not build tools
COPY --from=build /app/publish .

# Create non-root user (security best practice)
RUN useradd -m -u 1000 appuser && chown -R appuser:appuser /app

# Switch to non-root user
USER appuser

# Health check
# Docker periodically executes this
# If fails 3+ times, container marked unhealthy
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
    CMD dotnet exec /app/ProductApi.dll || exit 1

# Expose port (documentation, doesn't actually publish port)
EXPOSE 8080

# Environment variable for listening port
ENV PORT=8080

# Entry point (command to run when container starts)
ENTRYPOINT ["dotnet", "ProductApi.dll"]
```

**Why Multi-stage Build?**

```
Traditional Build (Single Stage):
├─ FROM mcr.microsoft.com/dotnet/sdk:8.0
├─ (everything in one image)
├─ sdk image: 4GB (includes compilers, build tools)
├─ Final image includes:
│  ├─ SDK (4GB)
│  ├─ Build tools (not needed at runtime)
│  ├─ Compiled application (small)
│  └─ Source code (not needed at runtime)
└─ Final image size: ~4GB
   └─ Wasteful!

Multi-stage Build:
├─ Stage 1 (Build): Uses SDK image (4GB)
│  ├─ Compiles code
│  ├─ Creates /app/publish directory
│  └─ Contains only runtime files
│
├─ Stage 2 (Runtime): Uses aspnet image (600MB)
│  ├─ Copies /app/publish from stage 1
│  ├─ Doesn't include build tools
│  ├─ Doesn't include source code
│  └─ Only runtime essentials
│
└─ Final image size: ~600MB
   └─ 85% smaller!

Result:
├─ Smaller image
├─ Faster deployment
├─ Faster pulls from registry
├─ Less network bandwidth
└─ Better security (no build tools in production)
```

### Building and Running Containers

**Building an Image:**

```bash
# Navigate to project root (where Dockerfile is)
cd ProductApi

# Build image
# -f: Dockerfile path (optional, assumes Dockerfile by default)
# -t: Tag (name:version)
docker build -f Dockerfile -t product-api:1.0.0 .

# What happens:
# 1. Docker reads Dockerfile
# 2. Executes each command
# 3. Creates layer for each RUN command
# 4. Tags final image as "product-api:1.0.0"
# 5. Stores locally

# Output:
# [1/10] FROM mcr.microsoft.com/dotnet/sdk:8.0
# Pulling from library/dotnet
# (downloading image, first time only)
# ...
# [2/10] WORKDIR /src
# ---> Running in abc123def456
# ---> abc1234 (layer hash)
# ...
# => exporting to image
# => => writing image sha256:abc123...
# => => naming as docker.io/library/product-api:1.0.0

# View built images
docker images | grep product-api

# Output:
# REPOSITORY    TAG       IMAGE ID      CREATED         SIZE
# product-api   1.0.0     abc123def456  5 minutes ago    612MB

# Push to registry (if applicable)
docker tag product-api:1.0.0 myregistry/product-api:1.0.0
docker push myregistry/product-api:1.0.0
```

**Running a Container:**

```bash
# Run container from image
# -p: Port mapping (host:container)
# -e: Environment variable
# --name: Container name
# -d: Detached (background)
docker run \
  --name product-api-1 \
  -p 8080:8080 \
  -e PORT=8080 \
  -d \
  product-api:1.0.0

# Output:
# abc123def456... (container ID)

# Check running containers
docker ps

# Output:
# CONTAINER ID  IMAGE              COMMAND             CREATED        STATUS       PORTS               NAMES
# abc123def456  product-api:1.0.0  "dotnet Produc..."  2 seconds ago  Up 1 second  0.0.0.0:8080->8080  product-api-1

# Test the API
curl http://localhost:8080/api/product

# View logs
docker logs -f product-api-1

# Stop container
docker stop product-api-1

# Remove container
docker rm product-api-1

# Remove image
docker rmi product-api:1.0.0
```

## Docker Best Practices

### Image Optimization

```
1. Use Minimal Base Images
   ❌ Bad: FROM ubuntu:22.04 (77MB)
   ✓ Good: FROM ubuntu:22.04-minimal (28MB)
   ✓ Better: FROM alpine:3.18 (7MB)
   ✓ Best: FROM scratch (0MB, for compiled binaries)

2. Order Commands for Caching
   ❌ Bad:
       COPY . /app           # Copy everything (includes node_modules)
       RUN npm ci            # Install dependencies
       RUN npm run build     # Compile
   (Changes to any file invalidate all later layers)
   
   ✓ Good:
       COPY package.json /app/
       RUN npm ci            # Only runs if package.json changed
       COPY . /app           # Copies everything
       RUN npm run build     # Only runs if code changed
   (Layers only rebuild when dependencies change)

3. Minimize Layer Count
   ❌ Bad:
       RUN apt-get update
       RUN apt-get install -y curl
       RUN apt-get install -y git
       (3 layers)
   
   ✓ Good:
       RUN apt-get update && \
           apt-get install -y curl git && \
           apt-get clean && \
           rm -rf /var/lib/apt/lists/*
       (1 layer, removes package manager cache)

4. Use .dockerignore
   ├─ Prevents copying unnecessary files
   ├─ Similar to .gitignore
   ├─ Speeds up context creation
   └─ Example:
       node_modules/
       .git
       .env
       .DS_Store
       *.log

5. Remove Build Artifacts
   ❌ Docker image includes:
       ├─ Source code
       ├─ Build tools
       ├─ Intermediate files
       └─ Everything → 2GB image

   ✓ Multi-stage build includes:
       └─ Only runtime files → 200MB image
```

---

# 7. KUBERNETES OBJECTS: COMPREHENSIVE GUIDE

## Understanding Kubernetes Objects

Kubernetes objects are persistent entities in the cluster. Let's understand them deeply.

### Common Kubernetes Workload Types

**Pods** - The basic deployable unit containing one or more containers that share network and storage.

**Deployments** - Manages stateless applications with rolling updates, rollbacks, and replica management.

**StatefulSets** - Manages stateful applications requiring stable network identities, persistent storage, and ordered deployment/scaling (databases, message queues).

**DaemonSets** - Ensures a copy of a pod runs on every node in the cluster, used for node-level services like log collectors, monitoring agents, or network plugins.

**Jobs** - Creates one or more pods that run to completion for batch processing, data migrations, or one-time tasks, then terminates.

**CronJobs** - Schedules Jobs to run at specific times or intervals, like cron in Linux, for periodic tasks such as backups, reports, or cleanup scripts.

---

### Pods: The Basic Unit

A Pod is the smallest deployable unit in Kubernetes.

**What is a Pod?**

```
Pod = Smallest deployable unit
├─ Contains 1+ containers (usually 1)
├─ Containers share:
│  ├─ Network namespace (same IP address)
│  ├─ Storage volumes
│  ├─ Localhost networking
│  └─ IPC (Inter-Process Communication)
│
├─ One IP per Pod (not per container)
├─ All containers in Pod reach each other via localhost
├─ Containers share port space
└─ Ephemeral (deleted, not recovered)

Why share networking?
├─ Containers tightly coupled
├─ Multiple containers doing single job
├─ Example: App container + logging sidecar
```

**Pod YAML Structure:**

```yaml
apiVersion: v1
kind: Pod
metadata:
  name: product-api-pod          # Unique name in namespace
  namespace: default             # Namespace (default if not specified)
  labels:
    app: product-api             # Labels for selection
    version: v1
  annotations:
    description: "Product API pod"

spec:
  containers:
    - name: api-container        # Container name (unique in pod)
      image: product-api:1.0.0   # Image to run
      imagePullPolicy: IfNotPresent  # Pull policy
      
      ports:
        - name: http
          containerPort: 8080    # Port app listens on
          protocol: TCP
      
      env:
        - name: PORT
          value: "8080"
        - name: LOG_LEVEL
          value: "INFO"
      
      resources:
        requests:
          cpu: 100m              # Guaranteed CPU
          memory: 256Mi          # Guaranteed memory
        limits:
          cpu: 500m              # Maximum CPU
          memory: 512Mi          # Maximum memory
      
      livenessProbe:
        httpGet:
          path: /api/product/health
          port: 8080
        initialDelaySeconds: 15  # Wait before first check
        periodSeconds: 10        # Check every 10 seconds
        failureThreshold: 3      # Fail after 3 failed checks
        timeoutSeconds: 5        # Timeout for each check
      
      readinessProbe:
        httpGet:
          path: /api/product/ready
          port: 8080
        initialDelaySeconds: 5
        periodSeconds: 5
        failureThreshold: 2

  restartPolicy: Always          # Restart if exits
  terminationGracePeriodSeconds: 30  # Time to gracefully shutdown
```

**Pod Lifecycle:**

```
Pod Creation to Running:

1. Pod Definition Submitted
   └─ kubectl apply -f pod.yaml
   └─ Sent to API Server

2. Pod Object Created in etcd
   ├─ API Server stores Pod
   ├─ Pod status: Pending
   └─ Scheduler notified: "New pod to schedule"

3. Scheduler Assigns Node
   ├─ Scheduler checks available nodes
   ├─ Selects best node (based on resources, affinity)
   ├─ Updates Pod: nodeAssignment = "worker1"
   └─ Scheduler notified kubelet on worker1

4. kubelet Prepares Environment
   ├─ Creates pod directories
   ├─ Sets up networking
   ├─ Mounts volumes
   ├─ Pod status: Pending
   └─ Continues preparing

5. Container Runtime Pulls Image
   ├─ Requests container runtime to prepare
   ├─ Runtime pulls image from registry
   ├─ If image cached locally, uses cache
   ├─ Downloads image layers
   ├─ Pod status: PullingImage
   └─ Continues

6. Container Runtime Creates Container
   ├─ From pulled image, creates container
   ├─ Sets up filesystem
   ├─ Sets up environment
   ├─ Creates network namespace
   ├─ Pod status: ContainerCreating
   └─ Continues

7. Container Starts
   ├─ Container runtime starts process
   ├─ Application begins running
   ├─ Pod status: Running (not Ready yet)
   ├─ kubelet runs readiness probe
   └─ Continues

8. Readiness Probe Passes
   ├─ Readiness probe: "Is app ready?"
   ├─ App responds: "Yes, I'm ready"
   ├─ kubelet: "Mark as Ready"
   ├─ Pod status: Running + Ready
   ├─ Service adds this pod to endpoints
   └─ Traffic can route to this pod

9. Pod Ready for Traffic
   ├─ Other pods can curl this pod
   ├─ Services route traffic here
   ├─ Pod fully operational
   └─ liveness probe monitoring begins
   
During Runtime:
├─ liveness probe: "Are you still alive?" (every 10 seconds)
├─ readiness probe: "Are you ready?" (every 5 seconds)
├─ If liveness fails 3 times: kubelet kills container
├─ If readiness fails 2 times: removed from service
├─ kubelet restarts failed containers
└─ Monitoring continues

Pod Deletion:
├─ User: kubectl delete pod
├─ kubelet receives: terminate pod
├─ Send SIGTERM to container (graceful shutdown)
├─ Wait up to 30 seconds for graceful shutdown
├─ If still running: Send SIGKILL (force kill)
├─ Unmount volumes
├─ Remove networking
├─ Delete pod object
└─ Pod completely gone

Total time: Creation ~10-30 seconds, Ready ~15-45 seconds
```

**Health Checks (Probes):**

Probes are Kubernetes mechanisms to check if your application is healthy and ready to serve traffic.

```
Three types of probes:

1. Startup Probe
   ├─ Question: "Is app starting up?"
   ├─ Purpose: For slow-starting apps
   ├─ Only runs at beginning
   ├─ Once passes, liveness/readiness start
   └─ Example: Java app taking 60 seconds to start
   
   ├─ If fails: Container killed, restarted
   └─ If succeeds: Move to readiness/liveness

2. Readiness Probe
   ├─ Question: "Is app ready for traffic?"
   ├─ Runs continuously (after startup)
   ├─ Example: "Can I handle requests?"
   │  └─ Connected to database? Loaded config? Ready?
   │
   ├─ If fails: Pod marked NotReady
   │  ├─ Service removes pod from endpoints
   │  ├─ Traffic stops routing to pod
   │  └─ Pod still running, might recover
   │
   ├─ Use case: Graceful shutdown
   │  └─ Readiness fails → traffic stops
   │  └─ But pod still processing existing requests
   │  └─ App can clean up
   │
   └─ If succeeds: Pod marked Ready
      ├─ Service adds pod to endpoints
      └─ Traffic can route to pod

3. Liveness Probe
   ├─ Question: "Are you still alive?"
   ├─ Runs continuously
   ├─ Example: Deadlock detection
   │  ├─ App seems running (process exists)
   │  ├─ But internal deadlock
   │  ├─ Won't respond to requests
   │  └─ Liveness probe detects and restarts
   │
   ├─ If fails: Container killed
   │  ├─ Restart policy determines action
   │  ├─ Usually: kubelet restarts container
   │  ├─ Appears as restart in kubectl
   │  └─ Service traffic moved to other pods
   │
   └─ Use case: Self-healing
      ├─ Automatic detection of stuck containers
      └─ No manual intervention needed
```

**Probe Implementation Types:**

```
HTTP GET Probe (most common):
livenessProbe:
  httpGet:
    path: /health      # Endpoint to hit
    port: 8080         # Port
    httpHeaders:       # Optional headers
      - name: Authorization
        value: Bearer token123
  initialDelaySeconds: 15     # Wait before first probe
  periodSeconds: 10           # Interval between probes
  timeoutSeconds: 5           # Timeout for each probe
  successThreshold: 1         # Passes after 1 success
  failureThreshold: 3         # Fails after 3 failures

How it works:
├─ Every 10 seconds
├─ httpGet to http://localhost:8080/health
├─ Wait max 5 seconds for response
├─ If response code 200-399: Success
├─ If response code 400+ or timeout: Failure
├─ After 3 failures: Container killed

TCP Socket Probe:
livenessProbe:
  tcpSocket:
    port: 8080         # Just check if port is open
  periodSeconds: 10
  failureThreshold: 3

How it works:
├─ Try to connect to port 8080
├─ If connection succeeds: Success
├─ If connection fails: Failure
└─ No application-level health check

Exec Probe (run command):
livenessProbe:
  exec:
    command:
      - /bin/sh
      - -c
      - curl http://localhost:8080/health || exit 1
  periodSeconds: 10

How it works:
├─ Execute command inside container
├─ If exit code 0: Success
├─ If exit code non-zero: Failure
└─ Can run custom health check scripts
```

---

# 8. DEPLOYMENTS: MANAGING APPLICATION LIFECYCLE

## Understanding Deployments

Deployments are the recommended way to manage pods in production.

### Why Not Just Use Pods?

```
Scenario: Your app crashes

With Raw Pod:
├─ Pod dies
├─ Nothing restarts it
├─ Users see errors
├─ You manually restart:
│  └─ kubectl apply -f pod.yaml (creates new pod)
│     or
│  └─ kubectl create pod (manual restart)
└─ Tedious manual process

With Deployment:
├─ Pod dies
├─ ReplicationController detects: "I should have 3, but only have 2"
├─ Automatically creates replacement pod
├─ Users maybe notice 1 second blip
├─ Automatic self-healing!
```

### Deployment Components

```
Deployment
├─ Desired state declaration
│  ├─ "I want 3 replicas"
│  ├─ "Using image product-api:1.0.0"
│  ├─ "Expose port 8080"
│  └─ "Update 1 at a time"
│
└─ Kubernetes components that implement it:

ReplicationController / ReplicaSet
├─ Watches Deployment spec
├─ "Current: 2 pods, Desired: 3 pods"
├─ Creates new pods
├─ Watches for crashed pods
├─ Removes old pods during updates
└─ Ensures replicas always match desired count

Pod Template
├─ Blueprint for pod creation
├─ "When creating new pod, use this template"
├─ Includes: image, ports, environment, resources
└─ Every pod from deployment uses same template
```

### Deployment YAML Deep Dive

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: product-api
  namespace: default
  labels:
    app: product-api

spec:
  # How many replicas (copies) do we want?
  replicas: 3
  
  # How do we identify pods managed by this deployment?
  selector:
    matchLabels:
      app: product-api
  
  # Rolling update strategy
  strategy:
    type: RollingUpdate
    rollingUpdate:
      maxSurge: 1        # Max 1 extra pod during update
      maxUnavailable: 0  # Never have 0 pods (high availability)
  
  # Template for creating new pods
  template:
    metadata:
      labels:
        app: product-api
        version: v1
    
    spec:
      containers:
        - name: api
          image: product-api:1.0.0
          imagePullPolicy: IfNotPresent
          
          ports:
            - name: http
              containerPort: 8080
          
          env:
            - name: PORT
              value: "8080"
          
          resources:
            requests:
              cpu: 100m
              memory: 256Mi
            limits:
              cpu: 500m
              memory: 512Mi
          
          livenessProbe:
            httpGet:
              path: /api/product/health
              port: 8080
            initialDelaySeconds: 15
            periodSeconds: 10
          
          readinessProbe:
            httpGet:
              path: /api/product/health
              port: 8080
            initialDelaySeconds: 5
            periodSeconds: 5
      
      restartPolicy: Always
      terminationGracePeriodSeconds: 30
```

### Rolling Updates: Deep Dive

Rolling updates allow zero-downtime deployments.

```
Initial State:
├─ 3 pods running v1
├─ Each getting ~33% of traffic
└─ Service routes to all 3

Rolling Update Process (maxSurge: 1, maxUnavailable: 0):

Step 1: Update deployment image
└─ kubectl set image deployment/product-api api=product-api:2.0.0

Step 2: Create first new v2 pod
├─ Temporary total: 3 v1 + 1 v2 = 4 pods
├─ Traffic distributed: 75% v1, 25% v2
├─ Monitor for errors

Step 3: Readiness probe passes on v2 pod
├─ V2 pod is ready for traffic
├─ Service routes traffic to it
└─ Still running v1 for comparison

Step 4: Delete oldest v1 pod
├─ Temporary total: 2 v1 + 1 v2 = 3 pods
├─ Traffic distributed: 67% v1, 33% v2
├─ Wait for new v2 pod to be ready

Step 5: Create second new v2 pod
├─ Temporary total: 2 v1 + 2 v2 = 4 pods
├─ Traffic distributed: 50% v1, 50% v2
└─ Monitor for errors

Step 6: Readiness probe passes
├─ Second v2 pod ready
└─ Service routes traffic to it

Step 7: Delete second oldest v1 pod
├─ Temporary total: 1 v1 + 2 v2 = 3 pods
├─ Traffic distributed: 33% v1, 67% v2
└─ Wait for third v2 pod to be ready

Step 8: Create third new v2 pod
├─ Temporary total: 1 v1 + 3 v2 = 4 pods
├─ Traffic distributed: 25% v1, 75% v2
└─ Monitor for errors

Step 9: Readiness probe passes
├─ Third v2 pod ready
└─ Service routes traffic to it

Step 10: Delete last v1 pod
├─ Final total: 0 v1 + 3 v2 = 3 pods
├─ Traffic: 100% v2
└─ Rolling update complete!

Time: ~2-5 minutes (depends on image pull time, startup time)
Downtime: 0 (zero-downtime deployment!)
Traffic impact: Minimal (gradual shift)
Rollback: Can roll back to v1 at any point

If v2 has bugs:
├─ kubectl rollout undo deployment/product-api
├─ Kubernetes immediately rolls back
├─ Creates v1 pods again
├─ Deletes v2 pods
├─ Back to previous version quickly
└─ Customers only see brief slowness
```

---

# 9. SERVICES: NETWORKING EXPLAINED

## Understanding Services

Services provide stable networking for pods.

### Why Services?

```
Problem: Pods are ephemeral

├─ Pod crashes: Deployment creates new pod
├─ Old pod IP: 10.244.1.5
├─ New pod IP: 10.244.1.8 (different!)
│
├─ Other services trying to reach old IP: 10.244.1.5
├─ But pod is gone
├─ Requests fail
└─ Need stable IP that doesn't change when pod dies

Solution: Service

├─ Service has stable IP: 10.96.0.5
├─ Doesn't change when pods change
├─ Forwards traffic to current pod IPs
├─ When pod dies and new one created:
│  ├─ New pod gets different IP
│  ├─ Service updates routing
│  ├─ Old IP still works (service IP)
│  └─ No code changes needed
└─ Pods are ephemeral, but service IP is stable
```

### Service Types

```
1. ClusterIP (Default)
   ├─ IP only accessible within cluster
   ├─ No external access
   ├─ Port 80 inside → 8080 on pod
   ├─ DNS: product-api.default.svc.cluster.local
   └─ Use case: Pod-to-pod communication

2. NodePort
   ├─ Accessible from outside cluster
   ├─ Uses node IP + high port (30000-32767)
   ├─ Example: 192.168.1.5:30123 → service
   ├─ Service routes to pod IP:port
   └─ Use case: Development, testing

3. LoadBalancer
   ├─ Cloud provider provisions load balancer
   ├─ External IP assigned
   ├─ Example: 203.0.113.5:80 (public IP)
   ├─ Load balancer forwards to NodePort
   ├─ NodePort forwards to Service
   ├─ Service forwards to pods
   └─ Use case: Production, public APIs

4. ExternalName
   ├─ Maps Kubernetes service to external DNS
   ├─ Example: mydb.example.com
   ├─ When pod connects to service
   ├─ Actually connects to external DNS
   └─ Use case: Connecting to external databases
```

### Service YAML

```yaml
apiVersion: v1
kind: Service
metadata:
  name: product-api
  namespace: default
  labels:
    app: product-api

spec:
  # Type of service
  type: ClusterIP
  
  # Which pods does this service route to?
  selector:
    app: product-api  # Matches deployment pods with this label
  
  # Ports
  ports:
    # External port (inside cluster)
    - port: 80
      # Port on pod
      targetPort: 8080
      # Protocol
      protocol: TCP
      # Optional name
      name: http
  
  # For LoadBalancer type
  sessionAffinity: None  # or ClientIP for sticky sessions
  sessionAffinityConfig:
    clientIP:
      timeoutSeconds: 10800
```

### How Services Route Traffic

```
Request Journey:

Client Pod (10.244.2.3) wants to call product-api

1. Client makes request
   └─ curl http://product-api/api/products

2. Client DNS lookup
   ├─ What is IP of "product-api"?
   ├─ Asks cluster DNS (CoreDNS)
   ├─ CoreDNS: "product-api = 10.96.0.5"
   └─ CoreDNS returns: 10.96.0.5

3. Client connects to service IP
   └─ curl http://10.96.0.5:80/api/products

4. Packet arrives at host network
   ├─ Destination: 10.96.0.5:80 (service IP)
   ├─ This IP doesn't really exist (virtual)
   ├─ Kernel: "This looks like a service"
   │  └─ (kube-proxy configured iptables)
   │
   └─ iptables rule: "DNAT to pod IP"

5. iptables rewrites packet
   ├─ Original: 10.96.0.5:80
   ├─ Service has 3 endpoints (3 pods):
   │  ├─ Pod1: 10.244.1.5:8080
   │  ├─ Pod2: 10.244.1.6:8080
   │  └─ Pod3: 10.244.2.7:8080
   │
   ├─ iptables picks one (round robin):
   │  └─ This time: Pod2 (10.244.1.6:8080)
   │
   ├─ Rewrites packet:
   │  └─ Destination: 10.244.1.6:8080
   │
   └─ Forwards packet

6. Packet reaches pod
   ├─ Pod receives request
   ├─ Response goes to pod IP: 10.244.1.6
   └─ Pod sends response

7. Response intercepted by iptables
   ├─ iptables sees response from pod
   ├─ Rewrites source address back
   │  └─ From: 10.244.1.6:8080
   │  └─ To: 10.96.0.5:80
   │
   └─ Forwards response

8. Client receives response
   ├─ Response appears to come from 10.96.0.5:80
   ├─ Client doesn't know about pod IP
   ├─ Transparent to client
   ├─ Client can call service IP again
   ├─ Might get routed to different pod
   └─ Load-balanced!

Load Balancing:
├─ Each request → random endpoint
├─ Over many requests → even distribution
├─ If pod dies → removed from endpoints
└─ Traffic automatically flows to healthy pods
```

---

# 10. ADVANCED RESOURCE MANAGEMENT

## Resource Requests and Limits

### Requests vs Limits

```
Container Resource Specification:

resources:
  requests:
    cpu: 100m         # Guaranteed
    memory: 256Mi
  limits:
    cpu: 500m         # Maximum
    memory: 512Mi

Analogy - Restaurant Reservation:
├─ Request: "I'm reserving a table for 4 people"
│  ├─ Restaurant guarantees space
│  ├─ Table reserved, not available to others
│  └─ You're guaranteed to get it
│
└─ Limit: "But we can squeeze up to 10 people"
   ├─ If we have extra space
   ├─ More people can sit
   ├─ But only if space available
   └─ Never more than 10 (hard limit)

Kubernetes Scheduling:
├─ Scheduler looks at requests
│  ├─ "I need 100m CPU and 256Mi memory"
│  ├─ "Which node has 100m + 256Mi free?"
│  ├─ Only schedules if available
│  └─ Guarantees availability
│
└─ Limits control runtime behavior
   ├─ CPU limit: Throttles if exceeded
   ├─ Memory limit: Kills container if exceeded
   └─ Protects node from overload
```

### CPU Units

```
CPU notation:
├─ 1000m = 1 CPU (one core)
├─ 100m = 0.1 CPU
├─ 500m = 0.5 CPU
└─ Can request fractional CPUs

Examples:
├─ Single-threaded app: 100m-250m
├─ Multi-threaded app: 250m-1000m
├─ Heavy computation: 1000m-4000m

In container:
├─ If request: 100m
├─ Container can use up to 100m of CPU
├─ Shared with other containers
│  └─ 10 containers × 100m = 1 CPU (if running simultaneously)
│
├─ If limit: 500m
│  ├─ Container might use 200m, 350m, 500m
│  ├─ Never exceeds 500m
│  └─ OS throttles if tries to exceed

What does "using 500m" mean?
├─ Container threads accumulate ~500m usage
├─ In time window (1 second)
├─ Single-threaded app: Can't use multiple cores
├─ Multi-threaded app: Can use multiple cores
└─ Example: 500m = 0.5 seconds out of 1 second actively running
```

### Memory Units

```
Memory notation:
├─ Mi = Mebibytes (2^20 bytes) = 1,048,576 bytes
├─ Gi = Gibibytes (2^30 bytes)
├─ M = Megabytes (10^6 bytes) - not recommended
├─ G = Gigabytes (10^9 bytes) - not recommended

Always use Mi/Gi for Kubernetes!

Examples:
├─ 256Mi = 268 MB (mebibytes)
├─ 512Mi = 536 MB
├─ 1Gi = 1024 Mi = 1.1 GB
└─ 2Gi = 2048 Mi = 2.1 GB

Typical sizes:
├─ Simple Node.js app: 256Mi-512Mi
├─ Python Flask app: 256Mi-512Mi
├─ Java Spring Boot: 512Mi-1Gi
├─ .NET Core app: 256Mi-512Mi
└─ Heavy analytics: 2Gi-4Gi+

Memory behavior:
├─ If request: 256Mi
│  └─ Container can use up to 256Mi
│
├─ If usage < request
│  └─ Node reserves memory
│  └─ Available for this container
│
├─ If limit: 512Mi
│  ├─ Container cannot exceed 512Mi
│  ├─ If tries: OOMKilled (Out of Memory)
│  ├─ Container process killed
│  ├─ Pod marked Failed
│  └─ Restarted (if restart policy allows)
```

### QoS Classes

Kubernetes uses Quality of Service to decide which pods to evict when resources tight.

```
Three QoS Classes:

1. Guaranteed (Highest Priority)
   ├─ Condition: requests == limits (for both CPU and memory)
   ├─ Example:
   │  └─ resources:
   │       requests:
   │         cpu: 500m
   │         memory: 512Mi
   │       limits:
   │         cpu: 500m
   │         memory: 512Mi
   │
   ├─ Behavior:
   │  ├─ Never evicted due to resource pressure
   │  ├─ Evicted last when node must be drained
   │  └─ Reserved resources guaranteed
   │
   └─ Use case: Critical services, databases

2. Burstable (Medium Priority)
   ├─ Condition: requests < limits (or only requests, no limits)
   ├─ Example:
   │  └─ resources:
   │       requests:
   │         cpu: 100m
   │         memory: 256Mi
   │       limits:
   │         cpu: 500m
   │         memory: 512Mi
   │
   ├─ Behavior:
   │  ├─ Can use more than requested (up to limit)
   │  ├─ Evicted if node runs out of resources
   │  ├─ Evicted before Best-Effort
   │  └─ Flexible resource usage
   │
   └─ Use case: Most applications

3. Best-Effort (Lowest Priority)
   ├─ Condition: No requests, no limits specified
   ├─ Example:
   │  └─ resources: {}  (or omitted)
   │
   ├─ Behavior:
   │  ├─ Can use all available resources
   │  ├─ First to be evicted if node tight
   │  ├─ No guarantee of availability
   │  └─ Useful for non-critical batch jobs
   │
   └─ Use case: Batch jobs, development
```

### Eviction Policy

When a node runs out of resources, Kubernetes evicts pods.

```
Node Resource Pressure:

Node has 16GB memory
├─ Available: 12GB
├─ Allocated (requests): 3GB
├─ Used by system: 1GB
└─ Total: 16GB (full!)

Node reaches eviction threshold:
├─ Available < 10% of total (1.6GB)
├─ Node enters "memory pressure"
├─ kubelet: "I must free up memory"

Eviction Order:

1. Best-Effort pods with highest memory usage
   └─ First to go

2. Burstable pods with memory usage > request
   └─ Using more than they requested
   └─ Next to go

3. Burstable pods with memory usage == request
   └─ Only if still not enough memory
   └─ Last burstable to go

4. Guaranteed pods
   └─ Never evicted (unless node is cordoned)
   └─ Final last resort only

Eviction Process:
├─ kubelet marks pod for deletion
├─ Pod receives termination grace period (30 seconds)
├─ Pod can clean up gracefully
├─ If still running: forcefully killed
└─ Pod deleted, restart based on restart policy

Prevention:
├─ Set appropriate requests/limits
├─ Don't overcommit (sum of requests > node capacity)
├─ Monitor resource usage
├─ Use HPA to scale before hitting limits
└─ Use PodDisruptionBudgets to ensure availability
```

---

# 11. STORAGE IN KUBERNETES

## Storage Concepts

### Volumes

A Volume is a directory accessible to containers in a Pod.

```
Without Volumes:
├─ Container filesystem is ephemeral
├─ Data written to /app/data
├─ Container dies/restarts
├─ /app/data is lost
├─ No persistent data
└─ Problem for databases, uploads, etc.

With Volumes:
├─ Container can mount a volume
├─ Write data to mounted directory
├─ Data persists on volume storage
├─ Container dies, data remains
├─ New container mounts same volume
├─ Data available again
└─ Persistent data achieved!

Volume Lifecycle:
├─ Created when pod created
├─ Data stored on underlying storage (disk, network, cloud)
├─ Container mounts volume in its filesystem
├─ Container accesses data like normal filesystem
├─ Deleted when pod deleted (depends on volume type)
```

### Volume Types

```
emptyDir: Temporary storage
├─ Created when pod created
├─ Initially empty
├─ Shared among containers in pod
├─ Survives container restart (but not pod restart)
├─ Deleted when pod deleted
├─ Use case: Temporary cache, scratch space

Example:
volumes:
- name: temp-storage
  emptyDir: {}

hostPath: Mount from host node
├─ Mounts directory from node filesystem
├─ Useful for local development
├─ Data persists on host machine
├─ Don't use in production (pod might move to different node)
├─ Use case: Local development with KIND

Example:
volumes:
- name: host-storage
  hostPath:
    path: /tmp/k8s-storage
    type: Directory

configMap: Configuration data
├─ Mounts ConfigMap as files
├─ Each key → one file
├─ Values → file contents
├─ Read-only (by default)
├─ Use case: Configuration files, scripts

Example:
volumes:
- name: config
  configMap:
    name: app-config
    items:
    - key: app.conf
      path: application.conf

secret: Sensitive data
├─ Like ConfigMap but for secrets
├─ Data base64 encoded
├─ Should be encrypted at rest (not default)
├─ Use case: Passwords, API keys, tokens

Example:
volumes:
- name: credentials
  secret:
    secretName: db-credentials
    items:
    - key: password
      path: db-password.txt

emptyDir + encryption: Encrypted temporary storage
├─ Like emptyDir but encrypted
├─ Sensitive temporary data
├─ Use case: Processing sensitive information temporarily

downwardAPI: Pod information
├─ Exposes pod metadata as files
├─ Pod name, namespace, labels, resource limits
├─ Read-only
├─ Use case: App needs to know about itself

Example:
volumes:
- name: pod-info
  downwardAPI:
    items:
    - path: "pod_name"
      fieldRef:
        fieldPath: metadata.name
```

### PersistentVolumes (PV) and PersistentVolumeClaims (PVC)

For production persistent storage, use PV/PVC.

```
Architecture:

Cluster Admin creates PersistentVolumes (PVs):
├─ Represents actual storage
├─ AWS EBS, Google Persistent Disk, NFS, etc.
├─ 10GB disk in AWS
├─ Created cluster-wide
├─ Lifecycle independent of pods

Cluster User creates PersistentVolumeClaim (PVC):
├─ Request for storage
├─ "I need 5GB storage"
├─ Doesn't care where from
├─ Kubernetes matches to PV
├─ Lifecycle tied to application

Pod uses PVC:
├─ References PVC by name
├─ PVC provides storage
├─ Container accesses normally

Example Workflow:

Admin: Create PV
└─ apiVersion: v1
   kind: PersistentVolume
   metadata:
     name: pv-database
   spec:
     capacity:
       storage: 100Gi
     accessModes:
       - ReadWriteOnce
     awsElasticBlockStore:
       volumeID: vol-123456
       fsType: ext4

Developer: Create PVC
└─ apiVersion: v1
   kind: PersistentVolumeClaim
   metadata:
     name: database-storage
   spec:
     accessModes:
       - ReadWriteOnce
     resources:
       requests:
         storage: 50Gi

Kubernetes: Matches PVC to PV
├─ PVC requesting 50Gi
├─ PV offering 100Gi
├─ Match! PV bound to PVC

Developer: Use PVC in Pod
└─ spec:
     volumes:
     - name: db-storage
       persistentVolumeClaim:
         claimName: database-storage
     containers:
     - name: db
       volumeMounts:
       - name: db-storage
         mountPath: /var/lib/postgresql

Pod uses storage:
├─ Mounts PVC as /var/lib/postgresql
├─ Writes data to underlying storage
├─ Data persists on storage
└─ Even if pod deleted
```

---

# 12. CONFIGMAPS AND SECRETS

ConfigMaps and Secrets allow you to decouple configuration data from application code, enabling you to manage environment-specific settings and sensitive information separately from your container images.

## ConfigMaps: Non-Sensitive Configuration

ConfigMaps store non-sensitive configuration data as key-value pairs that can be consumed by pods as environment variables, command-line arguments, or configuration files mounted as volumes.

### Creating ConfigMaps

```
Method 1: From literal values

kubectl create configmap app-config \
  --from-literal=LOG_LEVEL=INFO \
  --from-literal=MAX_CONNECTIONS=100 \
  --from-literal=ENVIRONMENT=production

Method 2: From file

# File: app.properties
# LOG_LEVEL=INFO
# MAX_CONNECTIONS=100

kubectl create configmap app-config \
  --from-file=app.properties

Method 3: From directory

# Directory: /etc/app/
# ├─ database.conf
# ├─ server.conf
# ├─ cache.conf

kubectl create configmap app-config \
  --from-file=/etc/app/

Method 4: From YAML

apiVersion: v1
kind: ConfigMap
metadata:
  name: app-config
  namespace: default
data:
  LOG_LEVEL: INFO
  MAX_CONNECTIONS: "100"
  ENVIRONMENT: production
  app.properties: |
    log.format=json
    log.output=stdout
```

### Using ConfigMaps in Pods

```
Method 1: Environment variables

spec:
  containers:
  - name: app
    image: myapp:1.0
    env:
    - name: LOG_LEVEL
      valueFrom:
        configMapKeyRef:
          name: app-config
          key: LOG_LEVEL

Result:
├─ Container has env var LOG_LEVEL
├─ Value: INFO (from ConfigMap)
├─ Can access in code: getenv("LOG_LEVEL")

Method 2: Mount as file

spec:
  containers:
  - name: app
    image: myapp:1.0
    volumeMounts:
    - name: config
      mountPath: /etc/config
  volumes:
  - name: config
    configMap:
      name: app-config
      items:
      - key: app.properties
        path: application.properties

Result:
├─ File /etc/config/application.properties in container
├─ Contents: value of app.properties key from ConfigMap
├─ App can read file: open("/etc/config/application.properties")

Method 3: Load all keys as env vars

spec:
  containers:
  - name: app
    image: myapp:1.0
    envFrom:
    - configMapRef:
        name: app-config

Result:
├─ Every key in ConfigMap → env var
├─ LOG_LEVEL → $LOG_LEVEL
├─ MAX_CONNECTIONS → $MAX_CONNECTIONS
├─ All available in container
```

## Secrets: Sensitive Data

Secrets store sensitive information like passwords, API keys, and certificates in a base64-encoded format, providing a secure way to pass confidential data to pods without exposing it in plain text in your configuration files.

### Creating Secrets

```
Method 1: Generic secret from literals

kubectl create secret generic db-secret \
  --from-literal=username=admin \
  --from-literal=password=supersecret123

Method 2: From file

# File: db-password.txt
# supersecret123

kubectl create secret generic db-secret \
  --from-file=password=db-password.txt

Method 3: Docker registry secret (for pulling images)

kubectl create secret docker-registry registry-secret \
  --docker-server=gcr.io \
  --docker-username=_json_key \
  --docker-password=$(cat ~/key.json) \
  --docker-email=user@example.com

Method 4: TLS certificate secret

kubectl create secret tls tls-secret \
  --cert=path/to/cert.pem \
  --key=path/to/key.pem

Method 5: From YAML

apiVersion: v1
kind: Secret
metadata:
  name: db-secret
  namespace: default
type: Opaque
stringData:
  username: admin
  password: supersecret123

Note: stringData is not base64 encoded
Don't use stringData in real files!
Use: data: (base64 encoded) instead
```

### Using Secrets

```
Same as ConfigMaps, but more secure

Method 1: As environment variables

spec:
  containers:
  - name: app
    env:
    - name: DB_PASSWORD
      valueFrom:
        secretKeyRef:
          name: db-secret
          key: password

Method 2: As mounted files

spec:
  volumes:
  - name: secret-volume
    secret:
      secretName: db-secret
  containers:
  - name: app
    volumeMounts:
    - name: secret-volume
      mountPath: /etc/secrets
      readOnly: true

Result:
├─ /etc/secrets/username (content: admin)
├─ /etc/secrets/password (content: supersecret123)
└─ Read-only, encrypted transport

Method 3: For image pull

spec:
  imagePullSecrets:
  - name: registry-secret
  containers:
  - name: app
    image: gcr.io/myproject/myapp:1.0

Result:
├─ Kubernetes uses registry-secret
├─ Authenticates to gcr.io
├─ Pulls image successfully
└─ Works with private registries
```

### Secrets Best Practices

```
✓ DO:
├─ Store in volume (not env var if possible)
│  └─ Env vars visible in kubectl describe
├─ Mount read-only
│  └─ Prevents accidental modification
├─ Use RBAC to limit secret access
│  └─ Not all users can read all secrets
├─ Enable encryption at rest
│  └─ etcd encryption (requires configuration)
├─ Rotate secrets regularly
│  └─ Change passwords, update certificates
├─ Use external secret management
│  └─ HashiCorp Vault, AWS Secrets Manager
└─ Use TLS for secret transmission
   └─ Kubernetes API uses TLS by default

✗ DON'T:
├─ Commit secrets to Git!
│  └─ Major security vulnerability
├─ Print secrets in logs
│  └─ Visible to anyone with log access
├─ Store in ConfigMap
│  └─ Not encrypted, not intended for secrets
├─ Use base64 as encryption
│  └─ Base64 is just encoding, not encryption
│  └─ Anyone can decode it
├─ Share secrets via email/chat
│  └─ Insecure transmission
└─ Hard-code in images
   └─ Visible when inspecting image
```

### Real-World Example: ProductApi with PostgreSQL and Authentication

Let's implement a complete microservices setup using both ConfigMaps and Secrets.

**Scenario:**
- **ProductApi** - Stores products in PostgreSQL database
- **OrderApi** - Consumes ProductApi with API key authentication
- **PostgreSQL** - Database requiring username/password
- **Secrets** - Database credentials and API keys
- **ConfigMaps** - Non-sensitive configuration (ports, hosts, environment)

#### Step 1: Create Secrets for Sensitive Data

```yaml
# secret-postgres.yaml
apiVersion: v1
kind: Secret
metadata:
  name: postgres-secret
  namespace: microservices
type: Opaque
stringData:
  POSTGRES_USER: productuser
  POSTGRES_PASSWORD: productpass123
  POSTGRES_DB: productdb
  DB_CONNECTION_STRING: "Host=postgres;Port=5432;Database=productdb;Username=productuser;Password=productpass123"
```

```yaml
# secret-api-auth.yaml
apiVersion: v1
kind: Secret
metadata:
  name: api-auth-secret
  namespace: microservices
type: Opaque
stringData:
  API_KEY: product-api-secret-key-12345
```

**Why Secrets?**
- Database credentials are **sensitive** - shouldn't be in ConfigMaps or Git
- API keys provide **authentication** - must be protected
- Secrets are **base64-encoded** in etcd and have access controls

#### Step 2: Create ConfigMaps for Non-Sensitive Config

```yaml
# configmap-product-api.yaml
apiVersion: v1
kind: ConfigMap
metadata:
  name: product-api-config
  namespace: microservices
data:
  PORT: "8080"
  ASPNETCORE_ENVIRONMENT: "Production"
  ASPNETCORE_URLS: "http://+:8080"
  DB_HOST: "postgres"
  DB_PORT: "5432"
```

```yaml
# configmap-order-api.yaml
apiVersion: v1
kind: ConfigMap
metadata:
  name: order-api-config
  namespace: microservices
data:
  PORT: "8081"
  ASPNETCORE_ENVIRONMENT: "Production"
  ASPNETCORE_URLS: "http://+:8081"
  PRODUCT_API_URL: "http://product-api"
```

**Why ConfigMaps?**
- Port numbers, hostnames are **not sensitive**
- Environment name (Production/Development) is **public knowledge**
- Service URLs are **discoverable** via Kubernetes DNS
- Can be committed to Git safely

#### Step 3: Deploy PostgreSQL StatefulSet

```yaml
# statefulset-postgres.yaml
apiVersion: apps/v1
kind: StatefulSet
metadata:
  name: postgres
  namespace: microservices
spec:
  serviceName: postgres
  replicas: 1
  selector:
    matchLabels:
      app: postgres
  template:
    metadata:
      labels:
        app: postgres
    spec:
      containers:
      - name: postgres
        image: postgres:15-alpine
        ports:
        - containerPort: 5432
        env:
        # All from Secret - sensitive data
        - name: POSTGRES_USER
          valueFrom:
            secretKeyRef:
              name: postgres-secret
              key: POSTGRES_USER
        - name: POSTGRES_PASSWORD
          valueFrom:
            secretKeyRef:
              name: postgres-secret
              key: POSTGRES_PASSWORD
        - name: POSTGRES_DB
          valueFrom:
            secretKeyRef:
              name: postgres-secret
              key: POSTGRES_DB
        volumeMounts:
        - name: postgres-data
          mountPath: /var/lib/postgresql/data
        resources:
          requests:
            memory: "256Mi"
            cpu: "100m"
          limits:
            memory: "512Mi"
            cpu: "500m"
        livenessProbe:
          exec:
            command: ["pg_isready", "-U", "productuser", "-d", "productdb"]
          initialDelaySeconds: 30
          periodSeconds: 10
  volumeClaimTemplates:
  - metadata:
      name: postgres-data
    spec:
      accessModes: ["ReadWriteOnce"]
      resources:
        requests:
          storage: 1Gi
```

```yaml
# service-postgres.yaml
apiVersion: v1
kind: Service
metadata:
  name: postgres
  namespace: microservices
spec:
  type: ClusterIP
  selector:
    app: postgres
  ports:
  - port: 5432
    targetPort: 5432
```

**Why StatefulSet?**
- Databases need **stable network identity** (always same DNS name)
- **Persistent volumes** survive pod restarts
- **Ordered deployment** ensures data consistency
- Unlike Deployment, pods get unique identifiers (postgres-0, postgres-1)

#### Step 4: Deploy ProductApi with ConfigMap + Secrets

```yaml
# deployment-product-api.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: product-api
  namespace: microservices
spec:
  replicas: 2
  selector:
    matchLabels:
      app: product-api
  template:
    metadata:
      labels:
        app: product-api
    spec:
      containers:
      - name: product-api
        image: product-api:1.0.0
        imagePullPolicy: IfNotPresent
        ports:
        - containerPort: 8080
        # Load all ConfigMap keys as env vars
        envFrom:
        - configMapRef:
            name: product-api-config
        env:
        # Individual secrets as env vars
        - name: DB_USER
          valueFrom:
            secretKeyRef:
              name: postgres-secret
              key: POSTGRES_USER
        - name: DB_PASSWORD
          valueFrom:
            secretKeyRef:
              name: postgres-secret
              key: POSTGRES_PASSWORD
        - name: DB_NAME
          valueFrom:
            secretKeyRef:
              name: postgres-secret
              key: POSTGRES_DB
        - name: API_KEY
          valueFrom:
            secretKeyRef:
              name: api-auth-secret
              key: API_KEY
        resources:
          requests:
            memory: "256Mi"
            cpu: "100m"
          limits:
            memory: "512Mi"
            cpu: "500m"
        livenessProbe:
          httpGet:
            path: /api/products/health
            port: 8080
          initialDelaySeconds: 30
          periodSeconds: 10
        readinessProbe:
          httpGet:
            path: /api/products/health
            port: 8080
          initialDelaySeconds: 10
          periodSeconds: 5
```

**Configuration Strategy:**
1. **ConfigMap (envFrom)** - Loads all non-sensitive config at once
   - PORT, ASPNETCORE_ENVIRONMENT, DB_HOST, DB_PORT
2. **Secrets (env)** - Individual sensitive values
   - DB_USER, DB_PASSWORD, DB_NAME, API_KEY
3. **Combination** gives complete configuration

#### Step 5: Deploy OrderApi with Authentication

```yaml
# deployment-order-api.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: order-api
  namespace: microservices
spec:
  replicas: 2
  selector:
    matchLabels:
      app: order-api
  template:
    metadata:
      labels:
        app: order-api
    spec:
      containers:
      - name: order-api
        image: order-api:1.0.0
        imagePullPolicy: IfNotPresent
        ports:
        - containerPort: 8081
        envFrom:
        - configMapRef:
            name: order-api-config
        env:
        # API Key to authenticate with ProductApi
        - name: PRODUCT_API_KEY
          valueFrom:
            secretKeyRef:
              name: api-auth-secret
              key: API_KEY
        resources:
          requests:
            memory: "256Mi"
            cpu: "100m"
          limits:
            memory: "512Mi"
            cpu: "500m"
        livenessProbe:
          httpGet:
            path: /api/orders/health
            port: 8081
          initialDelaySeconds: 30
          periodSeconds: 10
```

#### Deployment Order and Dependencies

```bash
# 1. Create namespace
kubectl apply -f namespace.yaml

# 2. Create secrets FIRST (before pods need them)
kubectl apply -f secret-postgres.yaml
kubectl apply -f secret-api-auth.yaml

# 3. Create ConfigMaps
kubectl apply -f configmap-product-api.yaml
kubectl apply -f configmap-order-api.yaml

# 4. Deploy PostgreSQL and wait for ready
kubectl apply -f statefulset-postgres.yaml
kubectl apply -f service-postgres.yaml
kubectl wait --for=condition=ready pod -l app=postgres -n microservices --timeout=120s

# 5. Deploy ProductApi (depends on PostgreSQL)
kubectl apply -f deployment-product-api.yaml
kubectl apply -f service-product-api.yaml

# 6. Deploy OrderApi (depends on ProductApi)
kubectl apply -f deployment-order-api.yaml
kubectl apply -f service-order-api.yaml
```

#### Verifying the Setup

```bash
# Check all resources
kubectl get all -n microservices

# Expected output:
NAME                               READY   STATUS    RESTARTS   AGE
pod/postgres-0                     1/1     Running   0          2m
pod/product-api-xxxxx-yyyyy        1/1     Running   0          1m
pod/product-api-xxxxx-zzzzz        1/1     Running   0          1m
pod/order-api-xxxxx-yyyyy          1/1     Running   0          30s
pod/order-api-xxxxx-zzzzz          1/1     Running   0          30s

# Check secrets (values are redacted)
kubectl get secrets -n microservices
kubectl describe secret postgres-secret -n microservices
# Data shows keys, not values (base64 encoded in etcd)

# Check ConfigMaps (values are visible)
kubectl get configmaps -n microservices
kubectl describe configmap product-api-config -n microservices
# Data shows actual values (not sensitive)

# Check environment variables in pod
kubectl exec -it deployment/product-api -n microservices -- env | grep -E 'DB_|API_|PORT|ASPNETCORE'
# Shows values from both ConfigMap and Secret

# Test ProductApi database connection
kubectl port-forward svc/product-api 8080:80 -n microservices
curl http://localhost:8080/api/products
# Should return products from PostgreSQL

# Test OrderApi authentication with ProductApi
kubectl port-forward svc/order-api 8081:80 -n microservices
curl http://localhost:8081/api/orders
# Should successfully call ProductApi with API_KEY
```

#### Key Learnings

**When to use ConfigMaps:**
- Port numbers, timeouts, retry limits
- Service URLs (Kubernetes DNS names)
- Feature flags (enabled/disabled)
- Log levels (DEBUG, INFO, WARN)
- Environment names (dev, staging, prod)
- Non-sensitive connection parameters

**When to use Secrets:**
- Database usernames and passwords
- API keys and tokens
- OAuth client IDs and secrets
- TLS certificates and private keys
- SSH keys
- Service account credentials
- Encryption keys

**Best Practices Applied:**
1. **Separation of Concerns** - ConfigMaps ≠ Secrets
2. **Least Privilege** - Only pods that need secrets get them
3. **Read-Only Mounts** - Secrets mounted as read-only volumes (when using volumes)
4. **Environment-Specific** - Same chart/manifests, different values per environment
5. **No Git Commits** - Secrets managed separately (not in version control)
6. **Service Mesh Ready** - Can add mTLS later without changing configs

---



# 13. HELM: PACKAGE MANAGEMENT MASTERY

## Understanding Helm

Helm is the **package manager for Kubernetes**, often described as "the apt/yum/homebrew for Kubernetes." It simplifies deploying, managing, and versioning complex Kubernetes applications by packaging all necessary resources into reusable, configurable units called **charts**.

### What is Helm?

**Helm** is a tool that:
- **Packages** multiple Kubernetes manifests into a single deployable unit
- **Templatizes** YAML files with variables for reusability across environments
- **Manages releases** with versioning, upgrades, and rollbacks
- **Handles dependencies** between applications and services
- **Shares** pre-built packages via repositories (like Docker Hub for charts)

**Key Concepts:**

1. **Chart** - A Helm package containing all Kubernetes resource definitions
   - Like a `.deb` package in Ubuntu or a formula in Homebrew
   - Contains templates, default values, metadata, and dependencies

2. **Release** - An instance of a chart deployed to a Kubernetes cluster
   - `helm install myapp-prod ./myapp-chart` creates a release named "myapp-prod"
   - You can deploy the same chart multiple times with different release names
   - Example: `myapp-dev`, `myapp-staging`, `myapp-prod` all from the same chart

3. **Repository** - A collection of charts stored and shared
   - Public repos: Artifact Hub, Bitnami, Elastic, etc.
   - Private repos: ChartMuseum, Harbor, cloud providers
   - Like npm registry for Node.js or PyPI for Python

4. **Values** - Configuration parameters that customize chart behavior
   - Define in `values.yaml` (defaults)
   - Override with custom files or CLI flags
   - Same chart + different values = different environments

**Helm Architecture:**

```
┌─────────────────────────────────────────────────────────┐
│                     Helm Client                         │
│  (CLI tool running on your machine)                     │
│                                                          │
│  Commands:                                               │
│  • helm install    - Deploy a chart                     │
│  • helm upgrade    - Update a release                   │
│  • helm rollback   - Revert to previous version         │
│  • helm uninstall  - Remove a release                   │
│  • helm list       - Show all releases                  │
└──────────────────┬──────────────────────────────────────┘
                   │
                   │ Communicates via Kubernetes API
                   ▼
┌─────────────────────────────────────────────────────────┐
│              Kubernetes API Server                      │
│                                                          │
│  Helm stores release info as Kubernetes Secrets         │
│  (in namespace where release is deployed)               │
│                                                          │
│  Release History:                                        │
│  • myapp v1 (deployed)                                  │
│  • myapp v2 (deployed)                                  │
│  • myapp v3 (current)                                   │
└─────────────────────────────────────────────────────────┘
```

**Helm 3 vs Helm 2:**

Helm 3 (current version) removed **Tiller** (server-side component) from Helm 2:
- ✅ More secure (no cluster-wide permissions needed)
- ✅ Simpler architecture (client-only)
- ✅ Release data stored as Kubernetes Secrets (no separate database)
- ✅ Better RBAC integration

### The Problem Helm Solves

```
Without Helm:

Deploy an application:
├─ Create deployment.yaml
├─ Create service.yaml
├─ Create configmap.yaml
├─ Create secret.yaml
├─ Create pvc.yaml
├─ Deploy all: kubectl apply -f *.yaml

Deploy to different environment:
├─ Copy all files
├─ Edit image tags (dev → prod)
├─ Edit replica counts
├─ Edit resource limits
├─ Edit environment variables
├─ Edit service types
├─ Manual editing, error-prone

Deploy multiple versions:
├─ Need to track many configuration variations
├─ Hard to manage dependencies between services
├─ No built-in versioning or release management
├─ Rollback is manual (remember what you changed?)
└─ Scaling teams is difficult

Solution: Helm

├─ Package all YAML into single unit
├─ Parameterize configuration
├─ Same chart → different values for dev/prod
├─ Built-in templating
├─ Dependency management
├─ Version history and rollback
└─ Like npm, pip for Kubernetes!
```

### Helm Chart Structure

```
Helm Chart = Package containing:

myapp/
├─ Chart.yaml              # Metadata about chart
│  ├─ name: myapp
│  ├─ version: 1.0.0       # Chart version
│  ├─ appVersion: 2.1      # App version
│  └─ description: My app
│
├─ values.yaml             # Default configuration values
│  ├─ image: myapp:2.1
│  ├─ replicas: 3
│  ├─ cpu: 100m
│  └─ ... many more settings
│
├─ templates/              # Kubernetes YAML templates
│  ├─ deployment.yaml      # Parameterized deployment
│  ├─ service.yaml         # Parameterized service
│  ├─ configmap.yaml       # Parameterized configmap
│  ├─ _helpers.tpl         # Template functions
│  └─ notes.txt            # Post-install instructions
│
├─ charts/                 # Dependencies (other charts)
│  ├─ postgresql/          # Database chart
│  └─ redis/               # Cache chart
│
├─ README.md               # Documentation
└─ values-dev.yaml         # Dev environment values
   values-prod.yaml        # Prod environment values

Total size: ~50KB
One chart deployment: ~10 files
```

### Values and Templating

**values.yaml** - Default values

```yaml
# Default configuration
replicaCount: 2

image:
  repository: myapp
  tag: "2.1"
  pullPolicy: IfNotPresent

service:
  type: ClusterIP
  port: 80
  targetPort: 8080

resources:
  limits:
    cpu: 500m
    memory: 512Mi
  requests:
    cpu: 100m
    memory: 256Mi

autoscaling:
  enabled: true
  minReplicas: 2
  maxReplicas: 5
  targetCPUUtilizationPercentage: 70

environment:
  LOG_LEVEL: INFO
  ENVIRONMENT: production
```

**templates/deployment.yaml** - Parameterized template

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: {{ include "myapp.fullname" . }}
  labels:
    {{- include "myapp.labels" . | nindent 4 }}

spec:
  {{- if not .Values.autoscaling.enabled }}
  replicas: {{ .Values.replicaCount }}
  {{- end }}
  
  selector:
    matchLabels:
      {{- include "myapp.selectorLabels" . | nindent 6 }}
  
  template:
    metadata:
      labels:
        {{- include "myapp.selectorLabels" . | nindent 8 }}
    
    spec:
      containers:
      - name: {{ .Chart.Name }}
        image: "{{ .Values.image.repository }}:{{ .Values.image.tag }}"
        imagePullPolicy: {{ .Values.image.pullPolicy }}
        
        ports:
        - name: http
          containerPort: {{ .Values.service.targetPort }}
        
        env:
        {{- range $key, $val := .Values.environment }}
        - name: {{ $key }}
          value: {{ $val | quote }}
        {{- end }}
        
        resources:
          {{- toYaml .Values.resources | nindent 10 }}

{{- if .Values.autoscaling.enabled }}
---
apiVersion: autoscaling/v2
kind: HorizontalPodAutoscaler
metadata:
  name: {{ include "myapp.fullname" . }}
spec:
  scaleTargetRef:
    apiVersion: apps/v1
    kind: Deployment
    name: {{ include "myapp.fullname" . }}
  minReplicas: {{ .Values.autoscaling.minReplicas }}
  maxReplicas: {{ .Values.autoscaling.maxReplicas }}
  metrics:
  - type: Resource
    resource:
      name: cpu
      target:
        type: Utilization
        averageUtilization: {{ .Values.autoscaling.targetCPUUtilizationPercentage }}
{{- end }}
```

### Helm Commands

```
Install chart:
helm install myrelease ./myapp/
# Deployment: 2 replicas, image: myapp:2.1, cpu: 100m, ...

Install with custom values:
helm install myrelease ./myapp/ -f values-prod.yaml
# Deployment: 3 replicas, image: myapp:2.1.5, cpu: 250m, ...

Upgrade release:
helm upgrade myrelease ./myapp/ --set image.tag=2.2.0
# Updates deployment to new image

Check values being used:
helm get values myrelease
# Shows actual values (merged defaults + custom)

See rendered templates:
helm template myrelease ./myapp/
# Shows actual YAML (without deploying)

List releases:
helm list
# Shows all deployed releases, versions, status

Rollback to previous version:
helm rollback myrelease 1
# Returns to previous deployment

Release history:
helm history myrelease
# Shows all deployment attempts
# Easy to see what changed and when

Uninstall:
helm uninstall myrelease
# Removes deployment, all resources
```

### values-dev.yaml vs values-prod.yaml

```
values-dev.yaml:
├─ replicas: 1
│  └─ One copy for development
├─ resources: very small (100m CPU, 256Mi memory)
│  └─ Cheaper, faster
├─ image.tag: dev-latest
│  └─ Latest development build
├─ autoscaling: disabled
│  └─ Single instance
└─ Total cost: ~$50/month

values-prod.yaml:
├─ replicas: 3
│  └─ Three copies for high availability
├─ resources: substantial (500m CPU, 1Gi memory)
│  └─ Reliable performance
├─ image.tag: 2.1.0
│  └─ Specific production version
├─ autoscaling: enabled (min 3, max 10)
│  └─ Handle traffic spikes
└─ Total cost: ~$500/month

Same chart, different values:

Deploy dev:
$ helm install myapp-dev ./myapp/ -f values-dev.yaml

Deploy prod:
$ helm install myapp-prod ./myapp/ -f values-prod.yaml

Both from same code!
```

---

# 14. k9s: TERMINAL UI DASHBOARD

## What is k9s?

**k9s** is a powerful, terminal-based user interface (TUI) for managing and monitoring Kubernetes clusters. It provides a real-time, interactive dashboard that makes it faster and easier to navigate, observe, and manage Kubernetes resources compared to using kubectl commands alone.

### Why Use k9s?

**The Problem:**
- `kubectl` commands require remembering syntax and typing lengthy commands
- Viewing logs, describing resources, editing configs requires multiple commands
- No real-time updates - need to keep re-running commands
- Switching between contexts and namespaces is verbose
- Difficult to get a quick overview of cluster health

**The Solution: k9s**
- **Real-time monitoring** - Auto-refreshes resource status (pods, deployments, services)
- **Interactive navigation** - Arrow keys and shortcuts instead of typing commands
- **Context switching** - Quick namespace and context changes with a keystroke
- **Built-in actions** - Delete, describe, logs, shell, edit without leaving the UI
- **Resource filtering** - Search and filter resources instantly
- **Color-coded status** - Visual indicators for healthy/unhealthy resources
- **Keyboard-driven** - Vim-like shortcuts for power users
- **Multiple views** - Logs, YAML editor, resource descriptions in one tool

### Key Features

1. **Resource Overview** - See all pods, deployments, services at a glance
2. **Live Logs** - View and follow logs from multiple containers
3. **Shell Access** - Execute into containers directly from the UI
4. **YAML Editing** - Edit resources in-place with syntax highlighting
5. **Port Forwarding** - Quick port-forward setup without kubectl commands
6. **Metrics Integration** - CPU/Memory usage when metrics-server is installed
7. **Namespace Isolation** - Easy switching between namespaces or view all
8. **Cluster Context** - Manage multiple clusters seamlessly
9. **Benchmarking** - Built-in resource benchmarks and pulses

### When to Use k9s

✅ **Great for:**
- Development and debugging workflows
- Quick cluster health checks
- Log monitoring and troubleshooting
- Learning Kubernetes resource relationships
- Day-to-day cluster management
- Demos and presentations

❌ **Not ideal for:**
- Automation and CI/CD (use kubectl or Helm)
- Scripting (need programmatic access)
- Production deployments (use declarative YAML)
- Access from non-terminal environments

### k9s vs Other Tools

| Tool | Type | Use Case |
|------|------|----------|
| **k9s** | Terminal UI | Interactive management, debugging |
| **kubectl** | CLI | Scripting, automation, CI/CD |
| **Lens** | Desktop GUI | Visual cluster management |
| **Kubernetes Dashboard** | Web UI | Browser-based monitoring |
| **k9s** advantage | Fast, keyboard-driven, no browser needed |

## Installing and Using k9s

### Installation

```bash
# macOS
brew install k9s

# Linux
go install github.com/derailed/k9s@latest

# Verify
k9s version
# Output: v0.31.0, ...

# First launch
k9s
# Connects to current kubectl context
# Displays pods in default namespace
```

### Main Views

```
k9s shows different resource types:

:pods (shortcut: p)
├─ Lists all pods
├─ Shows: Name, Status, Restarts, Age, CPU, Memory
├─ Search, filter, sort
├─ Select pod → Press 'l' → View logs
├─ Select pod → Press 's' → Shell (exec)
└─ Select pod → Press 'd' → Describe

:deployments (shortcut: d)
├─ Lists deployments
├─ Shows: Name, Ready, Updated, Available
├─ Select → View pods created by this deployment
├─ Select → 'e' to edit YAML
├─ Select → 'd' for details

:services (shortcut: s)
├─ Lists services
├─ Shows: Name, Type, Cluster-IP, Ports, Endpoints
├─ Select → View pods exposed by service
├─ Select → 'f' for port forward
└─ Select → 'e' to edit

:nodes (shortcut: n)
├─ Lists cluster nodes
├─ Shows: Name, Status, Ready, Capacity, Allocatable
├─ Select → View pods running on node
├─ Select → 'd' for node details
└─ Monitor resource allocation

:events (shortcut: e)
├─ Shows cluster events in real-time
├─ Warnings, errors, normal events
├─ Why pod didn't start (helpful!)
├─ Image pull failures
└─ Debugging tool

:configmaps (shortcut: cm)
├─ Lists all ConfigMaps
├─ View contents (values, files)
└─ Edit and update

:secrets (shortcut: sec)
├─ Lists all Secrets
├─ View contents (base64 decoded)
└─ Edit and update

:ingress (shortcut: ing)
├─ Lists Ingresses
├─ Shows routing rules
└─ Monitor public endpoints

:pvc
├─ PersistentVolumeClaims
├─ Storage allocation
└─ Binding status
```

### Advanced k9s Features

```
Real-time Metrics:
├─ k9s shows CPU/Memory per pod
├─ Updated every ~10 seconds
├─ See which pods consuming most resources
├─ Identify resource hogs
└─ Press 't' to toggle metrics

Search and Filter:
├─ Press '/' to search pods
├─ /myapp → shows only pods matching "myapp"
├─ Supports regex
├─ Very useful in large clusters

Sorting:
├─ Click column header to sort
├─ Name, Status, CPU, Memory, etc.
├─ Click again to reverse
└─ Find problematic pods (highest memory first)

Watching:
├─ Press 'Ctrl+l' to watch logs in real-time
├─ Logs update as they appear
├─ Timestamps helpful
├─ Scroll with arrow keys

Shell Access:
├─ Select pod
├─ Press 's' to exec shell
├─ Execute commands in pod
├─ Example: curl http://localhost:8080
├─ Exit: Ctrl+D
└─ Helpful for debugging

Describing Resources:
├─ Press 'd' for full description
├─ All metadata, events, status
├─ Debugging why pod failed
├─ Helpful error messages

Editing:
├─ Press 'e' to edit YAML
├─ Opens in default editor (vim, nano)
├─ Make changes
├─ Save and exit
├─ Changes applied immediately

Deleting:
├─ Press 'Delete' or 'x'
├─ Confirms deletion
├─ Pod deleted from cluster
└─ Use with caution!

Port Forwarding:
├─ Select service
├─ Press 'f' for port forward
├─ Enter local port: 8080
├─ Enter remote port: 80
├─ Access service at localhost:8080
└─ Useful for testing services

Switching Namespaces:
├─ Press ':' then 'ns' (goto namespaces)
├─ Select namespace
├─ k9s focuses on that namespace
├─ Or press Ctrl+a for all namespaces

Following Pods:
├─ Select deployment
├─ Press 'p' to see pods
├─ Creates dynamic list
├─ As deployment scales, list updates
└─ Watch scaling in real-time

Comparing Resources:
├─ Select multiple pods (Ctrl+Click)
├─ Press '+' to add to comparison
├─ Shows only differences
├─ Helps debug pod variations
└─ Good for troubleshooting

Navigating Relationships:
├─ Select Pod
├─ Press 'Ctrl+b' to see parent Deployment
├─ Select Deployment
├─ Press 'Ctrl+p' to see pods
├─ Navigate relationship tree
└─ Understand resource hierarchy
```

### k9s Configuration

```
Create ~/.k9s/skin.yml for custom theme:
├─ Colors, fonts, styles
├─ Dark mode, light mode
└─ Personal preferences

Create ~/.k9s/plugin.yml for custom commands:
├─ Custom shortcuts for common tasks
├─ Example: Auto-scale a deployment
├─ Example: Rollback a deployment
└─ Automation within k9s

Alias shortcuts:
├─ Configure custom keybindings
├─ Common tasks faster
├─ Personalize workflow
└─ Increase productivity

k9s is highly customizable and powerful for daily work!
```

---

# 15. SCALING APPLICATIONS: HPA & VPA

## What is Autoscaling in Kubernetes?

**Autoscaling** is the ability of Kubernetes to automatically adjust the number of running pods or the resources allocated to pods based on demand. This ensures your applications can handle varying loads efficiently while optimizing resource usage and costs.

### Why Autoscaling Matters

**The Challenge:**
- Traffic patterns are unpredictable (spikes during business hours, low at night)
- Manual scaling requires constant monitoring and human intervention
- Over-provisioning wastes resources and increases costs
- Under-provisioning leads to poor performance and downtime
- Different applications have different scaling needs

**The Solution:**
Kubernetes provides **automated scaling mechanisms** that respond to real-time metrics:

1. **Horizontal Pod Autoscaler (HPA)** - Scales the number of pod replicas
2. **Vertical Pod Autoscaler (VPA)** - Adjusts CPU/memory requests and limits
3. **Cluster Autoscaler** - Scales the number of nodes in the cluster

### Scaling Types Comparison

| Type | What it Scales | When to Use | Example |
|------|---------------|-------------|---------|
| **HPA** | Number of pods | Variable traffic, stateless apps | Web servers, APIs |
| **VPA** | Pod resources (CPU/RAM) | Unpredictable resource needs | Batch jobs, ML workloads |
| **Cluster Autoscaler** | Number of nodes | Need more compute capacity | When pods can't be scheduled |

**Key Differences:**

**Horizontal Scaling (HPA):**
- ✅ Adds more pods (scale out)
- ✅ Best for stateless applications
- ✅ Improves availability (more replicas)
- ✅ Handles traffic spikes well
- ❌ Requires load balancer
- ❌ Not ideal for stateful apps

**Vertical Scaling (VPA):**
- ✅ Increases pod resources (scale up)
- ✅ Good for single-instance apps
- ✅ Optimizes resource allocation
- ❌ Requires pod restart (downtime)
- ❌ Limited by node capacity
- ❌ No redundancy improvement

**Example Scenario:**

```
E-commerce Application:

Normal Load (9 AM - 5 PM):
├─ 3 replicas @ 200m CPU each = 600m total
├─ Handles ~300 req/sec
└─ 60% CPU utilization

Peak Load (Black Friday):
├─ Traffic increases to 3000 req/sec
├─ HPA scales to 30 replicas @ 200m CPU each = 6000m total
├─ Cluster Autoscaler adds nodes if needed
└─ Maintains 60-70% CPU utilization

Off-Peak (Night):
├─ Traffic drops to 50 req/sec
├─ HPA scales down to 2 replicas @ 200m CPU each = 400m
└─ Cluster Autoscaler removes unused nodes

Result:
✅ Performance maintained during peaks
✅ Costs reduced during off-peak hours
✅ No manual intervention required
```

### Prerequisites for Autoscaling

Before using HPA or VPA, ensure:

1. **Metrics Server** is installed (provides CPU/memory metrics)
   ```bash
   kubectl apply -f https://github.com/kubernetes-sigs/metrics-server/releases/latest/download/components.yaml
   ```

2. **Resource requests/limits** are defined in pod specs
   ```yaml
   resources:
     requests:
       cpu: 100m
       memory: 128Mi
     limits:
       cpu: 500m
       memory: 512Mi
   ```

3. **Custom metrics** (optional) - For scaling based on application metrics
   - Prometheus Adapter
   - Custom Metrics API

## Horizontal Pod Autoscaler (HPA)

### Understanding HPA

HPA automatically scales the number of pod replicas based on metrics.

```
Without HPA (Manual Scaling):

Peak hours:
├─ Traffic increases 10x
├─ Pods become slow
├─ Users complain
├─ On-call engineer gets paged
├─ Engineer manually increases replicas
├─ kubectl scale deployment myapp --replicas=10
├─ Takes 5-10 minutes
├─ Users already frustrated
└─ Cost increases

Off-peak hours:
├─ Traffic drops 90%
├─ Many pods idle (wasteful)
├─ Engineer manually decreases replicas
├─ Saves money but manual process
├─ Easy to forget, wastes resources all day
└─ Inconsistent

With HPA (Automatic Scaling):

Peak hours:
├─ Traffic increases 10x
├─ CPU usage increases
├─ HPA detects: "CPU > 70% target"
├─ Automatically creates new pods
├─ 10-30 seconds to scale up
├─ Users see minimal impact
└─ Cost increases automatically

Off-peak hours:
├─ Traffic drops
├─ CPU usage drops
├─ HPA detects: "CPU < 70% target"
├─ Automatically removes excess pods
├─ Saves money automatically
├─ Perfect optimization
└─ Zero manual intervention
```

### HPA Configuration

```yaml
apiVersion: autoscaling/v2
kind: HorizontalPodAutoscaler
metadata:
  name: product-api-hpa
  namespace: default

spec:
  # Which deployment to scale
  scaleTargetRef:
    apiVersion: apps/v1
    kind: Deployment
    name: product-api
  
  # Scaling limits
  minReplicas: 2    # Always have at least 2
  maxReplicas: 10   # Never exceed 10
  
  # Scaling metrics
  metrics:
  # Metric 1: CPU usage
  - type: Resource
    resource:
      name: cpu
      target:
        type: Utilization
        averageUtilization: 70  # Target 70% CPU
  
  # Metric 2: Memory usage
  - type: Resource
    resource:
      name: memory
      target:
        type: Utilization
        averageUtilization: 80  # Target 80% memory
  
  # Metric 3: Custom metric (if available)
  - type: Pods
    pods:
      metric:
        name: http_requests_per_second
      target:
        type: AverageValue
        averageValue: "1k"  # 1000 requests per pod
  
  # Scaling behavior
  behavior:
    scaleDown:
      stabilizationWindowSeconds: 300  # Wait 5 min before scaling down
      policies:
      - type: Percent
        value: 50  # Remove max 50% of pods
        periodSeconds: 15
    
    scaleUp:
      stabilizationWindowSeconds: 0  # Scale up immediately
      policies:
      - type: Percent
        value: 100  # Add 100% more pods (double)
        periodSeconds: 15
      - type: Pods
        value: 4  # Or add max 4 pods
        periodSeconds: 15
      selectPolicy: Max  # Pick the bigger increase
```

### HPA Algorithm Deep Dive

```
HPA updates every 15 seconds:

Step 1: Collect metrics
├─ Query metrics server
├─ Get current CPU usage for each pod
├─ Example: Pod1: 45%, Pod2: 62%, Pod3: 78%

Step 2: Calculate average
├─ Average: (45 + 62 + 78) / 3 = 61.7%

Step 3: Compare to target
├─ Current average: 61.7%
├─ Target: 70%
├─ Difference: 61.7% < 70% → No scaling (within acceptable range)

Step 4: Consider stabilization window
├─ If just scaled, wait stabilization window
├─ Prevents flapping (constant up-down scaling)
├─ Makes decisions based on sustained load

Example: Traffic spike scenario

T0: 3 pods, 50% CPU each
T15: Traffic increases 3x
     └─ Each pod now 85% CPU
     └─ HPA: "CPU > 70%, need more pods"
     └─ Desired replicas: 3 * (85 / 70) = 3.64 ≈ 4 pods
     └─ Create 1 new pod

T30: 4 pods, but 2 new pods starting
     └─ Traffic still high
     └─ Existing pods: 95% CPU
     └─ HPA: "Need more"
     └─ Desired: 4 * (95 / 70) = 5.43 ≈ 5 pods
     └─ Create 1 more

T45: 5 pods all running
     └─ Traffic still high
     └─ 80% CPU
     └─ HPA: "Still tight"
     └─ Desired: 5 * (80 / 70) = 5.71 ≈ 6 pods
     └─ Create 1 more

T60: 6 pods
     └─ Traffic peaks then starts decreasing
     └─ 72% CPU (above target but stable)
     └─ HPA: Wait (stabilization window)

T90: Traffic back to normal
     └─ 6 pods, 45% CPU
     └─ HPA: "Can scale down"
     └─ Stabilization window for scale-down: 5 minutes
     └─ Wait...

T300: After 5 minutes at low usage
      └─ 6 pods, 45% CPU
      └─ HPA: "Remove excess"
      └─ Desired: 6 * (45 / 70) = 3.86 ≈ 4 pods
      └─ Remove 2 pods

T315: 4 pods, 55% CPU
      └─ Happy state, no scaling needed
      └─ Continue monitoring

Result:
├─ Started with 3 pods
├─ Scaled to 6 during peak
├─ Scaled back to 4 after peak
├─ Zero manual intervention
├─ Cost optimized
└─ Performance maintained
```

### Resource Requests and HPA

**Important:** Pods MUST have resource requests for HPA to work!

```
Why requests matter:

HPA calculates: desired_replicas = current_replicas * (current_metric / target_metric)

Example with CPU:
├─ Current: 3 pods
├─ Total CPU limit: 3 * 500m = 1500m
├─ Current usage: 1200m
├─ Usage percentage: 1200 / 1500 = 80%
├─ Target: 70%
├─ Desired: 3 * (80 / 70) = 3.43 ≈ 4 pods

Without requests:
├─ Limits specified but no requests
├─ HPA can't calculate percentage
├─ HPA doesn't work
├─ Pods scale but not based on real capacity

With requests:
├─ Requests: 100m (guaranteed CPU)
├─ Total capacity: 3 * 100m = 300m
├─ Current usage: 280m
├─ Usage percentage: 280 / 300 = 93%
├─ Target: 70%
├─ Desired: 3 * (93 / 70) = 3.99 ≈ 4 pods

Correct approach:
resources:
  requests:
    cpu: 100m         # Used by HPA for calculation
    memory: 256Mi
  limits:
    cpu: 500m         # Actual limit at runtime
    memory: 512Mi
```

---

# 16. MONITORING & OBSERVABILITY

## What is Observability?

**Observability** is the ability to understand the internal state of a system by examining its external outputs. In our microservices environment (ProductApi and OrderApi running on Kubernetes), observability helps you answer critical questions: *What is happening? Why is it happening? Where is the problem?*

### Why Observability Matters for Our Microservices

**The Challenge with ProductApi and OrderApi:**

```
Your Microservices Architecture:
┌─────────────────────────────────────────────────────┐
│  User Request → OrderApi → ProductApi → Database    │
└─────────────────────────────────────────────────────┘

Problems without observability:
├─ OrderApi calls ProductApi - is it slow? Which one?
├─ Pods scale from 2→10 - which pods are healthy?
├─ Database query takes 5s - is it ProductApi's fault?
├─ Memory leak - which service? Which pod?
├─ 500 errors spike - ProductApi or OrderApi?
└─ Users complain "slow checkout" - where's the bottleneck?
```

**Real Scenarios from Your Microservices:**

1. **Distributed Complexity**
   - OrderApi depends on ProductApi for pricing
   - If ProductApi is slow, OrderApi appears slow too
   - Need to know: Is the delay in OrderApi logic or ProductApi call?

2. **Dynamic Nature**
   - HPA scales ProductApi from 2 to 5 pods during peak
   - One pod crashes and restarts - did it affect users?
   - Pods move between nodes - which node has issues?

3. **Inter-Service Communication**
   ```
   POST /api/orders (OrderApi)
     ├─ Creates order object (20ms)
     ├─ Calls ProductApi: GET /api/products/123 (??ms)
     ├─ Calculates total price (5ms)
     └─ Returns response
   
   Question: How long did ProductApi call take?
   Without tracing: Unknown! ❌
   With tracing: See exact timing ✅
   ```

4. **Error Propagation**
   ```
   ProductApi database timeout
     ↓
   ProductApi returns 500 error
     ↓
   OrderApi receives 500
     ↓
   OrderApi returns error to user
   
   Question: Which service failed first?
   Logs alone: Confusing ❌
   Distributed tracing: Clear origin ✅
   ```

**The Solution: Comprehensive Observability**

Monitor your ProductApi and OrderApi through:
- **Real-time metrics** - CPU, memory, request rates per service
- **Distributed traces** - Follow requests from OrderApi → ProductApi
- **Centralized logs** - Search across all pods in microservices namespace
- **Proactive alerts** - Notify before users are impacted
- **Service dependency mapping** - Visualize OrderApi → ProductApi relationship

### The Three Pillars of Observability

Modern observability is built on **three foundational pillars** that work together to provide complete system visibility:

#### 1. **Metrics** - What is happening?

**Definition:** Numerical measurements collected over time intervals

**Examples:**
- CPU usage: 75%
- Request rate: 1000 req/sec
- Error rate: 2.5%
- Memory consumption: 4GB
- Response time: 250ms (p95)

**Characteristics:**
- ✅ **Aggregatable** - Can be summed, averaged, counted
- ✅ **Efficient** - Low storage overhead (time-series data)
- ✅ **Trendable** - Easy to visualize over time
- ✅ **Alertable** - Set thresholds for notifications
- ❌ **Limited context** - Numbers don't explain *why*

**Use Cases:**
- System health dashboards
- Capacity planning
- Performance monitoring
- SLA/SLO tracking
- Autoscaling decisions

**Tools:** Prometheus, Datadog, New Relic, CloudWatch

---

#### 2. **Logs** - What happened in detail?

**Definition:** Timestamped records of discrete events in the system

**Examples:**
```
2025-12-24T10:15:30Z ERROR Database connection timeout after 5s
2025-12-24T10:15:31Z WARN Retrying connection (attempt 2/3)
2025-12-24T10:15:35Z INFO Successfully connected to database
2025-12-24T10:15:36Z INFO User 12345 logged in from IP 192.168.1.100
2025-12-24T10:15:37Z DEBUG Query executed in 23ms: SELECT * FROM orders
```

**Characteristics:**
- ✅ **Detailed** - Rich context about specific events
- ✅ **Searchable** - Query by keywords, patterns, time ranges
- ✅ **Debuggable** - Trace execution flow and errors
- ❌ **High volume** - Storage-intensive
- ❌ **Unstructured** - Harder to aggregate and analyze

**Use Cases:**
- Debugging application errors
- Security auditing
- Compliance requirements
- Understanding user behavior
- Troubleshooting specific incidents

**Tools:** Loki, ELK Stack (Elasticsearch, Logstash, Kibana), Fluentd, Splunk

---

#### 3. **Traces** - How did requests flow?

**Definition:** End-to-end journey of a request through distributed services

**Example Flow from Your Microservices:**
```
User Request: POST /api/orders (Create Order)

Trace ID: xyz789
├─ Span 1: OrderApi - POST /api/orders (250ms total)
│  ├─ Span 2: Create Order Object (10ms)
│  ├─ Span 3: HTTP Call to ProductApi (200ms)
│  │  └─ Span 4: ProductApi - GET /api/products/1 (200ms)
│  │     ├─ Span 5: Validate Request (5ms)
│  │     ├─ Span 6: Database Query (180ms)
│  │     └─ Span 7: Serialize Response (15ms)
│  ├─ Span 8: Calculate Total Price (5ms)
│  └─ Span 9: Return Order Response (10ms)
│
└─ Total Request Time: 250ms

Analysis:
- Slowest span: Database query in ProductApi (180ms)
- OrderApi → ProductApi HTTP call overhead: 20ms
- ProductApi processing time: 200ms (80% of total time)
```

**Characteristics:**
- ✅ **Context-rich** - Shows service dependencies and latencies
- ✅ **Request-scoped** - Follow single transaction across services
- ✅ **Bottleneck identification** - Find which service is slow
- ✅ **Error propagation** - See where failures originate
- ❌ **Complex setup** - Requires instrumentation across all services
- ❌ **Performance overhead** - Can impact application performance

**Use Cases:**
- Identifying performance bottlenecks
- Understanding service dependencies
- Debugging distributed transactions
- Optimizing microservices communication
- Root cause analysis for failures

**Tools:** Jaeger, Zipkin, AWS X-Ray, Azure Application Insights

---

### How the Three Pillars Work Together

**Real Scenario: User Reports Slow Order Creation in Your Microservices**

```
User complaint: "Creating an order takes forever!"
```

**Step 1: Metrics (Detection)**

Check Grafana dashboard for microservices namespace:

```
ProductApi Metrics:
├─ Request rate: 100 req/sec ✅ Normal
├─ Response time p95: 50ms ✅ Normal
├─ Error rate: 0.1% ✅ Normal
├─ CPU: 45% ✅ Normal
└─ Memory: 256Mi ✅ Normal

OrderApi Metrics:
├─ Request rate: 50 req/sec ✅ Normal
├─ Response time p95: 3000ms ⚠️ SLOW! (normally 150ms)
├─ Error rate: 0.1% ✅ Normal
├─ CPU: 30% ✅ Normal (not CPU-bound)
└─ Memory: 180Mi ✅ Normal (no memory leak)

Finding: OrderApi is slow, but CPU/memory normal → Not resource issue
```

**Step 2: Traces (Investigation)**

Open Jaeger and search for slow OrderApi traces:

```
Trace ID: abc123 (Order Creation - 3.2s total)
│
├─ OrderApi: POST /api/orders (3200ms total)
│  ├─ Create order object (15ms) ✅
│  ├─ Call ProductApi: GET /api/products/1 (3000ms) ⚠️ BOTTLENECK!
│  │  └─ ProductApi: Process request
│  │     ├─ Validate request (5ms)
│  │     ├─ Database query: SELECT * FROM products (2950ms) ⚠️ PROBLEM!
│  │     └─ Serialize response (10ms)
│  ├─ Calculate total price (5ms) ✅
│  └─ Return response (10ms) ✅
│
Conclusion: ProductApi database query taking 3 seconds!
```

**Step 3: Logs (Root Cause)**

Query Loki for ProductApi logs during the slow period:

```bash
# LogQL query
{namespace="microservices", app="product-api"} |= "SELECT"
```

ProductApi logs reveal:
```
2025-12-24T10:15:30Z INFO ProductService - Executing query: SELECT * FROM products WHERE id=1
2025-12-24T10:15:33Z WARN ProductService - Query took 2950ms - Missing index on products table!
2025-12-24T10:15:33Z ERROR ProductService - Database performance degraded
```

**Root Cause Found:**
```
Problem: Missing database index on products.id column
Impact: Every ProductApi call does full table scan
Solution: Add database index

After fix:
├─ ProductApi query time: 2950ms → 15ms ✅
├─ OrderApi response time: 3000ms → 150ms ✅
└─ User experience: Fast order creation! ✅
```

**The Observability Stack in Action:**
```
Metrics → Detected OrderApi slowness
   ↓
Traces → Pinpointed ProductApi database query
   ↓
Logs → Confirmed missing index issue
   ↓
Fix Applied → Problem solved!
```

---

## OpenTelemetry (OTEL): Unified Observability Standard

### What is OpenTelemetry?

**OpenTelemetry** is an open-source, vendor-neutral observability framework that provides a **single set of APIs, libraries, and agents** to collect metrics, logs, and traces from your applications.

**The Problem OTEL Solves:**

Before OpenTelemetry:
```
Your Application
├─ Prometheus SDK for metrics
├─ Jaeger SDK for tracing
├─ Fluentd for logs
├─ Different instrumentation libraries
├─ Vendor lock-in risk
└─ Maintenance nightmare (3+ SDKs)

Change observability backend?
└─ Re-instrument entire application 😱
```

After OpenTelemetry:
```
Your Application
├─ Single OTEL SDK
│  ├─ Collects metrics
│  ├─ Collects traces
│  └─ Collects logs
│
└─ OTEL Collector (configurable backends)
   ├─ Export to Prometheus
   ├─ Export to Jaeger
   ├─ Export to Loki
   ├─ Export to Datadog
   ├─ Export to AWS CloudWatch
   └─ Switch backends without code changes! 🎉
```

### Key Components of OpenTelemetry

1. **OTEL SDK** - Instrumentation libraries for multiple languages
   - Auto-instrumentation (zero-code for common frameworks)
   - Manual instrumentation (custom spans and metrics)
   - Supports: Java, Go, Python, .NET, JavaScript, and more

2. **OTEL Collector** - Central data processing pipeline
   - Receives telemetry data from applications
   - Processes, filters, and transforms data
   - Exports to multiple backends simultaneously

3. **OTEL Protocol (OTLP)** - Standard wire format
   - Efficient binary protocol (gRPC and HTTP)
   - Language-agnostic
   - Future-proof

### OpenTelemetry Architecture

```
┌─────────────────────────────────────────────────────────┐
│              Your Application (Instrumented)            │
│  ┌──────────────────────────────────────────────────┐   │
│  │  OTEL SDK (auto or manual instrumentation)      │   │
│  │  • Records metrics, traces, logs                │   │
│  │  • Adds context (trace IDs, span IDs)          │   │
│  └────────────────┬─────────────────────────────────┘   │
└───────────────────┼─────────────────────────────────────┘
                    │ OTLP (gRPC/HTTP)
                    ▼
┌─────────────────────────────────────────────────────────┐
│           OTEL Collector (Data Pipeline)                │
│  ┌──────────────────────────────────────────────────┐   │
│  │  Receivers → Processors → Exporters             │   │
│  │  • Receive OTLP data                            │   │
│  │  • Filter, sample, enrich                       │   │
│  │  • Export to multiple backends                  │   │
│  └──────────────────────────────────────────────────┘   │
└──────────┬──────────┬────────────┬─────────────┬────────┘
           │          │            │             │
           ▼          ▼            ▼             ▼
    ┌──────────┐ ┌────────┐ ┌──────────┐ ┌─────────────┐
    │Prometheus│ │ Jaeger │ │   Loki   │ │  Datadog    │
    │(Metrics) │ │(Traces)│ │  (Logs)  │ │(All-in-one) │
    └──────────┘ └────────┘ └──────────┘ └─────────────┘
```

### Benefits of OpenTelemetry

✅ **Vendor Neutrality** - No lock-in, switch backends easily
✅ **Unified Instrumentation** - Single SDK for all telemetry
✅ **Community Support** - CNCF project, industry standard
✅ **Auto-Instrumentation** - Minimal code changes required
✅ **Multi-Backend** - Send data to multiple systems simultaneously
✅ **Future-Proof** - Evolving standard adopted by major vendors
✅ **Cost Optimization** - Sample/filter data before expensive storage

### OpenTelemetry vs Traditional Approach

| Aspect | Traditional | OpenTelemetry |
|--------|-------------|---------------|
| **Instrumentation** | Multiple SDKs per backend | Single OTEL SDK |
| **Vendor Lock-in** | High (custom APIs) | None (standard API) |
| **Switching Costs** | Re-instrument entire app | Config change only |
| **Data Format** | Vendor-specific | Standardized (OTLP) |
| **Community** | Fragmented | Unified CNCF project |
| **Adoption** | Varies by vendor | Growing rapidly |

---

## Prometheus: Metrics Collection

### Prometheus Architecture

```
Prometheus Stack:

Prometheus Server
├─ Scrapes metrics from targets
├─ Stores time-series data
├─ Evaluates alert rules
└─ Web UI for querying

Exporters
├─ Applications expose metrics
├─ /metrics endpoint (Prometheus format)
├─ Example: Kubernetes kubelet /metrics
├─ Example: Application /actuator/prometheus

AlertManager
├─ Receives alerts from Prometheus
├─ Routes to different channels
├─ Email, Slack, PagerDuty, etc.
└─ Deduplicates and groups alerts

Grafana
├─ Visualization tool
├─ Reads from Prometheus
├─ Beautiful dashboards
├─ Alert integration
└─ User-friendly

Kubernetes Integration:
├─ kubelet exposes metrics
├─ Services expose application metrics
├─ Prometheus discovers targets via Kubernetes API
├─ Automatic scraping
└─ Self-updating as pods scale
```

### Prometheus Metrics

**Three types:**

```
1. Counter: Always increasing number
   ├─ http_requests_total: 15324
   ├─ errors_total: 42
   ├─ bytes_written_total: 123456789
   └─ Can only go up, never down
   
   Use case: Tracking total occurrences
   Query: rate(http_requests_total[1m]) → requests per minute

2. Gauge: Can go up or down
   ├─ cpu_usage: 45%
   ├─ memory_usage: 768MB
   ├─ active_connections: 234
   └─ Current snapshot value
   
   Use case: Current state measurements
   Query: cpu_usage → current CPU percentage

3. Histogram: Distribution of values
   ├─ request_duration_seconds: [0.001, 0.005, 0.01, 0.05, 0.1, 0.5, 1, 5]
   ├─ Buckets showing request latency distribution
   ├─ Helpful for percentiles
   └─ How many requests < 10ms, < 100ms, etc.
   
   Use case: Understanding distribution
   Query: histogram_quantile(0.95, rate(request_duration_seconds[5m])) → 95th percentile latency
```

### PromQL Queries

```
Simple queries:

node_memory_MemAvailable_bytes
└─ Raw metric: available memory in bytes

rate(http_requests_total[1m])
└─ How fast requests increasing (per second)

Aggregation:

sum(rate(http_requests_total[1m]))
└─ Total requests per second across all pods

avg(container_cpu_usage_seconds_total)
└─ Average CPU usage across all containers

top 5 by (value) (http_requests_total)
└─ Top 5 endpoints by request count

Filtering:

http_requests_total{job="api-server"}
└─ Requests for specific job

http_requests_total{status=~"5.."}
└─ 5xx errors (regex filter)

container_memory_usage_bytes{pod_name=~"product-api.*"}
└─ Memory for pods matching pattern

Combinations:

(rate(errors_total[1m]) / rate(http_requests_total[1m])) * 100
└─ Error percentage

rate(http_request_duration_seconds_sum[1m]) / rate(http_requests_total[1m])
└─ Average request duration
```

### Installing Prometheus

```bash
# Using Helm (recommended)
helm repo add prometheus-community https://prometheus-community.github.io/helm-charts
helm repo update

helm install prometheus prometheus-community/kube-prometheus-stack \
  --namespace monitoring \
  --create-namespace \
  --values values.yaml

# What gets installed:
# ├─ Prometheus Server
# ├─ AlertManager
# ├─ Grafana
# ├─ Node Exporter (node metrics)
# ├─ kube-state-metrics (Kubernetes metrics)
# └─ Service monitors (automatic scraping)

# Access Prometheus
kubectl port-forward -n monitoring svc/prometheus-kube-prometheus-prometheus 9090:9090
# http://localhost:9090

# Access Grafana
kubectl port-forward -n monitoring svc/prometheus-grafana 3000:80
# http://localhost:3000
# Default login: admin / prom-operator
```

## Grafana: Visualization and Dashboarding

### What is Grafana?

**Grafana** is an open-source analytics and interactive visualization platform. It connects to various data sources (Prometheus, Loki, Elasticsearch, databases) and transforms metrics into beautiful, insightful dashboards.

**Key Capabilities:**
- **Multi-source dashboards** - Combine data from Prometheus, Loki, databases
- **Real-time visualization** - Live updates as metrics change
- **Alerting** - Set thresholds and get notifications (Slack, email, PagerDuty)
- **Template variables** - Create dynamic dashboards (filter by namespace, pod)
- **Pre-built dashboards** - Import community dashboards from Grafana.com
- **Custom queries** - PromQL, LogQL, SQL support
- **Access control** - Role-based access (viewer, editor, admin)

### Grafana Architecture

```
┌─────────────────────────────────────────────────────────┐
│                   Grafana Server                        │
│  ┌──────────────────────────────────────────────────┐   │
│  │            Dashboard Engine                      │   │
│  │  • Renders panels and visualizations            │   │
│  │  • Executes queries to data sources             │   │
│  │  • Handles alerting rules                       │   │
│  └──────────────────────────────────────────────────┘   │
└────┬─────────┬─────────┬─────────┬─────────────────────┘
     │         │         │         │
     ▼         ▼         ▼         ▼
┌──────────┐ ┌────────┐ ┌────────┐ ┌─────────────┐
│Prometheus│ │ Loki   │ │ Jaeger │ │  PostgreSQL │
│(Metrics) │ │ (Logs) │ │(Traces)│ │  (App Data) │
└──────────┘ └────────┘ └────────┘ └─────────────┘

Users access Grafana → Queries fetch data → Rendered in browser
```

### Creating Dashboards

```
Dashboard = Collection of panels showing metrics

Panel Types:
├─ Graph: Time-series line graph
├─ Gauge: Single value with threshold
├─ Stat: Large number display
├─ Table: Time-series as table
├─ Heatmap: 2D histogram over time
├─ Alert List: Recent alerts
└─ Many more...

Example Dashboard: API Performance

Panel 1: Request Rate
├─ Query: sum(rate(http_requests_total[1m]))
├─ Graph showing requests per second
├─ Red line increases during peak hours

Panel 2: Error Rate
├─ Query: (sum(rate(errors_total[1m])) / sum(rate(http_requests_total[1m]))) * 100
├─ Percentage of errors
├─ Alert if > 5%

Panel 3: Latency
├─ Query: histogram_quantile(0.95, rate(request_duration_seconds_bucket[5m]))
├─ P95 latency (95% of requests faster than this)
├─ Alert if > 500ms

Panel 4: Pod Count
├─ Query: count(container_last_seen{pod_name=~"myapp.*"})
├─ Number of pods currently running

Panel 5: CPU Usage
├─ Query: sum(rate(container_cpu_usage_seconds_total{pod_name=~"myapp.*"}[1m]))
├─ Total CPU used by all pods

Panel 6: Memory Usage
├─ Query: sum(container_memory_working_set_bytes{pod_name=~"myapp.*"})
├─ Total memory used by all pods

All updated in real-time!
```

### Grafana Best Practices

**Dashboard Organization:**
- Group related panels logically (infrastructure, application, business metrics)
- Use rows to collapse/expand sections
- Add descriptive titles and tooltips
- Set consistent time ranges across related panels

**Performance Optimization:**
- Use template variables to reduce dashboard count
- Set appropriate query intervals (don't query every second for daily trends)
- Use caching for expensive queries
- Limit data retention in data sources

**Alerting Best Practices:**
- Set meaningful thresholds (avoid alert fatigue)
- Use notification channels appropriately (critical → PagerDuty, warnings → Slack)
- Include context in alert messages (which service, what metric, runbook link)
- Test alerts in non-prod for ProductApi & OrderApi?**

In your microservices architecture:
```
User Request: Create Order (productId=1, quantity=2)
    ↓
OrderApi receives request (5ms)
    ↓
OrderApi calls ProductApi to get product price (150ms)
    ├→ ProductApi validates request (5ms)
    ├→ ProductApi queries database (120ms)
    └→ ProductApi returns product details (25ms)
    ↓
OrderApi calculates total price (10ms)
    ↓
OrderApi returns order confirmation (5ms)

Total: 295ms

Questions Jaeger Answers:
• Which service is slower? → ProductApi (150ms) vs OrderApi (145ms)
• What's the critical path? → OrderApi → ProductApi → Database
• Where's the latency? → Database query in ProductApi (120ms)
• Are there retry storms? → See if OrderApi retries ProductApi calls
• How many ProductApi calls per order? → Should be 1, not N
• Network latency between services? → See HTTP call overhead

Specific to Your Deployment:
• Which ProductApi pod handled the request? → See pod label in span
• Is latency consistent across all ProductApi replicas? → Compare pods
• Does the problem occur in specific availability zone? → Check node labels
        └→ External Payment Gateway (100ms)

Total: 620ms

Questions Jaeger Answers:
• Which service is the bottleneck? → Payment Service (120ms)
• What's the critical path? → Gateway → Auth → Order → Payment → Gateway
• Where's the latency? → External Payment Gateway taking 100ms
• Are there retry storms? → See duplicate spans
• Which services call each other? → Service dependency graph
```

### Jaeger Architecture

```
┌─────────────────────────────────────────────────────────┐
│        Your Microservices (Instrumented)                │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐             │
│  │ Service  │  │ Service  │  │ Service  │             │
│  │    A     │→ │    B     │→ │    C     │             │
│  │ (OTEL)   │  │ (OTEL)   │  │ (OTEL)   │             │
│  └────┬─────┘  └────┬─────┘  └────┬─────┘             │
└───────┼────────────┼─────────────┼───────────────────┘
        │            │             │ Send spans (OTLP/Jaeger protocol)
        ▼            ▼             ▼
┌─────────────────────────────────────────────────────────┐
│              Jaeger Collector                           │
│  • Receives traces from services                        │
│  • Validates and processes spans                        │
│  • Batches and forwards to storage                      │
└────────────────────┬────────────────────────────────────┘
                     │
                     ▼
┌─────────────────────────────────────────────────────────┐
│              Storage Backend                            │
│  • Cassandra, Elasticsearch, or Badger                  │
│  • Stores traces and spans                              │
│  • Indexed for fast queries                             │
└────────────────────┬────────────────────────────────────┘
                     │
                     ▼
┌─────────────────────────────────────────────────────────┐
│              Jaeger Query & UI                          │
│  • Search traces by service, operation, tags            │
│  • Visualize trace timelines                            │
│  • Service dependency graphs                            │
│  • Compare traces                                       │
└─────────────────────────────────────────────────────────┘
```

### Key Concepts in Jaeger

1. **Trace** - Complete journey of a request through all services
   - Unique Trace ID (e.g., `abc123def456`)
   - Contains multiple spans

2. **Span** - Single operation within a trace
   - Span ID
   - Parent Span ID (creates hierarchy)
   - Start time and duration
   - Operation name (e.g., "GET /api/orders")
   - Tags (metadata like HTTP status, error flag)
   - Logs (timestamped events within span)

3. **Tags** - Key-value metadata attached to spans
   ```
   http.method: GET
   http.url: /api/orders/123
   http.status_code: 200
   db.type: postgresql
   error: false
   ```

4. **Logs** - Timestamped events within a span
   ```
   10:15:30.100 - Cache miss, querying database
   10:15:30.150 - Database query completed
   10:15:30.180 - Response serialized
   ```

### Installing Jaeger on Kubernetes

```bash
# Install Jaeger Operator (recommended for production)
kubectl create namespace observability
kubectl apply -f https://github.com/jaegertracing/jaeger-operator/releases/download/v1.51.0/jaeger-operator.yaml -n observability

# Deploy Jaeger instance
cat <<EOF | kubectl apply -f -
apiVersion: jaegertracing.io/v1
kind: Jaeger
metadata:
  name: jaeger-all-in-one
  namespace: observability
spec:
  strategy: allInOne  # For development (use 'production' for prod)
  allInOne:
    image: jaegertracing/all-in-one:latest
  storage:
    type: memory  # Use elasticsearch for production
  ingress:
    enabled: false
EOF

# Access Jaeger UI
kubectl port-forward -n observability svc/jaeger-all-in-one-query 16686:16686
# Open http://localhost:16686
```

### Jaeger UI Features

**1. Search Traces**
- Filter by service, operation, tags, duration
- Find slow requests (duration > 1s)
- Find errors (error=true tag)
- Time range filtering

**2. Trace Timeline View**
``` (Your Microservices)**
```
         ┌─────────────────┐
         │  User / Client  │
         └────────┬─────────┘
                  │
         ┌────────▼─────────┐
         │    OrderApi      │ (2 pods, ClusterIP)
         │  Namespace:      │
         │  microservices   │
         └────────┬─────────┘
                  │
                  │ HTTP GET /api/products/{id}
                  │
         ┌────────▼─────────┐
         │   ProductApi     │ (2 pods, ClusterIP)
         │  Namespace:      │
         │  microservices   │
         └────────┬─────────┘
                  │
                  │ (Future: Database query)
                  │
         ┌────────▼─────────┐
         │    Database      │
         │  (Not yet impl.) │
         └──────────────────┘

Jaeger shows:
├─ Call rate: OrderApi → ProductApi (requests/sec)
├─ Success rate: 99.5%
├─ Average latency: 50ms
├─ Error rate: 0.5%
└─ Traffic volume: Thickness of arrow shows call frequency
         ┌─────▼─────┐
         │   Auth    │
         └─────┬─────┘
               │
         ┌─────▼─────┐
         │   Order   │
         └─────┬─────┘
              ┌┴┐
         ┌────▼──────┐
         │           │
    ┌────▼────┐ ┌───▼────┐
    │ Product │ │Payment │
    └────┬────┘ └───┬────┘
         │          │
    ┌────▼────┐ ┌───▼────┐
    │   DB    │ │PayGW   │
    └─────────┘ └────────┘
```

### Jaeger Use Cases

✅ **Performance Troubleshooting**
- Identify slow services in request chain
- Find database query bottlenecks
- Detect N+1 query problems

✅ **Error Analysis**
- Trace error propagation through services
- Find where errors originate
- See retry behavior

✅ **Dependency Mapping**
- Understand service relationships
- Identify unused services
- Plan deprecation strategies

✅ **Latency Optimization**
- Compare fast vs slow traces
- Find opportunities for parallelization
- Optimize critical paths

---

## Loki: Log Aggregation

### What is Loki?

**Loki** is a horizontally scalable, highly available log aggregation system designed by Grafana Labs. Unlike traditional log systems (Elasticsearch), Loki **indexes only metadata** (labels) instead of full-text indexing, making it more cost-effective and performant.

**Key Philosophy: "Like Prometheus, but for logs"**

```
Traditional Logging (Elasticsearch):
├─ Indexes every word in every log line
├─ Expensive storage and compute
├─ Complex cluster management
├─ Overkill for most Kubernetes use cases
└─ High resource consumption

Loki Approach:
├─ Indexes only labels (pod, namespace, service)
├─ Stores log content as-is (compressed)
├─ Grep-like queries on selected streams
├─ Lightweight, cost-effective
└─ Works perfectly with Kubernetes labels
```

### Loki Architecture

```
┌─────────────────────────────────────────────────────────┐
│            Your Kubernetes Pods                         │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐             │
│  │  Pod A   │  │  Pod B   │  │  Pod C   │             │
│  │ (stdout) │  │ (stdout) │  │ (stdout) │             │
│  └────┬─────┘  └────┬─────┘  └────┬─────┘             │
└───────┼────────────┼─────────────┼───────────────────┘
        │            │             │
        ▼            ▼             ▼
┌─────────────────────────────────────────────────────────┐
│              Promtail (Log Agent)                       │
│  • Runs as DaemonSet on each node                       │
│  • Tails logs from /var/log/pods/*                      │
│  • Adds Kubernetes labels (pod, namespace, container)   │
│  • Pushes log streams to Loki                           │
└────────────────────┬────────────────────────────────────┘
                     │ HTTP Push
                     ▼
┌─────────────────────────────────────────────────────────┐
│                Loki (Distributor)                       │
│  • Receives log streams                                 │
│  • Validates labels and timestamps                      │
│  • ForwarQuery Your Microservices Logs

LogQL queries for ProductApi and OrderApi:

```bash
# ============================================
# BASIC QUERIES - Your Microservices
# ============================================

# All ProductApi logs
{namespace="microservices", app="product-api"}

# All OrderApi logs
{namespace="microservices", app="order-api"}

# Logs from specific ProductApi pod
{namespace="microservices", app="product-api", pod="product-api-7d8f9c-abc12"}

# All microservices logs
{namespace="microservices"}


# ============================================
# FILTERING - Find Issues
# ============================================

# ProductApi errors
{namespace="microservices", app="product-api"} |= "ERROR"

# OrderApi exceptions
{namespace="microservices", app="order-api"} |~ "Exception|Error"

# HTTP 500 errors in any service
{namespace="microservices"} |= "500"

# Slow database queries (if you add logging)
{namespace="microservices", app="product-api"} |~ "query took [5-9][0-9]{3}ms"


# ============================================
# PARSING - Extract Information
# ============================================

# Parse JSON logs (if your apps log JSON)
{namespace="microservices", app="product-api"} | json

# Extract and filter by log level
{namespace="microservices"} | json | level="ERROR"

# Find logs with specific HTTP status
{namespace="microservices"} | json | status_code="500"


# ============================================
# AGGREGATIONS - Metrics from Logs
# ============================================

# ProductApi log rate (logs per second)
sum(rate({namespace="microservices", app="product-api"}[1m]))

# Error count in last 5 minutes
count_over_time({namespace="microservices"} |= "ERROR" [5m])

# Error rate per service
sum(rate({namespace="microservices"} |= "ERROR" [1m])) by (app)

# HTTP 500 errors by pod
sum(rate({namespace="microservices"} |= "500" [5m])) by (pod)


# ============================================
# REAL-WORLD SCENARIOS
# ============================================

# Scenario 1: OrderApi → ProductApi call failures
{namespace="microservices", app="order-api"} |~ "ProductApi.*failed|ProductApi.*timeout"

# Scenario 2: Find which products cause errors
{namespace="microservices", app="product-api"} |= "ERROR" | json | product_id!=""

# Scenario 3: Requests taking > 1 second
{namespace="microservices"} | json | duration > 1000

# Scenario 4: Correlate with trace ID (if instrumented)
{namespace="microservices"} | json | trace_id="abc123def456"

# Scenario 5: Pod restart investigation
{namespace="microservices"} |~ "starting|initializing|ready"


# ============================================
# TROUBLESHOOTING PATTERNS
# ============================================

# Check OrderApi startup logs
{namespace="microservices", app="order-api"} |= "started" or |= "listening"

# Find connection issues between services
{namespace="microservices"} |~ "connection refused|timeout|unreachable"

# Memory or resource issues
{namespace="microservices"} |~ "OutOfMemory|out of memory|memory pressure"

# Health check failures
{namespace="microservices"} |= "/health" |= "failed"
```

**Using in Grafana:**
1. Go to Explore tab
2. Select Loki data source
3. Choose "microservices" namespace
4. Select "product-api" or "order-api" app
5. Add filters for errors, specific operations
6. See logs in real-time or historical viewog filtering
{app="product-api"} |= "error"
# Contains "error"

{app="product-api"} != "debug"
# Doesn't contain "debug"

{app="product-api"} |~ "ERROR|FATAL"
# Regex match for ERROR or FATAL

# Parsing and extracting fields
{app="product-api"} | json
# Parse JSON logs

{app="product-api"} | logfmt | duration > 1s
# Parse logfmt and filter by duration

# Aggregations (like PromQL)
sum(rate({app="product-api"}[1m]))
# Log rate per second

count_over_time({app="product-api"} |= "error" [5m])
# Count of error logs in last 5 minutes

# Combining with metrics
sum(rate({namespace="microservices"} |= "500" [1m])) by (pod)
# Rate of 500 errors per pod
```

### Installing Loki and Promtail

```bash
# Using Helm
helm repo add grafana https://grafana.github.io/helm-charts
helm repo update

# Install Loki stack (Loki + Promtail + Grafana)
helm install loki grafana/loki-stack \
  --namespace monitoring \
  --create-namespace \
  --set grafana.enabled=true \
  --set prometheus.enabled=true \
  --set promtail.enabled=true

# What gets installed:
# ├─ Loki (log storage and querying)
# ├─ Promtail (log collector DaemonSet)
# └─ Grafana (visualization)

# Access Grafana
kubectl port-forward -n monitoring svc/loki-grafana 3000:80
# Login with: admin / <get password from secret>

kubectl get secret -n monitoring loki-grafana -o jsonpath="{.data.admin-password}" | base64 --decode
```

### Loki Use Cases

✅ **Debugging Application Issues**
- Search logs for specific errors
- Correlate with metrics (CPU spike + error logs)
- Follow request flow through services

✅ **Security Auditing**
- Track authentication failures
- Monitor unauthorized access attempts
- Compliance log retention

✅ **Performance Analysis**
- Find slow queries in logs
- Identify timeout patterns
- Analyze request patterns

✅ **Cost Optimization**
- Much cheaper than Elasticsearch (no full-text indexing)
- Scales with object storage (S3, GCS)
- Lower resource requirements

---

## Cloud-Native Observability Alternatives

### AWS Observability Stack

**AWS** provides managed observability services that integrate seamlessly with EKS (Elastic Kubernetes Service):

#### 1. **Amazon CloudWatch** (Metrics & Logs)

**What it does:**
- Centralized metrics and logs for AWS resources and applications
- Native integration with EKS, EC2, Lambda, and other AWS services
- Automatic dashboards for AWS resources

**Key Features:**
```
Metrics:
├─ CloudWatch Container Insights
│  ├─ Pod-level CPU/memory metrics
│  ├─ Node-level metrics
│  ├─ Namespace-level aggregations
│  └─ Automatic dashboard generation
│
Logs:
├─ CloudWatch Logs
│  ├─ Centralized log aggregation
│  ├─ Log groups and streams
│  ├─ CloudWatch Logs Insights (query language)
│  └─ Log retention policies

Alarms:
├─ Metric-based alarms
├─ Composite alarms (multiple conditions)
├─ SNS, Lambda, Auto Scaling integrations
└─ Anomaly detection (ML-based)
```

**Setup for EKS:**
```bash
# Install CloudWatch agent as DaemonSet
kubectl apply -f https://raw.githubusercontent.com/aws-samples/amazon-cloudwatch-container-insights/latest/k8s-deployment-manifest-templates/deployment-mode/daemonset/container-insights-monitoring/quickstart/cwagent-fluentd-quickstart.yaml

# Metrics and logs automatically sent to CloudWatch
# View in AWS Console: CloudWatch → Container Insights → Performance Monitoring
```

**Pros:**
- ✅ Native AWS integration (no additional setup for AWS resources)
- ✅ Unified billing (part of AWS ecosystem)
- ✅ Automatic dashboards
- ✅ Long-term retention with lifecycle policies

**Cons:**
- ❌ Vendor lock-in (AWS-specific)
- ❌ Less flexible than Prometheus/Grafana
- ❌ Costs can grow with volume
- ❌ Query language not as powerful as PromQL

---

#### 2. **AWS X-Ray** (Distributed Tracing)

**What it does:**
- Distributed tracing for microservices
- Service maps showing dependencies
- Latency analysis and error tracking

**Architecture:**
```
Your EKS Pods (instrumented with X-Ray SDK)
    ↓
X-Ray Daemon (DaemonSet on each node)
    ↓
AWS X-Ray Service (managed)
    ↓
X-Ray Console (visualize traces)
```

**Setup for EKS:**
```bash
# Deploy X-Ray daemon
kubectl apply -f https://github.com/aws/aws-xray-kubernetes/blob/master/xray-daemon-config.yaml

# Instrument your application
# Add X-Ray SDK to your app (e.g., for Node.js)
npm install aws-xray-sdk

# Code instrumentation
const AWSXRay = require('aws-xray-sdk');
const app = AWSXRay.express.openSegment('ProductApi');
// Your Express app code
AWSXRay.express.closeSegment();
```

**Pros:**
- ✅ Deep AWS service integration (Lambda, API Gateway, DynamoDB)
- ✅ Service map visualization
- ✅ Anomaly detection
- ✅ Pay-per-use pricing

**Cons:**
- ❌ Requires code instrumentation (not as plug-and-play as Jaeger with OTEL)
- ❌ Limited to AWS ecosystem
- ❌ Not as feature-rich as Jaeger for Kubernetes workloads

---

#### 3. **Amazon Managed Prometheus (AMP) & Managed Grafana (AMG)**

**What it does:**
- Fully managed Prometheus-compatible service
- Scalable, highly available, without managing infrastructure
- Integrated with Managed Grafana for visualization

**Why use it:**
```
Self-hosted Prometheus:
├─ You manage Prometheus servers
├─ Handle storage, backups, HA setup
├─ Scale manually
└─ Complex to operate

Amazon Managed Prometheus:
├─ AWS manages infrastructure
├─ Automatic scaling
├─ HA and backups included
├─ Scrape from EKS clusters
└─ Pay only for ingestion and storage
```

**Setup:**
```bash
# Create AMP workspace
aws amp create-workspace --alias my-prometheus

# Configure Prometheus to remote-write to AMP
# In your Prometheus ConfigMap:
remote_write:
  - url: https://aps-workspaces.us-east-1.amazonaws.com/workspaces/ws-abc123/api/v1/remote_write
    sigv4:
      region: us-east-1

# Query from Managed Grafana
# AWS Grafana → Add data source → Amazon Managed Prometheus
```

**Pros:**
- ✅ No Prometheus server management
- ✅ Compatible with existing Prometheus tools
- ✅ Integrated with AWS security (IAM, VPC)
- ✅ Long-term storage

**Cons:**
- ❌ Additional cost vs self-hosted
- ❌ Limited customization vs self-hosted
- ❌ Still AWS-locked

---

### Azure Observability Stack

**Azure** provides comprehensive monitoring for AKS (Azure Kubernetes Service):

#### 1. **Azure Monitor** (Metrics & Logs)

**What it does:**
- Unified monitoring for Azure resources and Kubernetes
- Container Insights for AKS monitoring
- Log Analytics workspace for log aggregation

**Key Components:**
```
Azure Monitor Container Insights:
├─ Node and pod performance metrics
├─ Container CPU/memory usage
├─ Live Logs (tail logs in real-time)
├─ Recommended alerts (pre-configured)
└─ Kubernetes event monitoring

Log Analytics Workspace:
├─ KQL (Kusto Query Language) for log queries
├─ Workbooks (interactive reports)
├─ Alerts and action groups
└─ Integration with Azure Sentinel (SIEM)
```

**Setup for AKS:**
```bash
# Enable Container Insights on AKS
az aks enable-addons \
  --resource-group myResourceGroup \
  --name myAKSCluster \
  --addons monitoring \
  --workspace-resource-id <log-analytics-workspace-id>

# Automatically deploys:
# ├─ OMS Agent (DaemonSet)
# ├─ Metrics collection
# └─ Log forwarding to Log Analytics

# View in Azure Portal:
# Azure Monitor → Containers → Select your AKS cluster
```

**Sample KQL Queries:**
```kusto
// Find error logs
ContainerLog
| where LogEntry contains "ERROR"
| project TimeGenerated, ContainerName, LogEntry
| order by TimeGenerated desc

// CPU usage over time
Perf
| where ObjectName == "K8SContainer"
| where CounterName == "cpuUsageNanoCores"
| summarize avg(CounterValue) by bin(TimeGenerated, 5m), Computer
| render timechart

// Pod restart count
KubePodInventory
| where PodStatus == "Failed"
| summarize count() by PodName, Namespace
```

**Pros:**
- ✅ Deep AKS integration (one-click enablement)
- ✅ Rich visualization with Workbooks
- ✅ Unified billing with Azure
- ✅ Advanced analytics with KQL

**Cons:**
- ❌ Azure-specific (not portable)
- ❌ KQL learning curve vs PromQL
- ❌ Costs scale with data ingestion

---

#### 2. **Azure Application Insights** (APM & Distributed Tracing)

**What it does:**
- Application Performance Monitoring (APM)
- Distributed tracing with dependency mapping
- Exception tracking and diagnostics

**Features:**
```
Application Performance:
├─ Request rates and response times
├─ Dependency call tracking (DB, external APIs)
├─ Exception and failure tracking
├─ Live metrics stream
└─ Smart detection (anomaly alerts)

Distributed Tracing:
├─ End-to-end transaction views
├─ Application Map (service dependencies)
├─ Performance profiling
└─ Correlation with logs and metrics
```

**Setup:**
```bash
# Install Application Insights agent in AKS
# Add SDK to your application (e.g., .NET)
dotnet add package Microsoft.ApplicationInsights.AspNetCore

# Configure in appsettings.json
{
  "ApplicationInsights": {
    "ConnectionString": "InstrumentationKey=<your-key>;..."
  }
}

# Code changes
builder.Services.AddApplicationInsightsTelemetry();

# Auto-instrumentation for:
# ├─ HTTP requests/responses
# ├─ Database calls (EF Core, SQL)
# ├─ External HTTP calls
# └─ Exceptions
```

**Pros:**
- ✅ Rich APM features (profiling, diagnostics)
- ✅ Smart detection with ML-based anomalies
- ✅ Code-level insights
- ✅ Integration with Visual Studio

**Cons:**
- ❌ Requires code instrumentation
- ❌ Azure-specific
- ❌ Can be expensive at scale

---

#### 3. **Azure Managed Grafana & Managed Prometheus**

**What it does:**
- Fully managed Grafana and Prometheus for Azure
- Similar to AWS managed services
- Native AKS integration

**Setup:**
```bash
# Create Azure Monitor managed service for Prometheus
az monitor account create \
  --name myPrometheus \
  --resource-group myResourceGroup \
  --location eastus

# Enable on AKS
az aks update \
  --resource-group myResourceGroup \
  --name myAKSCluster \
  --enable-azure-monitor-metrics \
  --azure-monitor-workspace-resource-id <workspace-id>

# Create Azure Managed Grafana
az grafana create \
  --name myGrafana \
  --resource-group myResourceGroup

# Access Grafana at: https://myGrafana-<random>.grafana.azure.com
```

**Pros:**
- ✅ No infrastructure management
- ✅ PromQL and Grafana compatibility
- ✅ Azure AD integration for auth
- ✅ Pre-built AKS dashboards

**Cons:**
- ❌ Azure-specific
- ❌ Additional cost
- ❌ Limited customization vs self-hosted

---

### Comparison: Open Source vs Cloud-Native

| Feature | Open Source (Prometheus, Grafana, Jaeger, Loki) | AWS (CloudWatch, X-Ray, AMP/AMG) | Azure (Monitor, App Insights, Managed Prometheus/Grafana) |
|---------|-------------|---------|----------|
| **Cost** | Infrastructure only (compute, storage) | Pay-per-use (ingestion, storage, queries) | Pay-per-use (similar to AWS) |
| **Portability** | ✅ Multi-cloud, on-prem, hybrid | ❌ AWS-locked | ❌ Azure-locked |
| **Management Overhead** | High (you operate) | Low (AWS manages) | Low (Azure manages) |
| **Customization** | ✅ Full control | Limited | Limited |
| **Learning Curve** | PromQL, LogQL | CloudWatch Logs Insights, X-Ray | KQL, Application Insights |
| **Integration** | Kubernetes-native | AWS services | Azure services |
| **Scaling** | Manual (or operators) | Automatic | Automatic |
| **Vendor Lock-in** | None | High | High |
| **Community Support** | Large (CNCF projects) | AWS documentation | Azure documentation |
| **Best For** | Multi-cloud, control, cost-optimization | AWS-heavy environments | Azure-heavy environments |

### Recommendations

**Use Open Source (Prometheus, Grafana, Jaeger, Loki) when:**
- Multi-cloud or hybrid cloud strategy
- Need full control and customization
- Cost optimization is critical
- Large community support is important
- Avoiding vendor lock-in

**Use AWS CloudWatch/X-Ray when:**
- All infrastructure on AWS
- Want minimal operational overhead
- Deep AWS service integration needed
- Budget allows for managed services

**Use Azure Monitor/App Insights when:**
- All infrastructure on Azure
- Leveraging .NET and Visual Studio ecosystem
- Need advanced APM features
- KQL expertise available

**Hybrid Approach:**
- Use OpenTelemetry for instrumentation (vendor-neutral)
- Export to both open source and cloud services
- Flexibility to switch backends without code changes

---

# 17. SERVICE MESH: SOLVING MICROSERVICES CHALLENGES

## Problems Service Mesh Solves

```
In Kubernetes with microservices, services need:

1. Reliable Communication
   ├─ What if service occasionally fails?
   ├─ Automatic retry logic
   ├─ Circuit breaker (stop calling failing service)
   ├─ Timeout protection
   └─ Built into service mesh

2. Security Between Services
   ├─ Service A talks to Service B
   ├─ Traffic is plain HTTP (unencrypted!)
   ├─ Anyone in cluster can see traffic
   ├─ Eavesdropping possible
   └─ Service mesh: Automatic mTLS encryption

3. Traffic Management
   ├─ Route 90% to v1, 10% to v2 (canary)
   ├─ Route based on headers
   ├─ Route based on request path
   ├─ Service mesh makes this easy

4. Observability
   ├─ What services are talking?
   ├─ How much traffic?
   ├─ What's the latency?
   ├─ Error rate between services?
   ├─ Service mesh provides automatic metrics

5. Compliance & Control
   ├─ Enforce which services can talk
   ├─ Audit trail of inter-service communication
   ├─ Network policies at application level
   └─ Service mesh: Authorization policies
```

### Why Not Implement in Application Code?

```
❌ Implementing in application code:

Each service implements:
├─ Retry logic
│  └─ Write code to retry failed requests
├─ Timeouts
│  └─ Configure timeout per request
├─ Circuit breaker
│  └─ Track failures, stop calling
├─ Metrics
│  └─ Instrument code to collect metrics
├─ Logging
│  └─ Log all requests/responses
├─ Encryption (mTLS)
│  └─ Complex certificate management
├─ Authorization checks
│  └─ Verify caller identity
└─ Load balancing
   └─ Distribute load manually

Problems:
├─ Duplicate code across all services
├─ Different implementations per team
├─ Inconsistent behavior
├─ Complex, error-prone
├─ Hard to test
├─ Performance overhead
└─ Tight coupling

✓ Service Mesh Solution:

Sidecar proxies handle all this:
├─ Transparently intercept traffic
├─ Apply policies consistently
├─ Services don't change
├─ One place to configure
├─ Automatic for all services
├─ Single source of truth
└─ Infrastructure-level, not application-level

Benefits:
├─ Services simplified
├─ Consistent policies
├─ Easy to change behavior
├─ Decoupled from application
├─ Can change without redeploying apps
└─ Truly polyglot (works with any language)
```

---

# 18. ENVOY PROXY: THE DATAPLANE ENGINE

## Envoy Architecture

### What Envoy Does

```
Envoy Proxy = Advanced network proxy

In service mesh:
├─ Envoy runs as sidecar (same pod as application)
├─ All traffic goes through Envoy
├─ Envoy applies policies, metrics, encryption
├─ Application code unchanged
└─ Transparent to application

Envoy Intercepts:

Request from Client Pod:
curl http://api-service/data
  ↓
Packet reaches client pod's Envoy
  ├─ Destination: api-service IP
  ├─ Envoy recognizes: "This is a service"
  ├─ Envoy looks up: where is api-service?
  ├─ Finds: 3 pods with api-service label
  ├─ Selects one (round-robin)
  ├─ Forwards to that pod's Envoy
  └─ Encrypts with mTLS
  ↓
Packet reaches server pod's Envoy
  ├─ Decrypts mTLS
  ├─ Checks authorization: Is this caller allowed?
  ├─ Records metrics: latency, bytes, status
  ├─ Adds headers (tracing headers)
  ├─ Forwards to actual application
  └─ Application receives normal HTTP request
  ↓
Application processes, returns response
  ↓
Server pod's Envoy intercepts response
  ├─ Records metrics
  ├─ Encrypts with mTLS
  ├─ Sends back to client
  ↓
Client pod's Envoy receives response
  ├─ Decrypts mTLS
  ├─ Forwards to client application
  ↓
Client gets response (completely transparent!)
```

### Envoy Features

```
1. Load Balancing

Algorithms:
├─ Round Robin (default): 1-2-3-1-2-3...
├─ Least Request: Send to pod with fewest requests
├─ Ring Hash: Consistent hashing (sticky)
├─ Random: Random selection
├─ Weighted: Some pods get more traffic
└─ Locality: Prefer same availability zone

Configuration:
loadBalancer:
  simple: ROUND_ROBIN

2. Outlier Detection (Circuit Breaking)

Detects failing backends:
├─ Track consecutive errors per endpoint
├─ If Pod1: 5 consecutive errors
│  ├─ Eject from load balancing
│  ├─ Stop sending requests to Pod1
│  ├─ Try again after 30 seconds
│  └─ If working, add back
│
├─ Protects healthy pods
├─ Prevents cascading failures
└─ Automatic recovery

Configuration:
outlierDetection:
  consecutive5xxErrors: 5
  interval: 30s
  baseEjectionTime: 30s

3. Retries

Automatic retry on failure:
├─ Request fails with 5xx error
├─ Envoy automatically retries
├─ Same request to different pod
├─ Transparent to client
├─ Max 3 retries (configurable)

Configuration:
retryPolicy:
  retryOn: "5xx"
  numRetries: 3
  perTryTimeout: 10s

4. Timeouts

Prevent hanging requests:
├─ Request takes > 30 seconds
├─ Envoy kills request
├─ Returns 504 Gateway Timeout
├─ Prevents resource exhaustion

Configuration:
timeout: 30s

5. Traffic Splitting

Canary deployments:
├─ 90% traffic to v1
├─ 10% traffic to v2
├─ Gradually shift: 80/20, 50/50, 0/100
├─ No code changes
└─ Dynamic policy changes

Configuration:
route:
  - destination:
      host: myapp
      subset: v1
    weight: 90
  - destination:
      host: myapp
      subset: v2
    weight: 10

6. Header Manipulation

Add/remove/modify headers:
├─ Add tracing headers (for distributed tracing)
├─ Add security headers
├─ Remove sensitive headers
├─ Route based on headers
└─ Transparent to application

7. Protocol Upgrades

Support multiple protocols:
├─ HTTP/1.1
├─ HTTP/2
├─ gRPC
├─ WebSocket
└─ TCP (raw proxy mode)

8. Metrics & Observables

Automatic metrics:
├─ Requests per second
├─ Latency distribution
├─ Error rates
├─ Bytes in/out
├─ Active connections
└─ All per service, per pod
```

---

# 19. SIDECAR INJECTION & TRANSPARENT PROXYING

## How Sidecars Get Injected

```
Problem: How do applications use Envoy automatically?

Solution: Sidecar Injection

Manual approach (❌ Don't do this):
├─ Edit every deployment YAML
├─ Add Envoy container to spec
├─ Add port forwarding
├─ Add volume mounts
├─ Error-prone, inconsistent
└─ Required for every deployment

Automatic approach (✓ Best):
├─ Enable sidecar injection on namespace
├─ kubectl label namespace istio-injection=enabled
├─ New pods get Envoy automatically
├─ No YAML changes needed
├─ Consistent across all services
└─ Transparent to developers

How it works:

MutatingWebhook:
├─ Kubernetes feature: Intercept pod creation
├─ Before pod created: Webhook called
├─ Webhook checks: Is namespace labeled for injection?
├─ If yes: Modify pod spec
│  ├─ Add Envoy container
│  ├─ Add istio-init init container (iptables setup)
│  ├─ Add volumes for config
│  └─ Add security context
│
└─ Modified pod created with Envoy included

Result:
├─ Developer deploys pod normally
├─ Envoy injected automatically
├─ Transparent to application
├─ Zero code changes
└─ Service mesh magic!
```

### Network Redirect with iptables

```
How traffic reaches Envoy:

Pod Network Stack:
├─ Container has its own network namespace
├─ All processes in pod see same IP
├─ Traffic from app (port 8080) goes out
├─ Goes to kernel network stack
├─ Kernel: "Where should this go?"

iptables Rules (set up by Envoy init container):
├─ Rule 1: All outbound TCP traffic
│  └─ REDIRECT to Envoy (port 15001)
│
├─ Rule 2: Envoy can bypass itself
│  └─ Envoy traffic not redirected (would loop!)
│
├─ Rule 3: Local traffic (localhost)
│  └─ Not redirected (local communication)
│
└─ Rule 4: Kubernetes DNS (port 53)
   └─ Not redirected (let kubelet handle)

Flow:

Application:
├─ curl http://api-service/data
└─ Creates TCP connection to api-service IP

Kernel Network Stack:
├─ Sees outbound connection
├─ Checks iptables rules
├─ Rule matches: "Outbound TCP traffic"
├─ REDIRECT to 127.0.0.1:15001
└─ Connection goes to Envoy instead!

Envoy:
├─ Receives connection (thinks it's original destination)
├─ Reads: "Original destination: api-service:80"
├─ Looks up: What pods have api-service label?
├─ Chooses one pod (load balancing)
├─ Connects to that pod's Envoy on port 15000
├─ Encrypts with mTLS
├─ Sends request
└─ Remote Envoy decrypts, forwards to app

Result:
├─ Application thinks talking to api-service directly
├─ Actually going through Envoys (transparent!)
├─ Encryption, routing, metrics all automatic
└─ No code changes needed!
```

---

# 20. ISTIO: COMPLETE IMPLEMENTATION GUIDE

## Istio Installation

```bash
# Download Istio
curl -L https://istio.io/downloadIstio | sh -
cd istio-*
export PATH=$PWD/bin:$PATH

# Check version
istioctl version
# Output: 1.19.0

# Install (with demo profile)
istioctl install --set profile=demo -y

# Output:
# ✓ Istio core installed
# ✓ Istiod installed
# ✓ Ingress gateways installed
# ✓ Egress gateways installed

# Verify installation
kubectl get pods -n istio-system
# Output: istio-ingressgateway-xxx, istiod-xxx, etc.

# Enable sidecar injection for namespace
kubectl label namespace microservices istio-injection=enabled

# Redeploy existing pods (to inject sidecars)
kubectl rollout restart deployment -n microservices

# Or simply delete pods, deployment recreates them
kubectl delete pods --all -n microservices
```

## Core Istio Resources

### VirtualService: Traffic Routing

```yaml
apiVersion: networking.istio.io/v1beta1
kind: VirtualService
metadata:
  name: product-api
  namespace: microservices
spec:
  # Which host(s) does this apply to?
  hosts:
  - product-api
  - product-api.microservices.svc.cluster.local
  
  # Routing rules (in order of precedence)
  http:
  # Rule 1: Match specific conditions
  - match:
    - uri:
        prefix: /api/admin
      sourceLabels:
        role: admin
    # Only admins can access /api/admin
    route:
    - destination:
        host: product-api
        port:
          number: 80
    timeout: 10s
  
  # Rule 2: Canary (90/10 split)
  - match:
    - uri:
        prefix: /api/
    route:
    - destination:
        host: product-api
        subset: v1
      weight: 90
    - destination:
        host: product-api
        subset: v2
      weight: 10
    timeout: 30s
    retries:
      attempts: 3
      perTryTimeout: 10s
  
  # Rule 3: Default (catch-all)
  - route:
    - destination:
        host: product-api
        subset: v1
```

### DestinationRule: Load Balancing & Outlier Detection

```yaml
apiVersion: networking.istio.io/v1beta1
kind: DestinationRule
metadata:
  name: product-api
  namespace: microservices
spec:
  host: product-api
  
  # Traffic policy for all subsets
  trafficPolicy:
    loadBalancer:
      simple: ROUND_ROBIN
    
    # Detect and eject failing endpoints
    outlierDetection:
      consecutive5xxErrors: 5
      interval: 30s
      baseEjectionTime: 30s
      maxEjectionPercent: 50  # Max 50% ejected
      minRequestVolume: 5  # Require 5 requests before judgment
  
  # Define subsets (different versions)
  subsets:
  # Subset v1: pods with label version=v1
  - name: v1
    labels:
      version: v1
    trafficPolicy:
      loadBalancer:
        simple: LEAST_CONN  # Different LB for v1
  
  # Subset v2: pods with label version=v2
  - name: v2
    labels:
      version: v2
    trafficPolicy:
      connectionPool:
        http:
          http1MaxPendingRequests: 100
          maxRequestsPerConnection: 2
```

### PeerAuthentication: mTLS Configuration

```yaml
apiVersion: security.istio.io/v1beta1
kind: PeerAuthentication
metadata:
  name: default
  namespace: microservices
spec:
  # STRICT: All traffic must be mTLS encrypted
  mtls:
    mode: STRICT

# Alternatively:
# - PERMISSIVE: Allows both mTLS and plain HTTP
# - DISABLE: No mTLS (not recommended)

# Can also be applied per-port:
portLevelMtls:
  8080:
    mode: DISABLE  # Port 8080 doesn't use mTLS (e.g., health checks)
```

### AuthorizationPolicy: Access Control

```yaml
apiVersion: security.istio.io/v1beta1
kind: AuthorizationPolicy
metadata:
  name: product-api-authz
  namespace: microservices
spec:
  # Selector: This policy applies to pods with these labels
  selector:
    matchLabels:
      app: product-api
  
  # Action: ALLOW these requests, DENY others
  action: ALLOW
  
  # Rules: Who can access what?
  rules:
  # Rule 1: Order Service can call GET /api/product
  - from:
    - source:
        principals:
        - cluster.local/ns/microservices/sa/order-api
    to:
    - operation:
        methods: ["GET"]
        paths: ["/api/product*"]
  
  # Rule 2: Anyone can call health check
  - from:
    - source:
        principals:
        - "*"  # Anyone
    to:
    - operation:
        methods: ["GET"]
        paths: ["/health"]
  
  # Rule 3: Admin namespace can do anything
  - from:
    - source:
        namespaces: ["admin"]
    to:
    - operation:
        methods: ["*"]
        paths: ["*"]
```

---

# 21. TRAFFIC MANAGEMENT: ADVANCED PATTERNS

## Canary Deployment

```
Scenario: Deploy ProductApi v2

Desired Process:
├─ Deploy v2 (1 pod)
├─ Route 5% traffic to v2
├─ Monitor for errors
├─ If all good: Increase to 25%
├─ Continue: 50%, 75%, 100%
├─ If errors: Rollback to v1

Steps:

1. Deploy v2 pods
   kubectl set image deployment/product-api \
     product-api=product-api:2.0.0 \
     -n microservices

2. Update VirtualService (90/10 split)
   kubectl apply -f - <<EOF
   apiVersion: networking.istio.io/v1beta1
   kind: VirtualService
   metadata:
     name: product-api
     namespace: microservices
   spec:
     hosts:
     - product-api
     http:
     - route:
       - destination:
           host: product-api
           subset: v1
         weight: 90
       - destination:
           host: product-api
           subset: v2
         weight: 10
   EOF

3. Monitor metrics (5-10 minutes)
   kubectl logs -f deployment/product-api -c product-api
   # Watch for errors in v2 pods
   
   Prometheus queries:
   rate(http_requests_total{pod=~"product-api-.*"}[1m])
   rate(errors_total{pod=~"product-api-.*"}[1m])

4. If all good, increase to 25/75
   # Update VirtualService weight: 75/25

5. Continue: 50/50, 75/25, 100/0

6. Full deployment complete!
   All traffic to v2, v1 pods can be deleted
```

## Blue-Green Deployment

```
Two complete environments:

Blue (Current):
├─ 3 pods running v1
├─ Service → Blue
├─ All traffic to v1
└─ Stable production

Green (New):
├─ 3 pods running v2
├─ Not receiving traffic yet
├─ Thoroughly tested
└─ Ready to switch

Deployment Process:

1. Deploy Green environment
   kubectl apply -f green-deployment.yaml
   # Creates 3 pods running v2

2. Run tests against Green
   # Curl Green service endpoints
   # Run integration tests
   # Performance tests
   # Load tests

3. Switch traffic to Green
   kubectl patch service product-api \
     -p '{"spec":{"selector":{"version":"v2"}}}'
   # Now service routes to Green pods
   
   # Or use VirtualService:
   kubectl patch VirtualService product-api \
     --type merge \
     -p '{"spec":{"hosts":[{"destination":{"subset":"v2","weight":100}}]}}'

4. Monitor Green environment
   # Watch for errors
   # Check performance
   # Monitor resource usage

5. If issues, rollback
   kubectl patch service product-api \
     -p '{"spec":{"selector":{"version":"v1"}}}'
   # Traffic immediately back to Blue

6. Once confident, delete Blue
   kubectl delete deployment product-api-v1

Advantages:
├─ Complete, immediate switch
├─ Easy rollback
├─ Full testing before switch
├─ No partial failures
└─ Downtime only during switch (milliseconds)
```

---

# 22. SECURITY: mTLS & AUTHORIZATION

## Mutual TLS (mTLS)

### How mTLS Works

```
Regular HTTPS (Client→Server):
├─ Client verifies server certificate
├─ Client creates encrypted connection
├─ Server doesn't verify client
└─ Server assumes client is legitimate

mTLS (Mutual TLS):
├─ Client verifies server certificate
├─ Server verifies client certificate
├─ Both encrypt communication
├─ Both authenticate each other
└─ Strongest security model

In Kubernetes with Istio:

Service A (Pod1) calls Service B (Pod2):

1. Service A's Envoy initiates connection to Service B

2. Certificate Exchange
   ├─ Service B's Envoy sends certificate
   ├─ A's Envoy: "Is this really Service B?"
   ├─ Checks certificate chain
   ├─ Verifies certificate signed by cluster CA
   ├─ Extracts identity: "cluster.local/ns/default/sa/service-b"
   │
   ├─ Service B's Envoy requests A's certificate
   ├─ A's Envoy sends certificate
   ├─ B's Envoy: "Is this really Service A?"
   ├─ Checks certificate chain
   ├─ Verifies certificate signed by cluster CA
   ├─ Extracts identity: "cluster.local/ns/default/sa/service-a"
   │
   └─ Both sides authenticated!

3. TLS Connection Established
   ├─ Both sides negotiate encryption
   ├─ Exchange encryption keys
   ├─ Connection encrypted with symmetric key
   └─ Both sides can communicate securely

4. Application Communication
   ├─ Service A sends HTTP request
   ├─ Encrypted by TLS layer
   ├─ Sent over network (safe)
   ├─ Service B's Envoy decrypts
   ├─ Forwards to Application B
   └─ Application unaware of encryption

5. Identity Available for Authorization
   ├─ Service B's Envoy knows caller identity
   ├─ "This request is from service-a"
   ├─ Can check AuthorizationPolicy
   ├─ Allow/deny based on identity
   └─ Fine-grained access control
```

### Istio Certificate Management

```
Citadel (Istio CA) generates and manages certificates:

Automatic Certificate Generation:

1. Each service account gets certificate
   ├─ service-a service account → certificate for service-a
   ├─ service-b service account → certificate for service-b
   └─ All signed by Istio CA

2. Certificates rotated automatically
   ├─ Istio default: 90 day lifetime
   ├─ Rotated every 24 hours automatically
   ├─ No manual intervention needed
   └─ Graceful rotation (old cert still valid for 24h)

3. Certificates stored in Kubernetes secrets
   └─ Mounted to pods by Envoy sidecar

4. mTLS enforcement
   ├─ PeerAuthentication: mode STRICT
   └─ All pod-to-pod communication encrypted
```

---

# 23. HANDS-ON LABS: REAL-WORLD IMPLEMENTATION

## Lab 1: Deploy Microservices

```bash
# 1. Create namespace
kubectl create namespace microservices

# 2. Build and push Docker images
docker build -f ProductApi/Dockerfile -t myregistry/product-api:1.0.0 .
docker build -f OrderApi/Dockerfile -t myregistry/order-api:1.0.0 .
docker push myregistry/product-api:1.0.0
docker push myregistry/order-api:1.0.0

# For KIND (local development):
kind load docker-image product-api:1.0.0 --name k8s-dev-cluster
kind load docker-image order-api:1.0.0 --name k8s-dev-cluster

# 3. Deploy ProductApi
kubectl apply -f - <<EOF
apiVersion: apps/v1
kind: Deployment
metadata:
  name: product-api
  namespace: microservices
spec:
  replicas: 2
  selector:
    matchLabels:
      app: product-api
  template:
    metadata:
      labels:
        app: product-api
        version: v1
    spec:
      containers:
      - name: api
        image: product-api:1.0.0
        ports:
        - containerPort: 8080
        resources:
          requests:
            cpu: 100m
            memory: 256Mi
          limits:
            cpu: 500m
            memory: 512Mi
        livenessProbe:
          httpGet:
            path: /api/product/health
            port: 8080
          initialDelaySeconds: 15
          periodSeconds: 10
        readinessProbe:
          httpGet:
            path: /api/product/health
            port: 8080
          initialDelaySeconds: 5
          periodSeconds: 5
---
apiVersion: v1
kind: Service
metadata:
  name: product-api
  namespace: microservices
spec:
  selector:
    app: product-api
  ports:
  - port: 80
    targetPort: 8080
EOF

# 4. Deploy OrderApi (calls ProductApi)
kubectl apply -f - <<EOF
apiVersion: apps/v1
kind: Deployment
metadata:
  name: order-api
  namespace: microservices
spec:
  replicas: 2
  selector:
    matchLabels:
      app: order-api
  template:
    metadata:
      labels:
        app: order-api
        version: v1
    spec:
      containers:
      - name: api
        image: order-api:1.0.0
        ports:
        - containerPort: 8081
        env:
        - name: PRODUCT_API_URL
          value: "http://product-api.microservices.svc.cluster.local"
        resources:
          requests:
            cpu: 100m
            memory: 256Mi
          limits:
            cpu: 500m
            memory: 512Mi
        livenessProbe:
          httpGet:
            path: /api/order/health
            port: 8081
          initialDelaySeconds: 15
          periodSeconds: 10
        readinessProbe:
          httpGet:
            path: /api/order/health
            port: 8081
          initialDelaySeconds: 5
          periodSeconds: 5
---
apiVersion: v1
kind: Service
metadata:
  name: order-api
  namespace: microservices
spec:
  selector:
    app: order-api
  ports:
  - port: 80
    targetPort: 8081
EOF

# 5. Verify deployments
kubectl get pods -n microservices
kubectl get svc -n microservices

# 6. Test connectivity
kubectl port-forward svc/order-api 8081:80 -n microservices

# In another terminal:
curl http://localhost:8081/api/order
```

## Lab 2: Implement Istio Service Mesh

```bash
# 1. Install Istio (if not already installed)
istioctl install --set profile=demo -y

# 2. Enable sidecar injection
kubectl label namespace microservices istio-injection=enabled

# 3. Restart pods to inject sidecars
kubectl rollout restart deployment -n microservices

# 4. Verify sidecars injected
kubectl get pods -n microservices -o jsonpath='{.items[0].spec.containers[*].name}'
# Should see: api, istio-proxy

# 5. Create DestinationRule for product-api
kubectl apply -f - <<EOF
apiVersion: networking.istio.io/v1beta1
kind: DestinationRule
metadata:
  name: product-api
  namespace: microservices
spec:
  host: product-api
  trafficPolicy:
    loadBalancer:
      simple: ROUND_ROBIN
    outlierDetection:
      consecutive5xxErrors: 5
      interval: 30s
      baseEjectionTime: 30s
  subsets:
  - name: v1
    labels:
      version: v1
EOF

# 6. Create VirtualService for routing
kubectl apply -f - <<EOF
apiVersion: networking.istio.io/v1beta1
kind: VirtualService
metadata:
  name: product-api
  namespace: microservices
spec:
  hosts:
  - product-api
  http:
  - route:
    - destination:
        host: product-api
        subset: v1
    timeout: 30s
    retries:
      attempts: 3
      perTryTimeout: 10s
EOF

# 7. Enable mTLS
kubectl apply -f - <<EOF
apiVersion: security.istio.io/v1beta1
kind: PeerAuthentication
metadata:
  name: default
  namespace: microservices
spec:
  mtls:
    mode: STRICT
EOF

# 8. Create AuthorizationPolicy
kubectl apply -f - <<EOF
apiVersion: security.istio.io/v1beta1
kind: AuthorizationPolicy
metadata:
  name: product-api-allow
  namespace: microservices
spec:
  selector:
    matchLabels:
      app: product-api
  action: ALLOW
  rules:
  - from:
    - source:
        namespaces: ["microservices"]
    to:
    - operation:
        methods: ["GET", "POST"]
EOF

# 9. Test with mTLS
kubectl port-forward svc/order-api 8081:80 -n microservices
curl http://localhost:8081/api/order
# Should work (within-cluster communication encrypted with mTLS)

# 10. View traffic with Kiali
kubectl port-forward -n istio-system svc/kiali 20000:20000
# Open http://localhost:20000
# Graph → select namespace microservices
# See service topology
```

## Lab 3: Canary Deployment with Istio

```bash
# 1. Deploy v2 alongside v1
kubectl apply -f - <<EOF
apiVersion: apps/v1
kind: Deployment
metadata:
  name: product-api-v2
  namespace: microservices
spec:
  replicas: 1
  selector:
    matchLabels:
      app: product-api
      version: v2
  template:
    metadata:
      labels:
        app: product-api
        version: v2
    spec:
      containers:
      - name: api
        image: product-api:2.0.0
        ports:
        - containerPort: 8080
        resources:
          requests:
            cpu: 100m
            memory: 256Mi
          limits:
            cpu: 500m
            memory: 512Mi
        livenessProbe:
          httpGet:
            path: /api/product/health
            port: 8080
          initialDelaySeconds: 15
          periodSeconds: 10
        readinessProbe:
          httpGet:
            path: /api/product/health
            port: 8080
          initialDelaySeconds: 5
          periodSeconds: 5
EOF

# 2. Label existing v1 deployment
kubectl patch deployment product-api -n microservices \
  -p '{"spec":{"template":{"metadata":{"labels":{"version":"v1"}}}}}'

# 3. Update DestinationRule to include v2
kubectl patch DestinationRule product-api -n microservices \
  --type merge \
  -p '{"spec":{"subsets":[{"name":"v1","labels":{"version":"v1"}},{"name":"v2","labels":{"version":"v2"}}]}}'

# 4. Update VirtualService for 90/10 split
kubectl apply -f - <<EOF
apiVersion: networking.istio.io/v1beta1
kind: VirtualService
metadata:
  name: product-api
  namespace: microservices
spec:
  hosts:
  - product-api
  http:
  - route:
    - destination:
        host: product-api
        subset: v1
      weight: 90
    - destination:
        host: product-api
        subset: v2
      weight: 10
    timeout: 30s
    retries:
      attempts: 3
      perTryTimeout: 10s
EOF

# 5. Monitor traffic split
kubectl logs -f deployment/product-api -c api | grep "Creating"
kubectl logs -f deployment/product-api-v2 -c api | grep "Creating"

# 6. Gradually increase v2 traffic
# 25/75, 50/50, 75/25, 100/0

# 7. Delete v1 when confident
kubectl delete deployment product-api -n microservices

# 8. Rename v2 to become primary
kubectl delete deployment product-api-v2 -n microservices
```

---

# 24. PRODUCTION PATTERNS & BEST PRACTICES

## Production Checklist

```
Before deploying to production:

√ Application Level:
├─ All endpoints have health checks
├─ Graceful shutdown (SIGTERM handling)
├─ Proper logging (structured JSON)
├─ Error handling and recovery
├─ Resource requests/limits configured
├─ No hardcoded credentials
└─ Security scanning (SAST, container scan)

√ Kubernetes Configuration:
├─ Resource requests and limits set
├─ Health checks (liveness + readiness)
├─ Proper labels and selectors
├─ ConfigMaps for configuration
├─ Secrets for sensitive data
├─ PersistentVolumes for state
├─ RBAC policies configured
└─ Network policies defined

√ Deployment:
├─ Images scanned for vulnerabilities
├─ Images signed and verified
├─ Helm charts used for deployments
├─ GitOps workflow (ArgoCD/Flux)
├─ Changes tracked in Git
├─ Automated rollbacks configured
└─ Canary/Blue-Green deployment ready

√ Monitoring:
├─ Prometheus scraping configured
├─ Key metrics being collected
├─ Grafana dashboards created
├─ Alerts configured (email/Slack)
├─ Log aggregation (ELK/Loki)
├─ Tracing enabled (Jaeger/Zipkin)
└─ On-call rotation established

√ High Availability:
├─ Multiple replicas (min 2)
├─ Pod Disruption Budgets defined
├─ Load balancing configured
├─ Failover tested
├─ Backup strategy implemented
├─ Disaster recovery plan documented
└─ Regular chaos engineering tests

√ Security:
├─ Network policies enforced
├─ RBAC with least privilege
├─ Secrets encrypted at rest
├─ TLS for all communication
├─ mTLS between services (Istio)
├─ Image registry requires authentication
├─ Container runtime security enabled
├─ Audit logging enabled
└─ Regular security audits

√ Operations:
├─ Runbooks for common issues
├─ On-call process defined
├─ Escalation path documented
├─ Communication channels setup
├─ Change management process
├─ Maintenance windows scheduled
└─ Incident response plan

√ Cost Management:
├─ Resource requests not excessive
├─ HPA configured (auto-scale)
├─ Unused resources cleaned up
├─ Reserved instances utilized
├─ Cost monitoring dashboards
└─ Budget alerts configured
```

## Common Failure Scenarios and Solutions

```
Scenario 1: Pod stuck in Pending

Symptoms:
├─ kubectl get pods → pod Status: Pending for 10+ minutes
├─ Events show: "0/3 nodes are available"
└─ Pod never becomes Running

Causes:
├─ Insufficient resources on nodes
│  └─ Request: 1Gi memory, nodes only have 256Mi free
├─ Node selector not matching any nodes
│  └─ Pod requires: disktype=ssd, but no nodes have it
├─ PVC not bound
│  └─ Volume doesn't exist yet

Solutions:
├─ Check events: kubectl describe pod <name>
├─ Check node resources: kubectl top nodes
├─ Increase cluster size: add more nodes
├─ Lower resource requests if possible
├─ Fix node selectors/affinity rules
├─ Create required volumes
└─ Use kubectl debug pod <name> to investigate

---

Scenario 2: CrashLoopBackOff

Symptoms:
├─ kubectl get pods → Status: CrashLoopBackOff
├─ Container keeps restarting
├─ Logs show errors
└─ Pod never stays running

Causes:
├─ Application crashing on startup
├─ Readiness probe failing continuously
├─ Missing configuration/environment variables
├─ Volume mount permission denied
├─ Database connection can't be established

Solutions:
├─ Check logs: kubectl logs <pod-name> --previous
│  └─ See why container crashed last time
├─ Increase initialDelaySeconds on probes
│  └─ Give app more time to start
├─ Check environment variables: kubectl exec <pod> -- env
├─ Test locally: docker run <image>
│  └─ Try running image locally to see errors
├─ Check permissions: kubectl describe pod <pod>
│  └─ Look for permission-denied errors
└─ Use kubectl debug to exec into pod pre-crash

---

Scenario 3: Nodes OutOfMemory

Symptoms:
├─ Kubectl describe node → memory pressure: True
├─ Some pods evicted
├─ Application unreliable
└─ Performance degraded

Causes:
├─ Pods requesting more memory than available
├─ Multiple pods on same node (bad scheduling)
├─ Memory leaks in applications
├─ Node has not enough memory for workloads

Solutions:
├─ Check memory usage: kubectl top pods
│  └─ See which pods consuming most memory
├─ Increase node size (more memory)
├─ Add more nodes to cluster
├─ Lower memory requests/limits
├─ Implement memory-aware HPA
├─ Use memory profiling to find leaks
├─ Spread pods across more nodes (pod affinity)
└─ Enable quality-of-service to protect critical pods

---

Scenario 4: Service endpoints not updating

Symptoms:
├─ kubectl get endpoints <service> → no endpoints listed
├─ Pods are running but service doesn't route to them
├─ Connections fail
└─ "No endpoints available"

Causes:
├─ Pod selector labels don't match deployment labels
├─ Readiness probe failing (pod removed from endpoints)
├─ Service in different namespace than pods
├─ Port numbers mismatched

Solutions:
├─ Check selectors: kubectl get svc <name> -o yaml
│  └─ Verify labels match pod labels
├─ Check pod labels: kubectl get pods --show-labels
├─ Verify readiness: kubectl get pods
│  └─ Status should be Running and Ready
├─ Check port: kubectl describe svc <name>
│  └─ targetPort must match container port
├─ Increase readiness probe timeout
└─ Check namespaces match

---

Scenario 5: 502 Bad Gateway

Symptoms:
├─ curl returns: 502 Bad Gateway
├─ Service exists but requests fail
├─ Intermittent failures
└─ Error from upstream service

Causes:
├─ All backend pods down/unready
├─ Service load balancer can't reach pods
├─ Network policies blocking traffic
├─ Incorrect service endpoint configuration
├─ Resource limits exceeded (slow response)

Solutions:
├─ Check pods: kubectl get pods
│  └─ Ensure pods Running and Ready
├─ Check logs: kubectl logs <pod>
│  └─ See application errors
├─ Check endpoints: kubectl get endpoints <service>
│  └─ Ensure endpoints listed
├─ Check network policy: kubectl get networkpolicy
│  └─ Verify traffic allowed
├─ Check resource usage: kubectl top pods
│  └─ See if pods CPU/memory throttled
└─ Increase timeout: adjust timeout in VirtualService or configmap
```

## GitOps Workflow

```
GitOps = Git as source of truth

Workflow:

1. Developer makes changes
   ├─ Updates application code
   ├─ Commits to Git: git push
   ├─ Pull request review
   └─ Merged to main

2. CI pipeline triggered
   ├─ Build Docker image
   ├─ Run tests
   ├─ Scan for vulnerabilities
   ├─ Push to registry: myregistry/myapp:1.2.3
   └─ Update Helm values.yaml with new tag

3. GitOps tool (ArgoCD/Flux) watches Git
   ├─ Detects changes in repository
   ├─ New image version in values.yaml
   ├─ Applies changes to cluster: helm upgrade ...
   └─ Deployment happens automatically

4. Continuous reconciliation
   ├─ ArgoCD checks cluster state every 5 minutes
   ├─ Actual state != desired state in Git?
   ├─ Automatically applies changes
   ├─ Cluster = Git, always
   └─ No manual kubectl apply needed

5. Audit trail
   ├─ All changes in Git history
   ├─ Who deployed what and when
   ├─ Easy to see differences (git diff)
   ├─ Rollback: git revert + push
   └─ Perfect for compliance

Benefits:
├─ Single source of truth (Git)
├─ Automated deployments (no manual kubectl)
├─ Easy rollback (git revert)
├─ Audit trail (all in Git)
├─ Consistent environment (Git replicated to all)
├─ Self-healing (ArgoCD auto-syncs)
└─ Disaster recovery (Git → cluster)

Tools:
├─ ArgoCD (declarative continuous deployment)
├─ Flux (GitOps for Kubernetes)
├─ Spinnaker (advanced deployment)
└─ Jenkins-X (cloud-native CI/CD)
```

---

## Conclusion: Your Kubernetes Mastery Journey

Congratulations! You've learned:

✅ **Kubernetes Architecture** - How every component works
✅ **Deployment** - Getting applications running reliably
✅ **Networking** - Services, DNS, load balancing
✅ **Storage** - Persistent data management
✅ **Configuration** - ConfigMaps and Secrets
✅ **Helm** - Package management
✅ **Monitoring** - Observability and metrics
✅ **Service Mesh** - Istio for advanced networking
✅ **Security** - mTLS and authorization
✅ **Scaling** - Auto-scaling for demand
✅ **Tools** - kubectl and k9s
✅ **Production Patterns** - Real-world practices

### Next Steps:

1. **Deploy Real Applications**
   ├─ Start with simple apps
   ├─ Graduate to microservices
   └─ Learn from mistakes safely

2. **Implement Istio**
   ├─ Service mesh brings superpowers
   ├─ Start with traffic management
   ├─ Add mTLS for security
   └─ Enable advanced observability

3. **Setup Monitoring**
   ├─ Prometheus for metrics
   ├─ Grafana for dashboards
   ├─ Alerts for incidents
   └─ Understand your systems

4. **Embrace GitOps**
   ├─ Git as source of truth
   ├─ Automated deployments
   ├─ Easy rollbacks
   └─ Audit trail for compliance

5. **Master the Tools**
   ├─ kubectl for management
   ├─ k9s for debugging
   ├─ Helm for packaging
   └─ Custom scripts for automation

6. **Learn from Community**
   ├─ Kubernetes blog
   ├─ CNCF ecosystem
   ├─ Open source projects
   └─ Community conferences

### Key Takeaways:

1. **Declarative > Imperative**
   - Tell Kubernetes what you want, not how
   - System figures out how to achieve it
   - Easier to automate, version, rollback

2. **Infrastructure as Code**
   - All config in Git
   - Reproducible
   - Auditable
   - Version controlled

3. **Reliability through Redundancy**
   - Multiple replicas
   - Multiple nodes
   - Health checks
   - Auto-healing
   - Graceful degradation

4. **Observability is Key**
   - Can't manage what you can't measure
   - Metrics guide decisions
   - Logs explain problems
   - Traces show relationships

5. **Security Matters**
   - Defense in depth
   - Network policies
   - RBAC
   - Encryption
   - Least privilege

6. **Plan for Failure**
   - Chaos engineering
   - Disaster recovery
   - Backup strategies
   - Runbooks for incidents
   - Communication plans

### Final Wisdom:

> "Kubernetes is not magic. It's well-designed infrastructure 
> automation with clear principles. Understand the principles,
> and the tool becomes natural."

You now have the knowledge. The rest is practice, experimentation, and learning from your deployments. Start small, scale gradually, and never stop learning.
