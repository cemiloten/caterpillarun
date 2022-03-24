#!/bin/sh
mkdir -p ../Assets/Kwalee/Unity/
mkdir -p ../Assets/Kwalee/iOS/
mkdir -p ../Assets/Kwalee/Android/
mkdir -p ../Assets/Kwalee/Standalone/
mkdir -p ../Assets/Kwalee/Editor/


cp luna/KWCoreConfig.dll ../Assets/Kwalee/KWCoreConfig.dll
cp luna/Unity/KWCore.dll ../Assets/Kwalee/Unity/KWCore.dll
cp luna/iOS/KWCore.dll ../Assets/Kwalee/iOS/KWCore.dll
cp luna/Standalone/KWCore.dll ../Assets/Kwalee/Standalone/KWCore.dll
cp luna/Android/KWCore.dll ../Assets/Kwalee/Android/KWCore.dll
cp luna/Editor/KWCoreEditor.dll ../Assets/Kwalee/Editor/KWCoreEditor.dll

cd ../KWTools/Scripts
./EditDefineSymbols.py -a KWALEE_DEBUG
./EditDefineSymbols.py -a KWALEE_LUNA_REPLAY
