#!/usr/bin/python
# Copyright (c) 2014 Adafruit Industries
# Author: Tony DiCola
#
# Permission is hereby granted, free of charge, to any person obtaining a copy
# of this software and associated documentation files (the "Software"), to deal
# in the Software without restriction, including without limitation the rights
# to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
# copies of the Software, and to permit persons to whom the Software is
# furnished to do so, subject to the following conditions:
#
# The above copyright notice and this permission notice shall be included in
# all copies or substantial portions of the Software.
#
# THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
# IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
# FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
# AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
# LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
# OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
# THE SOFTWARE.

# Can enable debug output by uncommenting:
# import logging
#logging.basicConfig(level=logging.DEBUG)

import time
import math
import json;
import Adafruit_GPIO.SPI as SPI
import Adafruit_MAX31855.MAX31855 as MAX31855
from azure.servicebus import ServiceBusService

with open("settings.json") as json_file:
    api_key = json.load(json_file)

sbs = ServiceBusService(api_key["namespace"],
                        shared_access_key_name=api_key["policy_name"],
                        shared_access_key_value=api_key["policy_secret"])

# Define a function to convert celsius to fahrenheit.
def c_to_f(c):
    return c * 9.0 / 5.0 + 32.0

# Raspberry Pi software SPI configuration.
CLK = 25
CS = 24
DO = 18
sensor = MAX31855.MAX31855(CLK, CS, DO)

# Loop printing measurements every second.
print ('Press Ctrl-C to quit.')
while True:
    temp = sensor.readTempC()
    internal = sensor.readInternalC()
    print ('Thermocouple Temperature: {0:0.3F}*C / {1:0.3F}*F').format(temp, c_to_f(temp))
    print ('    Internal Temperature: {0:0.3F}*C / {1:0.3F}*F').format(internal, c_to_f(internal))
    if not math.isnan(temp):
#        print 'This is not Nan'
        sbs.send_event('woodstove2', '{ "DeviceId":"dev-01", "Temperature":"'+ str(int(c_to_f(temp))) +'" }')
    time.sleep(1.0)