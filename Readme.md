
# TP24 Technical Test Solution

## **Overview**
This repository contains the TP24 Technical Test implementation, providing an HTTP API to handle receivables data.

## **Limitations:**
- Only supports the management of receivables.
- **Performance** might vary based on the batch size.
- **In-Memory DB** for convenience used in memory database needs to configure Ms Sql if used in production 
- **Time Spent:** Approximately 3 hours.
- **Limitations:** Prioritized main features; **lacks authentication or user management**.

## **Getting Started**
1. **Clone the project:**
    ```bash
   git clone https://github.com/Prakkie91/Tp24.git
   ```

2. **Navigate to the project:**

   ```bash
   cd path/to/Tp24
   ```

3. **Restore NuGet packages:**
   ```bash
   dotnet restore
   ```

4. **Build the solution:**
   ```bash
   dotnet build
   ```

5. **Navigate to the API project:**
   ```bash
   cd src/Web/Tp24.Api
   ```

5. **Run the application:**
   ```bash
   dotnet run
   ```

> Visit [http://localhost:5000/swagger](http://localhost:5000/swagger) for API documentation.


## **Project Structure**

- **Tp24.sln:** Main solution file.
### **Source Code (/src)**
  - **Library:**
    - `Tp24.Application`: Application layer.
    - `Tp24.Common`: Common utilities and shared resources.
    - `Tp24.Core`: Core business logic.
    - `Tp24.Infrastructure`: Infrastructure-related code.
  - **Web:**
    - `Tp24.Api`: The web API project.

### **Tests (/test)**
  - `Tp24.IntegrationTest`: Integration tests.
  - `Tp24.UnitTest`: Unit tests.

## API Summary

The Tp24 API developed offers services for the creation of receivables. The API is categorized into three main endpoints:

### **1. Receivable Summary**
- **Endpoint:** `/api/v1/receivable/summary`
- **Method:** GET
- **Description:** Provides a summary of all receivables.
- **Responses:**
  - `200`: Returns a ReceivableSummaryResponse with details such as total receivables, open invoice count, closed invoice count, etc.
  - `400`: Bad Request with messages.
  - `500`: Internal Server Error.

### **2. Individual Receivable Creation**
- **Endpoint:** `/api/v1/receivable`
- **Method:** POST
- **Description:** Allows the addition of individual receivables.
- **Request:** Accepts a JSON body with details of the receivable.
- **Responses:**
  - `200`: Returns a unique identifier (UUID) for the added receivable.
  - `400`: Bad Request with messages.
  - `500`: Internal Server Error.

### **3. Batch Receivable Creation**
- **Endpoint:** `/api/v1/receivable/batch`
- **Method:** POST
- **Description:** Facilitates the addition of receivables in batches.
- **Request:** Accepts a JSON body containing an array of receivable details.
- **Responses:**
  - `200`: Returns details about the batch addition.
  - `400`: Bad Request with messages.
  - `500`: Internal Server Error.

