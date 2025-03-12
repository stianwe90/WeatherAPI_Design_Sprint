-- initdb/init.sql
CREATE TABLE IF NOT EXISTS weather_data (
    id SERIAL PRIMARY KEY,
    location VARCHAR(255) NOT NULL,
    timestamp TIMESTAMPTZ NOT NULL DEFAULT NOW(), -- Use TIMESTAMPTZ for timezone support
    raw_json JSONB NOT NULL
);

-- Adding indexes for better query performance:
CREATE INDEX idx_weather_location ON weather_data(location);
CREATE INDEX idx_weather_timestamp ON weather_data(timestamp);
