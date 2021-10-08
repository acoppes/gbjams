# UNITY_EXECUTABLE_PATH="/mnt/c/UnityHub/2020.2.0b2/Editor/Unity.exe"

GB_GAME_NAME="Nekosama"

# -logfile Client/Logs/build-client-windows.log
BUILD_COMMAND="u3d run -r -- -buildWindows64Player builds/windows/${GB_GAME_NAME}.exe -buildTarget Win64 -forgetProjectPath -quit -silent-crashes -batchmode -nographics"

echo ${BUILD_COMMAND}
${BUILD_COMMAND}