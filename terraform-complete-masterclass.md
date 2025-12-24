# Terraform Complete Masterclass
## Infrastructure as Code on AWS - Comprehensive Tutorial

---

## What is Infrastructure as Code (IaC)?

**Infrastructure as Code (IaC)** is the practice of managing and provisioning infrastructure through machine-readable definition files, rather than physical hardware configuration or interactive configuration tools. It treats infrastructure the same way developers treat application code.

### The Core Concept

```
Traditional Infrastructure:
┌──────────────────────────────────────────────────────┐
│  Manual Operations (ClickOps)                        │
│                                                       │
│  Admin logs into console → Clicks buttons           │
│  → Creates resources → Documents in wiki/Excel      │
│  → Hopes someone follows the doc next time          │
└──────────────────────────────────────────────────────┘

Infrastructure as Code:
┌──────────────────────────────────────────────────────┐
│  Code-Driven Infrastructure                          │
│                                                       │
│  Engineer writes code → Code defines infrastructure  │
│  → Version control tracks changes                    │
│  → Automation applies code → Infrastructure exists  │
│  → Same code = Same infrastructure (always!)        │
└──────────────────────────────────────────────────────┘
```

### Key Principles of IaC

#### 1. **Declarative vs Imperative**

**Imperative (How to do it):**
```bash
# Imperative: Step-by-step commands
aws ec2 create-vpc --cidr-block 10.0.0.0/16
aws ec2 create-subnet --vpc-id vpc-123 --cidr-block 10.0.1.0/24
aws ec2 create-internet-gateway
aws ec2 attach-internet-gateway --vpc-id vpc-123 --internet-gateway-id igw-456
# ... 50 more commands ...

Problem: You manage HOW to create infrastructure
```

**Declarative (What you want):**
```hcl
# Declarative: Describe desired state (Terraform)
resource "aws_vpc" "main" {
  cidr_block = "10.0.0.0/16"
}

resource "aws_subnet" "public" {
  vpc_id     = aws_vpc.main.id
  cidr_block = "10.0.1.0/24"
}

# Terraform figures out HOW to create it
# You just describe WHAT you want
```

#### 2. **Idempotency**

Running the same IaC code multiple times produces the same result:

```
Without IaC (Imperative scripts):
Run script 1st time: Creates VPC ✅
Run script 2nd time: ERROR - VPC already exists ❌
Run script 3rd time: Creates duplicate VPC ❌❌

With IaC (Terraform):
Run terraform apply 1st time: Creates VPC ✅
Run terraform apply 2nd time: No changes (already exists) ✅
Run terraform apply 3rd time: No changes (still exists) ✅
Run terraform apply 100th time: Still no changes ✅

Idempotent = Safe to run repeatedly
```

#### 3. **Version Control Integration**

```
Infrastructure Code Lifecycle:
┌─────────────────────────────────────────────────────┐
│ 1. Write Code                                       │
│    main.tf: Define VPC, EC2, RDS                   │
│                                                     │
│ 2. Commit to Git                                   │
│    git add main.tf                                 │
│    git commit -m "Add production VPC"              │
│    git push origin main                            │
│                                                     │
│ 3. Review (Pull Request)                           │
│    Team reviews infrastructure changes             │
│    Just like code review!                          │
│                                                     │
│ 4. Apply Changes                                   │
│    terraform apply                                 │
│    Infrastructure created from code                │
│                                                     │
│ 5. Track Changes                                   │
│    git log: See who changed what, when             │
│    git diff: See what changed between versions     │
│    git revert: Rollback infrastructure changes     │
└─────────────────────────────────────────────────────┘

Benefits:
✅ Audit trail (who changed what, when, why)
✅ Collaboration (multiple engineers work on same infra)
✅ Code review (catch mistakes before they break production)
✅ Rollback (revert to previous working state)
✅ Branching (test changes in isolation)
```

#### 4. **Self-Documentation**

```
Traditional Documentation:
┌─────────────────────────────────────────────┐
│ wiki.company.com/infrastructure-setup       │
│                                             │
│ "To set up production VPC:                 │
│  1. Go to AWS Console                      │
│  2. Click VPC                              │
│  3. Create VPC with CIDR 10.0.0.0/16       │
│  4. Create subnet with CIDR 10.0.1.0/24    │
│  5. Create Internet Gateway                │
│  6. Attach IGW to VPC                      │
│  7. ..."                                   │
│                                             │
│ Last updated: 6 months ago                 │
│ By: Someone who left the company           │
│ Accuracy: Questionable ❌                  │
└─────────────────────────────────────────────┘

Infrastructure as Code:
┌─────────────────────────────────────────────┐
│ main.tf (The Code IS the Documentation)    │
│                                             │
│ resource "aws_vpc" "production" {          │
│   cidr_block = "10.0.0.0/16"              │
│   tags = {                                 │
│     Name = "Production VPC"                │
│     Environment = "prod"                   │
│   }                                        │
│ }                                          │
│                                             │
│ resource "aws_subnet" "public" {           │
│   vpc_id     = aws_vpc.production.id       │
│   cidr_block = "10.0.1.0/24"              │
│ }                                          │
│                                             │
│ Always up-to-date ✅                       │
│ Code = Single source of truth ✅           │
└─────────────────────────────────────────────┘
```

### IaC Tools Comparison

| Tool | Type | Language | Cloud Support | Use Case |
|------|------|----------|---------------|----------|
| **Terraform** | Declarative | HCL | Multi-cloud | Universal IaC, any cloud/service |
| **CloudFormation** | Declarative | JSON/YAML | AWS only | AWS-native, deep integration |
| **Pulumi** | Imperative | Python, TypeScript, Go | Multi-cloud | Developers preferring real languages |
| **Ansible** | Declarative | YAML | Multi-cloud | Configuration management + IaC |
| **ARM Templates** | Declarative | JSON | Azure only | Azure-native deployments |
| **CDK (AWS)** | Imperative | TypeScript, Python | AWS + K8s | Generate CloudFormation via code |

**Why Terraform Wins for Multi-Cloud:**
```
Terraform Benefits:
├─ Multi-cloud (AWS, Azure, GCP, DigitalOcean, etc.)
├─ 3000+ providers (GitHub, Datadog, PagerDuty, etc.)
├─ Large community and modules
├─ State management (tracks what exists)
├─ Plan before apply (preview changes)
├─ Mature and battle-tested
└─ Free and open-source (with enterprise option)

CloudFormation Benefits:
├─ Native AWS integration (no credentials needed)
├─ Automatic rollback on failure
├─ CloudFormation Designer (visual editor)
├─ AWS-specific features (e.g., StackSets)
└─ Free (no additional cost)

Choose Terraform when:
✅ Multi-cloud strategy
✅ Need to manage non-AWS resources (GitHub, DNS, monitoring)
✅ Team prefers HCL over JSON/YAML
✅ Want provider ecosystem (3000+ integrations)

Choose CloudFormation when:
✅ AWS-only infrastructure
✅ Want native AWS support
✅ Require StackSets for multi-account deployments
✅ Prefer not managing state files
```

### The IaC Workflow

```
┌─────────────────────────────────────────────────────────┐
│               IaC Development Lifecycle                 │
└─────────────────────────────────────────────────────────┘

1. WRITE
   ├─ Define infrastructure in code
   ├─ main.tf: resource definitions
   ├─ variables.tf: input parameters
   └─ outputs.tf: export values

2. VERSION CONTROL
   ├─ git add .
   ├─ git commit -m "Add S3 bucket for logs"
   ├─ git push
   └─ Code stored in repository

3. REVIEW
   ├─ Create Pull Request
   ├─ Team reviews changes
   ├─ Terraform plan output attached
   ├─ Automated tests run
   └─ Approve or request changes

4. PLAN (Preview)
   ├─ terraform plan
   ├─ Shows what WILL change
   ├─ + create
   ├─ ~ modify
   ├─ - destroy
   └─ Review before applying

5. APPLY (Execute)
   ├─ terraform apply
   ├─ Creates/modifies infrastructure
   ├─ Updates state file
   └─ Outputs results

6. MONITOR
   ├─ Infrastructure running
   ├─ State file tracks reality
   ├─ Drift detection
   └─ Continuous validation

7. UPDATE
   ├─ Modify code for changes
   ├─ terraform plan (preview)
   ├─ terraform apply (execute)
   └─ Repeat cycle

8. DESTROY (Cleanup)
   ├─ terraform destroy
   ├─ Removes all resources
   └─ Clean slate
```

### Real-World IaC Benefits

**Before IaC (Manual):**
```
Scenario: Deploy new environment

Time to provision:
├─ VPC: 10 minutes (manual clicking)
├─ Subnets: 15 minutes (create, tag, route tables)
├─ Security Groups: 20 minutes (complex rules)
├─ EC2 instances: 10 minutes (configure, connect)
├─ Load Balancer: 30 minutes (target groups, listeners)
├─ RDS Database: 20 minutes (setup, configure)
├─ Documentation: 30 minutes (screenshots, wiki update)
└─ Total: 2+ hours per environment

Consistency: ❌ Different every time
Reproducibility: ❌ Hard to replicate exactly
Audit trail: ❌ Who changed what?
Rollback: ❌ Manual undo (if you remember)
Team collaboration: ❌ Coordination nightmare
```

**With IaC (Terraform):**
```
Scenario: Deploy new environment

Time to provision:
├─ terraform apply: 10 minutes (automated)
├─ Same code for dev, staging, prod
└─ Total: 10 minutes per environment

Consistency: ✅ Identical every time
Reproducibility: ✅ Run anywhere, anytime
Audit trail: ✅ Git history shows everything
Rollback: ✅ git revert + terraform apply
Team collaboration: ✅ Pull requests, code review

Cost savings: 90% time reduction
Risk reduction: Eliminates human error
Scalability: Deploy 10 environments in same time as 1
```

### IaC Adoption Journey

```
Stage 1: Manual (ClickOps)
├─ AWS Console, CLI scripts
├─ Excel documentation
├─ Tribal knowledge
└─ "Works on my machine"

Stage 2: Basic Automation
├─ Bash scripts
├─ Some AWS CLI automation
├─ Still manual for complex setups
└─ Inconsistent results

Stage 3: Configuration Management
├─ Ansible, Chef, Puppet
├─ Automate server configuration
├─ Still creating infrastructure manually
└─ Focus on post-provision setup

Stage 4: Infrastructure as Code
├─ Terraform, CloudFormation
├─ Define infrastructure in code
├─ Version controlled
├─ Plan before apply
└─ Reproducible infrastructure

Stage 5: GitOps
├─ IaC + CI/CD integration
├─ Git as single source of truth
├─ Automated deployments
├─ Pull request = Infrastructure change
└─ Full automation and governance
```

---

## TABLE OF CONTENTS

