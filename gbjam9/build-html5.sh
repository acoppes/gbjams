export GB_GAME_NAME="Nekosama"
export GB_GAME_BUILD_PATH="builds/html/"

BUILD_COMMAND="${UNITY_EDITOR_PATH} -forgetProjectPath -quit -silent-crashes -batchmode -nographics -executeMethod GBJAM9.Editor.BuildScript.BuildWebGL"

echo ${BUILD_COMMAND}
${BUILD_COMMAND}