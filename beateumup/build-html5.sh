export PROJECT_PATH=$(wslpath -w $(pwd))
export GB_GAME_NAME="Beatemup"
export GB_GAME_BUILD_PATH="builds/html5/"
export GB_GAME_BUILD_LOG_PATH="builds/html5.txt"
export WSLENV=$WSLENV:GB_GAME_BUILD_PATH/w
export BUILD_COMMAND="${GB_UNITY_EDITOR_PATH} -projectPath ${PROJECT_PATH} -quit -silent-crashes -batchmode -nographics -logFile ${GB_GAME_BUILD_LOG_PATH} -executeMethod Beatemup.Editor.BuildScript.BuildWebGL"
echo "${BUILD_COMMAND}"
${BUILD_COMMAND}