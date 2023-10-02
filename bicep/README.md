# Bicep

Bicep is a tool that allows you can define Infrastructure as code (IaC) in Azure. 

With Bicep you can quickly deploy new resources, or make change existing resources.

Read more about how to use Bicep in the [docs](/docs/azure/bicep.md).

## Parameters

The ``main.parameters.json`` file contains the actual parameter values used by the Bicep templates.

These may be updated if necessary.

## What's not in the Bicep template

These templates do not assign the roles, or create the actual secrets.

For information about how to set up roles, [read this](/docs/azure/identities-and-roles.md).

Here is about [secrets](/docs/azure/secrets.md).

## The original ARM template

``main.json`` is the original ARM template that got de-compiled to Bicep.
