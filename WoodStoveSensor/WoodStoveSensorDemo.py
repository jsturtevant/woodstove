from azure.servicebus import ServiceBusService
import time
import json;

with open("settings.json") as json_file:
    api_key = json.load(json_file) 

sbs = ServiceBusService(api_key["namespace"],
                        shared_access_key_name=api_key["policy_name"],
                        shared_access_key_value=api_key["policy_secret"])


for i in range(100):
    temp = {'DeviceId': 'dev-01', 'Temperature': str(i)}
    sbs.send_event('woodstove2', json.dumps(temp))
    time.sleep(1)
    print(i)