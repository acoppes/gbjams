GB_GAME_NAME="Nekosama"

# TODO: install u3d in windows?
BUILD_COMMAND="u3d run -r -- -buildWindows64Player builds/windows/${GB_GAME_NAME}.exe -buildTarget Win64 -forgetProjectPath -quit -silent-crashes -batchmode -nographics"

echo ${BUILD_COMMAND}
${BUILD_COMMAND}