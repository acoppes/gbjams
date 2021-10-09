GB_GAME_NAME="Nekosama"

# TODO: install u3d in windows?
BUILD_COMMAND="u3d run -r -- -buildOSXUniversalPlayer builds/macos/${GB_GAME_NAME}.app -buildTarget OSXUniversal -forgetProjectPath -quit -silent-crashes -batchmode -nographics"

echo ${BUILD_COMMAND}
${BUILD_COMMAND}