#!/bin/bash

export PROJECT_PATH=$(pwd)

PROJECT_NAME=gbjam12
export GEMSERK_PROJECT_NAME=${PROJECT_NAME,,}
export GEMSERK_GAME_BUILD_PATH="builds/${GEMSERK_PROJECT_NAME}/macos/${GEMSERK_PROJECT_NAME}.app"
export GEMSERK_GAME_BUILD_LOG_PATH="builds/${GEMSERK_PROJECT_NAME}/macos.txt"
# export WSLENV=$WSLENV:GEMSERK_GAME_BUILD_PATH:GEMSERK_PROJECT_NAME/w

export BUILD_COMMAND="${GEMSERK_UNITY_EDITOR_PATH} -projectPath ${PROJECT_PATH} -quit -silent-crashes -batchmode -nographics -logFile ${GEMSERK_GAME_BUILD_LOG_PATH} -executeMethod Gemserk.BuildTools.Editor.BuildScript.BuildMacos"

echo "${BUILD_COMMAND}"
${BUILD_COMMAND}