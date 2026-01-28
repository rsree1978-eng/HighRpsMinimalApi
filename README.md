# HighRpsMinimalApi

A minimal ASP.NET Core API designed for high throughput, with caching and simulated latency.  
Includes a k6 load test to validate performance.

---

## 🚀 Features

- Minimal API (no MVC overhead)
- `/price` endpoint:
  - returns a random price
  - cached for 1 second
  - includes a 50ms non-blocking delay
- `/calculate` endpoint:
  - add, subtract, multiply, divide
  - includes a 50ms non-blocking delay
- Designed for high RPS and horizontal scaling
- Load testing with k6 (Docker-compatible)

---

## 🧠 Design Principles

### ✔ High Throughput
- Uses ASP.NET Core Minimal API for minimal pipeline overhead.
- Avoids blocking calls (`Task.Delay` instead of `Thread.Sleep`).

### ✔ Stateless
- No session state.
- Easily scalable horizontally behind a load balancer.

### ✔ Cache to reduce load
- Price is cached in-memory with a short TTL.
- Prevents repeated expensive computations.

### ✔ Simulated latency
- 50ms `Task.Delay` simulates downstream I/O (DB, external API, etc.)
- Delay is async to avoid blocking threads.

---

## 📦 Run API

```bash
cd src/HighRpsMinimalApi
dotnet run -c Release
API will run on:

http://localhost:5000
🔥 Endpoints
GET /price
Returns a cached random price:

{ "price": 537 }
GET /calculate?a=10&b=5&op=mul
Example:

50