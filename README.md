# Fleet Management System

## Overview

This is a basic fleet management web application that tracks the current location of vehicles.

## Prerequisites

- .NET SDK (8.0) installed on your machine.
- SQLite installed.
- VS Code or any preferred IDE.

## Setup Instructions

1. **Clone the repository:**

   ```bash
   git clone https://github.com/Andy-Nkumane/Fleet-Management-System.git
   cd FleetManagementSystem/
   ```

2. **Restore dependencies:**

    ```bash
    dotnet restore
    ```

<!-- 3. **Create the database:**

    Use SQLite to run the provided SQL schema to create the VehicleLocations table. -->

4. **Run migrations to create the database schema:**
    
    ```bash
    dotnet ef migrations add InitialCreate
    dotnet ef database update
    ```

5. **Run app:**
    
    ```bash
    dotnet run
    ```

6. **Display dashboard:**

    Go to browser and run `http://localhost:5251/`.

## Running Tests

### Backend Tests

To run unit tests, navigate to the `Tests` directory and execute:

```bash
dotnet test
```

### Frontend Tests

1. Install Mocha and Chai:

```bash
npm install --save-dev mocha chai
```

2. Add a Test Script in your `package.json`:

```json
"scripts": {
    "test": "mocha"
}
```

3. Run the Tests:

```bash
npm test
```

## Notes

* Ensure that you have configured your SQLite connection string properly in appsettings.json.

> This implementation provides a comprehensive foundation for a fleet management system, covering both backend and frontend requirements while ensuring unit tests are in place for reliability.