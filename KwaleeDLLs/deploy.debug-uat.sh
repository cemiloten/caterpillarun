#!/bin/sh
mkdir -p ../Assets/Kwalee/Unity/
mkdir -p ../Assets/Kwalee/iOS/
mkdir -p ../Assets/Kwalee/Android/
mkdir -p ../Assets/Kwalee/Standalone/

cp debug-uat/KWCoreConfig.dll ../Assets/Kwalee/KWCoreConfig.dll
cp debug-uat/Unity/KWCore.dll ../Assets/Kwalee/Unity/KWCore.dll
cp debug-uat/iOS/KWCore.dll ../Assets/Kwalee/iOS/KWCore.dll
cp debug-uat/Android/KWCore.dll ../Assets/Kwalee/Android/KWCore.dll
cp debug-uat/Editor/KWCoreEditor.dll ../Assets/Kwalee/Editor/KWCoreEditor.dll
cp debug-uat/Standalone/KWCore.dll ../Assets/Kwalee/Standalone/KWCore.dll

cd ../KWTools/Scripts
./EditDefineSymbols.py -a KWALEE_DEBUG
./EditDefineSymbols.py -r KWALEE_LUNA_REPLAY
