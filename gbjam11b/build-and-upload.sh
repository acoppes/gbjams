#!/bin/bash

./build-all.sh noroominspace

if [ $? -eq 0 ]
then 
    echo "Uploading builds to itch"
    ./upload-itchio-all.sh noroominspace
else
    echo "Some of the builds failed, not uploading."
fi