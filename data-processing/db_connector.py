import os
import psycopg2
import json

# Database connection settings
### Move these to environment variables in production###
DB_NAME = "weather_db"
DB_USER = "admin"
DB_PASSWORD = "secret"
DB_HOST = "localhost"
DB_PORT = "5432"

def connect_db():
    """ Establishes a connection to the PostgreSQL database. """
    try:
        conn = psycopg2.connect(
            dbname=DB_NAME,
            user=DB_USER,
            password=DB_PASSWORD,
            host=DB_HOST,
            port=DB_PORT
        )
        return conn
    except Exception as e:
        print(f"Database connection failed: {e}")
        return None

def insert_weather_data(location, weather_json):
    """ Inserts weather data into the database. """
    conn = connect_db()
    if not conn:
        return
    
    try:
        cursor = conn.cursor()
        cursor.execute(
            """
            INSERT INTO weather_data (location, timestamp, raw_json)
            VALUES (%s, NOW(), %s)
            """,
            (location, json.dumps(weather_json))
        )
        conn.commit()
        cursor.close()
        print(f"Data inserted for {location}")
    except Exception as e:
        print(f"Error inserting data: {e}")
    finally:
        conn.close()
