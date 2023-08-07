# GitHub Actions

## Workflows

Each service has its own workflow that is triggered when files in their specific directories have changed.

### Hard-coded .NET versions

There might be .NET versions hardcoded in the workflow files. 

This is mainly due to running a preview-release of .NET 8.

So when the release has changed you need to update the values.

Look for the environment variable ``DOTNET_VERSION``

And if there are migrations for the ``dotnet tools install`` command.

## Verify GitHub Actions with Act

To verify GitHub Actions, install [Act](https://github.com/nektos/act).

```
act -j <job-name>  -W .github/workflows/<workflow-file>.yaml
``````