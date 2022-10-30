GB_GAME_NAME="Beatemup"
GB_GAME_BUILD_LOG_PATH="builds/windows.txt"
WSLENV=$WSLENV:GB_GAME_BUILD_PATH/w
BUILD_COMMAND="${GB_UNITY_EDITOR_PATH} -projectPath . -buildWindows64Player builds/windows/${GB_GAME_NAME}.exe -logFile ${GB_GAME_BUILD_LOG_PATH} -buildTarget Win64 -forgetProjectPath -quit -silent-crashes -batchmode -nographics"
echo "${BUILD_COMMAND}"
${BUILD_COMMAND}