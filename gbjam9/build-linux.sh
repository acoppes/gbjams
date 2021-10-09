GB_GAME_NAME="Nekosama"

BUILD_COMMAND="u3d run -r -- -buildLinux64Player builds/linux/${GB_GAME_NAME} -buildTarget Linux64 -forgetProjectPath -quit -silent-crashes -batchmode -nographics"

echo ${BUILD_COMMAND}
${BUILD_COMMAND}