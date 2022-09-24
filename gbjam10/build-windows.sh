GB_GAME_NAME="GBJAM10"

BUILD_COMMAND="${GB_UNITY_EDITOR_PATH} -projectPath . -buildWindows64Player builds/windows/${GB_GAME_NAME}.exe -buildTarget Win64 -forgetProjectPath -quit -silent-crashes -batchmode -nographics"

echo ${BUILD_COMMAND}
${BUILD_COMMAND}