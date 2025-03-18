import os
import json
import psycopg2
import logging as logger
from psycopg2 import pool
from dotenv import load_dotenv

load_dotenv()  # Load environment variables

# DBConnector class encapsulates database operations using a connection pool.
class DBConnector:
    def __init__(self, db_pool):
        self.db_pool = db_pool  # Explanation: Store the connection pool that will be used for all DB operations.

    def get_db_connection(self):
        """Retrieve a connection from the pool."""
        try:
            conn = self.db_pool.getconn()
            # Explanation: Successfully retrieved a connection from the pool.
            return conn
        except Exception as e:
            logger.exception(f"Failed to get connection: {e}")
            return None

    def release_db_connection(self, conn):
        """Return a connection to the pool."""
        try:
            self.db_pool.putconn(conn)
            logger.info("db connection released")
        except Exception as e:
            logger.exception(f"Failed to release connection: {e}")

    def insert_weather_data(self, location, weather_json):
        """Insert weather data using a pooled connection."""
        conn = self.get_db_connection()
        if not conn:
            return
        logger.info("db connection retrieved for inserting data")
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
            logger.info(f"Data inserted for {location}")
        except Exception as e:
            logger.exception(f"Error inserting data: {e}")
        finally:
            self.release_db_connection(conn)

    def get_recent_data(self, location):
        """Fetch recent weather data if it is less than 10 minutes old."""
        conn = self.get_db_connection()
        if not conn:
            return None
        try:
            cursor = conn.cursor()
            cursor.execute(
                """
                SELECT raw_json FROM weather_data
                WHERE location = %s
                AND timestamp >= NOW() - INTERVAL '10 minutes'
                """,
                (location,)
            )
            data = cursor.fetchone()
            cursor.close()
            if data:
                logger.info("Data < 10 min old, returning cached data")
                return data[0]
            else:
                return None
        except Exception as e:
            logger.exception(f"Error fetching recent data for {location}: {e}")
            return None
        finally:
            self.release_db_connection(conn)

# Factory function to create a DBConnector instance with a live connection pool.
def create_db_connector():
    """
    Reads environment variables, creates a connection pool and returns a DBConnector.
    Explanation: Centralizes DB configuration and connection pool initialization.
    """
    DB_USER = os.getenv("POSTGRES_USER")
    DB_PASSWORD = os.getenv("POSTGRES_PASSWORD")
    DB_HOST = os.getenv("POSTGRES_HOST", "127.0.0.1")
    DB_PORT = os.getenv("POSTGRES_PORT", 5432)
    DB_NAME = os.getenv("POSTGRES_DB")
    
    db_pool_instance = pool.SimpleConnectionPool(
        minconn=1,
        maxconn=10,
        dbname=DB_NAME,
        user=DB_USER,
        password=DB_PASSWORD,
        host=DB_HOST,
        port=DB_PORT
    )
    return DBConnector(db_pool_instance)
