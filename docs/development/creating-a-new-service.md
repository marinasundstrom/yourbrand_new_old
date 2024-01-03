# Creating a new service (WIP)

Here is a checklist for when creating a new service:

- [ ] Copy the template service: YourService and all of its sub-projects
- [ ] Add the projects to the solution, also create solution folders.
- [ ] Rename the directories and files containing the name "YourService"
- [ ] Update the appsettings.json files: Database connection string etc.
- [ ] Update environment variables
- [ ] Add service to the Docker Compose files for Production
- [ ] Add documentation
- [ ] Add to Bicep
- [ ] Set up and Publish to Azure
- [ ] Set the permissions for resources and databases.

## Template: YourService

In the YourService.API project:

* Application layer: Features folder
* Domain layer: Entities and Domain Services
* Persistence layer: Repositories