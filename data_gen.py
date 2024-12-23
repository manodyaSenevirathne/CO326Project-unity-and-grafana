import time
import random
from datetime import datetime
from influxdb_client import InfluxDBClient, Point, WriteOptions

# Configuration - replace these values with your actual InfluxDB configuration
INFLUXDB_URL = "http://localhost:8086/"
INFLUXDB_TOKEN = "NdsPlJLyTBW_YRzsLfR99Mf9ZnyK-xf11R4qQw4xREY2u6_zudlg4aG_F5aVCPsMnoK2ZIT7_mvyoGW079E3Vw=="  # Replace with your actual token
INFLUXDB_ORG = "326Project"                # Replace with your actual organization name
INFLUXDB_BUCKET = "electrical system"     # Replace with your actual bucket name

# Initialize the InfluxDB client
client = InfluxDBClient(url=INFLUXDB_URL, token=INFLUXDB_TOKEN, org=INFLUXDB_ORG)
write_api = client.write_api(write_options=WriteOptions(batch_size=500, flush_interval=10_000))

def generate_synthetic_data():
    current_time = datetime.utcnow().isoformat()
    sensors_data = []

    # Power meter data
    power_meter_value = random.uniform(100, 1000)  # watts
    sensors_data.append(Point("power_meter").field("power", power_meter_value).time(current_time))

    # Current meter data
    current_meter_value = random.uniform(10, 100)  # amperes
    sensors_data.append(Point("current_meter").field("current", current_meter_value).time(current_time))

    # Voltage meter data
    voltage_meter_value = random.uniform(220, 240)  # volts
    sensors_data.append(Point("voltage_meter").field("voltage", voltage_meter_value).time(current_time))

    # Smoke detector data
    smoke_detector_value = random.choice([0, 1])  # 0: no smoke, 1: smoke detected
    sensors_data.append(Point("smoke_detector").field("smoke", smoke_detector_value).time(current_time))

    # Temperature sensor data
    temperature_value = random.uniform(20, 35)  # Celsius
    sensors_data.append(Point("temperature_sensor").field("temperature", temperature_value).time(current_time))

    # Additional sensors
    humidity_value = random.uniform(30, 70)  # percentage
    sensors_data.append(Point("humidity_sensor").field("humidity", humidity_value).time(current_time))

    co2_value = random.uniform(400, 1000)  # ppm
    sensors_data.append(Point("co2_sensor").field("co2", co2_value).time(current_time))

    total_power = random.uniform(400, 2000)  
    sensors_data.append(Point("overall").field("total_power", total_power).time(current_time))

    floor2_pow = random.uniform(100, 1000)  
    sensors_data.append(Point("floor2").field("floor2_pow", floor2_pow).time(current_time))

    floor3_pow = random.uniform(100, 1000)  
    sensors_data.append(Point("floor3").field("floor3_pow", floor3_pow).time(current_time))

    return sensors_data

def main():
    while True:
        # Generate synthetic data
        data_points = generate_synthetic_data()

        # Write data to InfluxDB
        write_api.write(bucket=INFLUXDB_BUCKET, org=INFLUXDB_ORG, record=data_points)

        # Print the data for debugging
        for point in data_points:
            print(f"Written: {point.to_line_protocol()}")

        # Sleep for a bit before generating new data
        time.sleep(10)

if __name__ == "__main__":
    main()
