# Nig-Shop

## 1. Introduction
**This application is an educational project. We strongly discourage deploying it to a production environment without proper testing.**

Nig-Shop is a modular **e-commerce system** built on a microservices architecture.  
The main goal of the project is to provide scalability, flexibility, and easy extensibility for features such as product management, payments, and order processing.  

The project follows the **Backend-for-Frontend + microservices** approach, with a clear separation between the frontend interface and backend services.  

---

## 2. Features

The main workflow of the application is designed to automate product sales and invoice generation. Our client can conveniently purchase a product from the catalog on the website — by adding it to the cart, reviewing its properties, etc. — proceed with payment, automatically receive a generated invoice, and have the completed order stored in the database. This allows us to track which orders are pending fulfillment on our side.

Additionally, the application provides an account management system — including registration, login, storing user data, and selecting from multiple delivery addresses.

The entire application is based on API microservices that communicate with each other. This architecture enables efficient enhancement of the application’s functionality and, in case of operational issues, ensures partial availability of the system despite service disruptions.

---

## 3. Installation

Follow these steps to run the project locally:

```bash
# Clone repository
git clone https://github.com/kapi2334/nig-shop.git
cd nig-shop

# Run all services with Docker Compose
docker-compose up --build

# (optional) configure nginx locally by editing `nginx` config and port mapping
```
## 4. Quick Start

After setup:

1. The frontend should be available at `http://localhost:3000`  
2. Backend services expose APIs on their assigned ports (see `docker-compose.yml`/ table below for mappings)  
3. Example workflow:  
   - Register a user  (use frontend user app)
   - Add products to the catalog (u can use Swagger to speed up this process)  
   - Place an order  
   - Process a payment  
   - Generate an invoice  

---

## 5. Architecture

The system consists of the following components:


| Component      | Responsibility | Port|
|----------------|----------------|----------------|
| Frontend       | User interface of the shop | :4000 |
| userService    | Authentication, registration, user data management | :3001 |
| productService | CRUD operations for products, categories, stock | :3000 |
| orderService   | Order placement, processing, and tracking | :3003 |
| paymentService | Payment processing (external API integration) | :3004 |
| invoiceService | Invoice generation and storage | :3002 |
| nginx          | Reverse proxy, routing, SSL, load balancing |  |

Each component operates as an independent API — receiving information, processing it, and returning the result to the user.
In line with the microservices architecture, every component also maintains its own PostgreSQL database — which significantly increases both scalability and the reliability of each service.

---

## 6. FAQ

**Q: Can I use another proxy instead of nginx?**  
A: Yes, nginx is the default, but you can replace or extend it with other reverse proxies.  

**Q: How can I add a new microservice?**  
A: Copy the structure of an existing service, adjust configuration (ports, dependencies), update `docker-compose.yml`, and add routing rules in nginx if needed.  

**Q: Is this version of the application a ready-to-go release that I can immediately deploy in my production environment?**
A: Absolutely not. We strongly recommend performing thorough testing before deployment and verifying whether this system is suitable for your specific use case.

**Q: How do I prepare for production deployment?**  
A: If rapid deployment of this solution to a production environment is essential for you, at a bare minimum we recommend the following steps:
- **In each microservice, change the EnvironmentName variable to Production.**
- **Add an authorization method to each API endpoint.**
- Configure environment variables for each service.
- Set up a databases (including migrations).
- Enable SSL.
- Implement monitoring and CI/CD pipelines.
- Check whether the outgoing invoice format, as well as the UI design and implementation, meet your requirements.
These steps are critical to ensure the system runs securely and reliably in a production environment.

---

## 7. Tech Stack

- **Backend:** .NET (C#)  
- **Frontend:** Blazor (C#, HTML + CSS)
- **Containerization:** Docker
- **Proxy / Routing:** nginx  
- **Databases:** PostgreSQL  
- **Other libraries:** Pdf sharp

---

## 8. Authors / License

Authors: *Kacper Bentkowski*, *Kacper Jaszcz*. 

License: MIT License

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.





