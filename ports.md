#localhost configurated ports
- 5000->3000 - Products Service API
- 5001->3001 - Users Service API
- 5002->3002 - Invoice Service API
- 5003->3003 - Order Service API
- 5004->3004 - Payment Service API
- 6000->3100 - Admin Service API
#UI ports
- 5010->4000 - UI dashboard
- 5011->4001 - Admin dashboard
#Database ports
 |port|    container name    |  db name  |     

- 5401 - user-service-db-1    (userservice)
- 5402 - product-service-db-1 (productservice)
- 5403 - invoice-service-db-1 (invoiceservice)
- 5404 - order-service-db-1   (orderservice) 
