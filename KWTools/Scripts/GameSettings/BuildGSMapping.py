#!/usr/bin/python
import sys
import os,shutil
import sys
import requests
import json
import csv
import argparse
import re
from termcolor import colored, cprint
from prettytable import PrettyTable
from jinja2 import Template, Environment, FileSystemLoader
from pathlib import Path

uglyGameSettings_json = ""
gamesettings_json = dict() # = "GameSettings/Game-Settings.json"
templateFolder = str(Path(os.getcwd() + "/GameSettings/templates"))
gamesettings_folder = str(Path(os.getcwd() + "/../../Assets/Kwalee/Game Settings/"))
mainObjectName = ""

#maybe there should be a general list and specific for publishing and internal
listOfSettingsToIgnorPub = ["abGroup", "adPlacementProviderWaterfall", "adSettingsAndroid", "adsPlacementProviders", "analyticsSampling",
                            "crossPromotionGlobalSettings","firehoseSettings","forceConnectionSettings", "forceConnectionSettingsAndroid", "gameID", "gdpr", "kangaSettings",
                            "links", "marketSettings","mutationHistorySettings", "pushNotificationInfos", "pushNotificationSettings","ratingsSettings", "slowDeviceCheckSettings",
                            "subscriptionSettings","userAdquisitionEventsSettings", "adSettings"]
listOfSettingsToIgnorInt = ["gameID", "abGroup", "webImageSettings" , "pushNotificationInfos" , "pushNotificationSettings", "links" , "forceConnectionSettings" ,
                            "forceConnectionSettingsAndroid", "crossPromotionGlobalSettings","ratingsSettings","mutationHistorySettings","subscriptionSettings",
                            "analyticsSampling","firehoseSettings", "gdpr", "adSettings", "adPlacementProviderWaterfall",
                            "marketSettings", "kangaSettings", "showNoAds", "cosmeticSettings", "prizeBoxSettings", "progressRewardSettings", "inProgressPushNotificationInfos",
                            "xpSystemSettings","slowDeviceCheckSettings", "userAdquisitionEventsSettings", "saveDataManagerSettings","questSettings", "subscriptionSettings",
                            "onboardingLevelSkipSettings", "paletteGameSettings", "gameTuneSettings", "specialOfferLocations"]
isPublishing = False
isInternal = False
listOfNames = []
listOfTypes = []
listOfMainClassNames = []
listOfMainClassTypes =[]

class my_dictionary(dict):

    def __init__(self):
        self = dict()

    def add(self, key, value):
        self[key] = value

def capitalizeFirst(pValue):
    x = lambda x: x[0].upper() + x[1:]
    return x(pValue)

def listToDic(listNames, listTypes):
    list_of_dict = [{'name': L1, 'type': L2} for (L1, L2) in zip(list1, list2)]
    return list_of_dict

def createFileWithInfo(direct, name, content):
    created_file = str(Path(direct + "/" + name))
    ensure_dir(created_file)
    crtFile = open(created_file, 'w+')
    text = content.encode('utf-8')
    crtFile.write(text.decode('utf-8'))
    crtFile.close()

def ensure_dir(f):
    d = os.path.dirname(f)
    print(d)
    if not os.path.exists(d):
        os.makedirs(d)
        print("\nDirectory " + d + " \t:Created")

def addToProjectSettings(pType, typeName):
    cprint("Adding " + pType + " " + typeName)

def populateClass(dictionary, objName):
    del listOfTypes[:]
    del listOfNames[:]
    objDict = {}
    isObject = False
    isArray = False
    if (bool(dictionary)):
        for key, value in dictionary.items():
            cprint(str(key), 'yellow')
            valueName = str(key)
            listOfNames.append(valueName)
            #the value is dictionary
            for vType, vValue in value.items():
                if(str(vType) == "__type__"):
                    #print(str(vValue))
                    if(str(vValue) == "object"):
                        isObject = True
                        cprint("Value type is object -->" + valueName)
                        listOfTypes.append(capitalizeFirst(valueName))
                    elif(str(vValue) == "array"):
                        cprint("Value type is an array -->" + valueName)
                        #need to find the type
                        isArray = True
                    elif(str(vValue) == "boolean"):
                        cprint("Value type is bool -->" + valueName)
                        listOfTypes.append("bool")
                    elif(str(vValue) == "double"):
                        cprint("Value type is double -->" + valueName)
                        listOfTypes.append("float")
                    else:
                        cprint("Value type is value -->" + valueName)
                        listOfTypes.append(str(vValue))
                elif(str(vType) == "__value__"):
                    if(isArray == True and type(vValue) == type(list())):
                        #print ("it's a list")
                        isArray = False
                        for i, arrayType in enumerate(d['__type__'] for d in vValue):
                            if (str(arrayType) == "object"):
                                listOfTypes.append(capitalizeFirst(valueName) + " []")
                                isObject = True
                                break
                            else:
                                cprint("Value array -> " + str(arrayType), 'yellow')
                                listOfTypes.append(str(arrayType) + "[]")
                                break
                        if (isObject == True):
                            isObject = False
                            for i, arrayType in enumerate(d['__value__'] for d in vValue):
                                objectValue = {valueName: arrayType}
                                objDict.update(objectValue)
                                break
                    elif(isObject == True and type(vValue) == type(dict())):
                        isObject = False
                        objectValue = {valueName: vValue}
                        objDict.update(objectValue)

    cprint("Inside Class got created "+objName,'green')
    createClass(objName)
    del listOfTypes[:]
    del listOfNames[:]
    if (bool(objDict)):
        for objType, objValue in objDict.items():
            print(str(objType))
            populateClass(objValue, str(objType))
        objDict.clear()
    #if is not empty do recursive

