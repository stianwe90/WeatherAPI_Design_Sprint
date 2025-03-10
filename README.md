WeatherAPI Project
This project demonstrates an end-to-end pipeline from API ingestion to API exposure using a weather data service. The system ingests weather data from api.met.no and processes it for consumption via a custom-built REST API.

Overview
The project is divided into two main components:

Data Ingestion & Processing (Python):

Retrieves current weather data and forecast information (in JSON/XML formats) from api.met.no.
Handles API access according to api.met.no guidelines, including setting a valid User-Agent header.
Stores raw data locally in JSON format with an option to convert to Parquet for optimized querying.
Extracts key weather attributes (e.g., temperature, humidity, wind speed).

REST API (C#):

Exposes the processed weather data through a secure REST API.
Incorporates CORS for controlled public access.
Implements robust error handling and logging.
This design serves as a prototype and lays the groundwork for future enhancements, such as migrating local storage to an Azure Data Lake environment or implementing a reverse proxy.

Features
API Ingestion:
Seamless integration with api.met.no, including adherence to their guidelines for API access (e.g., setting a proper User-Agent) and handling of error responses.

Data Processing & Storage:
Efficient structuring and storage of raw weather data with feature extraction for key metrics.

REST API Exposure:
Provides endpoints to retrieve weather data for integration with a simple front-end or other services.

Scalability:
Potential future expansion could include transitioning from a local environment to cloud-based storage on Azure.
