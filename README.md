# Congestion Tax Calculator

This project calculates the congestion tax for vehicles passing through toll stations in a city, specifically focusing on Gothenburg. The tax varies based on time of day, and the system allows exemptions for certain vehicle types and holidays.

## Features

- **Congestion Tax Calculation**: Calculates the tax based on time intervals and vehicle type.
- **Tax Rules**: Time-based tax rules that vary throughout the day.
- **Exemptions**: Exemptions for certain vehicles (e.g., emergency vehicles, buses) and holidays.
- **City-specific Rules**: Configurations specific to Gothenburg, including holidays and max daily fee.

## Getting Started

Follow these steps to set up and run the project locally.

### Prerequisites

Make sure you have the following installed on your machine:
- [.NET SDK](https://dotnet.microsoft.com/download) (8.0 or higher)
- [SQLite](https://www.sqlite.org/download.html) (or use an in-memory database during development)
- A text editor or IDE of your choice (e.g., Visual Studio Code)

### Setup

1. **Clone the Repository**:

    ```bash
    git clone https://github.com/ElahehFarokhi/CongestionTax.git
    cd CongestionTax
    ```

2. **Install Dependencies**:

    Run the following command to restore the required packages:

    ```bash
    dotnet restore
    ```

3. **Configure the Database**:

    The project uses SQLite for database storage. By default, it will create a `taxrules.db` file. The schema will be automatically generated via EF Core migrations.

4. **Run Migrations**:

    After restoring the packages, run the following command to apply the migrations and create the necessary database schema:

    ```bash
    dotnet ef database update
    ```

    This will create the necessary tables in your SQLite database (`taxrules.db`).

5. **Seed the Database**:

    The database will be seeded with the required data (e.g., tax rules, holidays, city configurations) the first time you run the application.

6. **Run the Application**:

    You can now run the application to calculate the congestion tax for vehicles:

    ```bash
    dotnet run
    ```

    The program will calculate the tax based on the timestamps defined in **`ExampleDates.txt`**. This file contains the dates and times when a vehicle passed through the toll.

## Structure

- **Application Layer**: Contains the logic to calculate the congestion tax (`TaxCalculator`).
- **Domain Layer**: Contains the models for tax rules, holidays, vehicles, and city configurations.
- **Infrastructure Layer**: Contains the database context (`TaxDbContext`) and seed data.
- **Console Application**: The entry point for running the application and performing the tax calculation.

## Files Overview

- **`TaxCalculator.cs`**: Calculates the tax based on timestamps, vehicle type, and tax rules.
- **`Program.cs`**: The entry point for running the application, loading data, and calculating taxes.
- **`SeedData.cs`**: Seeds the database with initial data (e.g., city configuration, tax rules, and holidays).
- **`TaxDbContext.cs`**: Entity Framework Core context for managing the database.
- **Models**: Includes the definitions for `CityConfig`, `Holiday`, `RuleSet`, `TaxRule`, and `Vehicle`.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

Feel free to reach out if you have any questions or suggestions!