#find all objects and save them in a list
def findObjects(dictionary,objName):
    #first iteration
    #should save the name of the main class
    del listOfTypes[:]
    del listOfNames[:]
    objDictionary = {}
    isObject = False;
    isArray = False;
    isClass = False;
    for key, value in dictionary.items():
        cprint(str(key), 'yellow')
        #cprint(type(value), 'red')
        if(type(value) == type(dict())):
            #we create the project settings mapping here
            listOfMainClassNames.append(objName)
            listOfMainClassTypes.append(capitalizeFirst(objName))
            for keyD,valueD in value.items():
               #to know for next iteration that this was an object
                if(str(valueD) == "object"):
                    isObject = True
                    isClass = True
                if(isObject == True and type(valueD) == type(dict())):
                    isObject = False
                    cprint ("This is an object", 'green')
                    #need to pass the next value
                    for keyObj, valueObj in valueD.items():
                        cprint("value name = " + str(keyObj), 'cyan')
                        #cprint("value type = " + str(valueObj), 'cyan')
                        listOfNames.append(str(keyObj))
                        valueName = str(keyObj)
                        #to know the type of the parameters in the object
                        for keyType, valueType in valueObj.items():
                            if (str(keyType) == "__type__"):
                                if(str(valueType) == "object"):
                                    isObject = True
                                    listOfTypes.append(capitalizeFirst(valueName))
                                elif(str(valueType) == "array"):
                                    isArray = True
                                elif(str(valueType) == "boolean"):
                                    listOfTypes.append("bool")
                                    cprint("Value type -> " + "bool", 'yellow')
                                elif (str(valueType) == "double"):
                                    listOfTypes.append("float")
                                    cprint("Value type -> " + "bool", 'yellow')
                                else:
                                    cprint("Value type -> " + str(valueType), 'yellow')
                                    listOfTypes.append(str(valueType))

                            #if array we need to iterate to know the type
                            elif (str(keyType) == "__value__"):
                                if (isArray == True and type(valueType) == type(list())):
                                    isArray = False
                                    #list of dictionaries
                                    if not (bool(valueType)):
                                        cprint("The array is EMPTY for " + valueName, 'red')
                                        listOfTypes.append("string" + "[]")
                                   #we need to find the type array holds
                                    for i, arrayType in enumerate(d['__type__'] for d in valueType):
                                        if(str(arrayType) == "object"):
                                            cprint("Value type -> " + str(arrayType), 'yellow')
                                            listOfTypes.append(capitalizeFirst(valueName) + " []")
                                            isObject = True
                                            break
                                        else:
                                            cprint("Value type -> " + str(arrayType), 'yellow')
                                            if (str(arrayType) == "double"):
                                                cprint("Changing " + str(arrayType) + " to a float", 'yellow')
                                                listOfTypes.append("float []")
                                                break
                                            else:
                                                listOfTypes.append(str(arrayType) + "[]")
                                                break
                                    #if array has objects in it.
                                    if (isObject == True):
                                        isObject = False
                                        for i, arrayType in enumerate(d['__value__'] for d in valueType):
                                            objectValue = {valueName: arrayType}
                                            objDictionary.update(objectValue)
                                            break
                                #the object to get the value the object has.
                                elif(isObject == True and type(valueType) == type(dict())):
                                    #add to dictionary
                                    isObject = False
                                    objectValue = {valueName:valueType}
                                    objDictionary.update(objectValue)
                #else:
                   # isClass = False
                    #we should add these to ProjectSettings withouth needing to create a class
                   # print ("THIS SHOULDN'T BE A CLASS")



    cprint("The End. Created " + objName, 'red')
    if (isClass == True):
        createClass(objName)
        # clear list for other object
        del listOfTypes[:]
        del listOfNames[:]
        #print(objDictionary)
        if(bool(objDictionary)):
            for objType,objValue in objDictionary.items():
                print(str(objType))
                populateClass(objValue, str(objType))
            objDictionary.clear()

