## Setup

Follow these steps to set up the project on your local machine.
### Installation

1. Clone the repository:
   ```sh
   git clone https://github.com/betty-thomas/AuditEventService.git
2. Navigate to the project directory:
   ```sh 
   cd AuditEventService
3. Install dependencies
   ```sh
    dotnet restore
4. Run application
   ```sh
   dotnet build
   dotnet run --project AuditEventService/AuditEventService.csproj


### API Requests
You can view the API requests locally by running the application and visiting:
   http://localhost:[PORT]/swagger/index.html

### Assumptions and Areas for Improvement
Assumptions
- The application runs locally and is not deployed.
- Events are stored in memory
- Paging works with default values for page size.
Areas for Improvement
- Implement a database for persistent storage.
- Add authentication (JWT or OAuth) for secure API access.
