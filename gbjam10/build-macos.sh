GB_GAME_NAME="GBJAM10"

BUILD_COMMAND="${GB_UNITY_EDITOR_PATH} -buildOSXUniversalPlayer builds/macos/${GB_GAME_NAME}.app -buildTarget OSXUniversal -forgetProjectPath -quit -silent-crashes -batchmode -nographics"

echo ${BUILD_COMMAND}
${BUILD_COMMAND}