def createClass(className, isProjectSettings = False):
    if(className == ""):
        return
    file_loader = FileSystemLoader(templateFolder)
    env = Environment(loader=file_loader)

    if (isProjectSettings == False):
        list_of_dict = [{'name': L1, 'type': L2} for (L1, L2) in zip(listOfNames, listOfTypes)]
        cprint (list_of_dict, 'magenta')

        template = env.get_template('GS_template.cs')
        output = template.render(ClassName=capitalizeFirst(className), data_dictionary=list_of_dict)
        newDir = str(Path(gamesettings_folder + "/CustomGS/" + "/" + mainObjectName + "/"))
        ensure_dir(newDir)
        createFileWithInfo(newDir, capitalizeFirst(className) + '.cs', output)
    elif (isProjectSettings == True):
        list_of_dicti = [{'name': L1, 'type': L2} for (L1, L2) in zip(listOfMainClassNames, listOfMainClassTypes)]
        if (isPublishing == True):
            template = env.get_template('ProjectGameSettings_P_template .cs')
            output = template.render(data_dictionary=list_of_dicti)
            createFileWithInfo(gamesettings_folder, "ProjectGameSettings.cs",output)
           # gamesettings_folder
        elif(isInternal == True):
            template = env.get_template('ProjectGameSettings_IN_template.cs')
            output = template.render(data_dictionary=list_of_dicti)
            createFileWithInfo(gamesettings_folder, "ProjectGameSettings.cs", output)

def objectTagCheck(dictionary):
    if(type(dictionary) == type(dict())):
        for tags in dictionary:
            if (tags == "__tag__"):
                if (dictionary[tags] == "custom"):
                    return True
                else:
                    return False
    else:
        return False

def cleanJsonFile():
    with open(uglyGameSettings_json, 'r') as json_file:
        jsonPretty = json.dumps(json.load(json_file), sort_keys=True, indent=4)
        return json.loads(jsonPretty)

def deleteFilesInDir(f):
    d = os.path.dirname(f)
    if os.path.exists(d):
        shutil.rmtree(d)

if __name__ == "__main__":
    arg_parser = argparse.ArgumentParser(description='Build GameSettings mapping for Kwalee')
    arg_parser.add_argument('-c', '--config', help='Cofiguration for Publishing or Internal.', required=True, nargs='*')
    arg_parser.add_argument('-d', '--dest', help='Destination path.', required=True, nargs='*')
    args = arg_parser.parse_args()
    configuration = args.config[0]
    destination = args.dest[0]
    uglyGameSettings_json = destination

    if not (configuration.lower() in ('pub','publishing', 'int','internal')):
       print ("The config name was incorrect")
       print ("\nFor publishing games: ")
       print ("-c pub")
       print ("-c publishing")
       print (" ")
       print("For internal games: ")
       print("-c int")
       print("-c internal \n")
       sys.exit("Wrong configuration")
    elif(configuration == "pub" or configuration == "publishing"):
        isPublishing = True
        cprint("Chose Publishing", 'red')
    else:
        isInternal = True
        cprint("Chose Internal", 'red')

    gamesettings_json = cleanJsonFile()
    deleteFilesInDir(str(gamesettings_folder + "/CustomGS/"))
    if (bool(gamesettings_json)):
        for key, value in gamesettings_json.items():
            # if publishing
            if (isPublishing == True):
                if not (str(key) in listOfSettingsToIgnorPub):
                    mainObjectName = str(key)
                    new_dic = my_dictionary()
                    new_dic.add(key, value)
                    #we check if tag is correct
                    if (objectTagCheck(value) == True):
                        findObjects(new_dic, mainObjectName)
            elif(isInternal == True):
                if not (str(key) in listOfSettingsToIgnorInt):
                    mainObjectName = str(key)
                    new_dic = my_dictionary()
                    new_dic.add(key, value)
                    findObjects(new_dic, mainObjectName)
        # here add the parameters and names to Project Game Settings
        createClass("ProjectGameSettings", True)
        print('\x1b[0;30;42m' + "\nTHE PROCESS HAS FINISHED WITH SUCESS" + '\x1b[0m')
    else:
        print('\x1b[0;30;41m' + "\nTHE PROCESS HAS FAILED" + '\x1b[0m')
