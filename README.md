# CongestionTaxCalculator

This project is a Congestion Tax Calculator built with .NET 8. It calculates the congestion tax for different types of vehicles based on the time of day and other criteria.

## Table of Contents

## Getting Started

These instructions will help you set up and run the project on your local machine for development and testing purposes.

### Prerequisites

- [.NET 8 SDK]
- [SQL Server]

### Installation

Clone the repository:
git clone https://github.com/NusratP/CongestionTaxCalculator.git


## Endpoint documentation - Swagger

## Connection string
Change the connection string in the appsettings.json file
 "DefaultConnection": "ConnectionStringHere"

## Database migration command
dotnet ef migrations add {migration_Name} 
dotnet ef database update


## CalculateTax API
 **-   CalculateTax**
<details><summary> Payload : </summary>

[
  "2013-12-13T19:15:09.011Z"
]

**Response:**
Successful : NoContent
Error: BadRequest (400), InternalServerError (500) 