GB_GAME_NAME="SeedcityChasers"

BUILD_COMMAND="${GB_UNITY_EDITOR_PATH} -projectPath . -buildLinux64Player builds/linux/${GB_GAME_NAME} -buildTarget Linux64 -forgetProjectPath -quit -silent-crashes -batchmode -nographics"

echo ${BUILD_COMMAND}
${BUILD_COMMAND}