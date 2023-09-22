#!/bin/bash

./build-html5.sh noroominspace

if [ $? -ne 0 ]
then
    exit 1
fi

./build-windows.sh noroominspace

if [ $? -ne 0 ]
then
     exit 1
fi

#./build-macos.sh $1

./build-linux.sh noroominspace

if [ $? -ne 0 ]
then
     exit 1
fi