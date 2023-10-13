# Design

This document is about the overall design.

More on every individual services, and their designs, in their project folders.

## Web apps

### Store Web

E-commerce site that allows the user to view and buy products.

Blazor Web app.

Integrates with the Catalog and Carts services.

### Admin Web

Allows the user to add and update products.

Blazor Web Assembly app hosted with ASP.NET Web API.

Integrates with the Catalog and Carts services.

## Services

### Catalog

Contains information about the products.

Publishes events for other services to subscribe to when product details, price, or image changes.

Accessed via Web API

### Carts

Holds the carts/baskets with their items, used by customers.

Is independent from Catalog. But kept updated via events from Catalog.

Accessed vua Web API, or through Asynchronous messaging.