#!/bin/bash
if [ $# -gt 2 -o $# -lt 1 ];
then
	echo "usage: $0 num_of_players [configuration_file]";
	exit 1;
fi
if [ "2" == $# ];
then
	config=$2;
else
	config="Agent/Configuration/DefaultRedConfig.txt";
fi
if ! [ -e $config ];
then
	echo "cant find file `realpath $config`";
	exit 1;
fi
config=`realpath $config`;
echo "using config at $config"

x=$1
while [ $x -gt 0 ];
do
	echo "Launching agent $x"
	dotnet run --project Agent "$config" &
	x=$(($x-1));
	sleep 1;
done
read -n 1

