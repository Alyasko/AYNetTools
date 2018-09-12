#!/bin/bash
echo -e "\n--- Kill process that listens specified port ---\n"

read -p "Enter port number which should be freed: " port

echo "List of processes that listen to $port port"
pids=$(lsof -PiTCP -sTCP:LISTEN | grep -i ":$port")

if [ -z "$pids" ]
then
      echo "No processes";
      exit;
fi

echo -e "\n$pids\n"

while true; do
    read -e -p "Kill proccesses? [y/n] " yn
    case $yn in
        [Yy]* ) kill $(echo $pids | awk -F' ' '{ print $2 }'); echo 'Killed'; break;;
        [Nn]* ) exit;;
        * ) echo "Please answer yes or no.";;
    esac
done