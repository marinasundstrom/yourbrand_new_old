# Asynchronous messaging

The services in this project communicate by sending (or publishing) messages to each other. 

Letting the services process messages asynchronously, rather than having a persistent connection for the duration a request (like with HTTP).

## Commands and Queries

Some messages take on the semantics of commands and queries.

Commands tell the system to do something - changing something in the system:

* ``PlaceOrder``
* ``UpdateProductDetails``

Queries just ask to retrieve information - no modification of state:

* ``GetProducts``
* ``GetCartById``

Not to say that a command can't both modify and return a result.

## Event-driven architecture

An EDA denotes a system that is driven by services publishing and subscribing to events. Events are notifications that semantically tell you that: *Something has happened* (Past tense).

* ``OrderPlaced``
* ``EmploymentEnded``

 Often events are published as a response to a command being executed:

* ``PlaceOrder`` -> ``OrderPlaced``
* ``UpdateProductDetails`` -> ``ProductDetailsUpdated``

## MassTransit

MassTransit is a asynchronous messaging framework that abstracts away the details of the transport (RabbitMQ, Azure Service Bus, AWS etc).

It is what allows us to easily switch from using on transport to another depending on environment. Like Azure Service Bus in the cloud, to be using RabbitMQ locally.