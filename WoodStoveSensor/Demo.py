from azure.servicebus import ServiceBusService
import time
import json
import random

with open("settings.json") as json_file:
    api_key = json.load(json_file) 

sbs = ServiceBusService(api_key["namespace"],
                        shared_access_key_name=api_key["policy_name"],
                        shared_access_key_value=api_key["policy_secret"])


for i in range(100):
    roomtemp = random.randint(60, 70)

    dev1 = random.randint(80, 90)
    temp = {'DeviceId': 'demo-1', 'Temperature': str(dev1), 'RoomTemperature': str(roomtemp)}
    sbs.send_event('woodstove2', json.dumps(temp))
    print(json.dumps(temp))

    time.sleep(1)
    print(i)