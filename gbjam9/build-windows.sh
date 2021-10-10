GB_GAME_NAME="Nekosama"

BUILD_COMMAND="${UNITY_EDITOR_PATH} -buildWindows64Player builds/windows/${GB_GAME_NAME}.exe -buildTarget Win64 -forgetProjectPath -quit -silent-crashes -batchmode -nographics"

echo ${BUILD_COMMAND}
${BUILD_COMMAND}