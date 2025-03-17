# tests/test_db_connector.py
import pytest
from data_processing.db_connector import DBConnector

# Dummy pool to simulate a connection pool for tests.
class DummyPool:
    def __init__(self):
        self.connection = "dummy_connection"  # non-None dummy connection

    def getconn(self):
        return self.connection

    def putconn(self, conn):
        # For our dummy, we simply store the connection back.
        self.connection = conn

# Pytest fixture to provide a DBConnector instance with a dummy pool.
@pytest.fixture
def dummy_db_connector(): # Passes a dummy pool to the DBConnector
    dummy_pool = DummyPool()
    return DBConnector(dummy_pool)

def test_get_db_connection(dummy_db_connector): # uses the dummy_db_connector and checks if the connection is returned
    conn = dummy_db_connector.get_db_connection()
    assert conn == "dummy_connection"

# insert weather data test

