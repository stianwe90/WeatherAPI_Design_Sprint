import json
import os
import time
import requests
from flask import Flask, request, jsonify
from flask_limiter import Limiter
from flask_limiter.util import get_remote_address
from db_connector import insert_weather_data
from dotenv import load_dotenv

load_dotenv()  # Load environment variables

app = Flask(__name__)
limiter = Limiter(get_remote_address, app=app, default_limits=["10 per minute"])

CACHE_FILE = "weather_cache.json"

HEADERS = {"User-Agent": "WeatherAPI (waersland@live.no)"}


def get_cached_data(location, data_type):
    """ Check cache before making API call """
    try:
        with open(CACHE_FILE, "r") as f:
            cache = json.load(f)
            if location in cache and data_type in cache[location]:
                # Check if data is less than 10 min old
                if time.time() - cache[location][data_type]["timestamp"] < 600:
                    return cache[location][data_type]["data"]
    except FileNotFoundError:
        pass  # Cache file does not exist
    return None  # No valid cache found

# Saves a copy of the latest weather data in a JSON file for caching
def save_cache(location, data_type, data):
    """ Save weather data in cache """
    try:
        with open(CACHE_FILE, "r") as f:
            cache = json.load(f)
    except FileNotFoundError:
        cache = {}

    if location not in cache:
        cache[location] = {}

    cache[location][data_type] = {
        "data": data,
        "timestamp": time.time()
    }

    with open(CACHE_FILE, "w") as f:
        json.dump(cache, f)

# Returns the latitude and longitude of a city based on city name
def get_coordinates(city):
    """ Convert city name to latitude and longitude """
    url = f"https://nominatim.openstreetmap.org/search?city={city}&format=json&limit=1"
    response = requests.get(url, headers=HEADERS)
    if response.status_code == 200 and response.json():
        data = response.json()[0]
        return data["lat"], data["lon"]
    return None, None


@app.route("/weather/current", methods=["GET"])
@limiter.limit("10 per minute")
def current_weather():
    """ Fetch current weather from API or cache """
    location = request.args.get("location")
    if not location:
        return jsonify({"error": "Location is required"}), 400

    lat, lon = get_coordinates(location)
    if not lat or not lon:
        return jsonify({"error": "Invalid location"}), 400

    cached_data = get_cached_data(location, "current")
    if cached_data:
        return jsonify(cached_data)

    url = f"https://api.met.no/weatherapi/locationforecast/2.0/compact?lat={lat}&lon={lon}"
    response = requests.get(url, headers=HEADERS)
    if response.status_code != 200:
        return jsonify({"error": "Failed to fetch data"}), response.status_code

    weather_data = response.json()
    save_cache(location, "current", weather_data)
    
     # Store in PostgreSQL
    insert_weather_data(location, weather_data)
    
    return jsonify(weather_data)

if __name__ == "__main__":
    app.run(host="127.0.0.1", port=5001)
