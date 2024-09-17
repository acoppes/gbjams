#!/bin/bash

./build-all.sh gbjam12

if [ $? -eq 0 ]
then 
    echo "Uploading builds to itch"
    ./upload-itchio-all.sh gbjam12
else
    echo "Some of the builds failed, not uploading."
fi