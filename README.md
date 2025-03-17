# WeatherAPI Project
This project demonstrates an end-to-end pipeline from API ingestion to API exposure using a weather data service. The system ingests weather data from api.met.no and processes it for consumption via a custom-built REST API. 

## Overview
The project is divided into two main components:

### Data Ingestion & Processing (Python):
Retrieves current weather data and forecast information (in JSON formats) from api.met.no.
Handles API access according to api.met.no guidelines, including setting a valid User-Agent header.
Stores raw data locally in a PostgreSQL database located on a local docker instance


### REST API (C#):
Exposes the processed weather data through a REST API.
Provides endpoints to retrieve weather data for integration with a simple front-end or other services.

# Features
API Ingestion:
Seamless integration with api.met.no and nominatim.openstreetmap.org, including adherence to their guidelines for API access (e.g., setting a proper User-Agent).

# Data Processing & Storage:
Efficient structuring and storage of raw weather data with feature extraction for key metrics.

# Scalability:
Potential future expansion could include transitioning from a local environment to cloud-based storage on Azure.


# Technology Stack
- Python, PostgreSQL, Docker, C#, Flask, ASP.NET

# Design Decisions
- **Why PostgreSQL?**
  - Quick setup for prototyping, structured data querying, and simplified testing environment.
  - Avoided a data lake (Parquet) setup due to complexity and scope, better suited for a separate focused project.

# Change of public API
- Opted to use api.met.no instead of OpenWeatherAPI due easier access

# Future Extensions
- Improved error handling, logging, and data validation.
- Transition to Azure infrastructure for scalability.
- Authentication, API security improvements, and additional API features (forecasts, historical data).

While there are room for improvement and further extensions, I feel this 2.5 week mini-project has helped me showcase my understanding of the systems involved. If I were to continue on this, here is what I would work on next.

# What to focus on next

- Implement JSON schema validation for incoming API responses.
- Validate key data ranges (e.g., temperature, humidity).

- Strengthen exception handling in database interactions (e.g., connection issues, transaction errors).
- Validate data integrity before inserting records.

- Improve JSON parsing and error handling in the C# service layer.
- Expand unit and integration tests to cover error scenarios like invalid JSON, API downtime, and DB errors.
