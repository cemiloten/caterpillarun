#!/usr/bin/python
# -*- coding: utf-8 -*-
#
#  Downloads localisations from Google Docs to local JSON.
#
#  Copyright (c) 2011 Kwalee Ltd. All rights reserved.
#
# PYTHON 2 might need to switch this script to Python 3

import sys
try:
    reload(sys)
    sys.setdefaultencoding('utf8')
except:
    print("")

import os,shutil
import sys
import requests
import json
import csv
from pathlib import Path
try:
    from StringIO import StringIO
except ModuleNotFoundError:
    from io import StringIO
import argparse
import re
from prettytable import PrettyTable
from jinja2 import Template, Environment, FileSystemLoader

output_temp_dir = "/output/"
output_dir = "../../../Assets/Resources/Configs/Localisation/"
android_res_dir = "../../../Assets/Plugins/Android/res/"
templateFolder = os.getcwd() + '/Localisation/templates'
loc_keys_dir = os.getcwd() + '/../../Assets/Kwalee/Localisation/'

def generate_and_check_locale_table(keys, trans, check):
    locale_table = {}

    for idx, key in enumerate(keys):

        indexOfDot = key.find('.')
        if indexOfDot != -1:
            prekey = key[0:indexOfDot]
        else:
            prekey = ""

        if (prekey == "" or prekey != "ignore"):
            if not trans[idx] == '':
                m = re.findall('(\%(\%[^\%]+\%\%|(\d\.?\d*)[\@dDuUxXoOfeEgGcCsSaAFi]?|[\@dDuUxXoOfeEgGcCsSaAFi]))',
                               check[idx])
                if m:
                    dummy = trans[idx]
                    for match in m:
                        count = 0
                        p = re.compile(match[0])
                        [dummy, count] = p.subn('', dummy, count=1)
                        if count != 1:
                            print ("\n")
                            print ("WARNING string: " + match[0] + " not found in row key: " + key)
                            print ("Translated text : " + trans[idx])
                            print ("Original Version: " + check[idx] + "\n")
                locale_table[key] = trans[idx].replace('\\n', '\n')

    return locale_table

def output_locale_file(trans, locale_key):
    json_out = json.dumps({"localeTable": trans}, sort_keys=True, indent=4)
    ensure_dir(os.getcwd() + output_temp_dir)
    locale_filename = os.getcwd() + output_temp_dir + locale_key + ".json"

    file = open(locale_filename, 'w+')
    file.write(json_out)
    file.close()

def createFileWithInfo(direct, name, content):
    created_file = os.getcwd() + "/" + direct + "/" + name
    crtFile = open(created_file, 'wb+')
    crtFile.write(content.encode('utf-8'))
    crtFile.close()

