#!/bin/bash

./build-all.sh nekosama2

if [ $? -eq 0 ]
then 
    echo "Uploading builds to itch"
    ./upload-itchio-all.sh nekosama2
else
    echo "Some of the builds failed, not uploading."
fi