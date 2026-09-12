#!/bin/bash

./build-all.sh

if [ $? -eq 0 ]
then 
    echo "Uploading builds to itch"
    ./upload-itchio-all.sh
else
    echo "Some of the builds failed, not uploading."
fi