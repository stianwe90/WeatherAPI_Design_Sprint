import json
import os
import time
import requests
import logging as logger
from flask import Flask, request, jsonify
from flask_limiter import Limiter
from flask_limiter.util import get_remote_address
from db_connector import create_db_connector
from dotenv import load_dotenv

load_dotenv()  # Load environment variables

app = Flask(__name__)
limiter = Limiter(get_remote_address, app=app, default_limits=["10 per minute"])

#logging configuration
logger.basicConfig(level=os.getenv("LOGLEVEL", "INFO"))

# Create a DBconnector instance using live connection pool
# Explanation: The DBConnector abstracts away pool management and database operations.
db_connector = create_db_connector()
####################

# User-Agent header for API requests
HEADERS = {"User-Agent": os.getenv("USER_AGENT")}

# Explanation: Retrieve latitude and longitude for the given city using OpenStreetMap
def get_coordinates(city):
    """ Convert city name to latitude and longitude """
    url = f"https://nominatim.openstreetmap.org/search?city={city}&format=json&limit=1"
    response = requests.get(url, headers=HEADERS)
    if response.status_code == 200 and response.json():
        data = response.json()[0]
        logger.info("Coordinates for city %s: %s, %s", city, data["lat"], data["lon"])
        return data["lat"], data["lon"]
    return None, None


@app.route("/weather/current", methods=["GET"])
@limiter.limit("10 per minute")
def current_weather():
    """Fetch current weather from the API or return cached data from the database."""
    location = request.args.get("location")
    if not location:
        logger.warning("Location parameter missing in request.")
        return jsonify({"error": "Location is required"}), 400

    logger.info("Received current weather request for location: %s", location)
    lat, lon = get_coordinates(location)
    if not lat or not lon:
        logger.warning("Invalid location provided: %s", location)
        return jsonify({"error": "Invalid location"}), 400

    # Explanation: Attempt to retrieve cached data if available and recent.
    cached_data = db_connector.get_recent_data(location)
    if cached_data:
        logger.info("Returning cached data for location: %s", location)
        return jsonify(cached_data)

    # Explanation: No cached data - fetching from external weather API.
    url = f"https://api.met.no/weatherapi/locationforecast/2.0/compact?lat={lat}&lon={lon}"
    try:
        response = requests.get(url, headers=HEADERS)
        response.raise_for_status()
    except Exception as e:
        logger.exception("Failed to fetch data from api.met.no for location: %s", location)
        return jsonify({"error": "Failed to fetch data"}), 500

    weather_data = response.json()
    logger.info("Fetched new weather data for location: %s", location)

    # Explanation: Save the new weather data into the database.
    try:
        db_connector.insert_weather_data(location, weather_data)
    except Exception as e:
        logger.exception("Failed to insert weather data into DB for location: %s", location)
        # Depending on needs, you might return an error or simply log and continue.

    return jsonify(weather_data)


if __name__ == "__main__":
    app.run(host="127.0.0.1", port=5001)
