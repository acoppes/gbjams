GB_GAME_NAME="GBJAM10"

BUILD_COMMAND="${GB_UNITY_EDITOR_PATH} -buildLinux64Player builds/linux/${GB_GAME_NAME} -buildTarget Linux64 -forgetProjectPath -quit -silent-crashes -batchmode -nographics"

echo ${BUILD_COMMAND}
${BUILD_COMMAND}