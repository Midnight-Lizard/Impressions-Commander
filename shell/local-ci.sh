#!/bin/sh
set -e
TAG=$(date +"%Y-%m-%d--%H-%M-%S")
PROJ=impressions-commander
REGISTRY=localhost:5000
IMAGE=$REGISTRY/$PROJ:$TAG
eval $(docker-machine env default --shell bash)
docker build -t $IMAGE \
    --build-arg DOTNET_CONFIG=Build \
    --build-arg INSTALL_CLRDBG="apt-get update \
        && apt-get install -y --no-install-recommends unzip \
        && rm -rf /var/lib/apt/lists/* \
        && curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l /vsdbg" \
    ../
kubectl config use-context minikube
docker push $IMAGE
./helm-deploy.sh -i $IMAGE -r $PROJ -c ../kube/$PROJ \
    --set env.ASPNETCORE_ENVIRONMENT=Development \
    --set env.IDENTITY_URL=http://192.168.1.44:32006/ \
    --set env.ALLOWED_ORIGINS=http://192.168.1.44:31565 \
