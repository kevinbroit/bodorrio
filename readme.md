# 💒 Bodorrio N

A lightweight Azure Function app for managing wedding invitations and guest responses.

---

## 📦 Features

- 🌐 REST API built with Azure Functions (Isolated .NET)
- 📄 Invite creation, retrieval, and deletion
- 🔒 Secure access via function keys
- 🧪 OpenAPI/Swagger integration for API testing

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- [Azure Functions Core Tools](https://learn.microsoft.com/en-us/azure/azure-functions/functions-run-local)
- [Azure CLI](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli) (optional)
- A storage account (for Azure Table Storage)

### Run locally

Using Azure Functions CLI:

```bash
func start
```

Or using .NET CLI:

```bash
dotnet run
```

---

### 🔐 Add your local settings

Create a file called `local.settings.json`:

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "<your_connection_string>",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated"
  }
}
```

> 🔒 Make sure `local.settings.json` is excluded via `.gitignore`

---

## 🧪 API Endpoints

| Method | Route         | Description        |
|--------|---------------|--------------------|
| GET    | `/api/invites` | Get all invites    |
| POST   | `/api/invites` | Add a new invite   |
| DELETE | `/api/invites` | Delete an invite   |

### 📝 Example request (POST)

```json
{
  "partitionKey": "1",
  "rowKey": "Nacho"
}
```

---

## 📚 Technologies

- Azure Functions (Isolated .NET)
- Azure Table Storage
- C#
- OpenAPI (Swagger)

---

## 📄 License

MIT

---

## 👤 Author

**Kevin Broit**  
[https://linkedin.com/in/kevinbroit](https://linkedin.com/in/kevinbroit)
