from ..data_processing.db_connector import get_db_connection, release_db_connection, insert_weather_data, get_recent_data

# dummy db pool
class DummyPool:
    def __init__(self):
        self.connection = "dummy_connection"  # changed to non-None dummy connection
    def getconn(self):
        return self.connection
    def putconn(self, conn):
        self.connection = conn

def test_get_db_connection():
    pool = DummyPool()
    conn = get_db_connection(pool.getconn())
    assert conn == pool.connection