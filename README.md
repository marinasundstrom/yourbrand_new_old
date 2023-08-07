# YourBrand

Store Web: https://yourbrand-store-web.kindgrass-70ab37e8.swedencentral.azurecontainerapps.io/

Admin Web: https://yourbrand-admin-web.kindgrass-70ab37e8.swedencentral.azurecontainerapps.io/


## Deploy to Azure using GitHub Actions

Requires you to add secrets to your GitHub projects.

Every project in this repository should have its own workflow file that execute when they change.

In order for deployment to work, you need to make sure that the credentials (added to secrets) are correct. 

You also need to make sure that the container apps that you are deploying to exist in Azure.