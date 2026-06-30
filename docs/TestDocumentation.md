# Test Documentation

## Overview
This document outlines the testing strategy for the Community Event Management system. The project utilizes **xUnit** as the primary testing framework, along with **Moq** for mocking dependencies, ensuring isolated unit tests.

## Testing Strategy
The testing strategy focuses on unit testing the critical components of the system to ensure robustness and reliability:
1.  **Repositories**: Verifying that data access logic (CRUD operations) works correctly without actually hitting a real database (using In-Memory database for EF Core or mocking).
2.  **Controllers**: Verifying that API endpoints/MVC actions process input correctly, interact with the repository layer properly, handle exceptions gracefully, and return the expected HTTP responses or View results.

## Covered Scenarios & Edge Cases

### 1. `EventRepository` Tests
- **Scenario: Get All Events**
  - *Standard*: Should return a list of all events.
  - *Edge Case*: Should return an empty list if no events exist.
- **Scenario: Get Event by ID**
  - *Standard*: Should return the correct event when a valid ID is provided.
  - *Edge Case*: Should return `null` when a non-existent ID is provided.
- **Scenario: Create Event**
  - *Standard*: Should successfully add a new event to the database.

### 2. `RegistrationsController` Tests
- **Scenario: Register for Event (Success)**
  - *Standard*: When a valid participant registers for an event with available capacity, the registration should succeed, and they should be redirected.
- **Scenario: Register for Event (Capacity Exceeded)**
  - *Boundary Condition*: When a participant tries to register for an event that has reached its `MaxCapacity`.
  - *Expected Result*: The controller should catch an `EventCapacityExceededException` and display a specific error message to the user, preventing the registration.
- **Scenario: Register for Event (Already Registered)**
  - *Edge Case*: When a participant tries to register for the same event multiple times.
  - *Expected Result*: The controller should catch a `ParticipantAlreadyRegisteredException` and display an appropriate error message.

## How to Run Tests
1. Navigate to the `CommunityEvents.Tests` directory.
2. Run the command: `dotnet test`
3. All tests should pass, demonstrating the reliability of the core logic and error handling.
