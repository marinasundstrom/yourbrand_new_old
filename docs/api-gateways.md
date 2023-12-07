# API Gateways

An API Gateway is a facade to the internal service APIs. In essence, it is a proxy that transforms the requests, appends information.

In our case, the gateways can be hosted by services, or be an 

## In this solution

In the most simple cases, we use YARP to forward requests sent to certain endpoint to another service.

Other times, we have build own APIs with endpoints that call another service. The transformation is then expressed in code. Should be avoided unless you need to add logic.

## Gateways

### Store Web

The backend acts as an API gateway to the Storefront service, and other services, through which the client go.

All requests to ``/storefront`` are forwarded to Storefront

### Admin Web

The backend acts as an API gateway to the Catalog service, and other services, through which the client go.

All requests to ``/catalog`` are forwarded to Catalog

## Configuration

There are two registrations: one for dev and another for production

The production config is defined in config file

The development config is loaded programmatically using service discovery.