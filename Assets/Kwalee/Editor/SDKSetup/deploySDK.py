#!/usr/bin/python

import argparse
import os
import sys
import shutil
import subprocess
from zipfile import ZipFile, ZipInfo
from distutils.dir_util import copy_tree
from pathlib import Path

allDllsDir = 'KwaleeDLLs'
kwaleeDir = 'Assets/Kwalee'
selfZipDir = kwaleeDir + '/Editor/SDKSetup/deploySDK.py'
dependenciesDir = kwaleeDir + '/Dependencies'
nativeiOSDir = kwaleeDir + '/Native/iOS'
kwaleePluginsDir = 'Assets/Plugins/Android/kwaleeplugins'
mopubDir = 'Assets/MoPub'
pythonDir = str(Path.home()) + '/virtual_env/kwalee/bin/python'


class ZipFileWithPermissions(ZipFile):
    def _extract_member(self, member, targetpath, pwd):
        if not isinstance(member, ZipInfo):
            member = self.getinfo(member)

        targetpath = super()._extract_member(member, targetpath, pwd)

        attr = member.external_attr >> 16
        if attr != 0:
            os.chmod(targetpath, attr)
        return targetpath


# Copies and replaces all the files from the folder
def copyAndPasteFiles(fromSrc, toDst):
    if not os.path.exists(toDst):
        os.makedirs(toDst)
        print ('Directory did not exist. Created a new one ' + toDst)
    copy_tree(fromSrc, toDst)
    print ('Copied the files to ' + toDst)


# Cleans up a whole directory
def cleanUp(directory):
    if os.path.exists(directory):
        shutil.rmtree(directory)
        print ('Deleted ' + directory)
      

if __name__ == '__main__':

    arg_parser = argparse.ArgumentParser(description='Deploy zipped Kwalee SDK to the project.')
    arg_parser.add_argument('-z', '--zipdir', help='Kwalee SDK zip directory.', required=True, nargs='*')
    arg_parser.add_argument('-d', '--destination', help='Project directory to deploy.', required=False, nargs='*')
    arg_parser.add_argument('-u', '--updateSelf', help='Update script and then deploy.', required=False, nargs='*')
    args = arg_parser.parse_args()
    
    zipDir = args.zipdir[0]
    # Remove .zip extension
    zipOutputDir = zipDir[:-4]
    
    outputDir = args.destination[0] if args.destination is not None else os.getcwd()
    updateSelf = args.updateSelf[0] if args.updateSelf is not None else False
    
#UPDATE SELF
    if updateSelf:
        with ZipFile(zipDir) as z:
            scriptDir = os.path.dirname(os.path.realpath(__file__)) + '/' + os.path.basename(__file__)
            with open(scriptDir, 'wb') as f:
                f.write(z.read(selfZipDir))
        print('Updated self.')
        compileCommand = pythonDir + ' ' + scriptDir + ' -z "' + zipDir + '" -d ' + outputDir
        compileProcess = subprocess.Popen(compileCommand, shell=True)
        compileProcess.wait()
    else:
        # UNZIP SDK
        cleanUp(zipOutputDir)
        with ZipFileWithPermissions(zipDir, 'r') as zip_ref:
            zip_ref.extractall(zipOutputDir)

        # CLEAN UP EXISTING FILES
        cleanUp(allDllsDir)
        cleanUp(dependenciesDir)
        cleanUp(nativeiOSDir)
        cleanUp(kwaleePluginsDir)
        cleanUp(mopubDir)

        # COPY AND PASTE ALL FILES
        copyAndPasteFiles(zipOutputDir, outputDir)
    
        # DELETE UNZIPPED FOLDER
        cleanUp(zipOutputDir)
    
        print('Imported successfully!')
