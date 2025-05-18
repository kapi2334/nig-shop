-- Function to safely create database and handle permissions
CREATE OR REPLACE FUNCTION create_database_if_not_exists(
    db_name text,
    db_owner text
) RETURNS void AS $$
BEGIN
    -- Check if database exists
    IF NOT EXISTS (SELECT 1 FROM pg_database WHERE datname = db_name) THEN
        -- Create database if it doesn't exist
        EXECUTE format('CREATE DATABASE %I', db_name);
        
        -- Connect to the new database and set up permissions
        EXECUTE format('GRANT ALL PRIVILEGES ON DATABASE %I TO %I', db_name, db_owner);
        EXECUTE format('ALTER DATABASE %I OWNER TO %I', db_name, db_owner);
        
        RAISE NOTICE 'Created database: %', db_name;
    ELSE
        RAISE NOTICE 'Database % already exists, skipping creation', db_name;
    END IF;
END;
$$ LANGUAGE plpgsql;

-- Create databases safely
DO $$
BEGIN
    PERFORM create_database_if_not_exists('userservice', 'postgres');
    PERFORM create_database_if_not_exists('productservice', 'postgres');
    PERFORM create_database_if_not_exists('invoiceservice', 'postgres');
    PERFORM create_database_if_not_exists('orderservice', 'postgres');
END;
$$;