def apple_localisation(translation_table, file_lang_key, english_table):
    templateFile = os.getcwd() + '/template.xliff'

    fixed_language = re.sub("_", "-", file_lang_key)

    print('\033[91m' + "\nLanguage = " + fixed_language + '\033[0m')

    xclocDirec = "../../../Xcode.Extras/Localisation/" + fixed_language + ".xcloc"
    localDirec = xclocDirec + "/Localized Contents/"
    srcContentDirec = xclocDirec + "/Source Contents/en.lproj/"
    hasApple = True

    if not ("Apple.CFBundleDisplayName" in translation_table):
        print('\033[91m' + "ERROR: Missing Apple.CFBundleDisplayName" + '\033[0m')
        hasApple = False
    if not ("Apple.CFBundleName" in translation_table):
        print('\033[91m' + "ERROR: Missing Apple.CFBundleName" + '\033[0m')
        hasApple = False
    if not ("Apple.NSCalendarsUsageDescription" in translation_table):
        print('\033[91m' + "ERROR: Missing Apple.NSCalendarsUsageDescription" + '\033[0m')
        hasApple = False
    if not ("Apple.NSLocationAlwaysUsageDescription" in translation_table):
        print('\033[91m' + "ERROR: Missing Apple.NSLocationAlwaysUsageDescription" + '\033[0m')
        hasApple = False
    if not ("Apple.NSLocationWhenInUseUsageDescription" in translation_table):
        print('\033[91m' + "ERROR: Missing Apple.NSLocationWhenInUseUsageDescription" + '\033[0m')
        hasApple = False
    if not ("Apple.NSPhotoLibraryUsageDescription" in translation_table):
        print('\033[91m' + "ERROR: Missing Apple.NSPhotoLibraryUsageDescription" + '\033[0m')
        hasApple = False
    if not ("IDFA.NSUserTrackingUsageDescription" in translation_table):
        print('\033[91m' + "ERROR: Missing IDFA.NSUserTrackingUsageDescription" + '\033[0m')
        hasApple = False

    if (hasApple == True):
        if file_lang_key == "en_GB":
            loop_data_array = [
                {'id': 'CFBundleDisplayName',
                 'name': english_table['Apple.CFBundleDisplayName'],
                 'target': translation_table['Apple.CFBundleDisplayName'] + " "},

                {'id': 'CFBundleName',
                 'name': english_table['Apple.CFBundleName'],
                 'target': translation_table['Apple.CFBundleName']},

                {'id': 'NSCalendarsUsageDescription',
                 'name': english_table['Apple.NSCalendarsUsageDescription'],
                 'target': translation_table['Apple.NSCalendarsUsageDescription']},

                {'id': 'NSLocationAlwaysUsageDescription',
                 'name': english_table['Apple.NSLocationAlwaysUsageDescription'],
                 'target': translation_table['Apple.NSLocationAlwaysUsageDescription']},

                {'id': 'NSLocationWhenInUseUsageDescription',
                 'name': english_table['Apple.NSLocationWhenInUseUsageDescription'],
                 'target': translation_table['Apple.NSLocationWhenInUseUsageDescription']},

                {'id': 'NSPhotoLibraryUsageDescription',
                 'name': english_table['Apple.NSPhotoLibraryUsageDescription'],
                 'target': translation_table['Apple.NSPhotoLibraryUsageDescription']},

                {'id': 'NSUserTrackingUsageDescription',
                 'name': english_table['IDFA.NSUserTrackingUsageDescription'],
                 'target': translation_table['IDFA.NSUserTrackingUsageDescription']}
            ]
        else:
            loop_data_array = [
                {'id': 'CFBundleDisplayName',
                 'name': english_table['Apple.CFBundleDisplayName'],
                 'target': translation_table['Apple.CFBundleDisplayName']},

                {'id': 'CFBundleName',
                 'name': english_table['Apple.CFBundleName'],
                 'target': translation_table['Apple.CFBundleName']},

                {'id': 'NSCalendarsUsageDescription',
                 'name': english_table['Apple.NSCalendarsUsageDescription'],
                 'target': translation_table['Apple.NSCalendarsUsageDescription']},

                {'id': 'NSLocationAlwaysUsageDescription',
                 'name': english_table['Apple.NSLocationAlwaysUsageDescription'],
                 'target': translation_table['Apple.NSLocationAlwaysUsageDescription']},

                {'id': 'NSLocationWhenInUseUsageDescription',
                 'name': english_table['Apple.NSLocationWhenInUseUsageDescription'],
                 'target': translation_table['Apple.NSLocationWhenInUseUsageDescription']},

                {'id': 'NSPhotoLibraryUsageDescription',
                 'name': english_table['Apple.NSPhotoLibraryUsageDescription'],
                 'target': translation_table['Apple.NSPhotoLibraryUsageDescription']},

                {'id': 'NSUserTrackingUsageDescription',
                 'name': english_table['IDFA.NSUserTrackingUsageDescription'],
                 'target': translation_table['IDFA.NSUserTrackingUsageDescription']}
            ]
        print (templateFolder)
        file_loader = FileSystemLoader(templateFolder)
        env = Environment(loader=file_loader)

        # XLIFF FILE
        template = env.get_template('template.xliff')
        output = template.render(loop_data_array=loop_data_array, header_language=fixed_language, master_language="en")
        ensure_dir(localDirec)
        createFileWithInfo(localDirec, fixed_language + '.xliff', output)
        # print('Created iOS file at ' + localDirec)
        # CONTENTS JSON
        template1 = env.get_template('contents.json')
        output1 = template1.render(region_language="en", target_language=fixed_language)
        createFileWithInfo(xclocDirec, 'contents.json', output1)
        # print('Created iOS file at ' + xclocDirec)
        # INFOPLIST.STRINGS
        template2 = env.get_template('InfoPlist.strings')
        output2 = template2.render(data_array=loop_data_array)
        ensure_dir(srcContentDirec)
        createFileWithInfo(srcContentDirec, "InfoPlist.strings", output2)
        # print('Created iOS file at ' + srcContentDirec)

