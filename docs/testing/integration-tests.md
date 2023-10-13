# Integration tests

Each project has its own integration tests.

Currently, only ``Carts`` has an integration test project.

Integration tests are not part of any workflow, and will not be running automatically when pushing changes to GitHub.

## Testcontainers

It is recommended to use [Testcontainers](https://testcontainers.com/) while doing integration tests.

They allow you to configure instances of dependencies for testing, running in Docker containers during the duration an integration test.