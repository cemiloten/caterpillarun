#!/usr/bin/python
#
#  Downloads localisations from Google Docs to local JSON.
#
#  Copyright (c) 2011 Kwalee Ltd. All rights reserved.
#

import os
import sys
try:
    reload(sys)
    sys.setdefaultencoding('utf8')
except:
    print("")
import re
import shutil
import requests
try:
    from StringIO import StringIO
except ModuleNotFoundError:
    from io import StringIO
import argparse
import json

manifestJsonPath = "../../Packages/manifest.json"
projectSettingsPath = "../../ProjectSettings/ProjectSettings.asset"
facebookAssetPath = "../../Assets/FacebookSDK/SDK/Resources/FacebookSettings.asset"
googleServicePlist = "../../Assets/Data/GoogleService-Info.plist"

configPath = "../../Tools/Configs/"
embededCoreConfig = "../../Assets/Scripts/Misc/EmbededCoreConfig.cs"
projectDefinitionsTxtUATPath = configPath + "ProjectDefinitions.txt"
projectDefinitionsTxtLIVEPath = configPath + "ProjectDefinitions.txt.live"
chinaConfig = configPath + "ChinaConfigs.json"
panglePath = "../../Assets/MoPub/Mediation/Pangle/Editor/PangleDependencies.xml"
adDependencyPath = "../../Assets/MoPub/CustomNetworks/Editor/AdDependencies.xml"

def ensure_dir(f):
    d = os.path.dirname(f)
    if not os.path.exists(d):
        os.makedirs(d)
        print ("\nDirectory " + d + " \t:Created")

def deleteFilesInDir(f):
    if os.path.exists(f):
        d = os.path.dirname(f)
        if os.path.exists(d):
            shutil.rmtree(d)

def deleteLineInFile(filePath, lineToDelete):
    if os.path.exists(filePath):
        with open(filePath, "r+") as f:
            new_f = f.readlines()
            f.seek(0)
            for line in new_f:
                if lineToDelete not in line:
                    f.write(line)
            f.truncate()
        print ("in " + filePath + " deleting " + line)

def replaceStringInFile(filePath, stringToChange, changeTo):
    li = []
    if os.path.exists(filePath):
        with open(filePath, 'r') as file:
            for line in file:
                i = line.find(stringToChange, 1)
                if i >= 0:
                    lineval = line.replace(stringToChange, changeTo)
                    li.append(lineval)
                else:
                    lineval = line
                    li.append(lineval)
    j = 0
    if os.path.exists(filePath):
        with open(filePath, 'w') as file:
            while j < len(li):
                file.write(li[j])
                j += 1

def replaceOneFileWithAnother(fileFrom, fileTo):
    if os.path.exists(fileFrom) and os.path.exists(fileTo):
        with open(fileFrom) as f:
            with open(fileTo, "w") as f1:
                for line in f:
                    f1.write(line)

def cleanJsonFile(rootName):
    #Reading the ugly json to structurize it.

    with open(chinaConfig, 'r') as json_file:
        jsonData = json.load(json_file)
    for key, value in jsonData.items():
        if (str(key) == rootName):
            jsonPretty = json.dumps(value, sort_keys=True, indent=4)

            return json.loads(jsonPretty)
            break

def ConfigSetup(jsonData, filePath):
    if (bool(jsonData)):
        newContent = ""
        with open(filePath, "r") as f:
            content = f.read()
            new_content = content
            for key,item in jsonData.items():
                pattern = "(" + str(key) + ")(.*)"
                newContent = re.sub(pattern, r'\1": "' + str(item) + '",', new_content)
                new_content = newContent
            f.close()
        with open(filePath, "w") as f:
            f.write(newContent)
            print(newContent)
            f.close()

def GoogleSerivceSetup(jsonData, filePath):
    if (bool(jsonData)):
        newContent = ""
        with open(filePath, "r") as f:
            content = f.read()
            new_content = content
            for key,item in jsonData.items():
                pattern = "<key>" + str(key) + ".*\s*.*"
                newContent = re.sub(pattern, "<key>" + r'' + str(key) + "</key>\n  <string>" + str(item) + "</string>", new_content)
                new_content = newContent
            f.close()
        with open(filePath, "w") as f:
            f.write(newContent)
            print(newContent)
            f.close()

def FacebookSetup(jsonData, filePath):
    if (bool(jsonData)):
        newContent = ""
        with open(filePath, "r") as f:
            content = f.read()
            new_content = content
            for key,item in jsonData.items():
                pattern = str(key) + "\w*:\s*-\s*.*"
                #pattern = "appIds:\s*-\s*\d*"
                newContent = re.sub(pattern, r'' + str(key) + ":\n  - " + str(item), new_content)
                new_content = newContent
            f.close()
        with open(filePath, "w") as f:
            f.write(newContent)
            print(newContent)
            f.close()

def ProjectDefinitionSetup(jsonData, filePath):
    if (bool(jsonData)):
        newContent = ""
        with open(filePath, "r") as f:
            content = f.read()
            new_content = content
            for key, item in jsonData.items():
                pattern = "(\D" + str(key) + "\D*:)(\s\d)(\D)"
                newContent = re.sub(pattern, r'\g<1> ' + str(item) + r'\3', new_content)
                new_content = newContent
            f.close()
        with open(filePath, "w") as f:
            f.write(newContent)
            print(newContent)
            f.close()

