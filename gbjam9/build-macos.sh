GB_GAME_NAME="Nekosama"

BUILD_COMMAND="${UNITY_EDITOR_PATH} -buildOSXUniversalPlayer builds/macos/${GB_GAME_NAME}.app -buildTarget OSXUniversal -forgetProjectPath -quit -silent-crashes -batchmode -nographics"

echo ${BUILD_COMMAND}
${BUILD_COMMAND}