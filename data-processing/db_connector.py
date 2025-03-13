import os
import json
import psycopg2
from psycopg2 import pool
from dotenv import load_dotenv

load_dotenv()  # Load environment variables

DB_USER = os.getenv("POSTGRES_USER")
DB_PASSWORD = os.getenv("POSTGRES_PASSWORD")
DB_HOST = os.getenv("POSTGRES_HOST", "127.0.0.1")
DB_PORT = os.getenv("POSTGRES_PORT", 5432)
DB_NAME = os.getenv("POSTGRES_DB")

# Instantiate the connection pool when this module is imported.
db_pool = pool.SimpleConnectionPool(
    minconn=1,
    maxconn=10,
    dbname=DB_NAME,
    user=DB_USER,
    password=DB_PASSWORD,
    host=DB_HOST,
    port=DB_PORT
)

def get_db_connection():
    """Retrieve a connection from the pool."""
    try:
        return db_pool.getconn()
    except Exception as e:
        print(f"Failed to get connection: {e}")
        return None

def release_db_connection(conn):
    """Return a connection to the pool."""
    try:
        db_pool.putconn(conn)
    except Exception as e:
        print(f"Failed to release connection: {e}")

def insert_weather_data(location, weather_json):
    """Insert weather data using a pooled connection."""
    conn = get_db_connection()
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
        release_db_connection(conn)
