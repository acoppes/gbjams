#!/bin/bash

./build-html5.sh nekosama2

if [ $? -ne 0 ]
then
    exit 1
fi

./build-windows.sh nekosama2

if [ $? -ne 0 ]
then
     exit 1
fi

#./build-macos.sh $1

./build-linux.sh nekosama2

if [ $? -ne 0 ]
then
     exit 1
fi