def android_localisation(translation_table, file_lang_key):
    android_temp_file = os.getcwd() + '/strings.xml'
    split_key = file_lang_key.split('_')

    lang_value = split_key[0]
    if len(split_key) == 2:
        lang_value = split_key[0] + '+' + split_key[1]

    android_val_folder = android_res_dir + 'values-b+' + lang_value + '/'


    file_loader = FileSystemLoader(templateFolder)
    env = Environment(loader=file_loader)
    template = env.get_template('strings.xml')

    if ("Android.DisplayName" in translation_table):
        output = template.render(gameName = translation_table['Android.DisplayName'])
        output = output.replace("'", "\\'")
        ensure_dir(android_val_folder)
        createFileWithInfo(android_val_folder, "strings.xml", output)
    elif ("Apple.CFBundleDisplayName" in translation_table):
        output = template.render(gameName = translation_table['Apple.CFBundleDisplayName'])
        output = output.replace("'", "\\'")
        ensure_dir(android_val_folder)
        createFileWithInfo(android_val_folder, "strings.xml", output)

    print('Created android file at ' + android_val_folder)
    # print ('Generic.GameName')

def dump_language(translation_table, file_lang_key):
    master_table = {}

    for key in translation_table:
        create_path(master_table, key, 0, translation_table[key])

    json_out = json.dumps({"version": 0, "locale": file_lang_key, "text": master_table}, sort_keys=True, indent=4)
    ensure_dir(os.getcwd() + "/" + output_dir)
    locale_filename = os.getcwd() + "/" + output_dir + "Localisation_" + file_lang_key + ".txt"
    file = open(locale_filename, 'w+')
    file.write(json_out)
    file.close()
    print ("Written to: " + locale_filename)

def output_fallback_file():
    abs_path = os.path.abspath(__file__)
    dir_name = os.path.dirname(abs_path)
    os.chdir(dir_name)

    # print ("Downloading spreadsheet from: " + fallback_url)
    request = requests.get(fallback_url)
    try:
        csv_string = StringIO(request.content)
    except TypeError:
        csv_string = StringIO(request.content.decode('utf-8'))

    reader = csv.DictReader(csv_string, fieldnames=("Locale Key", "Language Name", "Fallback Key"))
    rows = [row for row in reader]
    rows.pop(0)
    rows.pop(0)

    fallback_table = {}

    for row in rows:
        fallback_table[row['Locale Key']] = row['Fallback Key']

    json_out = json.dumps({"fallbackTable": fallback_table}, sort_keys=True, indent=4)
    locale_filename = os.getcwd() + output_temp_dir + "fallback.json"

    file = open(locale_filename, 'w+')
    file.write(json_out)
    file.close()

    print ("Written to: " + locale_filename)

def check_array(key):
    m = re.search('\[(.*)\|(\d+)\]', key)
    if m:
        return [m.group(1), m.group(2)]

def create_path(table, key, index, value):
    path_array = key.split(".")
    path_key = path_array[index]

    if check_array(path_key):
        is_array = check_array(path_key)

        if is_array[0] in table:
            table[is_array[0]].append(value)
        else:
            table[is_array[0]] = [value]

    else:
        if type(table) is dict:
            if path_key not in table:
                table[path_key] = {}
            if index < (len(path_array) - 1):
                create_path(table[path_key], key, index + 1, value)
            else:
                table[path_key] = value

def ensure_dir(f):
    d = os.path.dirname(f)
    if not os.path.exists(d):
        os.makedirs(d)
        print ("\nDirectory " + d + " \t:Created")

def deleteFilesInDir(f):
    d = os.path.dirname(f)
    if os.path.exists(d):
        shutil.rmtree(d)

def deleteFilesInSub(dir,fileName):
    for f in Path(dir).glob(fileName):
        try:
            pathFile = str(f) + "/"
            deleteFilesInDir(pathFile)
        except OSError as e:
            print("WARNING: %s : %s" % (f, e.strerror))

