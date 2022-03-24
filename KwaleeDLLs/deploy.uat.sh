#!/bin/sh
mkdir -p ../Assets/Kwalee/Unity/
mkdir -p ../Assets/Kwalee/iOS/
mkdir -p ../Assets/Kwalee/Android/
mkdir -p ../Assets/Kwalee/Standalone/
mkdir -p ../Assets/Kwalee/Editor/


cp uat/KWCoreConfig.dll ../Assets/Kwalee/KWCoreConfig.dll
cp uat/Unity/KWCore.dll ../Assets/Kwalee/Unity/KWCore.dll
cp uat/iOS/KWCore.dll ../Assets/Kwalee/iOS/KWCore.dll
cp uat/Standalone/KWCore.dll ../Assets/Kwalee/Standalone/KWCore.dll
cp uat/Android/KWCore.dll ../Assets/Kwalee/Android/KWCore.dll
cp uat/Editor/KWCoreEditor.dll ../Assets/Kwalee/Editor/KWCoreEditor.dll

cd ../KWTools/Scripts
./EditDefineSymbols.py -a KWALEE_DEBUG
./EditDefineSymbols.py -r KWALEE_LUNA_REPLAY