1. [Introduction & Complete Setup Guide](#1-introduction--complete-setup-guide)
2. [Understanding Infrastructure as Code](#2-understanding-infrastructure-as-code)
3. [Terraform Core Concepts & Workflow](#3-terraform-core-concepts--workflow)
4. [HCL Language: Terraform Configuration](#4-hcl-language-terraform-configuration)
5. [AWS Provider Setup & Authentication](#5-aws-provider-setup--authentication)
6. [First Real Project: VPC & EC2](#6-first-real-project-vpc--ec2)
7. [State Management & Remote Backends](#7-state-management--remote-backends)
8. [Building Reusable Modules](#8-building-reusable-modules)
9. [Variables, Outputs & Locals](#9-variables-outputs--locals)
10. [Data Sources & Dynamic Values](#10-data-sources--dynamic-values)
11. [Provisioning RDS Database](#11-provisioning-rds-database)
12. [Application Load Balancer & Target Groups](#12-application-load-balancer--target-groups)
13. [Creating an EKS Kubernetes Cluster](#13-creating-an-eks-kubernetes-cluster)
14. [Deploying Applications to EKS with Terraform](#14-deploying-applications-to-eks-with-terraform)
15. [Workspaces for Multiple Environments](#15-workspaces-for-multiple-environments)
16. [Advanced: Loops, Conditionals & Dynamic Blocks](#16-advanced-loops-conditionals--dynamic-blocks)
17. [Terraform Testing & Validation](#17-terraform-testing--validation)
18. [CI/CD Integration & GitOps](#18-cicd-integration--gitops)
19. [Troubleshooting & State Management](#19-troubleshooting--state-management)
20. [Production Best Practices](#20-production-best-practices)
21. [Hands-On Labs: Real-World Projects](#21-hands-on-labs-real-world-projects)
22. [Advanced Topics & Optimization](#22-advanced-topics--optimization)

---

# 1. INTRODUCTION & COMPLETE SETUP GUIDE

## What is Terraform?

Terraform is an **Infrastructure as Code (IaC)** tool created by HashiCorp that allows you to define, provision, and manage cloud infrastructure using declarative configuration files written in HCL (HashiCorp Configuration Language).

### Why Terraform?

**Before Terraform:**

```
Traditional Infrastructure Management:

AWS Console (Manual Clicking)
├─ Log into AWS web console
├─ Click VPC section
├─ Create VPC manually
├─ Click EC2 section
├─ Launch instance manually
├─ Configure security groups manually
├─ Connect to instance
├─ Install software manually
├─ Document everything (hope people follow it!)
└─ Result: Inconsistent, error-prone, slow

Problems:
├─ Repeatable process is tedious
├─ Hard to track who changed what
├─ Difficult to replicate in another region/account
├─ No version control
├─ Disaster recovery requires remembering all steps
└─ Scaling requires more clicking
```

**With Terraform:**

```
Infrastructure as Code:

Write HCL Configuration Files
├─ Define VPC in code
├─ Define EC2 in code
├─ Define security groups in code
├─ Define everything as code
│
├─ Version control (GitHub/GitLab)
│  └─ Track every change
│  └─ Review changes before applying
│  └─ Rollback if needed
│
├─ Reusable
│  └─ Same code, different regions
│  └─ Same code, different environments
│  └─ Consistent results every time
│
└─ Automated
   └─ terraform plan (preview changes)
   └─ terraform apply (make changes)
   └─ terraform destroy (clean up)

Benefits:
├─ Reproducible
├─ Version controlled
├─ Team friendly
├─ Auditable
├─ Scalable
└─ Disaster recovery is built-in
```

## Problems Terraform Solves

### Problem 1: Infrastructure Consistency

**Without Terraform:**

```
Different environments created manually:

Development Environment:
├─ VPC CIDR: 10.0.0.0/16
├─ EC2 instance type: t2.micro
├─ Database: db.t2.micro
├─ Created: Jan 2024
└─ Created by: John

Production Environment:
├─ VPC CIDR: 10.1.0.0/16  (DIFFERENT!)
├─ EC2 instance type: m5.large (DIFFERENT!)
├─ Database: db.m5.2xlarge (DIFFERENT!)
├─ Created: Feb 2024
└─ Created by: Sarah

Result:
├─ Dev and Prod are DIFFERENT architecturally
├─ Dev doesn't match production reality
├─ Bug in dev might not appear in production
├─ Hard to debug environment-specific issues
└─ Why doesn't it work in production?
```

**With Terraform:**

```
Same code, consistent environments:

variables.prod.tfvars:
├─ instance_type = "m5.large"
├─ db_instance_type = "db.m5.2xlarge"
└─ environment = "production"

variables.dev.tfvars:
├─ instance_type = "t2.micro"
├─ db_instance_type = "db.t2.micro"
└─ environment = "development"

main.tf (Same code for both):
├─ aws_instance.web {
│  ├─ instance_type = var.instance_type
│  └─ (uses value from tfvars)
│
└─ aws_db_instance.main {
   ├─ instance_class = var.db_instance_type
   └─ (uses value from tfvars)

Result:
├─ Same architecture, different sizes
├─ Both created from identical code
├─ Consistent structure guaranteed
└─ Only variables differ
```

### Problem 2: Infrastructure Documentation

**Manual Documentation:**

```
Infrastructure runbook document:

Chapter 1: VPC Setup (outdated? who knows)
├─ "Create VPC with CIDR 10.0.0.0/16"
├─ "Create 2 public subnets..."
├─ "Create 2 private subnets..."
│
Chapter 2: EC2 Setup (outdated)
├─ "Launch t2.micro instance..."
├─ "Attach to subnet..."
│
... more outdated sections ...

Problems:
├─ Documentation falls out of sync with reality
├─ Someone forgets to update docs when changing infra
├─ Outdated docs cause more problems than help
├─ Takes time to write and maintain
├─ New team members confused
└─ "This doc says X, but actual infra is Y"
```

**Terraform is Documentation:**

```
main.tf is the source of truth:

resource "aws_vpc" "main" {
  cidr_block = "10.0.0.0/16"
  enable_dns_support = true
}

resource "aws_subnet" "public_1" {
  vpc_id = aws_vpc.main.id
  cidr_block = "10.0.1.0/24"
  availability_zone = "us-east-1a"
  map_public_ip_on_launch = true
}

Benefits:
├─ Code IS the documentation
├─ Always up to date (real infra reflects code)
├─ Clear, explicit, executable
├─ Version controlled
├─ Git history shows all changes
└─ "Why is it like this?" → Check git blame!
```

### Problem 3: Disaster Recovery & Scaling

**Without Terraform:**

```
Your production database is deleted accidentally!

Without backups:
├─ Panic
├─ Call AWS support
├─ Explain what happened
├─ Wait for response
├─ Manually recreate from notes (if you have them)
├─ Might miss some configuration
├─ Hope everything still works
└─ Total recovery time: Hours or days!

Scaling to new region:
├─ Open AWS console
├─ Remember all the manual steps
├─ Create resources one by one
├─ Hope you remember all the security group rules
├─ Hope you remember all the configuration
├─ Test everything
├─ Takes weeks
└─ Something will probably be wrong
```

**With Terraform:**

```
Disaster recovery is automatic:

Your database is deleted:
├─ terraform apply
├─ Terraform reads state file
├─ Sees database should exist
├─ Recreates it with exact same config
└─ Recovery time: Minutes!

Scaling to new region:
├─ Change region variable in tfvars:
│  └─ aws_region = "eu-west-1"
├─ terraform plan
│  └─ Shows exactly what will be created
├─ terraform apply
└─ Entire infrastructure in new region: Minutes!

Example:
├─ Dev environment in us-east-1 takes 5 minutes to provision
├─ Prod environment in us-west-2 takes 5 minutes to provision
├─ Both identical, just different regions
└─ Same code, zero extra effort!
```

## Installing Terraform

### macOS Installation

**Method 1: Homebrew (Recommended)**

```bash
# Install Homebrew (if not already installed)
/bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"

# Install Terraform
brew install terraform

# Verify installation
terraform version
# Output: Terraform v1.6.0
# on darwin_amd64
```

**Method 2: Direct Download**

```bash
# Download latest Terraform binary
curl -LO https://releases.hashicorp.com/terraform/1.6.0/terraform_1.6.0_darwin_amd64.zip

# Unzip
unzip terraform_1.6.0_darwin_amd64.zip

# Move to PATH
sudo mv terraform /usr/local/bin/

# Verify
terraform version
```

### Linux Installation (Ubuntu/Debian)

```bash
# Install dependencies
sudo apt-get update
sudo apt-get install -y wget unzip

# Download Terraform
wget https://releases.hashicorp.com/terraform/1.6.0/terraform_1.6.0_linux_amd64.zip

# Unzip
unzip terraform_1.6.0_linux_amd64.zip

# Move to PATH
sudo mv terraform /usr/local/bin/

# Add execute permission
chmod +x /usr/local/bin/terraform

# Verify
terraform version
```

### Windows Installation

**Method 1: Chocolatey**

```powershell
# Install Terraform via Chocolatey
choco install terraform

# Verify
terraform version
```

**Method 2: Manual Download**

```
1. Go to https://www.terraform.io/downloads.html
2. Download terraform_1.6.0_windows_amd64.zip
3. Extract to a folder (e.g., C:\terraform)
4. Add folder to PATH:
   - Control Panel → System → Advanced Settings
   - Environment Variables
   - Add C:\terraform to PATH
5. Verify in PowerShell:
   terraform version
```

## AWS Account Setup

### Prerequisites

You need an AWS account with programmatic access (Access Key and Secret Key).

**Create AWS Access Keys:**

```
1. Go to AWS Management Console
   └─ https://console.aws.amazon.com/

2. Navigate to IAM → Users
   └─ Click your username

3. Security credentials tab
   └─ Scroll to "Access keys"

4. Click "Create access key"
   └─ Save Access Key ID
   └─ Save Secret Access Key
   └─ Download CSV (you'll need this)
   └─ IMPORTANT: Don't share these with anyone!

Example credentials (DO NOT USE - FAKE):
├─ Access Key ID: AKIAIOSFODNN7EXAMPLE
├─ Secret Access Key: wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY
└─ Never commit these to Git!
```

### Configure AWS Credentials

**Method 1: AWS CLI (Recommended)**

```bash
# Install AWS CLI
# macOS:
brew install awscli

# Linux:
sudo apt-get install awscli

# Windows:
choco install awscli

# Configure credentials
aws configure

# You'll be prompted for:
# ├─ AWS Access Key ID: [paste access key]
# ├─ AWS Secret Access Key: [paste secret key]
# ├─ Default region: us-east-1 (or your preferred region)
# └─ Default output format: json

# Verify configuration
aws sts get-caller-identity

# Output:
# {
#     "UserId": "AIDAI...",
#     "Account": "123456789012",
#     "Arn": "arn:aws:iam::123456789012:user/your-username"
# }
```

**Method 2: Environment Variables**

```bash
# Set environment variables
export AWS_ACCESS_KEY_ID="AKIAIOSFODNN7EXAMPLE"
export AWS_SECRET_ACCESS_KEY="wJalrXUtnFEMI/K7MDENG..."
export AWS_DEFAULT_REGION="us-east-1"

# Verify
aws sts get-caller-identity
```

**Method 3: Terraform Variables (Not Recommended for Production)**

```hcl
# provider.tf
provider "aws" {
  region     = "us-east-1"
  access_key = var.aws_access_key
  secret_key = var.aws_secret_key
}

# DON'T DO THIS IN PRODUCTION!
# ├─ Credentials visible in code
# ├─ Easy to accidentally commit to Git
# ├─ Violates security best practices
# └─ Use AWS CLI configuration instead
```

## System Requirements

```
Minimum Requirements:
├─ OS: macOS 10.12+, Windows 7+, or Linux (any modern distro)
├─ RAM: 2GB (but 4GB+ recommended)
├─ Disk: 500MB free space
├─ Internet: Required for AWS API access
└─ AWS Account: Free tier eligible for most resources

Recommended for Production:
├─ RAM: 8GB+
├─ Disk: 10GB+ (for state files, Docker images if using Docker)
├─ Network: High-speed internet connection
└─ CI/CD server if running Terraform in pipelines
```

---

# 2. UNDERSTANDING INFRASTRUCTURE AS CODE

## What is Infrastructure as Code (IaC)?

Infrastructure as Code is the practice of defining cloud infrastructure using code files instead of manual console clicks.

### Key Characteristics of IaC

```
1. Declarative
   ├─ You describe WHAT you want
   ├─ Not HOW to create it step-by-step
   ├─ Terraform figures out the steps
   └─ Example: "I want 3 EC2 instances" (not "click here, then here...")

2. Versioned
   ├─ Infrastructure code stored in Git
   ├─ Track every change with commits
   ├─ See who changed what and when
   ├─ Rollback to previous versions
   └─ Audit trail built-in

3. Automated
   ├─ No manual clicking required
   ├─ Consistency guaranteed
   ├─ Can be run from CI/CD pipelines
   ├─ Faster deployments
   └─ Fewer human errors

4. Idempotent
   ├─ Running terraform apply twice = same result
   ├─ First apply creates resources
   ├─ Second apply sees resources exist, does nothing
   ├─ Safe to run repeatedly
   └─ No risk of duplicating resources

5. Testable
   ├─ Can preview changes with terraform plan
   ├─ Can test in dev before running in prod
   ├─ Can validate syntax before applying
   └─ Can catch errors early
```

## IaC vs Manual Management

### Comparison Table

| Aspect                | Manual (Console)           | Infrastructure as Code (Terraform) |
| --------------------- | -------------------------- | ---------------------------------- |
| **Speed**             | 30 minutes per environment | 5 minutes per environment          |
| **Consistency**       | Varies (human error)       | Perfect (code-based)               |
| **Documentation**     | External, outdated         | Code is documentation              |
| **Audit Trail**       | None                       | Git history                        |
| **Disaster Recovery** | Weeks (recreate manually)  | Minutes (terraform apply)          |
| **Scaling**           | Linear effort (click more) | Same effort (same code)            |
| **Version Control**   | Not possible               | Full Git integration               |
| **Collaboration**     | Difficult                  | Easy (code review)                 |
| **Testing**           | No easy way                | terraform plan                     |
| **Team Onboarding**   | Weeks (learn by doing)     | Days (read code)                   |

---

# 3. TERRAFORM CORE CONCEPTS & WORKFLOW

## The Terraform Workflow

```
Terraform uses a consistent workflow:

┌─────────────────────────────────┐
│      terraform init             │
│  (Initialize working directory) │
│  (Download provider plugins)    │
└──────────────┬──────────────────┘
               │
               ▼
┌─────────────────────────────────┐
│     terraform validate          │
│  (Check syntax is correct)      │
│  (Verify configuration format)  │
└──────────────┬──────────────────┘
               │
               ▼
┌─────────────────────────────────┐
│      terraform plan             │
│  (Preview what will happen)     │
│  (Show resource changes)        │
│  (Safe to run, makes no changes)│
└──────────────┬──────────────────┘
               │
               ├─ Review output
               │
               ▼
┌─────────────────────────────────┐
│      terraform apply            │
│  (Actually create resources)    │
│  (Update existing resources)    │
│  (Delete removed resources)     │
│  (Update state file)            │
└──────────────┬──────────────────┘
               │
               ▼
          Resources
          Created!

Optional final step:

┌─────────────────────────────────┐
│     terraform destroy           │
│  (Delete all resources)         │
│  (Clean up everything)          │
│  (Free up AWS resources)        │
└─────────────────────────────────┘
```

## Core Concepts

### 1. Resources

A resource is a thing you want to create in AWS (or any cloud).

```hcl
# Resource block structure:
resource "RESOURCE_TYPE" "RESOURCE_NAME" {
  attribute1 = "value1"
  attribute2 = "value2"
}

# Example: Create an EC2 instance
resource "aws_instance" "web_server" {
  ami           = "ami-0c55b159cbfafe1f0"  # Amazon Linux 2
  instance_type = "t2.micro"               # Instance size
  key_name      = "my-key-pair"            # SSH key

  tags = {
    Name = "WebServer"
  }
}

# Resource types:
# ├─ aws_instance (EC2)
# ├─ aws_db_instance (RDS)
# ├─ aws_vpc (VPC)
# ├─ aws_security_group (Security Group)
# ├─ aws_s3_bucket (S3)
# ├─ aws_lambda_function (Lambda)
# └─ ... 100+ AWS resource types
```

### 2. Variables

Variables are like function parameters - they let you pass values into your configuration.

```hcl
# Define a variable
variable "instance_type" {
  description = "EC2 instance type"
  type        = string
  default     = "t2.micro"
}

# Use the variable
resource "aws_instance" "web_server" {
  ami           = "ami-0c55b159cbfafe1f0"
  instance_type = var.instance_type  # Use variable
}

# Variable values can come from:
# ├─ defaults in code (shown above)
# ├─ terraform.tfvars file
# ├─ environment-specific vars (prod.tfvars, dev.tfvars)
# ├─ command line: terraform apply -var="instance_type=t2.small"
# └─ environment variables: export TF_VAR_instance_type="t2.small"
```

### 3. Outputs

Outputs are return values - they display important information after terraform apply.

```hcl
# Define an output
output "instance_ip" {
  description = "Public IP of the web server"
  value       = aws_instance.web_server.public_ip
}

output "instance_id" {
  description = "Instance ID"
  value       = aws_instance.web_server.id
}

# When you run terraform apply, you see:
# Outputs:
# instance_id = "i-0abcd1234efgh5678"
# instance_ip = "52.12.34.56"

# Use outputs for:
# ├─ Getting resource IDs for other tools
# ├─ Getting connection strings
# ├─ Getting URLs/endpoints
# ├─ Passing values to other Terraform modules
# └─ Documentation
```

### 4. State File (terraform.tfstate)

The state file is a critical file that Terraform uses to track what exists.

```json
{
  "version": 4,
  "terraform_version": "1.6.0",
  "serial": 1,
  "lineage": "a1b2c3d4-e5f6-g7h8-i9j0-k1l2m3n4o5p6",
  "outputs": {
    "instance_ip": {
      "value": "52.12.34.56",
      "type": "string"
    }
  },
  "resources": [
    {
      "mode": "managed",
      "type": "aws_instance",
      "name": "web_server",
      "instances": [
        {
          "schema_version": 1,
          "attributes": {
            "ami": "ami-0c55b159cbfafe1f0",
            "instance_type": "t2.micro",
            "id": "i-0abcd1234efgh5678",
            "public_ip": "52.12.34.56"
          }
        }
      ]
    }
  ]
}
```

**What does the state file do?**

```
State file is critical because:

1. Maps Configuration to Reality
   ├─ Terraform code says: "I want an EC2 instance"
   ├─ State file says: "I created instance i-0abcd1234efgh5678"
   ├─ This mapping is essential
   └─ Without it, Terraform doesn't know what you created

2. Enables Updates
   ├─ terraform plan compares code to state to reality
   ├─ Only changed resources are updated
   ├─ Resources that match are left alone
   └─ Much faster than recreating everything

3. Provides Context for Deletion
   ├─ When you remove a resource from code
   ├─ Terraform knows which real resource to delete
   ├─ Without state, Terraform wouldn't know what to clean up
   └─ Prevents accidental deletion of wrong resources

4. Stores Sensitive Data
   ├─ Database passwords stored in state (encrypted at rest recommended)
   ├─ API keys stored in state
   ├─ Other sensitive data
   └─ Keep state file secure!

5. Enables Remote Collaboration
   ├─ Shared state file allows team to work together
   ├─ State locking prevents conflicts
   ├─ Multiple people can safely run terraform
   └─ Must use remote backend (S3, not local file)

IMPORTANT:
├─ Never delete terraform.tfstate file!
├─ Never edit terraform.tfstate manually!
├─ Always commit .gitignore to prevent committing state
├─ Use remote backend for teams
├─ Backup state files regularly
└─ Treat state file like your production database - it's critical!
```

### 5. Providers

Providers are plugins that let Terraform communicate with cloud platforms (AWS, Azure, GCP, Kubernetes, etc).

```hcl
# Define providers
terraform {
  required_version = ">= 1.0"

  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.0"  # Version constraints
    }
    kubernetes = {
      source  = "hashicorp/kubernetes"
      version = "~> 2.0"
    }
  }
}

# Configure AWS provider
provider "aws" {
  region = "us-east-1"

  default_tags {
    tags = {
      Project     = "MyApp"
      Environment = "Production"
      ManagedBy   = "Terraform"
    }
  }
}

# Configure Kubernetes provider
provider "kubernetes" {
  host                   = aws_eks_cluster.main.endpoint
  cluster_ca_certificate = base64decode(aws_eks_cluster.main.certificate_authority[0].data)
  token                  = data.aws_eks_cluster_auth.main.token
}

# Providers are downloaded and installed with terraform init
```

### 6. Data Sources

Data sources let you query existing resources (not create new ones).

```hcl
# Data source: Get information about existing AWS resource
data "aws_ami" "amazon_linux_2" {
  most_recent = true
  owners      = ["amazon"]

  filter {
    name   = "name"
    values = ["amzn2-ami-hvm-*-x86_64-gp2"]
  }

  filter {
    name   = "virtualization-type"
    values = ["hvm"]
  }
}

# Use data source
resource "aws_instance" "web_server" {
  ami           = data.aws_ami.amazon_linux_2.id  # From data source
  instance_type = "t2.micro"
}

# Data sources are for reading existing resources:
# ├─ Get latest AMI ID
# ├─ Get existing VPC details
# ├─ Get availability zones in a region
# ├─ Get existing security groups
# └─ Many other read-only queries
```

---

# 4. HCL LANGUAGE: TERRAFORM CONFIGURATION

## HCL Syntax Basics

HCL (HashiCorp Configuration Language) is easy to read and write.

### Basic Structure

```hcl
# Comments start with #
# This is a comment

/*
 Multi-line comments
 are also supported
*/

# Basic syntax:
resource "resource_type" "resource_name" {
  attribute = "value"
  number    = 42
  bool      = true
  list      = ["item1", "item2", "item3"]
  map = {
    key1 = "value1"
    key2 = "value2"
  }
}
```

### Data Types

```hcl
# String
variable "name" {
  type = string
}
# name = "John Doe"

# Number (integer or float)
variable "count" {
  type = number
}
# count = 42 or count = 3.14

# Boolean
variable "enabled" {
  type = bool
}
# enabled = true or enabled = false

# List
variable "availability_zones" {
  type = list(string)
}
# availability_zones = ["us-east-1a", "us-east-1b", "us-east-1c"]

# Map
variable "tags" {
  type = map(string)
}
# tags = {
#   Environment = "production"
#   Team        = "backend"
# }

# Object (complex structure)
variable "database_config" {
  type = object({
    engine = string
    version = string
    allocated_storage = number
  })
}
# database_config = {
#   engine = "mysql"
#   version = "8.0"
#   allocated_storage = 100
# }
```

### String Interpolation

```hcl
# Simple interpolation
resource "aws_instance" "example" {
  tags = {
    Name = "Server-${var.environment}"
    # If environment = "prod", Name = "Server-prod"
  }
}

# Interpolation with attributes
resource "aws_security_group" "example" {
  description = "Security group for \${aws_instance.example.id}"
  # Description includes the instance ID
}

# Interpolation with functions
resource "aws_instance" "example" {
  availability_zone = "\${var.aws_region}a"
  # If region = "us-east-1", zone = "us-east-1a"
}
```

### Conditionals

```hcl
# if-then-else syntax
variable "create_production" {
  type = bool
}

resource "aws_instance" "production" {
  count = var.create_production ? 1 : 0
  # If create_production is true, create 1 instance
  # If create_production is false, create 0 instances (don't create)

  ami           = "ami-0c55b159cbfafe1f0"
  instance_type = "t2.micro"
}

# Usage:
# terraform apply -var="create_production=true"   # Creates instance
# terraform apply -var="create_production=false"  # Doesn't create instance
```

### Loops with for_each

```hcl
variable "servers" {
  type = map(object({
    instance_type = string
    environment   = string
  }))

  default = {
    web-1 = {
      instance_type = "t2.micro"
      environment   = "dev"
    }
    web-2 = {
      instance_type = "t2.small"
      environment   = "prod"
    }
    db-1 = {
      instance_type = "t2.large"
      environment   = "prod"
    }
  }
}

# Create multiple EC2 instances
resource "aws_instance" "servers" {
  for_each = var.servers

  ami           = "ami-0c55b159cbfafe1f0"
  instance_type = each.value.instance_type

  tags = {
    Name        = each.key
    Environment = each.value.environment
  }
}

# This creates:
# ├─ aws_instance.servers["web-1"]
# ├─ aws_instance.servers["web-2"]
# └─ aws_instance.servers["db-1"]

# Access individual instances:
output "web_1_id" {
  value = aws_instance.servers["web-1"].id
}
```

### Loops with count

```hcl
variable "instance_count" {
  type = number
  default = 3
}

resource "aws_instance" "cluster" {
  count = var.instance_count

  ami           = "ami-0c55b159cbfafe1f0"
  instance_type = "t2.micro"

  tags = {
    Name = "Instance-\${count.index + 1}"
    # Names: Instance-1, Instance-2, Instance-3
  }
}

# This creates:
# ├─ aws_instance.cluster[0]
# ├─ aws_instance.cluster[1]
# └─ aws_instance.cluster[2]

# Access individual instances:
output "first_instance_id" {
  value = aws_instance.cluster[0].id
}
```

### Built-in Functions

```hcl
# String functions
length("hello")                           # Returns: 5
upper("hello")                            # Returns: "HELLO"
lower("HELLO")                            # Returns: "hello"
join("-", ["a", "b", "c"])               # Returns: "a-b-c"
split(",", "a,b,c")                      # Returns: ["a", "b", "c"]
substr("hello", 1, 3)                    # Returns: "ell"
replace("hello", "l", "x")               # Returns: "hexxo"

# List functions
concat([1, 2], [3, 4])                   # Returns: [1, 2, 3, 4]
flatten([[1, 2], [3, 4]])               # Returns: [1, 2, 3, 4]
contains(["a", "b", "c"], "b")          # Returns: true
index(["a", "b", "c"], "b")             # Returns: 1 (zero-indexed)

# Map functions
keys({a=1, b=2, c=3})                    # Returns: ["a", "b", "c"]
values({a=1, b=2, c=3})                 # Returns: [1, 2, 3]
merge({a=1}, {b=2})                      # Returns: {a=1, b=2}

# Type functions
type("hello")                            # Returns: string
can(aws_instance.example.id)            # Check if resource exists

# Math functions
min(1, 2, 3)                             # Returns: 1
max(1, 2, 3)                             # Returns: 3
floor(3.7)                               # Returns: 3
ceil(3.2)                                # Returns: 4
```

---

# 5. AWS PROVIDER SETUP & AUTHENTICATION

## AWS Provider Configuration

### Basic Provider Setup

```hcl
# provider.tf

terraform {
  required_version = ">= 1.0"

  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.0"
    }
  }
}

provider "aws" {
  region = "us-east-1"
}
```

### Authentication Methods

**Method 1: AWS CLI Configuration (Recommended)**

```hcl
# provider.tf
provider "aws" {
  region = "us-east-1"
  # Automatically uses ~/.aws/credentials and ~/.aws/config
}

# No credentials in code = secure!
# Credentials managed by AWS CLI
```

**Method 2: Environment Variables**

```bash
export AWS_ACCESS_KEY_ID="AKIAIOSFODNN7EXAMPLE"
export AWS_SECRET_ACCESS_KEY="wJalrXUtnFEMI/K7MDENG..."
export AWS_DEFAULT_REGION="us-east-1"

# Then:
terraform init
terraform plan
```

**Method 3: Explicit Credentials (NOT Recommended)**

```hcl
provider "aws" {
  region     = "us-east-1"
  access_key = var.aws_access_key
  secret_key = var.aws_secret_key
}

# DON'T DO THIS!
# Credentials visible in code
# Easy to accidentally commit to Git
# Violates security
```

### Advanced Provider Configuration

```hcl
# provider.tf

provider "aws" {
  region = var.aws_region

  # Add default tags to all resources
  default_tags {
    tags = {
      Project     = "MyApp"
      Environment = var.environment
      ManagedBy   = "Terraform"
      CreatedAt   = timestamp()
    }
  }

  # Skip certain validations
  skip_metadata_api_check = false
  skip_region_validation  = false

  # Increase retry attempts for API calls
  max_retries = 10
}

# Use multiple AWS regions (multiple providers)
provider "aws" {
  alias  = "us-west-2"
  region = "us-west-2"
}

# Use the alternate provider
resource "aws_instance" "west_server" {
  provider      = aws.us-west-2
  ami           = "ami-0c55b159cbfafe1f0"
  instance_type = "t2.micro"
}
```

---

# 6. FIRST REAL PROJECT: VPC & EC2

Let's create a complete VPC with EC2 instance!

## Project Structure

```
terraform-project/
├── main.tf              # Main resources
├── variables.tf         # Variable definitions
├── outputs.tf           # Output definitions
├── provider.tf          # Provider configuration
├── terraform.tfvars     # Variable values
└── .gitignore           # Git ignore file
```

## Step 1: Create the directory

```bash
mkdir terraform-project
cd terraform-project
```

## Step 2: provider.tf

```hcl
terraform {
  required_version = ">= 1.0"

  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.0"
    }
  }
}

provider "aws" {
  region = var.aws_region

  default_tags {
    tags = {
      Project     = "Terraform-Demo"
      Environment = var.environment
      ManagedBy   = "Terraform"
    }
  }
}
```

## Step 3: variables.tf

```hcl
variable "aws_region" {
  description = "AWS region for resources"
  type        = string
  default     = "us-east-1"
}

variable "environment" {
  description = "Environment name"
  type        = string
  default     = "dev"

  validation {
    condition     = contains(["dev", "staging", "prod"], var.environment)
    error_message = "Environment must be dev, staging, or prod."
  }
}

variable "vpc_cidr" {
  description = "CIDR block for VPC"
  type        = string
  default     = "10.0.0.0/16"
}

variable "instance_type" {
  description = "EC2 instance type"
  type        = string
  default     = "t2.micro"
}

variable "instance_count" {
  description = "Number of EC2 instances to create"
  type        = number
  default     = 1

  validation {
    condition     = var.instance_count > 0 && var.instance_count <= 10
    error_message = "Instance count must be between 1 and 10."
  }
}
```

## Step 4: main.tf

```hcl
# Get latest Amazon Linux 2 AMI
data "aws_ami" "amazon_linux_2" {
  most_recent = true
  owners      = ["amazon"]

  filter {
    name   = "name"
    values = ["amzn2-ami-hvm-*-x86_64-gp2"]
  }

  filter {
    name   = "virtualization-type"
    values = ["hvm"]
  }
}

# VPC
resource "aws_vpc" "main" {
  cidr_block           = var.vpc_cidr
  enable_dns_support   = true
  enable_dns_hostnames = true

  tags = {
    Name = "\${var.environment}-vpc"
  }
}

# Internet Gateway
resource "aws_internet_gateway" "main" {
  vpc_id = aws_vpc.main.id

  tags = {
    Name = "\${var.environment}-igw"
  }
}

# Public Subnet
resource "aws_subnet" "public" {
  count                   = 2
  vpc_id                  = aws_vpc.main.id
  cidr_block              = "10.0.\${count.index + 1}.0/24"
  availability_zone       = data.aws_availability_zones.available.names[count.index]
  map_public_ip_on_launch = true

  tags = {
    Name = "\${var.environment}-public-subnet-\${count.index + 1}"
  }
}

# Get availability zones
data "aws_availability_zones" "available" {
  state = "available"
}

# Route table for public subnet
resource "aws_route_table" "public" {
  vpc_id = aws_vpc.main.id

  route {
    cidr_block      = "0.0.0.0/0"
    gateway_id      = aws_internet_gateway.main.id
  }

  tags = {
    Name = "\${var.environment}-public-rt"
  }
}

# Associate route table with public subnet
resource "aws_route_table_association" "public" {
  count          = 2
  subnet_id      = aws_subnet.public[count.index].id
  route_table_id = aws_route_table.public.id
}

# Security Group
resource "aws_security_group" "web" {
  name        = "\${var.environment}-web-sg"
  description = "Security group for web servers"
  vpc_id      = aws_vpc.main.id

  ingress {
    from_port   = 22
    to_port     = 22
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]  # WARNING: Allow SSH from anywhere (change this!)
  }

  ingress {
    from_port   = 80
    to_port     = 80
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  ingress {
    from_port   = 443
    to_port     = 443
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }

  tags = {
    Name = "\${var.environment}-web-sg"
  }
}

# EC2 Instance
resource "aws_instance" "web" {
  count                = var.instance_count
  ami                  = data.aws_ami.amazon_linux_2.id
  instance_type        = var.instance_type
  subnet_id            = aws_subnet.public[count.index % 2].id
  vpc_security_group_ids = [aws_security_group.web.id]

  user_data = base64encode(<<-EOF
              #!/bin/bash
              yum update -y
              yum install -y httpd
              systemctl start httpd
              systemctl enable httpd
              echo "<h1>Server \${count.index + 1}</h1>" > /var/www/html/index.html
              EOF
  )

  tags = {
    Name = "\${var.environment}-web-\${count.index + 1}"
  }

  depends_on = [aws_internet_gateway.main]
}
```

## Step 5: outputs.tf

```hcl
output "vpc_id" {
  description = "VPC ID"
  value       = aws_vpc.main.id
}

output "vpc_cidr" {
  description = "VPC CIDR block"
  value       = aws_vpc.main.cidr_block
}

output "public_subnet_ids" {
  description = "Public subnet IDs"
  value       = aws_subnet.public[*].id
}

output "instance_ids" {
  description = "EC2 instance IDs"
  value       = aws_instance.web[*].id
}

output "instance_ips" {
  description = "EC2 instance public IPs"
  value       = aws_instance.web[*].public_ip
}

output "security_group_id" {
  description = "Web security group ID"
  value       = aws_security_group.web.id
}
```

## Step 6: terraform.tfvars

```hcl
aws_region    = "us-east-1"
environment   = "dev"
vpc_cidr      = "10.0.0.0/16"
instance_type = "t2.micro"
instance_count = 2
```

## Step 7: .gitignore

```
# Terraform files
.terraform/
.terraform.lock.hcl
terraform.tfstate
terraform.tfstate.*
*.tfstate
*.tfstate.*

# IDE
.vscode/
.idea/
*.swp
*.swo

# Environment
.env
.env.local

# OS
.DS_Store
Thumbs.db
```

## Deploy!

```bash
# Initialize Terraform (download provider plugins)
terraform init

# Validate configuration
terraform validate

# Check what will be created
terraform plan

# Create resources!
terraform apply

# When prompted, type "yes" to confirm

# After apply, outputs are displayed:
# Outputs:
# instance_ids = ["i-0abcd1234efgh5678", "i-0abcd1234efgh9012"]
# instance_ips = ["52.12.34.56", "52.12.34.57"]
# vpc_id = "vpc-0abcd1234efgh5678"
```

## Test your infrastructure

```bash
# SSH into instance
ssh -i your-key-pair.pem ec2-user@52.12.34.56

# Or curl the web server
curl http://52.12.34.56

# You should see: <h1>Server 1</h1>
```

## Cleanup

```bash
# Destroy all resources
terraform destroy

# When prompted, type "yes" to confirm

# All AWS resources are deleted
# State file is updated
```

---

# 7. STATE MANAGEMENT & REMOTE BACKENDS

## Why Remote State is Critical

State files are too important to store locally.

```
Local State Problems:

1. No Collaboration
   ├─ Only one person can run terraform at a time
   ├─ State file on local laptop
   ├─ Teammate can't access it
   └─ How does team infrastructure happen?

2. No Backups
   ├─ Laptop crashes, state file gone
   ├─ Can't recover infrastructure state
   ├─ Disaster!
   └─ No way to rollback

3. No Locking
   ├─ Two people run terraform apply simultaneously
   ├─ Both read old state
   ├─ Both make changes
   ├─ One overwrites the other's changes
   ├─ Infrastructure inconsistency
   └─ Data loss

4. No Audit Trail
   ├─ Who changed what?
   ├─ When did they change it?
   ├─ No way to know
   └─ Compliance failures

5. No Encryption
   ├─ State file contains secrets (passwords, keys)
   ├─ Stored in plain text on laptop
   ├─ Easy to accidentally commit to Git
   ├─ Exposed secrets
   └─ Security breach
```

## Remote Backend with S3 & DynamoDB

Let's set up a professional remote backend.

### Step 1: Create S3 Bucket for State

```bash
# Create S3 bucket
aws s3api create-bucket \
  --bucket terraform-state-my-account \
  --region us-east-1

# Enable versioning (recover old states)
aws s3api put-bucket-versioning \
  --bucket terraform-state-my-account \
  --versioning-configuration Status=Enabled

# Enable encryption
aws s3api put-bucket-encryption \
  --bucket terraform-state-my-account \
  --server-side-encryption-configuration '{
    "Rules": [{
      "ApplyServerSideEncryptionByDefault": {
        "SSEAlgorithm": "AES256"
      }
    }]
  }'

# Block public access
aws s3api put-public-access-block \
  --bucket terraform-state-my-account \
  --public-access-block-configuration \
  "BlockPublicAcls=true,IgnorePublicAcls=true,BlockPublicPolicy=true,RestrictPublicBuckets=true"
```

### Step 2: Create DynamoDB Table for State Locking

```bash
aws dynamodb create-table \
  --table-name terraform-state-lock \
  --attribute-definitions AttributeName=LockID,AttributeType=S \
  --key-schema AttributeName=LockID,KeyType=HASH \
  --provisioned-throughput ReadCapacityUnits=5,WriteCapacityUnits=5
```

### Step 3: Add Backend Configuration

```hcl
# provider.tf

terraform {
  backend "s3" {
    bucket         = "terraform-state-my-account"
    key            = "dev/terraform.tfstate"
    region         = "us-east-1"
    encrypt        = true
    dynamodb_table = "terraform-state-lock"
  }
}

provider "aws" {
  region = var.aws_region
}
```

### Step 4: Migrate State to S3

```bash
# Delete local state (backup first!)
cp terraform.tfstate terraform.tfstate.backup

# Re-initialize with S3 backend
terraform init

# When prompted: Terraform will ask to copy state to S3
# Type "yes" to migrate

# Verify state is in S3
aws s3api list-objects-v2 --bucket terraform-state-my-account

# Local state files are no longer needed
rm terraform.tfstate terraform.tfstate.backup
```

### Now State is Managed Professionally

```
Benefits of Remote Backend:

1. Team Collaboration
   ├─ Multiple people can safely run terraform
   ├─ State locking prevents conflicts
   ├─ Everyone uses same state file
   └─ Infrastructure stays consistent

2. Disaster Recovery
   ├─ S3 has 11 nines durability (99.999999999%)
   ├─ Versioning enabled
   ├─ Can restore any previous state
   ├─ No data loss
   └─ Disaster recovery built-in

3. Automatic Locking
   ├─ terraform apply acquires lock in DynamoDB
   ├─ Only one terraform process runs at a time
   ├─ Lock released when done
   ├─ Prevents race conditions
   └─ Infrastructure integrity guaranteed

4. Security
   ├─ Encryption at rest (AES256)
   ├─ Encryption in transit (HTTPS)
   ├─ S3 bucket access restricted
   ├─ No secrets in Git
   └─ Secrets protected

5. Audit Trail
   ├─ S3 versioning logs all changes
   ├─ See who made changes (from CI/CD logs)
   ├─ See when changes happened
   ├─ See what changed (version diff)
   └─ Full compliance audit trail
```

## Managing Multiple Environments

```hcl
# Create different state files for different environments

# backend.tf

terraform {
  backend "s3" {
    bucket         = "terraform-state-my-account"
    key            = var.environment  # Different per environment!
    region         = "us-east-1"
    encrypt        = true
    dynamodb_table = "terraform-state-lock"
  }
}

# BUT: backend doesn't support variables!
# So we use partial configuration:
```

```bash
# Create backend.tfvars for each environment

# prod-backend.tfvars
bucket         = "terraform-state-my-account"
key            = "prod/terraform.tfstate"
region         = "us-east-1"
encrypt        = true
dynamodb_table = "terraform-state-lock"

# dev-backend.tfvars
bucket         = "terraform-state-my-account"
key            = "dev/terraform.tfstate"
region         = "us-east-1"
encrypt        = true
dynamodb_table = "terraform-state-lock"
```

```bash
# Initialize with environment-specific backend
terraform init -backend-config=dev-backend.tfvars

# Or switch environments
terraform init -backend-config=prod-backend.tfvars -reconfigure

# Now state is separate per environment!
```

---

# 8. BUILDING REUSABLE MODULES

Modules are how you create reusable infrastructure.

## Module Structure

```
terraform-project/
├── main.tf
├── variables.tf
├── outputs.tf
├── provider.tf
└── modules/
    ├── vpc/
    │   ├── main.tf
    │   ├── variables.tf
    │   ├── outputs.tf
    │   └── README.md
    ├── ec2/
    │   ├── main.tf
    │   ├── variables.tf
    │   ├── outputs.tf
    │   └── README.md
    └── rds/
        ├── main.tf
        ├── variables.tf
        ├── outputs.tf
        └── README.md
```

## VPC Module

### modules/vpc/variables.tf

```hcl
variable "environment" {
  description = "Environment name"
  type        = string
}

variable "vpc_cidr" {
  description = "VPC CIDR block"
  type        = string
  default     = "10.0.0.0/16"
}

variable "public_subnet_cidrs" {
  description = "Public subnet CIDR blocks"
  type        = list(string)
  default     = ["10.0.1.0/24", "10.0.2.0/24"]
}

variable "private_subnet_cidrs" {
  description = "Private subnet CIDR blocks"
  type        = list(string)
  default     = ["10.0.10.0/24", "10.0.11.0/24"]
}
```

### modules/vpc/main.tf

```hcl
data "aws_availability_zones" "available" {
  state = "available"
}

# VPC
resource "aws_vpc" "main" {
  cidr_block           = var.vpc_cidr
  enable_dns_support   = true
  enable_dns_hostnames = true

  tags = {
    Name = "\${var.environment}-vpc"
  }
}

# Internet Gateway
resource "aws_internet_gateway" "main" {
  vpc_id = aws_vpc.main.id

  tags = {
    Name = "\${var.environment}-igw"
  }
}

# Public Subnets
resource "aws_subnet" "public" {
  count                   = length(var.public_subnet_cidrs)
  vpc_id                  = aws_vpc.main.id
  cidr_block              = var.public_subnet_cidrs[count.index]
  availability_zone       = data.aws_availability_zones.available.names[count.index]
  map_public_ip_on_launch = true

  tags = {
    Name = "\${var.environment}-public-subnet-\${count.index + 1}"
  }
}

# Private Subnets
resource "aws_subnet" "private" {
  count             = length(var.private_subnet_cidrs)
  vpc_id            = aws_vpc.main.id
  cidr_block        = var.private_subnet_cidrs[count.index]
  availability_zone = data.aws_availability_zones.available.names[count.index]

  tags = {
    Name = "\${var.environment}-private-subnet-\${count.index + 1}"
  }
}

# Public Route Table
resource "aws_route_table" "public" {
  vpc_id = aws_vpc.main.id

  route {
    cidr_block      = "0.0.0.0/0"
    gateway_id      = aws_internet_gateway.main.id
  }

  tags = {
    Name = "\${var.environment}-public-rt"
  }
}

# Associate Public Subnets with Public Route Table
resource "aws_route_table_association" "public" {
  count          = length(aws_subnet.public)
  subnet_id      = aws_subnet.public[count.index].id
  route_table_id = aws_route_table.public.id
}

# Elastic IP for NAT Gateway
resource "aws_eip" "nat" {
  domain = "vpc"

  tags = {
    Name = "\${var.environment}-nat-eip"
  }

  depends_on = [aws_internet_gateway.main]
}

# NAT Gateway (for private subnet internet access)
resource "aws_nat_gateway" "main" {
  allocation_id = aws_eip.nat.id
  subnet_id     = aws_subnet.public[0].id

  tags = {
    Name = "\${var.environment}-nat-gw"
  }

  depends_on = [aws_internet_gateway.main]
}

# Private Route Table
resource "aws_route_table" "private" {
  vpc_id = aws_vpc.main.id

  route {
    cidr_block     = "0.0.0.0/0"
    nat_gateway_id = aws_nat_gateway.main.id
  }

  tags = {
    Name = "\${var.environment}-private-rt"
  }
}

# Associate Private Subnets with Private Route Table
resource "aws_route_table_association" "private" {
  count          = length(aws_subnet.private)
  subnet_id      = aws_subnet.private[count.index].id
  route_table_id = aws_route_table.private.id
}
```

### modules/vpc/outputs.tf

```hcl
output "vpc_id" {
  description = "VPC ID"
  value       = aws_vpc.main.id
}

output "vpc_cidr" {
  description = "VPC CIDR block"
  value       = aws_vpc.main.cidr_block
}

output "public_subnet_ids" {
  description = "Public subnet IDs"
  value       = aws_subnet.public[*].id
}

output "private_subnet_ids" {
  description = "Private subnet IDs"
  value       = aws_subnet.private[*].id
}

output "nat_gateway_id" {
  description = "NAT Gateway ID"
  value       = aws_nat_gateway.main.id
}

output "internet_gateway_id" {
  description = "Internet Gateway ID"
  value       = aws_internet_gateway.main.id
}
```

## Using the VPC Module

### main.tf

```hcl
module "vpc" {
  source = "./modules/vpc"

  environment          = var.environment
  vpc_cidr             = var.vpc_cidr
  public_subnet_cidrs  = var.public_subnet_cidrs
  private_subnet_cidrs = var.private_subnet_cidrs
}

# Now use module outputs
resource "aws_security_group" "web" {
  vpc_id = module.vpc.vpc_id
  # ...
}

resource "aws_instance" "web" {
  subnet_id = module.vpc.public_subnet_ids[0]
  # ...
}
```

### outputs.tf (Root)

```hcl
output "vpc_id" {
  description = "VPC ID"
  value       = module.vpc.vpc_id
}

output "public_subnet_ids" {
  description = "Public subnet IDs"
  value       = module.vpc.public_subnet_ids
}
```

---

# 9. VARIABLES, OUTPUTS & LOCALS

## Advanced Variable Usage

### Type Constraints

```hcl
# Simple types
variable "string_var" {
  type = string
}

variable "number_var" {
  type = number
}

variable "bool_var" {
  type = bool
}

# Collection types
variable "list_var" {
  type = list(string)
}

variable "map_var" {
  type = map(string)
}

variable "set_var" {
  type = set(string)
}

# Complex types
variable "tuple_var" {
  type = tuple([string, number, bool])
}

variable "object_var" {
  type = object({
    name = string
    age  = number
    tags = map(string)
  })
}

# Any type (flexible but less safe)
variable "any_var" {
  type = any
}
```

### Variable Validation

```hcl
variable "instance_type" {
  description = "EC2 instance type"
  type        = string

  validation {
    condition     = contains(["t2.micro", "t2.small", "t2.medium"], var.instance_type)
    error_message = "Instance type must be t2.micro, t2.small, or t2.medium."
  }
}

variable "port" {
  description = "Port number"
  type        = number

  validation {
    condition     = var.port > 0 && var.port <= 65535
    error_message = "Port must be between 1 and 65535."
  }
}

variable "environment" {
  description = "Environment name"
  type        = string

  validation {
    condition     = can(regex("^[a-z]+$", var.environment))
    error_message = "Environment must be lowercase letters only."
  }
}
```

### Sensitive Variables

```hcl
variable "db_password" {
  description = "Database password"
  type        = string
  sensitive   = true
  # Password won't be shown in logs/output
}

variable "api_key" {
  description = "API key"
  type        = string
  sensitive   = true
}

# Use in resources
resource "aws_db_instance" "main" {
  password = var.db_password
  # Password is hidden in terraform plan/apply output
}
```

## Locals (Local Variables)

```hcl
locals {
  # Computed values used throughout configuration

  common_tags = {
    Environment = var.environment
    Project     = "MyApp"
    ManagedBy   = "Terraform"
  }

  # Construct names dynamically
  name_prefix = "\${var.environment}-\${var.project_name}"

  # Calculate values
  total_instances = var.web_instances + var.app_instances + var.db_instances
}

# Use locals
resource "aws_instance" "web" {
  count = var.web_instances

  tags = merge(
    local.common_tags,
    {
      Name = "\${local.name_prefix}-web-\${count.index + 1}"
    }
  )
}

# Locals are great for:
# ├─ Reducing duplication
# ├─ Centralizing calculations
# ├─ Making code more readable
# └─ Easy to update common values
```

## Outputs in Detail

```hcl
# Basic output
output "instance_id" {
  value = aws_instance.web.id
}

# Output with metadata
output "instance_ip" {
  description = "Public IP of the web server"
  value       = aws_instance.web.public_ip
  sensitive   = false  # default
}

# Sensitive output (hidden from logs)
output "db_password" {
  description = "Database password"
  value       = aws_db_instance.main.password
  sensitive   = true  # Hidden from logs
}

# Conditional output
output "nat_gateway_ip" {
  description = "NAT Gateway IP (only if created)"
  value       = try(aws_nat_gateway.main[0].public_ip, null)
}

# List of outputs
output "instance_ids" {
  value = aws_instance.web[*].id
}

# Map of outputs
output "instance_details" {
  value = {
    for i, instance in aws_instance.web : "server-\${i+1}" => {
      id        = instance.id
      public_ip = instance.public_ip
      private_ip = instance.private_ip
    }
  }
}

# Outputs are displayed after terraform apply
# Outputs can be referenced by other modules
# Outputs are stored in terraform.tfstate
```

---

# 10. DATA SOURCES & DYNAMIC VALUES

Data sources let you query existing AWS resources.

```hcl
# Get latest Ubuntu AMI
data "aws_ami" "ubuntu" {
  most_recent = true
  owners      = ["099720109477"]  # Canonical

  filter {
    name   = "name"
    values = ["ubuntu/images/hvm-ssd/ubuntu-jammy-22.04-amd64-server-*"]
  }
}

# Use in resource
resource "aws_instance" "web" {
  ami           = data.aws_ami.ubuntu.id
  instance_type = "t2.micro"
}

# Get current AWS account ID
data "aws_caller_identity" "current" {}

# Get AWS region info
data "aws_region" "current" {}

# Get availability zones
data "aws_availability_zones" "available" {
  state = "available"
}

# Use in resource
resource "aws_subnet" "multi_az" {
  count             = length(data.aws_availability_zones.available.names)
  vpc_id            = aws_vpc.main.id
  cidr_block        = "10.0.\${count.index}.0/24"
  availability_zone = data.aws_availability_zones.available.names[count.index]
}

# Get existing VPC by name
data "aws_vpc" "default" {
  default = true
}

# Get existing security group
data "aws_security_group" "web" {
  name   = "web-sg"
  vpc_id = aws_vpc.main.id
}

# Output: Use data in other resources
resource "aws_network_acl_rule" "allow_http" {
  network_acl_id = aws_network_acl.main.id
  rule_number    = 100
  egress          = false
  protocol        = "tcp"
  rule_action     = "allow"
  cidr_block      = "0.0.0.0/0"
  from_port       = 80
  to_port         = 80
}
```

---

# 11. PROVISIONING RDS DATABASE

Let's create a production-grade MySQL database.

## RDS Module

### modules/rds/variables.tf

```hcl
variable "environment" {
  type = string
}

variable "db_engine" {
  type    = string
  default = "mysql"
}

variable "db_version" {
  type    = string
  default = "8.0"
}

variable "db_instance_class" {
  type    = string
  default = "db.t3.micro"
}

variable "db_name" {
  type = string
}

variable "db_username" {
  type = string
}

variable "db_password" {
  type      = string
  sensitive = true
}

variable "allocated_storage" {
  type    = number
  default = 20
}

variable "multi_az" {
  type    = bool
  default = false
}

variable "skip_final_snapshot" {
  type    = bool
  default = false
}

variable "vpc_id" {
  type = string
}

variable "subnet_ids" {
  type = list(string)
}

variable "security_group_id" {
  type = string
}
```

### modules/rds/main.tf

```hcl
# DB Subnet Group (required for RDS in VPC)
resource "aws_db_subnet_group" "main" {
  name       = "\${var.environment}-db-subnet-group"
  subnet_ids = var.subnet_ids

  tags = {
    Name = "\${var.environment}-db-subnet-group"
  }
}

# RDS Instance
resource "aws_db_instance" "main" {
  identifier     = "\${var.environment}-mysql"
  engine         = var.db_engine
  engine_version = var.db_version
  instance_class = var.db_instance_class

  # Storage
  allocated_storage = var.allocated_storage
  storage_type      = "gp3"
  storage_encrypted = true

  # Database
  db_name  = var.db_name
  username = var.db_username
  password = var.db_password

  # Network
  db_subnet_group_name   = aws_db_subnet_group.main.name
  vpc_security_group_ids = [var.security_group_id]
  publicly_accessible    = false  # Private access only

  # High Availability
  multi_az = var.multi_az

  # Backups
  backup_retention_period = 7
  backup_window          = "03:00-04:00"

  # Maintenance
  maintenance_window = "mon:04:00-mon:05:00"

  # Snapshots
  skip_final_snapshot       = var.skip_final_snapshot
  final_snapshot_identifier = "\${var.environment}-mysql-final-snapshot-\${formatdate("YYYY-MM-DD-hhmm", timestamp())}"

  # Performance Insights
  performance_insights_enabled = true

  tags = {
    Name = "\${var.environment}-mysql"
  }
}
```

### modules/rds/outputs.tf

```hcl
output "endpoint" {
  description = "RDS endpoint"
  value       = aws_db_instance.main.endpoint
}

output "address" {
  description = "RDS address (hostname)"
  value       = aws_db_instance.main.address
}

output "port" {
  description = "RDS port"
  value       = aws_db_instance.main.port
}

output "db_name" {
  description = "Database name"
  value       = aws_db_instance.main.db_name
}

output "username" {
  description = "Database username"
  value       = aws_db_instance.main.username
}
```

## Using RDS Module

```hcl
# Create security group for RDS
resource "aws_security_group" "rds" {
  name        = "\${var.environment}-rds-sg"
  description = "Security group for RDS"
  vpc_id      = module.vpc.vpc_id

  ingress {
    from_port       = 3306
    to_port         = 3306
    protocol        = "tcp"
    security_groups = [aws_security_group.ec2.id]
    # Only allow from EC2 security group
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
}

# Use RDS module
module "rds" {
  source = "./modules/rds"

  environment        = var.environment
  db_name            = var.db_name
  db_username        = var.db_username
  db_password        = var.db_password
  allocated_storage  = 50
  multi_az           = var.environment == "prod" ? true : false
  vpc_id             = module.vpc.vpc_id
  subnet_ids         = module.vpc.private_subnet_ids
  security_group_id  = aws_security_group.rds.id
}

# Output database connection string
output "db_connection_string" {
  value = "mysql://:@\${module.rds.address}:\${module.rds.port}/\${module.rds.db_name}"
}
```

---

# 12. APPLICATION LOAD BALANCER & TARGET GROUPS

Configure load balancing for high availability.

```hcl
# Target Group
resource "aws_lb_target_group" "web" {
  name        = "\${var.environment}-web-tg"
  port        = 80
  protocol    = "HTTP"
  vpc_id      = module.vpc.vpc_id

  health_check {
    healthy_threshold   = 2
    unhealthy_threshold = 2
    timeout             = 3
    interval            = 30
    path                = "/"
    matcher             = "200"
  }

  tags = {
    Name = "\${var.environment}-web-tg"
  }
}

# Register instances with target group
resource "aws_lb_target_group_attachment" "web" {
  count            = length(aws_instance.web)
  target_group_arn = aws_lb_target_group.web.arn
  target_id        = aws_instance.web[count.index].id
  port             = 80
}

# Security Group for ALB
resource "aws_security_group" "alb" {
  name        = "\${var.environment}-alb-sg"
  description = "Security group for ALB"
  vpc_id      = module.vpc.vpc_id

  ingress {
    from_port   = 80
    to_port     = 80
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  ingress {
    from_port   = 443
    to_port     = 443
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
}

# Application Load Balancer
resource "aws_lb" "main" {
  name               = "\${var.environment}-web-alb"
  internal           = false
  load_balancer_type = "application"
  security_groups    = [aws_security_group.alb.id]
  subnets            = module.vpc.public_subnet_ids

  enable_deletion_protection = var.environment == "prod"
  enable_http2               = true
  enable_cross_zone_load_balancing = true

  tags = {
    Name = "\${var.environment}-web-alb"
  }
}

# ALB Listener
resource "aws_lb_listener" "web" {
  load_balancer_arn = aws_lb.main.arn
  port              = "80"
  protocol          = "HTTP"

  default_action {
    type             = "forward"
    target_group_arn = aws_lb_target_group.web.arn
  }
}

# Output ALB DNS
output "alb_dns" {
  description = "DNS name of load balancer"
  value       = aws_lb.main.dns_name
}
```

---

# 13. CREATING AN EKS KUBERNETES CLUSTER

Terraform can create entire Kubernetes clusters on AWS!

## EKS Module

```hcl
# modules/eks/main.tf

# IAM Role for EKS Cluster
resource "aws_iam_role" "eks_cluster_role" {
  name = "\${var.environment}-eks-cluster-role"

  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Action = "sts:AssumeRole"
        Effect = "Allow"
        Principal = {
          Service = "eks.amazonaws.com"
        }
      }
    ]
  })
}

resource "aws_iam_role_policy_attachment" "eks_cluster_policy" {
  role       = aws_iam_role.eks_cluster_role.name
  policy_arn = "arn:aws:iam::aws:policy/AmazonEKSClusterPolicy"
}

# EKS Cluster
resource "aws_eks_cluster" "main" {
  name     = "\${var.environment}-eks"
  role_arn = aws_iam_role.eks_cluster_role.arn
  version  = var.kubernetes_version

  vpc_config {
    subnet_ids              = var.subnet_ids
    endpoint_private_access = true
    endpoint_public_access  = var.endpoint_public_access
    public_access_cidrs     = var.public_access_cidrs
  }

  enabled_cluster_log_types = ["api", "audit", "authenticator", "controllerManager", "scheduler"]

  tags = {
    Name = "\${var.environment}-eks"
  }
}

# Data source for authentication
data "aws_eks_cluster_auth" "main" {
  name = aws_eks_cluster.main.name
}

# IAM Role for Worker Nodes
resource "aws_iam_role" "eks_node_role" {
  name = "\${var.environment}-eks-node-role"

  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Action = "sts:AssumeRole"
        Effect = "Allow"
        Principal = {
          Service = "ec2.amazonaws.com"
        }
      }
    ]
  })
}

resource "aws_iam_role_policy_attachment" "eks_worker_node_policy" {
  role       = aws_iam_role.eks_node_role.name
  policy_arn = "arn:aws:iam::aws:policy/AmazonEKSWorkerNodePolicy"
}

resource "aws_iam_role_policy_attachment" "eks_cni_policy" {
  role       = aws_iam_role.eks_node_role.name
  policy_arn = "arn:aws:iam::aws:policy/AmazonEKS_CNI_Policy"
}

resource "aws_iam_role_policy_attachment" "eks_container_registry_policy" {
  role       = aws_iam_role.eks_node_role.name
  policy_arn = "arn:aws:iam::aws:policy/AmazonEC2ContainerRegistryReadOnly"
}

# Security Group for EKS Nodes
resource "aws_security_group" "eks_nodes" {
  name        = "\${var.environment}-eks-nodes-sg"
  description = "Security group for EKS nodes"
  vpc_id      = var.vpc_id

  ingress {
    from_port       = 0
    to_port         = 65535
    protocol        = "tcp"
    security_groups = [aws_security_group.eks_nodes.id]
    # Allow nodes to communicate with each other
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
}

# Launch Template for Node Group
resource "aws_launch_template" "eks_nodes" {
  name_prefix            = "\${var.environment}-eks-"
  image_id               = data.aws_ami.eks_node.id
  instance_type          = var.node_instance_type
  vpc_security_group_ids = [aws_security_group.eks_nodes.id]

  block_device_mappings {
    device_name = "/dev/xvda"

    ebs {
      volume_size           = 50
      volume_type           = "gp3"
      delete_on_termination = true
      encrypted             = true
    }
  }
}

# EKS Node Group
resource "aws_eks_node_group" "main" {
  cluster_name    = aws_eks_cluster.main.name
  node_group_name = "\${var.environment}-node-group"
  node_role_arn   = aws_iam_role.eks_node_role.arn
  subnet_ids      = var.subnet_ids

  scaling_config {
    desired_size = var.desired_size
    max_size     = var.max_size
    min_size     = var.min_size
  }

  launch_template {
    id      = aws_launch_template.eks_nodes.id
    version = aws_launch_template.eks_nodes.latest_version_number
  }

  tags = {
    Name = "\${var.environment}-node-group"
  }
}

# Get latest EKS-optimized AMI
data "aws_ami" "eks_node" {
  most_recent = true
  owners      = ["amazon"]

  filter {
    name   = "name"
    values = ["amazon-eks-node-\${var.kubernetes_version}-*"]
  }
}
```

## Using EKS with Kubernetes Provider

```hcl
# main.tf

module "eks" {
  source = "./modules/eks"

  environment            = var.environment
  kubernetes_version     = "1.27"
  vpc_id                 = module.vpc.vpc_id
  subnet_ids             = module.vpc.private_subnet_ids
  node_instance_type     = "t3.medium"
  desired_size           = 2
  max_size               = 4
  min_size               = 1
  endpoint_public_access = var.environment == "dev"
  public_access_cidrs    = var.environment == "dev" ? ["0.0.0.0/0"] : var.office_ips
}

# Configure Kubernetes Provider
provider "kubernetes" {
  host                   = module.eks.cluster_endpoint
  cluster_ca_certificate = base64decode(module.eks.cluster_ca_certificate)
  token                  = module.eks.cluster_auth_token
}

# Create namespace
resource "kubernetes_namespace" "app" {
  metadata {
    name = "app"
  }
}
```

---

# 14. DEPLOYING APPLICATIONS TO EKS WITH TERRAFORM

Deploy actual applications using Terraform!

```hcl
# Deploy NGINX to EKS
resource "kubernetes_deployment" "nginx" {
  metadata {
    name      = "nginx"
    namespace = kubernetes_namespace.app.metadata[0].name
  }

  spec {
    replicas = 3

    selector {
      match_labels = {
        app = "nginx"
      }
    }

    template {
      metadata {
        labels = {
          app = "nginx"
        }
      }

      spec {
        container {
          image = "nginx:latest"
          name  = "nginx"

          port {
            container_port = 80
          }

          resources {
            requests = {
              cpu    = "100m"
              memory = "128Mi"
            }
            limits = {
              cpu    = "500m"
              memory = "512Mi"
            }
          }
        }
      }
    }
  }
}

# Expose NGINX with a Service
resource "kubernetes_service" "nginx" {
  metadata {
    name      = "nginx-service"
    namespace = kubernetes_namespace.app.metadata[0].name
  }

  spec {
    selector = {
      app = kubernetes_deployment.nginx.spec[0].template[0].metadata[0].labels.app
    }

    port {
      port        = 80
      target_port = 80
      protocol    = "TCP"
    }

    type = "LoadBalancer"
  }
}

# Get LoadBalancer endpoint
output "nginx_endpoint" {
  value = kubernetes_service.nginx.status[0].load_balancer[0].ingress[0].hostname
}
```

---

# 15. WORKSPACES FOR MULTIPLE ENVIRONMENTS

Manage dev, staging, and prod separately!

```bash
# Create workspaces
terraform workspace new dev
terraform workspace new staging
terraform workspace new prod

# List workspaces
terraform workspace list

# Switch workspace
terraform workspace select dev

# Delete workspace
terraform workspace delete staging

# Current workspace in code
# Use local.workspace_prefix = terraform.workspace to differentiate
```

### Using Workspaces

```hcl
# variables.tf

variable "workspace_config" {
  type = map(object({
    instance_type  = string
    desired_size   = number
    multi_az       = bool
  }))

  default = {
    dev = {
      instance_type = "t2.micro"
      desired_size  = 1
      multi_az      = false
    }
    staging = {
      instance_type = "t2.small"
      desired_size  = 2
      multi_az      = true
    }
    prod = {
      instance_type = "t2.medium"
      desired_size  = 3
      multi_az      = true
    }
  }
}

# main.tf

locals {
  config = var.workspace_config[terraform.workspace]
}

resource "aws_instance" "web" {
  instance_type = local.config.instance_type
  # Uses correct instance type for current workspace
}

# When in dev workspace: instance_type = "t2.micro"
# When in prod workspace: instance_type = "t2.medium"
```

---

# 16. ADVANCED: LOOPS, CONDITIONALS & DYNAMIC BLOCKS

## Dynamic Blocks

```hcl
# Create security group with dynamic ingress rules
variable "ingress_rules" {
  type = list(object({
    from_port   = number
    to_port     = number
    protocol    = string
    cidr_blocks = list(string)
  }))

  default = [
    {
      from_port   = 80
      to_port     = 80
      protocol    = "tcp"
      cidr_blocks = ["0.0.0.0/0"]
    },
    {
      from_port   = 443
      to_port     = 443
      protocol    = "tcp"
      cidr_blocks = ["0.0.0.0/0"]
    }
  ]
}

resource "aws_security_group" "dynamic_example" {
  name = "dynamic-sg"

  dynamic "ingress" {
    for_each = var.ingress_rules

    content {
      from_port   = ingress.value.from_port
      to_port     = ingress.value.to_port
      protocol    = ingress.value.protocol
      cidr_blocks = ingress.value.cidr_blocks
    }
  }
}
```

## Complex Conditionals

```hcl
# Create resources conditionally
variable "enable_database" {
  type    = bool
  default = false
}

variable "db_config" {
  type = object({
    engine       = string
    instance_class = string
  })

  default = {
    engine         = "mysql"
    instance_class = "db.t2.micro"
  }
}

resource "aws_db_instance" "conditional" {
  count = var.enable_database ? 1 : 0

  engine         = var.db_config.engine
  instance_class = var.db_config.instance_class
}

# Output only if created
output "db_endpoint" {
  value = try(aws_db_instance.conditional[0].endpoint, "Database not created")
}
```

---

# 17. TERRAFORM TESTING & VALIDATION

Validate your Terraform before applying!

```bash
# Format check
terraform fmt -check -recursive

# Syntax validation
terraform validate

# Dry run - see what will happen
terraform plan -out=tfplan

# Review plan file
terraform show tfplan

# Validate variable values
terraform validate -var="instance_type=t2.micro"

# Plan with specific variables file
terraform plan -var-file="prod.tfvars"
```

### Testing with Terratest

```bash
# Install Terratest (Go-based testing framework)
go get -u github.com/gruntwork-io/terratest/v2/...
```

```go
// test/main_test.go

package test

import (
  "testing"

  "github.com/gruntwork-io/terratest/v2/terraform"
  "github.com/stretchr/testify/assert"
)

func TestTerraformExample(t *testing.T) {
  terraformOptions := &terraform.Options{
    TerraformDir: "../",
  }

  // Clean up resources
  defer terraform.Destroy(t, terraformOptions)

  // Create resources
  terraform.InitAndApply(t, terraformOptions)

  // Test outputs
  vpcId := terraform.Output(t, terraformOptions, "vpc_id")
  assert.NotEmpty(t, vpcId)
}
```

---

# 18. CI/CD INTEGRATION & GITOPS

Run Terraform automatically in GitHub Actions!

```yaml
# .github/workflows/terraform.yml

name: Terraform

on:
  push:
    branches: [main, develop]
  pull_request:
    branches: [main]

jobs:
  terraform:
    runs-on: ubuntu-latest

    env:
      AWS_ACCESS_KEY_ID: \${{ secrets.AWS_ACCESS_KEY_ID }}
      AWS_SECRET_ACCESS_KEY: \${{ secrets.AWS_SECRET_ACCESS_KEY }}
      AWS_REGION: us-east-1

    steps:
      - uses: actions/checkout@v3

      - uses: hashicorp/setup-terraform@v2
        with:
          terraform_version: 1.6.0

      - name: Initialize Terraform
        run: terraform init

      - name: Format Check
        run: terraform fmt -check -recursive

      - name: Validate
        run: terraform validate

      - name: Plan
        run: terraform plan -out=tfplan
        if: github.event_name == 'pull_request'

      - name: Apply
        run: terraform apply -auto-approve tfplan
        if: github.event_name == 'push' && github.ref == 'refs/heads/main'
```

---

# 19. TROUBLESHOOTING & STATE MANAGEMENT

Common issues and how to fix them.

## Common Issues

```
Issue: "Error acquiring the state lock"
├─ Cause: Another terraform process is running
├─ Fix: Wait for other process to finish
├─ Check: AWS DynamoDB for locks
│  aws dynamodb scan --table-name terraform-state-lock
├─ Force unlock (dangerous!):
│  terraform force-unlock <LOCK_ID>
└─ Better: Identify and stop the hung process

Issue: "No valid credential providers"
├─ Cause: AWS credentials not configured
├─ Fix: aws configure
├─ Or: export AWS_ACCESS_KEY_ID=...
├─ Or: Provide explicit credentials in provider
└─ Check: aws sts get-caller-identity

Issue: "State and infrastructure out of sync"
├─ Cause: Infrastructure changed outside Terraform
├─ Fix: terraform refresh (read current state)
├─ Or: terraform import <resource_id> (import to state)
├─ Or: Update code to match reality
└─ Better: Only change infrastructure through Terraform

Issue: "Dependency cycle detected"
├─ Cause: Resources depend on each other
├─ Fix: Add explicit depends_on
├─ Or: Restructure to break cycle
├─ Check: terraform graph (visualize dependencies)
└─ Example: depends_on = [aws_internet_gateway.main]

Issue: "Resource is tainted"
├─ Cause: Resource creation failed or was marked bad
├─ Fix: terraform untaint <resource_address>
├─ Or: terraform apply to recreate
├─ Check: terraform show | grep "tainted"
└─ Prevent: Use proper error handling
```

## State Management Commands

```bash
# List all resources in state
terraform state list

# Show specific resource details
terraform state show aws_instance.web

# Move resource in state
terraform state mv aws_instance.web aws_instance.web_old

# Remove resource from state (without deleting it)
terraform state rm aws_instance.old_server

# Backup state
terraform state pull > terraform.tfstate.backup

# Restore state
terraform state push terraform.tfstate.backup

# Lock state manually
terraform state lock

# Unlock state
terraform state unlock
```

## Debugging

```bash
# Enable debug logging
export TF_LOG=DEBUG
terraform plan

# Write logs to file
export TF_LOG_PATH=/tmp/terraform.log
terraform apply

# Use graphviz to visualize dependencies
terraform graph | dot -Tsvg > graph.svg
```

---

# 20. PRODUCTION BEST PRACTICES

Rules for running Terraform in production.

```
1. Always use remote state backend
   ├─ S3 with versioning
   ├─ DynamoDB for locking
   ├─ Encryption enabled
   └─ Access restricted

2. Version control everything
   ├─ All .tf files in Git
   ├─ terraform.tfvars in Git (.gitignore state files!)
   ├─ .gitignore includes .terraform, *.tfstate
   └─ Review all changes in PRs

3. Environment separation
   ├─ Dev, Staging, Prod in separate workspaces or directories
   ├─ Different AWS accounts if possible
   ├─ Separate state files
   └─ Different approval processes

4. State protection
   ├─ Never manually edit state file
   ├─ Backup state regularly
   ├─ Use terraform state commands only
   ├─ Encrypt state at rest
   └─ Enable versioning in S3

5. Credentials management
   ├─ Use AWS credentials from ~/.aws/credentials
   ├─ Never commit credentials to Git
   ├─ Use IAM roles for CI/CD
   ├─ Use environment variables for secrets
   └─ Enable MFA for human users

6. Code quality
   ├─ Use terraform fmt for consistent formatting
   ├─ Run terraform validate before commits
   ├─ Use linters (TFLint)
   ├─ Code review all changes
   └─ Test in non-prod first

7. Change management
   ├─ Always run terraform plan before apply
   ├─ Review plan output carefully
   ├─ Approve changes in PR first
   ├─ Apply during maintenance windows
   └─ Have rollback plan

8. Documentation
   ├─ Document all modules with README.md
   ├─ Document all variables and outputs
   ├─ Document architecture decisions
   ├─ Keep runbooks for common tasks
   └─ Document manual steps if any

9. Naming conventions
   ├─ Consistent naming: <environment>-<resource>-<purpose>
   ├─ Use lowercase and hyphens
   ├─ Avoid abbreviations
   └─ Make names descriptive

10. Resource tagging
    ├─ Tag all resources automatically
    ├─ Include: Project, Environment, Cost Center
    ├─ Use default_tags in provider
    └─ Enforce tags with policy

11. Error handling
    ├─ Use try() for optional resources
    ├─ Add validation to variables
    ├─ Handle null values gracefully
    ├─ Check data source existence
    └─ Provide helpful error messages

12. Performance
    ├─ Use for_each instead of count when possible
    ├─ Avoid large for loops (100+ items)
    ├─ Use data sources instead of recreating
    ├─ Enable parallel operations: -parallelism=10
    └─ Profile slow runs with TF_LOG=TRACE
```

---

# 21. HANDS-ON LABS: REAL-WORLD PROJECTS

## Lab 1: Complete 3-Tier Application

Create VPC + EC2 + RDS + Load Balancer

```bash
mkdir terraform-3tier
cd terraform-3tier

# Use all previous knowledge
# Directory structure:
# ├── modules/
# │   ├── vpc/
# │   ├── ec2/
# │   ├── rds/
# │   └── alb/
# ├── main.tf
# ├── variables.tf
# ├── outputs.tf
# ├── provider.tf
# ├── terraform.tfvars
# └── .gitignore

# Estimated time: 30 minutes
# Learning: Full application stack
```

## Lab 2: EKS Cluster with Application Deployment

Create and deploy to Kubernetes

```bash
mkdir terraform-eks-app
cd terraform-eks-app

# Modules:
# ├── vpc/
# ├── eks/
# └── app/

# Deploy:
# 1. VPC for EKS
# 2. EKS cluster with nodes
# 3. Kubernetes namespace
# 4. Deployment (app)
# 5. Service (LoadBalancer)
# 6. Autoscaling (HPA)

# Estimated time: 45 minutes
# Learning: Kubernetes on AWS with Terraform
```

## Lab 3: Multi-Environment Setup

Dev, Staging, Production

```bash
mkdir terraform-multi-env
cd terraform-multi-env

# Structure:
# ├── modules/ (shared)
# ├── environments/
# │   ├── dev/
# │   │   ├── main.tf
# │   │   ├── variables.tf
# │   │   ├── terraform.tfvars
# │   │   └── backend.tfvars
# │   ├── staging/
# │   │   └── ...
# │   └── prod/
# │       └── ...
# └── .gitignore

# Deploy each environment separately
# terraform init -backend-config=environments/dev/backend.tfvars
# terraform plan -var-file=environments/dev/terraform.tfvars

# Estimated time: 60 minutes
# Learning: Production-grade setup
```

---

# 22. ADVANCED TOPICS & OPTIMIZATION

## CDK for Terraform

Write Terraform in Python!

```python
from cdktf import App, TerraformStack
from constructs import Construct
from cdktf_cdktf_providers.aws.provider import AwsProvider
from cdktf_cdktf_providers.aws.instance import Instance

class MyStack(TerraformStack):
  def __init__(self, scope: Construct, ns: str):
    super().__init__(scope, ns)

    AwsProvider(self, "aws", region="us-east-1")

    Instance(self, "web",
      ami="ami-0c55b159cbfafe1f0",
      instance_type="t2.micro"
    )

app = App()
MyStack(app, "terraform")
app.synth()
```

## Terraform Registry

Share modules with community

```hcl
# Use public modules from registry
module "vpc" {
  source = "terraform-aws-modules/vpc/aws"
  version = "5.0"

  name = "my-vpc"
  cidr = "10.0.0.0/16"
}

# Or publish your own
# └─ terraform init --upgrade-modules
# └─ terraform module publish
```

## Policy as Code with Sentinel

Enforce rules automatically

```hcl
# policy.sentinel

import "tfplan/v2" as tfplan

# Disallow public databases
deny_public_databases = rule {
  all tfplan.resource_changes["aws_db_instance"] as _, db {
    db.change.after.publicly_accessible == false
  }
}

main = rule {
  deny_public_databases
}
```

---

## Conclusion

You now know Terraform from Zero to Pro!

**Key Takeaways:**

✅ Infrastructure as Code enables:

- Reproducible infrastructure
- Version control
- Collaboration
- Disaster recovery
- Compliance

✅ Terraform workflow:

- init → validate → plan → apply → destroy

✅ Best practices:

- Remote state
- Modules
- Variables
- Documentation
- CI/CD

✅ Production-grade infrastructure:

- Multi-region
- High availability
- Security
- Monitoring
- Disaster recovery

**Next Steps:**

1. Create a real project
2. Use in your organization
3. Contribute to community modules
4. Learn Terraform Cloud
5. Advanced: Policy as Code, CDK

**Resources:**

- Official Docs: https://www.terraform.io/docs
- Registry: https://registry.terraform.io
- AWS Provider: https://registry.terraform.io/providers/hashicorp/aws
- Kubernetes Provider: https://registry.terraform.io/providers/hashicorp/kubernetes
- Community: Discuss on HashiCorp forums

---

**Happy Terraforming! 🚀**
