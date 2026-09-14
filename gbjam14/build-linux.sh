#!/bin/bash

export PROJECT_PATH=$(pwd)

PROJECT_NAME=gbjam14
export GEMSERK_PROJECT_NAME=${PROJECT_NAME,,}
export GEMSERK_GAME_BUILD_PATH="builds/${GEMSERK_PROJECT_NAME}/linux/${GEMSERK_PROJECT_NAME}"
export GEMSERK_GAME_BUILD_LOG_PATH="builds/${GEMSERK_PROJECT_NAME}/linux.txt"
# export WSLENV=$WSLENV:GEMSERK_GAME_BUILD_PATH:GEMSERK_PROJECT_NAME/w

export BUILD_COMMAND="${GEMSERK_UNITY_EDITOR_PATH_6003} -projectPath ${PROJECT_PATH} -buildTarget linux64 -quit -silent-crashes -batchmode -nographics -logFile ${GEMSERK_GAME_BUILD_LOG_PATH} -executeMethod Gemserk.BuildTools.Editor.BuildScript.BuildLinux"

echo "${BUILD_COMMAND}"
${BUILD_COMMAND}