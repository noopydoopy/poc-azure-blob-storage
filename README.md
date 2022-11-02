# :space_invader: poc-azure-blob-storage :space_invader:
This project is a web application based on .net development aim to demonstrate the CRUD actions to the [Azure Blob Storage](https://learn.microsoft.com/en-us/dotnet/api/overview/azure/storage.blobs-readme?view=azure-dotnet) and only support connection via the connectionstring.

# Prerequisites
1. [DotNet Core 5](https://dotnet.microsoft.com/en-us/download/dotnet/5.0)
2. [Azure Storage Account](https://learn.microsoft.com/en-us/azure/storage/common/storage-account-create?tabs=azure-portal)

# Running the app
Running the app depends on user experience, better to run by debugging or console

### Console
```bash
cd .\poc-azure-blob-storage\AzureBlobStorageApp\
dotnet restore
dotnet run
```
Then it will serve up the application via
```http
https://localhost:5001
```

### Debugging
Open the solution using your [visual studio](https://visualstudio.microsoft.com/vs/) and run with debug mode and then t will serve up the application via
```http
https://localhost:5001
```

After the app is running then you can put your azure blob storage connectionstring to use the app and then enjoy! :wink:

To find your storage key access you can go into your azure storage account **Security + Networking** then **Access Keys**