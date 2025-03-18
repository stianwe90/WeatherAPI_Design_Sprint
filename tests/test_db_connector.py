# tests/test_db_connector.py
import pytest
from data_processing.db_connector import DBConnector

# DummyPool simulates a connection pool for tests.
class DummyPool:
    def __init__(self):
        self.connection = "dummy_connection"  # Explanation: A stand-in value to simulate a valid connection.

    def getconn(self):
        # Explanation: Mimics retrieving a connection from the pool.
        return self.connection

    def putconn(self, conn):
        # Explanation: Mimics returning a connection to the pool.
        self.connection = conn

# Pytest fixture that creates a DBConnector instance with the DummyPool.
@pytest.fixture
def dummy_db_connector():
    dummy_pool = DummyPool()
    return DBConnector(dummy_pool)

def test_get_db_connection(dummy_db_connector):
    # Explanation: Test that the dummy DBConnector returns the expected dummy connection.
    conn = dummy_db_connector.get_db_connection()
    assert conn == "dummy_connection"


