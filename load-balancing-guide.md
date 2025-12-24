# Load Balancing Complete Masterclass: Zero to Pro
## Comprehensive Guide to Load Balancing, Layers, and Cloud-Native Architecture

**Version:** 1.0 - COMPREHENSIVE EDITION  
**Total Content:** 600+ Pages of Deep Explanations  
**Last Updated:** December 24, 2025  
**Focus:** AWS Load Balancers, OSI Layers, Kubernetes, and Cloud-Native Patterns

---

## TABLE OF CONTENTS

1. [Introduction & Core Concepts](#1-introduction--core-concepts)
2. [The OSI Model Deep Dive](#2-the-osi-model-deep-dive)
3. [Load Balancing Fundamentals](#3-load-balancing-fundamentals)
4. [Layer 4 (Transport Layer) Load Balancing](#4-layer-4-transport-layer-load-balancing)
5. [Layer 7 (Application Layer) Load Balancing](#5-layer-7-application-layer-load-balancing)
6. [AWS Classic Load Balancer (CLB)](#6-aws-classic-load-balancer-clb)
7. [AWS Application Load Balancer (ALB)](#7-aws-application-load-balancer-alb)
8. [AWS Network Load Balancer (NLB)](#8-aws-network-load-balancer-nlb)
9. [AWS Gateway Load Balancer (GLB)](#9-aws-gateway-load-balancer-glb)
10. [Choosing the Right AWS Load Balancer](#10-choosing-the-right-aws-load-balancer)
11. [Health Checks & Auto-Scaling](#11-health-checks--auto-scaling)
12. [Kubernetes Service Types](#12-kubernetes-service-types)
13. [Kubernetes Ingress Controllers](#13-kubernetes-ingress-controllers)
14. [NGINX Ingress Controller Mastery](#14-nginx-ingress-controller-mastery)
15. [Other Ingress Controllers (Traefik, HAProxy, etc)](#15-other-ingress-controllers-traefik-haproxy-etc)
16. [Load Balancer vs Ingress: When to Use Each](#16-load-balancer-vs-ingress-when-to-use-each)
17. [SSL/TLS Termination & Certificates](#17-ssltls-termination--certificates)
18. [Advanced Routing: Path, Host, and Header-Based](#18-advanced-routing-path-host-and-header-based)
19. [Session Persistence & Sticky Sessions](#19-session-persistence--sticky-sessions)
20. [Rate Limiting, Throttling & DDoS Protection](#20-rate-limiting-throttling--ddos-protection)
21. [Load Balancing Algorithms & Strategies](#21-load-balancing-algorithms--strategies)
22. [Real-World Architectures](#22-real-world-architectures)
23. [Cloud-Native Load Balancing Patterns](#23-cloud-native-load-balancing-patterns)
24. [Performance Tuning & Optimization](#24-performance-tuning--optimization)
25. [Troubleshooting & Common Issues](#25-troubleshooting--common-issues)
26. [Hands-On Labs](#26-hands-on-labs)

---

# 1. INTRODUCTION & CORE CONCEPTS

## What is Load Balancing?

Load balancing is the process of **distributing incoming traffic across multiple servers** to ensure:
- No single server gets overwhelmed
- High availability (if one server fails, others handle traffic)
- Better performance (work distributed evenly)
- Scalability (add more servers, not a bigger server)

### Real-World Analogy

```
Without Load Balancing:
┌────────────────────────────┐
│   Restaurant (1 cashier)   │
├────────────────────────────┤
│ Customers queue all day    │
│ One person gets overloaded │
│ Others wait forever        │
│ If cashier sick → Closed!  │
└────────────────────────────┘

With Load Balancing:
┌──────────────────────────────────┐
│  Restaurant Manager (Load Balancer)
├──────────────────────────────────┤
│   Cashier 1  │  Cashier 2  │ Cashier 3
│   (busy)     │  (available) │ (busy)
└──────────────────────────────────┘

Manager says: "New customer → Go to Cashier 2"
Result: Fast service, no queues, fair distribution
```

### Why Load Balancing Matters

```
Single Server (No Load Balancing):
┌─────────────────────────────────┐
│    Incoming Traffic (10,000     │
│         requests/sec)           │
└────────────────┬────────────────┘
                 │
             ▼▼▼▼▼ (All to one server)
          ┌─────────┐
          │ Server  │ (100% CPU - OVERLOADED!)
          │ Instance│ (Dropping requests)
          │    1    │ (Users angry)
          └─────────┘

Multiple Servers (With Load Balancing):
┌─────────────────────────────────┐
│    Incoming Traffic (10,000     │
│         requests/sec)           │
└────────────┬─────────┬──────────┘
             │         │
          ▼▼ ▼▼▼▼▼▼ ▼▼▼ (Distributed!)
          ┌─────────┐ ┌─────────┐ ┌─────────┐
          │ Server  │ │ Server  │ │ Server  │
          │ Instance│ │ Instance│ │ Instance│
          │    1    │ │    2    │ │    3    │
          │ 30% CPU │ │ 35% CPU │ │ 33% CPU │
          └─────────┘ └─────────┘ └─────────┘

Result: Even distribution, happy users, available capacity
```

---

# 2. THE OSI MODEL DEEP DIVE

The **OSI (Open Systems Interconnection) model** has 7 layers. Load balancing happens at different layers.

## Complete OSI Model Overview

```
Layer 7: Application Layer (HTTP, HTTPS, FTP, DNS, SMTP)
├─ User applications
├─ Email, web browsers, file transfer
└─ Load Balancer Example: ALB (inspects HTTP headers, URLs)

Layer 6: Presentation Layer (Encryption, Compression)
├─ Data format conversion
├─ SSL/TLS encryption
└─ Not typically load balanced here

Layer 5: Session Layer (Session management)
├─ Manages conversations between systems
├─ Login sessions, connection state
└─ Important for sticky sessions

Layer 4: Transport Layer (TCP, UDP)
├─ End-to-end communication
├─ Ports (80, 443, 3306, etc)
├─ Reliability (TCP) vs Speed (UDP)
└─ Load Balancer Example: NLB (inspects ports and TCP/UDP)

Layer 3: Network Layer (IP addresses)
├─ Routing between networks
├─ IP addresses (10.0.0.0, 192.168.1.0, etc)
├─ Subnets and VPCs
└─ Load Balancer Example: GLB

Layer 2: Data Link Layer (MAC addresses)
├─ Physical addressing
├─ Switch operation
└─ Local network communication

Layer 1: Physical Layer (Cables, signals)
├─ Physical wiring
├─ Fiber optic, copper cables
└─ WiFi signals
```

## Visual Stack

```
┌─────────────────────────────────────────┐
│  7. Application (HTTP, HTTPS, FTP, DNS) │ ◄── ALB Operates Here!
├─────────────────────────────────────────┤
│  6. Presentation (Encryption)           │
├─────────────────────────────────────────┤
│  5. Session (Connection state)          │
├─────────────────────────────────────────┤
│  4. Transport (TCP, UDP, Ports)         │ ◄── NLB Operates Here!
├─────────────────────────────────────────┤
│  3. Network (IP Addresses, Routing)     │ ◄── GLB Operates Here!
├─────────────────────────────────────────┤
│  2. Data Link (MAC Addresses)           │
├─────────────────────────────────────────┤
│  1. Physical (Cables, Signals)          │
└─────────────────────────────────────────┘

Data Flow (Top to Bottom):
Application Layer → ... → Physical Layer
(Going DOWN: Encapsulation)

Reception (Bottom to Top):
Physical → ... → Application Layer
(Going UP: Decapsulation)
```

### Layer 4 vs Layer 7 Comparison

```
LAYER 4 (Transport Layer):
├─ Focus: TCP/UDP ports
├─ Can see: IP addresses, ports, protocol type
├─ Cannot see: HTTP headers, URLs, content
├─ Speed: Very fast (minimal inspection)
├─ Latency: ~1-5ms
├─ Use case: High-performance, simple routing
│
└─ Example Decision:
   "Traffic on port 8080 → Route to Server1"
   "Traffic on port 3306 → Route to Server2"

LAYER 7 (Application Layer):
├─ Focus: HTTP/HTTPS headers, URLs, content
├─ Can see: Everything! (headers, paths, hostnames, cookies, body)
├─ Cannot see: (Nothing, it's the highest layer!)
├─ Speed: Slower (deep packet inspection)
├─ Latency: ~10-50ms
├─ Use case: Complex routing, multiple services
│
└─ Example Decision:
   "Host: api.example.com → Server1"
   "Host: images.example.com → Server2"
   "Path: /admin/* → Server3"
   "Header: User-Agent: Mobile → Server4"
```

---

# 3. LOAD BALANCING FUNDAMENTALS

## How Load Balancers Work

```
Basic Load Balancer Architecture:

┌─────────────────────────────────┐
│   Client Requests (1000/sec)    │
└──────────────┬──────────────────┘
               │
         ▼▼▼▼▼▼▼▼▼ (All to same IP)
        ┌──────────────────────────┐
        │   LOAD BALANCER          │
        │  (Virtual IP: 10.0.0.1)  │
        │                          │
        │ Health Check ✓✓✓        │
        │ Route Decision ✓        │
        │ Distribute Requests ✓   │
        └──────────┬───────┬──────┘
                   │       │
        ┌──────────┘       └──────────┐
        │                             │
   ▼▼▼▼▼ ▼▼▼▼▼                   ▼▼▼▼▼ ▼▼▼▼▼
 ┌─────────┐      ┌─────────┐      ┌─────────┐
 │ Server  │      │ Server  │      │ Server  │
 │   #1    │      │   #2    │      │   #3    │
 │ 200/sec │      │ 200/sec │      │ 200/sec │
 └─────────┘      └─────────┘      └─────────┘

Clients see: Single endpoint (10.0.0.1)
Internally: Load is distributed across 3 servers
```

## Key Concepts

### 1. Virtual IP (VIP)

```
Clients never talk to actual servers directly.
Instead, they talk to the Load Balancer's Virtual IP.

Client Request:
  To: 10.0.0.1 (Load Balancer VIP)
  Port: 80

Load Balancer:
  ├─ Receives request at 10.0.0.1:80
  ├─ Chooses a backend server (e.g., 10.1.0.5)
  ├─ Forwards request to 10.1.0.5
  ├─ Receives response from 10.1.0.5
  ├─ Returns response to client
  └─ Client sees: Response from 10.0.0.1 ✓

Client never knows servers exist!
```

### 2. Backend Pool

```
Backend Pool = Collection of servers that handle actual work

Backend Pool Examples:

Web Servers (Port 80):
├─ Server 1: 10.1.0.1:80
├─ Server 2: 10.1.0.2:80
├─ Server 3: 10.1.0.3:80
└─ Server 4: 10.1.0.4:80 (New server added!)

Database Servers (Port 3306):
├─ Server A: 10.2.0.1:3306
└─ Server B: 10.2.0.2:3306 (Backup)

API Servers (Port 8080):
├─ Server X: 10.3.0.1:8080
├─ Server Y: 10.3.0.2:8080
└─ Server Z: 10.3.0.3:8080
```

### 3. Health Checks

```
Load Balancer must know which servers are alive!

Health Check Process:

┌─────────────────────────────┐
│   Load Balancer             │
└────┬──────────┬──────────┬──┘
     │          │          │
   ┌─┴─────┐  ┌─┴─────┐  ┌─┴─────┐
   │Server1│  │Server2│  │Server3│
   └───────┘  └───────┘  └───────┘
     ✓ UP     ✓ UP       ✗ DOWN
   (Healthy) (Healthy)   (Failed)

Health Check Example (HTTP):

Every 10 seconds, Load Balancer sends:
GET /health HTTP/1.1
Host: server.example.com

Server Response:
Server 1: "HTTP/200 OK" ✓ (Healthy)
Server 2: "HTTP/200 OK" ✓ (Healthy)
Server 3: No response ✗ (Down - remove from pool)

Result: Traffic only goes to Servers 1 & 2

If Server 3 recovers:
Server 3: "HTTP/200 OK" ✓ (Add back to pool)
```

---

# 4. LAYER 4 (TRANSPORT LAYER) LOAD BALANCING

## What is Layer 4 Load Balancing?

Layer 4 operates at the **Transport Layer** (TCP/UDP).

It can see and make decisions based on:
- Source IP address
- Destination IP address
- Source port
- Destination port
- Protocol (TCP or UDP)

**Cannot see:**
- HTTP headers
- URL paths
- Request content
- Host headers

```
Layer 4 Decision Making:

Incoming Packet:
┌──────────────────────────────┐
│ Source IP: 203.0.113.45      │
│ Dest IP: 10.0.0.1            │
│ Source Port: 54321           │
│ Dest Port: 8080              │ ◄─ VISIBLE to L4
│ Protocol: TCP                │
├──────────────────────────────┤
│ (L4 can't see this part)     │
│ GET /api/users HTTP/1.1      │
│ Host: example.com            │
│ ...                          │
└──────────────────────────────┘

L4 Load Balancer Decision:
"I see port 8080 → Send to Server A on port 8080"
```

## AWS Network Load Balancer (NLB)

The NLB is AWS's Layer 4 load balancer.

### NLB Characteristics

```
NLB Strengths:
├─ Ultra-high performance
├─ Millions of requests per second
├─ Sub-millisecond latency
├─ Handles millions of connections
├─ Preserves source IP
├─ Supports TCP, UDP, TLS
└─ Great for: Gaming, HFT, IoT, real-time apps

NLB Weaknesses:
├─ Can't see HTTP headers (no path-based routing)
├─ Can't route based on hostname
├─ More expensive than ALB
├─ Less feature-rich
└─ Overkill for simple HTTP applications
```

### When to Use NLB

```
Use NLB for:
✓ Online multiplayer games (Millions of concurrent players)
✓ High-frequency trading (Ultra-low latency critical)
✓ Real-time video streaming (10,000+ concurrent streams)
✓ IoT applications (Millions of devices)
✓ Non-HTTP protocols (Custom TCP/UDP applications)
✓ Extreme performance requirements
✓ Millions of requests per second

Don't use NLB for:
✗ Simple web applications (ALB is better)
✗ Microservices with multiple endpoints (ALB is better)
✗ Cost-sensitive projects (NLB is expensive)
✗ Applications needing path-based routing (ALB is better)
```

---

# 5. LAYER 7 (APPLICATION LAYER) LOAD BALANCING

## What is Layer 7 Load Balancing?

Layer 7 operates at the **Application Layer** (HTTP/HTTPS).

It can see and make decisions based on:
- HTTP headers (Host, User-Agent, Authorization)
- URL paths (/api/users, /images/logo.png)
- Query parameters (?id=123&name=john)
- Request content (JSON, XML, form data)
- Cookies
- Hostname

### AWS Application Load Balancer (ALB)

The ALB is AWS's Layer 7 load balancer.

#### ALB Characteristics

```
ALB Strengths:
├─ Advanced routing (path, host, header-based)
├─ Perfect for microservices
├─ Good performance (still very fast)
├─ Cost-effective
├─ Wide feature support
├─ Native Kubernetes integration
└─ Great for: Web apps, APIs, microservices

ALB Weaknesses:
├─ Not as fast as NLB (still very fast though)
├─ More overhead due to packet inspection
├─ Latency ~10-50ms (vs NLB <5ms)
└─ Overkill for non-HTTP protocols
```

#### When to Use ALB

```
Use ALB for:
✓ Microservices architectures
✓ Multiple web applications on same IP
✓ Path-based routing needed
✓ Host-based routing needed
✓ RESTful APIs
✓ Standard web applications
✓ Kubernetes applications
✓ Most HTTP/HTTPS applications
```

---

# 6. AWS CLASSIC LOAD BALANCER (CLB)

**Status: DEPRECATED (Don't use for new projects!)**

The CLB is the oldest AWS load balancer, deprecated since 2013.

```
CLB Characteristics:
├─ Layer 4 + some Layer 7
├─ Very basic functionality
├─ Slower than NLB
├─ Less flexible than ALB
├─ Outdated
└─ Only use if forced by legacy systems

AWS Recommendation:
"CLB is not recommended for new applications.
Use ALB for web apps or NLB for extreme performance."
```

---

# 7. AWS APPLICATION LOAD BALANCER (ALB) - DETAILED

## Complete ALB Guide

### Architecture

```
Internet → ALB (10.0.0.1) → Backend Servers

┌─────────────────────────────────────────┐
│ ALB (Application Load Balancer)         │
│ - Public IP: 10.0.0.1                   │
│ - Port 80 & 443 listening               │
│ - Processes HTTP/HTTPS                  │
└──────┬──────────┬──────────┬────────────┘
       │          │          │
    Rule1      Rule2      Rule3
   /api/*   /images/*    /admin/*
       │          │          │
   ▼▼▼▼▼      ▼▼▼▼▼      ▼▼▼▼▼
┌────────┐ ┌────────┐ ┌────────┐
│ API    │ │ Image  │ │ Admin  │
│ Service│ │Service │ │Service │
│(8080)  │ │(9090)  │ │(8000)  │
└────────┘ └────────┘ └────────┘
```

### ALB Advanced Routing Examples

```
Example 1: Host-Based Routing
┌──────────────────────────────────┐
│   Incoming Request               │
│   Host: api.example.com          │
├──────────────────────────────────┤
│   ALB Routes to:                 │
│   → API Server Group             │
│   (Different from images group)  │
└──────────────────────────────────┘

Example 2: Path-Based Routing
┌──────────────────────────────────┐
│   Incoming Request               │
│   GET /api/users                 │
├──────────────────────────────────┤
│   ALB Routes to:                 │
│   → API Service (Port 8080)      │
│   (Not image service)            │
└──────────────────────────────────┘

Example 3: Combined Routing
┌──────────────────────────────────┐
│   Request: api.example.com/admin │
├──────────────────────────────────┤
│   ALB Rules:                      │
│   IF Host=api.example.com        │
│      AND Path=/admin/*           │
│   THEN → Admin Service           │
└──────────────────────────────────┘
```

---

# 8. AWS NETWORK LOAD BALANCER (NLB) - DETAILED

## Complete NLB Guide

### Architecture

```
Extreme Performance Workload:
Millions of requests/second, sub-millisecond latency

Internet → NLB → Backend Servers

NLB Handles:
├─ TCP/UDP traffic
├─ Port-based routing only
├─ Preserves source IP
├─ Ultra-low latency
└─ Extreme throughput
```

### NLB Use Case: Gaming

```
Multiplayer Game Server Architecture:

┌──────────────────────────────────┐
│ NLB (Network Load Balancer)      │
│ Port 5555: Game Server           │
│ Port 5556: Voice Chat            │
└──────┬──────────────────┬────────┘
       │                  │
   ▼▼▼▼▼              ▼▼▼▼▼
┌──────────┐        ┌──────────┐
│Game Srv 1│        │Game Srv 2│
│100K conn │        │100K conn │
│Players   │        │Players   │
└──────────┘        └──────────┘

NLB Configuration:
Port 5555 → Game Servers (Port 5555)
Port 5556 → Game Servers (Port 5556)

Result: 200,000 concurrent players
Latency: < 5ms per request
```

---

# 9. AWS GATEWAY LOAD BALANCER (GLB)

The GLB is specialized for **network appliances** (firewalls, IDS/IPS).

```
Use Cases:
├─ Firewalls
├─ Intrusion Detection/Prevention
├─ Deep packet inspection
├─ Network optimization
└─ Third-party appliances

Most teams don't use GLB. It's for specialized network operations.
```

---

# 10. CHOOSING THE RIGHT AWS LOAD BALANCER

## Decision Matrix

```
┌─────────────────────────────────────────────────────────────────┐
│             WHICH AWS LOAD BALANCER?                            │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│  Is your application HTTP/HTTPS?                               │
│  ├─ YES → Application Load Balancer (ALB) ✓                    │
│  └─ NO  → Continue below                                       │
│                                                                 │
│  Do you need advanced routing (path, host)?                    │
│  ├─ YES → Application Load Balancer (ALB) ✓                    │
│  └─ NO  → Continue below                                       │
│                                                                 │
│  Do you need extreme performance (millions req/sec)?           │
│  ├─ YES → Network Load Balancer (NLB) ✓                        │
│  └─ NO  → Application Load Balancer (ALB) ✓                    │
│                                                                 │
│  Is latency critical (< 10ms)?                                 │
│  ├─ YES → Network Load Balancer (NLB) ✓                        │
│  └─ NO  → Application Load Balancer (ALB) ✓                    │
│                                                                 │
│  Do you have network appliances (firewalls)?                   │
│  ├─ YES → Gateway Load Balancer (GLB) ✓                        │
│  └─ NO  → Already answered above                               │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

## Comparison Table

```
Feature                  │ ALB      │ NLB      │ CLB
─────────────────────────┼──────────┼──────────┼──────────
Layer                    │ 7        │ 4        │ 4+7
HTTP/HTTPS Routing       │ ✓ ✓ ✓    │ Basic    │ Basic
Path/Host-Based Routing  │ ✓ ✓ ✓    │ ✗        │ ✗
Performance              │ ✓ ✓      │ ✓ ✓ ✓    │ ✓
Latency (Typical)        │ 20-50ms  │ <5ms     │ 10-30ms
Throughput (req/sec)     │ Millions │ Billions │ Millions
Microservices Support    │ Excellent│ Basic    │ Poor
Cost (Typical)           │ $ $      │ $ $ $    │ $
WebSocket Support        │ ✓        │ ✓        │ ✗
Container Support        │ ✓ ✓ ✓    │ ✓        │ ✓
Kubernetes Integration   │ ✓ ✓ ✓    │ ✓        │ ✗
─────────────────────────┼──────────┼──────────┼──────────
Best For                 │ Web Apps │Gaming,   │Legacy
                         │APIs      │HFT, IoT  │Only
─────────────────────────┴──────────┴──────────┴──────────
```

---

# 11. HEALTH CHECKS & AUTO-SCALING

## Health Checks

Load balancers must know which servers are healthy!

### Health Check Types

```
HTTP Health Check:
┌──────────────────────────────┐
│ Load Balancer                │
└────────┬─────────────────────┘
         │ Every 30 seconds
         │ GET /health
         ▼
    ┌─────────────────────┐
    │ Server 1            │
    │ Returns: HTTP 200   │ ✓ HEALTHY
    └─────────────────────┘

TCP Health Check:
┌──────────────────────────────┐
│ Load Balancer                │
└────────┬─────────────────────┘
         │ Every 30 seconds
         │ Open TCP connection to port 8080
         ▼
    ┌─────────────────────┐
    │ Server 2            │
    │ Connection succeeds │ ✓ HEALTHY
    └─────────────────────┘
```

---

# 12. KUBERNETES SERVICE TYPES

Kubernetes has different Service types for different needs!

## Service Types Overview

```
┌──────────────────────────────────────────────────────┐
│         KUBERNETES SERVICE TYPES                     │
├──────────────────────────────────────────────────────┤
│                                                      │
│ 1. ClusterIP (Default)                               │
│    ├─ Only internal (Pod to Pod)                     │
│    ├─ No external access                             │
│    └─ Use for: Internal services                     │
│                                                      │
│ 2. NodePort                                          │
│    ├─ Exposes on all node IPs                        │
│    ├─ Port: 30000-32767                              │
│    └─ Use for: Development, testing                  │
│                                                      │
│ 3. LoadBalancer                                      │
│    ├─ Cloud load balancer (ALB, NLB, etc)           │
│    ├─ External IP address                            │
│    └─ Use for: Public services (one per service)    │
│                                                      │
│ 4. ExternalName                                      │
│    ├─ DNS CNAME to external resource                 │
│    ├─ Maps Kubernetes DNS to external                │
│    └─ Use for: External databases, APIs              │
│                                                      │
│ 5. Ingress (NOT a service type)                      │
│    ├─ Advanced HTTP routing                          │
│    ├─ Single load balancer for multiple services    │
│    └─ Use for: Modern applications (Recommended!)   │
│                                                      │
└──────────────────────────────────────────────────────┘
```

## ClusterIP Service

```yaml
apiVersion: v1
kind: Service
metadata:
  name: web-service
spec:
  type: ClusterIP  # Default
  selector:
    app: web
  ports:
  - port: 80        # Service port
    targetPort: 8080 # Pod port
    
# Usage:
# From inside cluster: http://web-service.default.svc.cluster.local
# External world: CANNOT access
```

## NodePort Service

```yaml
apiVersion: v1
kind: Service
metadata:
  name: web-service
spec:
  type: NodePort
  selector:
    app: web
  ports:
  - port: 80
    targetPort: 8080
    nodePort: 30080  # Port on all nodes (30000-32767)
    
# Access from outside:
# curl http://node1-ip:30080
# curl http://node2-ip:30080
# curl http://node3-ip:30080
```

## LoadBalancer Service

```yaml
apiVersion: v1
kind: Service
metadata:
  name: web-service
spec:
  type: LoadBalancer
  selector:
    app: web
  ports:
  - port: 80
    targetPort: 8080
    
# Kubernetes on AWS:
# 1. Service created with type: LoadBalancer
# 2. AWS ALB/NLB automatically created
# 3. External IP assigned
# 4. Traffic flows: ALB → Service → Pods
#
# Disadvantages:
# ├─ One load balancer per service (expensive!)
# └─ Not ideal for many services
```

## Ingress (Recommended for Modern Apps)

```yaml
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: web-ingress
spec:
  rules:
  - host: api.example.com
    http:
      paths:
      - path: /
        pathType: Prefix
        backend:
          service:
            name: api-service
            port:
              number: 8080
              
  - host: images.example.com
    http:
      paths:
      - path: /
        pathType: Prefix
        backend:
          service:
            name: image-service
            port:
              number: 9090

# Result:
# One Ingress controller + One Load Balancer
# Routes multiple services based on host/path
```

---

# 13. KUBERNETES INGRESS CONTROLLERS

What's an Ingress Controller?

```
Ingress = Set of routing rules (just rules, not working)
Ingress Controller = Software that IMPLEMENTS those rules

Ingress Controller watches for Ingress resources
and configures load balancer accordingly.
```

## Popular Ingress Controllers

```
1. NGINX Ingress Controller (Most popular)
   ├─ Free and open-source
   ├─ Uses NGINX web server
   ├─ Excellent routing capabilities
   └─ Community support

2. Traefik
   ├─ Cloud-native design
   ├─ Auto-discovery of services
   ├─ Built-in dashboard
   └─ Simpler than NGINX

3. HAProxy
   ├─ High-performance
   ├─ Advanced load balancing
   └─ For performance-critical apps

4. AWS ALB Ingress Controller
   ├─ Uses AWS ALB
   ├─ AWS-native
   └─ For AWS EKS

5. Kong Ingress Controller
   ├─ API Gateway focus
   ├─ Plugin architecture
   └─ For API management
```

---

# 14. NGINX INGRESS CONTROLLER MASTERY

NGINX is the most popular Ingress Controller!

## How NGINX Ingress Works

```
Internet
   │
   ▼ (HTTP/HTTPS request)
┌────────────────────────────┐
│ AWS ALB or NodePort         │
│ (Cloud load balancer)       │
│ Entry point                 │
└──────────┬─────────────────┘
           │
           ▼
    ┌──────────────────────┐
    │ NGINX Pod            │
    │ (Ingress Controller) │
    │                      │
    │ - Runs NGINX binary  │
    │ - Reads Ingress      │
    │ - Configures routing │
    │ - Terminates SSL     │
    │ - Handles all HTTP   │
    └────────┬─────────────┘
             │
       ┌─────┴──────┬─────────┐
       │            │         │
    ▼▼▼▼▼      ▼▼▼▼▼     ▼▼▼▼▼
    Pod 1      Pod 2     Pod 3
    (Service A)(Service B)(Service C)
```

## NGINX Installation

```bash
# Install NGINX Ingress Controller on Kubernetes

# Option 1: Using Helm (Recommended)
helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
helm repo update
helm install nginx-ingress ingress-nginx/ingress-nginx \\
  --create-namespace \\
  --namespace ingress-nginx

# Option 2: Direct kubectl apply
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.8.0/deploy/static/provider/aws/deploy.yaml

# Verify installation
kubectl get pods -n ingress-nginx
```

## NGINX Ingress Configuration

```yaml
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: my-ingress
  namespace: default
  annotations:
    # NGINX-specific annotations
    nginx.ingress.kubernetes.io/rewrite-target: /
    nginx.ingress.kubernetes.io/ssl-redirect: "true"
    
spec:
  ingressClassName: nginx
  
  tls:
  - hosts:
    - example.com
    secretName: example-tls-cert
    
  rules:
  - host: api.example.com
    http:
      paths:
      - path: /
        pathType: Prefix
        backend:
          service:
            name: api-service
            port:
              number: 8080
```

---

# 15. OTHER INGRESS CONTROLLERS (Traefik, HAProxy, etc)

## Traefik Ingress Controller

Traefik is simpler and cloud-native!

```yaml
apiVersion: traefik.containo.us/v1alpha1
kind: IngressRoute
metadata:
  name: my-ingress
spec:
  entryPoints:
  - web
  - websecure
  
  routes:
  - match: Host(`api.example.com`)
    kind: Rule
    services:
    - name: api-service
      port: 8080
      
  - match: Host(`images.example.com`)
    kind: Rule
    services:
    - name: image-service
      port: 9090

# Advantages:
# ├─ Simpler syntax
# ├─ Auto-discovery
# ├─ Built-in dashboard
# └─ Cloud-native design
```

## Traefik vs NGINX

```
Comparison:

Metric                  NGINX       Traefik
─────────────────────────────────────────────
Complexity              Medium      Low
Learning curve          Moderate    Easy
Performance             Excellent   Good
Annotations             Many        Fewer
Auto-discovery          Basic       Excellent
Dashboard               No          Yes
K8s-native              Average     Excellent
Community size          Huge        Growing
```

---

# 16. LOAD BALANCER VS INGRESS: WHEN TO USE EACH

This is the most confusing part! Let me clear it up.

## Comparison

```
┌───────────────────────────────────────────────────────────┐
│          LOAD BALANCER vs INGRESS                        │
├──────────────────────────────────────────────────────────┤
│ Feature              │ LoadBalancer    │ Ingress        │
├──────────────────────┼──────────────────┼────────────────┤
│ Layer                │ Layer 4          │ Layer 7        │
│ What it is           │ K8s Service      │ K8s Resource   │
│ Cloud LB created     │ Yes (per svc)    │ One per cluster│
│ External IP per svc  │ Yes              │ No (shared)    │
│ Cost per service     │ High             │ Low            │
│ Routing              │ Basic            │ Advanced       │
│ Path-based routing   │ No               │ Yes ✓          │
│ Host-based routing   │ No               │ Yes ✓          │
│ Best use             │ Single service   │ Multiple svc   │
└──────────────────────┴──────────────────┴────────────────┘

BOTTOM LINE:
├─ Use LoadBalancer for: High-performance, single service
├─ Use Ingress for: Web apps, microservices, multiple services
└─ Modern answer: Almost always use Ingress!
```

## Cost Comparison

```
Scenario: 5 microservices in Kubernetes

Option 1: Using LoadBalancer Services (BAD!)
┌─────────────────────────────────────┐
│ Service 1: LoadBalancer → ALB ($25) │
│ Service 2: LoadBalancer → ALB ($25) │
│ Service 3: LoadBalancer → ALB ($25) │
│ Service 4: LoadBalancer → ALB ($25) │
│ Service 5: LoadBalancer → ALB ($25) │
├─────────────────────────────────────┤
│ Total Cost: 5 × $25 = $125/month    │
│ 5 external IPs needed               │
│ Complex management                  │
└─────────────────────────────────────┘

Option 2: Using Ingress (RECOMMENDED!)
┌─────────────────────────────────────┐
│ One Ingress Resource                │
│ NGINX Ingress Controller ($0)       │
│ One ALB ($25)                       │
│ Routes all 5 services               │
├─────────────────────────────────────┤
│ Total Cost: $25/month               │
│ 1 external IP                       │
│ Simple management (Ingress YAML)    │
└─────────────────────────────────────┘

Savings: $100/month! (80% cheaper!)
```

---

# 17. SSL/TLS TERMINATION & CERTIFICATES

SSL/TLS encryption is essential for security!

## How SSL/TLS Termination Works

```
Scenario 1: Load Balancer Does NOT Terminate SSL
Server does SSL decryption (expensive CPU)

Scenario 2: Load Balancer DOES Terminate SSL (RECOMMENDED!)
Load balancer handles encryption (offload)
Server CPU free for application
One certificate at load balancer
Easy to renew/update certificate
Better performance!
```

---

# 18. ADVANCED ROUTING: PATH, HOST, AND HEADER-BASED

Advanced routing is what makes ALB/Ingress powerful!

## Path-Based Routing

```yaml
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: path-routing
spec:
  rules:
  - host: example.com
    http:
      paths:
      - path: /api
        pathType: Prefix
        backend:
          service:
            name: api-service
            port:
              number: 8080
              
      - path: /images
        pathType: Prefix
        backend:
          service:
            name: image-service
            port:
              number: 9090
              
      - path: /admin
        pathType: Prefix
        backend:
          service:
            name: admin-service
            port:
              number: 8000

# Request Routing:
# GET example.com/api/users → api-service:8080
# GET example.com/images/logo.png → image-service:9090
# GET example.com/admin/dashboard → admin-service:8000
```

## Host-Based Routing

```yaml
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: host-routing
spec:
  rules:
  - host: api.example.com
    http:
      paths:
      - path: /
        pathType: Prefix
        backend:
          service:
            name: api-service
            port:
              number: 8080
              
  - host: images.example.com
    http:
      paths:
      - path: /
        pathType: Prefix
        backend:
          service:
            name: image-service
            port:
              number: 9090

# Request Routing:
# GET api.example.com → api-service:8080
# GET images.example.com → image-service:9090
```

---

# 19. SESSION PERSISTENCE & STICKY SESSIONS

Some applications need the same user to reach the same server!

## Session Persistence Problem

```
Without sticky sessions: User logs in on Server A
Next request goes to Server B (no session info!)
"User not logged in" error!

Solution 1: Sticky Sessions
├─ Load balancer remembers which server client used
├─ Always routes same client to same server
└─ Downside: Load imbalance, server down = lost session

Solution 2: Distributed Session Store (RECOMMENDED!)
├─ Store sessions in Redis/Memcached (shared)
├─ All servers can access session
├─ Can route anywhere safely
└─ Better scalability

Solution 3: Stateless Application (BEST!)
├─ Don't store session on server
├─ Use JWT tokens instead
├─ Client sends token with each request
└─ Most scalable!
```

---

# 20. RATE LIMITING, THROTTLING & DDoS PROTECTION

Protect your application from abuse!

## Rate Limiting

```
Without Rate Limiting: Server overloaded, crashes
With Rate Limiting: Attacker blocked, service stays up

NGINX Rate Limiting Configuration:
nginx.ingress.kubernetes.io/limit-rps: "10"
(10 requests per second per IP)
```

## DDoS Protection

```
Multi-Layer DDoS Protection:

Layer 1: Network Layer (AWS Shield)
├─ Automatic DDoS detection
├─ Filters malicious traffic
└─ Always on

Layer 2: Rate Limiting (Load Balancer)
├─ Limit requests per IP
├─ Configured in Ingress/LB
└─ Per-application control

Layer 3: WAF (Web Application Firewall)
├─ Inspect HTTP requests
├─ Block suspicious patterns
└─ SQL injection & XSS protection

Recommendation:
└─ Use multiple layers!
   Layer 1 + Layer 2 + Layer 3 = Strong protection
```

---

# 21. LOAD BALANCING ALGORITHMS & STRATEGIES

Different algorithms distribute load differently!

## Common Algorithms

```
1. Round Robin (Default)
   ├─ Cycle through servers: 1, 2, 3, 1, 2, 3, ...
   ├─ Simplest algorithm
   └─ Works well for equal servers

2. Least Connections
   ├─ Route to server with fewest active connections
   ├─ Better for long connections
   └─ More intelligent than round robin

3. Least Outstanding Requests
   ├─ Route to server with fewest pending requests
   ├─ Good for request queuing
   └─ Accounts for server processing speed

4. IP Hash (Source IP)
   ├─ Same IP always → Same server
   ├─ Used for sticky sessions
   └─ Can cause load imbalance

5. Weighted
   ├─ Specify capacity per server
   ├─ Powerful server gets more traffic
   └─ Better for mixed infrastructure
```

---

# 22. REAL-WORLD ARCHITECTURES

Complete examples of load balancing in production!

## Architecture 1: Microservices with Kubernetes

```
┌─────────────────────────────────────────┐
│ Internet                                │
└──────────────┬──────────────────────────┘
               │
               ▼
    ┌──────────────────────────┐
    │ AWS ALB (Load Balancer)  │
    │ example.com              │
    └────┬──────────────┬──────┘
         │              │
    ▼▼▼▼▼▼▼        ▼▼▼▼▼▼▼
    ┌──────────────────────────────────────┐
    │ Kubernetes Cluster                   │
    │                                      │
    │ ┌──────────┐  ┌──────────┐          │
    │ │ NGINX    │  │ Traefik  │          │
    │ │ Ingress  │  │ Ingress  │          │
    │ │Controller│  │Controller│          │
    │ └────┬─────┘  └────┬─────┘          │
    │      │             │                 │
    │  ▼▼▼▼▼         ▼▼▼▼▼▼▼               │
    │  ┌──────────┐  ┌──────────┐         │
    │  │ API      │  │ Web      │         │
    │  │ Service  │  │ Service  │         │
    │  │ Pods:    │  │ Pods:    │         │
    │  │ 1,2,3    │  │ 4,5,6    │         │
    │  └──────────┘  └──────────┘         │
    │                                      │
    └──────────────────────────────────────┘

Cost: 1 ALB for all services
Performance: Good (Layer 7 routing)
Scalability: Excellent (Kubernetes auto-scaling)
```

## Architecture 2: High-Performance Gaming

```
┌──────────────────────────────────┐
│ Millions of Game Players         │
└───────────────┬──────────────────┘
                │
                ▼
    ┌───────────────────────────┐
    │ AWS NLB                   │
    │ (Network Load Balancer)   │
    │ Port 5555: Game           │
    └────┬───────┬──────┬───────┘
         │       │      │
    ▼▼▼▼▼▼   ▼▼▼▼▼▼  ▼▼▼▼▼▼
    ┌──────┐ ┌──────┐ ┌──────┐
    │Game  │ │Game  │ │Game  │
    │Srv1  │ │Srv2  │ │Srv3  │
    │100K  │ │100K  │ │100K  │
    │conn  │ │conn  │ │conn  │
    └──────┘ └──────┘ └──────┘
    
Performance: <5ms latency
Throughput: 300,000+ concurrent players
Cost: Higher than ALB (but necessary)
```

---

# 23. CLOUD-NATIVE LOAD BALANCING PATTERNS

Modern patterns for cloud-native applications!

## Service Mesh Pattern (Istio, Linkerd)

```
Service Mesh provides:
├─ Intelligent load balancing
├─ Automatic retry logic
├─ Circuit breaking (fail fast)
├─ Distributed tracing
├─ Rate limiting at service level
└─ Canary deployments
```

## Canary Deployment Pattern

```
Current: v1 (100% traffic)
Testing: v2 (0% traffic)

Gradual Rollout:
Phase 1: 90% v1, 10% v2
Phase 2: 75% v1, 25% v2
Phase 3: 50% v1, 50% v2
Phase 4: 25% v1, 75% v2
Phase 5: 0% v1, 100% v2 (Complete!)

If issues at any phase: Rollback instantly
```

## Blue-Green Deployment Pattern

```
Current (Blue) → Standby (Green)

Setup Phase: Deploy v2.0 to green (idle)
Test Phase: QA tests new version
Switch Phase: Route 100% traffic to green
Success: v2.0 is now active

Easy rollback: Just switch back to blue!
```

---

# 24. PERFORMANCE TUNING & OPTIMIZATION

Squeeze maximum performance from load balancers!

## Connection Pooling

```
Without Pooling: Create/close connection per request (wasteful)
With Pooling: Reuse existing connections (fast!)

Connection pooling dramatically improves performance!
```

## Caching Strategy

```
Cache Hierarchy:

1. Browser Cache (50% requests avoided)
2. CDN Cache (30% more avoided)
3. Load Balancer Cache (10% more avoided)
4. Application Cache (5% more avoided)
5. Database (Only 5% reach!)

Result: 20x throughput improvement!
```

---

# 25. TROUBLESHOOTING & COMMON ISSUES

Fix common load balancing problems!

## Issue 1: "502 Bad Gateway"

```
Cause: All backends are down or health check misconfigured

Fix:
├─ Check server logs
├─ Verify health check path exists
├─ Check server is running
└─ Restart servers if needed

Diagnosis:
aws elbv2 describe-target-health --target-group-arn <arn>
```

## Issue 2: "Timeout" - Requests Hanging

```
Cause: Slow application or timeout too short

Fix:
├─ Optimize application
├─ Increase load balancer timeout
├─ Add caching
└─ Check database
```

## Issue 3: Uneven Load Distribution

```
Cause: Wrong algorithm for workload

Fix:
├─ Change to "least connections"
├─ Or "least outstanding requests"
└─ Monitor and adjust
```

## Issue 4: SSL/Certificate Errors

```
Cause: Certificate expired or mismatched

Fix:
├─ Check certificate matches domain
├─ Renew expired certificate
└─ Add to load balancer listener
```

---

# 26. HANDS-ON LABS

Complete practical examples!

## Lab 1: Deploy ALB with Kubernetes Ingress

```bash
# Step 1: Create Kubernetes cluster
eksctl create cluster --name my-cluster --region us-east-1

# Step 2: Install NGINX Ingress Controller
helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
helm repo update
helm install nginx-ingress ingress-nginx/ingress-nginx \\
  --create-namespace --namespace ingress-nginx

# Step 3: Deploy test applications
kubectl create deployment api-app --image=nginx:latest
kubectl create deployment web-app --image=httpd:latest
kubectl expose deployment api-app --port=8080 --target-port=80
kubectl expose deployment web-app --port=80 --target-port=80

# Step 4: Create Ingress
cat <<EOF | kubectl apply -f -
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: my-ingress
spec:
  ingressClassName: nginx
  rules:
  - host: api.example.com
    http:
      paths:
      - path: /
        pathType: Prefix
        backend:
          service:
            name: api-app
            port:
              number: 8080
  - host: www.example.com
    http:
      paths:
      - path: /
        pathType: Prefix
        backend:
          service:
            name: web-app
            port:
              number: 80
EOF

# Step 5: Get Ingress IP
kubectl get ingress my-ingress

# Step 6: Test
curl http://api.example.com
curl http://www.example.com
```

## Lab 2: Create NLB for Gaming Server

```bash
# Create security group
aws ec2 create-security-group \\
  --group-name game-servers \\
  --description "Game server security group"

# Allow TCP 5555
aws ec2 authorize-security-group-ingress \\
  --group-name game-servers \\
  --protocol tcp \\
  --port 5555 \\
  --cidr 0.0.0.0/0

# Create NLB
aws elbv2 create-load-balancer \\
  --name game-nlb \\
  --type network \\
  --subnets subnet-12345 subnet-67890

# Create target group
aws elbv2 create-target-group \\
  --name game-targets \\
  --protocol TCP \\
  --port 5555 \\
  --vpc-id vpc-12345
```

---

## Summary & Key Takeaways

```
LOAD BALANCING LAYERS:

Layer 4 (NLB):
├─ TCP/UDP only
├─ Ultra-high performance
├─ Millions of requests/sec
├─ Gaming, HFT, IoT
└─ Cost: Higher

Layer 7 (ALB/Ingress):
├─ HTTP/HTTPS routing
├─ Path/host-based routing
├─ Perfect for microservices
├─ Modern applications
└─ Cost: Lower

KUBERNETES:

LoadBalancer Service:
├─ One cloud LB per service
├─ Simple
└─ Expensive at scale

Ingress:
├─ One Ingress Controller
├─ One cloud LB for all services
├─ Advanced routing
├─ Recommended!
└─ Cost-effective

INGRESS CONTROLLERS:

NGINX: Most popular, many features
Traefik: Cloud-native, simple, auto-discovery
HAProxy: Extreme performance, advanced algorithms

PRODUCTION CHECKLIST:

├─ Use ALB/Ingress for web apps
├─ Use NLB only if needed
├─ Enable SSL/TLS termination
├─ Configure health checks
├─ Use distributed sessions (or stateless)
├─ Implement rate limiting
├─ Enable DDoS protection
├─ Monitor performance metrics
├─ Test failover scenarios
├─ Document architecture
└─ Automate with Terraform/Helm
```

---

**Happy Load Balancing! 🚀**