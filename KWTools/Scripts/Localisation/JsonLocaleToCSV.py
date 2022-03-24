#!/usr/bin/python
#
#  Localisation local file to a CSV.
#
#  Copyright (c) 2011 Kwalee Ltd. All rights reserved.
#

import os
import sys
import requests
import json
import csv
import StringIO
import re
import glob

from prettytable import PrettyTable

output_dir = "./../../../Assets/Resources/Configs/Locales/"
input_dir = "./../../../Assets/Resources/Configs/Localisation/"
languages = {"en_GB": 0}

def generate_and_check_locale_table(keys, trans, check):
    locale_table = {}

    for idx, key in enumerate(keys):
        if not trans[idx] == '':
            m = re.findall('(\%(\%[^\%]+\%\%|(\d\.?\d*)[\@dDuUxXoOfeEgGcCsSaAFi]?|[\@dDuUxXoOfeEgGcCsSaAFi]))', check[idx])
            if m:
                dummy = trans[idx]
                for match in m:
                    count = 0
                    p = re.compile(match[0])
                    [dummy, count] = p.subn('',dummy, count=1)
                    if count != 1:
                        print "\n"
                        print "ERROR string: " + match[0] + " not found in row key: " + key
                        print "Translated text : " + trans[idx]
                        print "Original Version: " + check[idx] + "\n"
            locale_table[key] = trans[idx]

    return locale_table

def output_locale_file(trans, locale_key):
    json_out = json.dumps({ "localeTable" : trans }, sort_keys=True, indent=4)
    locale_filename = os.getcwd() + output_dir + locale_key + ".json"

    file = open(locale_filename, 'w+')
    file.write(json_out)
    file.close()

    print "Written to: " + locale_filename

def output_fallback_file():
    abs_path = os.path.abspath(__file__)
    dir_name = os.path.dirname(abs_path)
    os.chdir(dir_name)

    print "Downloading spreadsheet from: " + fallback_url
    request = requests.get(fallback_url)
    csv_string = StringIO.StringIO(request.content)
    reader = csv.DictReader(csv_string, fieldnames = ("Locale Key", "Language Name", "Fallback Key"))
    rows = [row for row in reader]
    rows.pop(0)
    rows.pop(0)

    fallback_table = {}

    for row in rows:
        fallback_table[row['Locale Key']] = row['Fallback Key']

    json_out = json.dumps({ "fallbackTable" : fallback_table }, sort_keys=True, indent=4)
    locale_filename = os.getcwd() + output_dir + "fallback.json"

    file = open(locale_filename, 'w+')
    file.write(json_out)
    file.close()

    print "Written to: " + locale_filename

def flatten_json(json_data, output_data, key_path, language):
    if language in languages:
        for key in json_data:
            if type(json_data[key]) is dict:
                flatten_json(json_data[key], output_data, key_path + key + ".", language)
            elif type(json_data[key]) is list:
                counter = 0
                for array_key in json_data[key]:
                    if type(array_key) is unicode:
                        output_key = key_path + "[" + key + "|" + str(counter) + "]"
                        counter = counter + 1

                        if not ((output_key) in output_data):
                            output_data[output_key] = ["", "", "", "", ""]

                        output_data[output_key][languages[language]] = array_key.encode("utf-8")
                    else:
                        print(type(array_key))
            else:
                if type(json_data[key]) is unicode:
                    output_key = key_path + key
                    if not ((output_key) in output_data):
                        output_data[output_key] = ["", "", "", "", ""]

                    output_data[output_key][languages[language]] = json_data[key].encode("utf-8")
                else:
                    print("End " + type(json_data[key]))

if __name__ == "__main__":
    abs_path = os.path.abspath(__file__)
    dir_name = os.path.dirname(abs_path)
    os.chdir(dir_name)

    localisation_files = glob.glob(input_dir+"*.txt")
    print("Flattening Json Files:")
    for localisation_file in localisation_files:
        print(localisation_file)
    output_data = {}

    print("Flattened languages:")
    for localisation_file in localisation_files:
        json_file = open(localisation_file)
        json_data = json.load(json_file)

        language = json_data["locale"]
        print(language)
        flatten_json(json_data["text"], output_data, "", language)


    with open("output/Locale.csv", "wb") as csvfile:
        spamwriter = csv.writer(csvfile, dialect='excel')
        for key in output_data:
            spamwriter.writerow([key] + output_data[key])