def generate_keys_script(localisation_rows):
    class_name = 'LocalisationKeys'

    loc_keys = [row['Locale Key'] for row in localisation_rows]
    loc_keys = filter(None, loc_keys)
    # Eliminate duplicates.
    loc_keys = list(dict.fromkeys(loc_keys))
    loc_keys.sort()

    script_content = 'public class ' + class_name + '\n{\n'
    for key in loc_keys:
        script_content += '\tpublic const string ' + key.replace('.','_').replace('-','') + ' = "' + key + '";\n'
    script_content += '}'

    if not os.path.exists(loc_keys_dir):
        os.makedirs(loc_keys_dir)
    with open(loc_keys_dir + class_name +'.cs', 'w') as keys_file:
        keys_file.write(script_content)

if __name__ == "__main__":
    configFile = open(os.getcwd() + os.sep + "../.." + os.sep + "Tools/Configs" + os.sep + "LocalisationSources.json")
    configJson = json.load(configFile)

    deleteFilesInDir("../../Xcode.Extras/Localisation/")
    deleteFilesInDir("../../Assets/Resources/Configs/Localisation/")
    deleteFilesInSub("../../Assets/Plugins/Android/res/", "values*")
    deleteFilesInDir("Localisation/output/")

    default_enviroment = "live"
    valid_enviroments = set(configJson.keys())

    arg_parser = argparse.ArgumentParser(description='Generate project localisation files from google spreadsheets.')
    arg_parser.add_argument('-e', '--enviroment',
                            help='Defaults to: ' + default_enviroment + '. The enviroment from where the localisation files are going to be pulled from.',
                            required=False, choices=valid_enviroments, default=default_enviroment)

    args = arg_parser.parse_args()

    print("\n")
    print("Configured enviroment:   " + args.enviroment)
    print("Master language:         " + configJson[args.enviroment]['master_language'])
    print("Supported languages:     " + json.dumps(configJson[args.enviroment]['supported_languages']))
    print("Fallback config:         " + json.dumps(configJson[args.enviroment]['language_fallbacks']))
    print("\n")

    spreadsheet_url = configJson[args.enviroment]['url']

    if not (spreadsheet_url):
        print ("There is not url added to LocalisationSource.json")
        sys.exit(0)

    abs_path = os.path.abspath(__file__)
    dir_name = os.path.dirname(abs_path)
    os.chdir(dir_name)

    print ("Downloading spreadsheet from: " + spreadsheet_url)

    master_language = configJson[args.enviroment]['master_language']
    supported_languages = [master_language]
    supported_languages.extend(configJson[args.enviroment]['supported_languages'])
    language_fallbacks = configJson[args.enviroment]['language_fallbacks']

    request = requests.get(spreadsheet_url)

    try:
        csv_string = StringIO(request.content)
    except TypeError:
        csv_string = StringIO(request.content.decode('utf-8'))

    reader = csv.DictReader(csv_string, fieldnames=(
    "Locale Key", "en_GB", "Unused", "Unused", "Unused", "Notes", "de", "Notes", "fr", "Notes", "it", "Notes", "es",
    "Notes", "pt_PT", "Notes", "ru", "Notes", "zh_Hans", "Notes", "zh_Hant", "Notes", "ja", "Notes", "ko", "Notes",
    "ar", "Notes", "tr", "Notes", "nl", "Notes", "da", "Notes", "pl", "Notes", "sv", "Notes"))
    rows = [row for row in reader]

    # Remove the first three rows, which is just the column headers.
    rows.pop(0)
    rows.pop(0)
    rows.pop(0)

    generate_keys_script(rows)
    
    translations = {}
    translated = {}
    for language in supported_languages:
        translations[language] = []

    keys = []

    row_counter = 4
    for row in rows:
        keys.append(row['Locale Key'])
        for language in supported_languages:
            translations[language].append(row[language])

        row_counter += 1

    for language in supported_languages:
        translated[language] = generate_and_check_locale_table(keys, translations[language],
                                                               translations[master_language])

    for language in supported_languages:
        for fallbacks in language_fallbacks[language]:
            dump_language(translated[language], fallbacks)
            apple_localisation(translated[language], fallbacks, translated['en_GB'])
            android_localisation(translated[language], fallbacks)

    for language in supported_languages:
        output_locale_file(translated[language], language)
    print('\x1b[0;30;42m' + "\nFINISHED CREATING LOCALISATION FILES" + '\x1b[0m')
