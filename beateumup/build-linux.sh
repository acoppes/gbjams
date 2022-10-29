GB_GAME_NAME="Beatemup"
export GB_GAME_BUILD_LOG_PATH="builds/linux.txt"

BUILD_COMMAND="${GB_UNITY_EDITOR_PATH} -projectPath . -buildLinux64Player builds/linux/${GB_GAME_NAME} -logFile ${GB_GAME_BUILD_LOG_PATH} -buildTarget Linux64 -forgetProjectPath -quit -silent-crashes -batchmode -nographics"

echo ${BUILD_COMMAND}
${BUILD_COMMAND}