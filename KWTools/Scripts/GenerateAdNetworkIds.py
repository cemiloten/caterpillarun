#!/usr/bin/python3
# -*- coding: utf-8 -*-
#
#  Downloads ad network ids from Google Docs to local JSON.
#
#  Copyright (c) 2011 Kwalee Ltd. All rights reserved.

import os
import sys
import requests
import csv
from io import StringIO
import json

PROJECT_DIR = os.getcwd() + '/../../'
OUTPUT_DIR = PROJECT_DIR + 'Tools/Configs/'
OUTPUT_FILENAME = 'AdNetworkIds.json'
CONFIG_DIR = PROJECT_DIR + 'Tools/Configs/AdNetworksConfig.json'
CONFIG_URL_KEY = 'spreadsheet_url'


if __name__ == "__main__":
    # Get spreadsheet url
    if not os.path.exists(CONFIG_DIR):
        sys.exit('Could not find config at "{}".'.format(CONFIG_DIR))
    config_file = open(CONFIG_DIR)
    config_json = json.load(config_file)
    if CONFIG_URL_KEY not in config_json:
        sys.exit('Could not find key "{}" in config "{}".'.format(CONFIG_URL_KEY, CONFIG_DIR))
    elif not config_json[CONFIG_URL_KEY]:
        sys.exit('Key "{}" is empty in config "{}".'.format(CONFIG_URL_KEY, CONFIG_DIR))
    spreadsheet_url = config_json[CONFIG_URL_KEY]

    # Download spreadsheet
    request = requests.get(spreadsheet_url)
    try:
        csv_string = StringIO(request.content)
    except TypeError:
        csv_string = StringIO(request.content.decode('utf-8'))

    # Read network ids from spreadsheet
    reader = csv.reader(csv_string, delimiter=',')
    included_cols = [1]
    network_ids = []
    next(reader, None)  # Skip first row
    for row in reader:
        network_id = list((row[i] for i in included_cols))[0]
        if network_id and network_id not in network_ids:
            network_ids.append(network_id)
            print('Ad network id: ' + network_id)

    # Write network ids to output
    if not os.path.exists(OUTPUT_DIR):
        os.makedirs(OUTPUT_DIR)
    with open(OUTPUT_DIR + OUTPUT_FILENAME, "w") as f:
        f.write(json.dumps(network_ids))

    print('Ad network ids generated successfully')