def ProjectSettingsSetup(jsonData, filePath):
    if (bool(jsonData)):
        newContent = ""
        with open(filePath, "r") as f:
            content = f.read()
            new_content = content
            for key, item in jsonData.items():
                pattern = "(" + str(key) + ": [a-zA-Z0-9\.][a-zA-Z0-9\.].*)"
                newContent = re.sub(pattern, r'' + str(key) + ": "+ str(item), new_content)
                if(str(key) == "UNITY_STORE"):
                    newContent = re.sub("UNITY_STORE;", str(item), new_content)
                elif(str(key) == "scriptingDefineSymbols"):
                    newPattern = "(scriptingDefineSymbols:\n[^\>]*\s4:)(.*)"
                    newContent = re.sub(newPattern, r'\1' + r'\2' + ";" + str(item) + ";", new_content)
                else:
                    newContent = re.sub(pattern, r'' + str(key) + ": " + str(item), new_content)
                new_content = newContent
            f.close()
        with open(filePath, "w") as f:
            f.write(newContent)
            f.close()

def ManifestSetup(jsonData, filePath):
    if (bool(jsonData)):
        newContent = ""
        with open(filePath, "r") as f:
            content = f.read()
            new_content = content
            for key, item in jsonData.items():
                #.*com.unity.purchasing.*
                pattern = ".*" + str(key) + ".*"
                newContent = re.sub(pattern, "", new_content)
                new_content = newContent
            f.close()
        with open(filePath, "w") as f:
            f.write(newContent)
            #print(newContent)
            f.close()

def BuildConfigSetup(jsonData, filePath):
    if (bool(jsonData)):
        newContent = ""
        with open(filePath, "r") as f:
            content = f.read()
            new_content = content
            for key, item in jsonData.items():
                pattern = "(" + str(key) + ")(.*)"
                newContent = re.sub(pattern, r'\1": ' + str(item) + ',', new_content)
                new_content = newContent
            f.close()
        with open(filePath, "w") as f:
            f.write(newContent)
            print(newContent)
            f.close()

def EmbededCoreConfig(jsonData, filePath):
    if (bool(jsonData)):
        newContent = ""
        if (filePath == ""):
            with open(embededCoreConfig, "r") as f:
                content = f.read()
                new_content = content
                for key, item in jsonData.items():
                    pattern = "(.*" + str(key) + " =\s)(.*)"
                    newContent = re.sub(pattern, r'\1"' + str(item) + '";', new_content)
                    new_content = newContent
                f.close()
            with open(embededCoreConfig, "w") as f:
                f.write(newContent)
                print(newContent)
                f.close()
        else:
            with open(filePath, "r") as f:
                content = f.read()
                new_content = content
                for key, item in jsonData.items():
                    pattern = "(.*" + str(key) + " =\s)(.*)"
                    newContent = re.sub(pattern, r'\1"' + str(item) + '";', new_content)
                    new_content = newContent
                f.close()
            with open(filePath, "w") as f:
                f.write(newContent)
                print(newContent)
                f.close()

def PangleChange(filePath):
    with open (filePath, "r+") as f:
        f.truncate(0)
        f.write('<dependencies>\n\t<iosPods>\n\t\t<iosPod name="Ads-CN"/>\n\t\t<iosPod name="CSJ-mopub-adapter"/>\n\t</iosPods>\n</dependencies>')
        f.close()

if __name__ == "__main__":

    #ARGUMENTS
    arg_parser = argparse.ArgumentParser(description='China Build Preparation for Kwalee')
    arg_parser.add_argument('-f', '--file', help='Release flag', required=True, nargs='*')
    arg_parser.add_argument('-n', '--name', help='Release flag', required=False, nargs='*')

    args = arg_parser.parse_args()
    fileName = args.file[0]
    print (fileName)
    #Core Config
    if(fileName == "PangleDependencies.xml"):
        PangleChange(panglePath)
    elif(fileName == "FacebookSettings.asset"):
        FacebookSetup(cleanJsonFile(fileName),facebookAssetPath)
    elif(fileName == "CoreConfig.json"):
        ConfigSetup(cleanJsonFile(fileName),coreConfigPath)
    elif(fileName == "ProjectSettings.asset"):
        ProjectSettingsSetup(cleanJsonFile(fileName),projectSettingsPath)
    elif(fileName == "ProjectDefinitions.txt"):
        ProjectDefinitionSetup(cleanJsonFile(fileName),projectDefinitionsTxtUATPath)
    elif(fileName == "ProjectDefinitions.txt.live"):
        ProjectDefinitionSetup(cleanJsonFile(fileName),projectDefinitionsTxtLIVEPath)
    elif(fileName == "GoogleService-Info.plist"):
        GoogleSerivceSetup(cleanJsonFile(fileName),googleServicePlist)
    elif (fileName == "manifest.json"):
        ManifestSetup(cleanJsonFile(fileName),manifestJsonPath)
    elif (fileName == "BuildConfig.json"):
        BuildConfigSetup(cleanJsonFile(fileName), args.name[0])
    elif (fileName == "EmbededCoreConfig.cs"):
        if args.name is not None:
            EmbededCoreConfig(cleanJsonFile(fileName),args.name[0])
        else:
            EmbededCoreConfig(cleanJsonFile(fileName), "")




