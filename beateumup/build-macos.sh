export PROJECT_PATH=$(wslpath -w $(pwd))
export GB_GAME_NAME="Beatemup"
export GB_GAME_BUILD_LOG_PATH="builds/macos.txt"
export WSLENV=$WSLENV:GB_GAME_BUILD_PATH/w
export BUILD_COMMAND="${GB_UNITY_EDITOR_PATH} -projectPath ${PROJECT_PATH} -buildOSXUniversalPlayer builds/macos/${GB_GAME_NAME}.app -logFile ${GB_GAME_BUILD_LOG_PATH} -buildTarget OSXUniversal -forgetProjectPath -quit -silent-crashes -batchmode -nographics"
echo "${BUILD_COMMAND}"
${BUILD_COMMAND}