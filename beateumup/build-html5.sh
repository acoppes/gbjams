GB_GAME_NAME="Beatemup"
GB_GAME_BUILD_PATH="builds/html5/"
GB_GAME_BUILD_LOG_PATH="builds/html5.txt"
WSLENV=$WSLENV:GB_GAME_BUILD_PATH/w
BUILD_COMMAND="${GB_UNITY_EDITOR_PATH} -projectPath . -quit -silent-crashes -batchmode -nographics -logFile ${GB_GAME_BUILD_LOG_PATH} -executeMethod Beatemup.Editor.BuildScript.BuildWebGL"
echo "${BUILD_COMMAND}"
${BUILD_COMMAND}