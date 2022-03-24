#!/bin/sh
mkdir -p ../Assets/Kwalee/Unity/
mkdir -p ../Assets/Kwalee/iOS/
mkdir -p ../Assets/Kwalee/Android/
mkdir -p ../Assets/Kwalee/Standalone/


cp live/KWCoreConfig.dll ../Assets/Kwalee/KWCoreConfig.dll
cp live/Unity/KWCore.dll ../Assets/Kwalee/Unity/KWCore.dll
cp live/iOS/KWCore.dll ../Assets/Kwalee/iOS/KWCore.dll
cp live/Android/KWCore.dll ../Assets/Kwalee/Android/KWCore.dll
cp live/Standalone/KWCore.dll ../Assets/Kwalee/Standalone/KWCore.dll
cp live/Editor/KWCoreEditor.dll ../Assets/Kwalee/Editor/KWCoreEditor.dll

cd ../KWTools/Scripts
./EditDefineSymbols.py -r KWALEE_DEBUG
./EditDefineSymbols.py -r KWALEE_LUNA_REPLAY
