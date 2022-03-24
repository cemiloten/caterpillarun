#!/bin/sh
mkdir -p ../Assets/Kwalee/Unity/
mkdir -p ../Assets/Kwalee/iOS/
mkdir -p ../Assets/Kwalee/Android/
mkdir -p ../Assets/Kwalee/Standalone/


cp live-debug/KWCoreConfig.dll ../Assets/Kwalee/KWCoreConfig.dll
cp live-debug/Unity/KWCore.dll ../Assets/Kwalee/Unity/KWCore.dll
cp live-debug/iOS/KWCore.dll ../Assets/Kwalee/iOS/KWCore.dll
cp live-debug/Android/KWCore.dll ../Assets/Kwalee/Android/KWCore.dll
cp live-debug/Editor/KWCoreEditor.dll ../Assets/Kwalee/Editor/KWCoreEditor.dll
cp live-debug/Standalone/KWCore.dll ../Assets/Kwalee/Standalone/KWCore.dll

cd ../KWTools/Scripts
./EditDefineSymbols.py -a KWALEE_DEBUG
./EditDefineSymbols.py -r KWALEE_LUNA_REPLAY
