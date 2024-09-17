#!/bin/bash

./build-html5.sh gbjam12

if [ $? -ne 0 ]
then
    exit 1
fi

./build-windows.sh gbjam12

if [ $? -ne 0 ]
then
     exit 1
fi

#./build-macos.sh $1

./build-linux.sh gbjam12

if [ $? -ne 0 ]
then
     exit 1
fi