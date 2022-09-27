# Introduction 
TODO: Give a short introduction of your project. Let this section explain the objectives or the motivation behind this project. 

# Getting Started
TODO: Guide users through getting your code up and running on their own system. In this section you can talk about:
1.	Installation process
2.	Software dependencies
3.	Latest releases
4.	API references


## Installation


### Create Cluster
```sh

> kind create cluster

> dapr init -k

# Setup Redis
> helm repo add bitnami https://charts.bitnami.com/bitnami
> helm repo update
> helm install redis bitnami/redis --set image.tag=6.2

#Redis&reg; can be accessed on the following DNS names from within your cluster:
#
#    redis-master.default.svc.cluster.local for read/write operations (port 6379)
#    redis-replicas.default.svc.cluster.local for read-only operations (port 6379)
#
#
#
#To get your password run:
#
#    export REDIS_PASSWORD=$(kubectl get secret --namespace default redis -o jsonpath="{.data.redis-password}" | base64 -d)
#
#To connect to your Redis&reg; server:
#
#1. Run a Redis&reg; pod that you can use as a client:
#
#   kubectl run --namespace default redis-client --restart='Never'  --env REDIS_PASSWORD=$REDIS_PASSWORD  --image docker.io/bitnami/redis:6.2 --command -- sleep #infinity
#
#   Use the following command to attach to the pod:
#
#   kubectl exec --tty -i redis-client \
#   --namespace default -- bash
#
#2. Connect using the Redis&reg; CLI:
#   REDISCLI_AUTH="$REDIS_PASSWORD" redis-cli -h redis-master
#   REDISCLI_AUTH="$REDIS_PASSWORD" redis-cli -h redis-replicas
#
#To connect to your database from outside the cluster execute the following commands:
#
#    kubectl port-forward --namespace default svc/redis-master 6379:6379 &
#    REDISCLI_AUTH="$REDIS_PASSWORD" redis-cli -h 127.0.0.1 -p 6379
#WARNING: Rolling tag detected (bitnami/redis:6.2), please note that it is strongly recommended to avoid using rolling tags in a production environment.
#+info https://docs.bitnami.com/containers/how-to/understand-rolling-tags-containers/
#
# Apply StateStore

#Setup mssql
# Secret for sqlServer
 kubectl create secret generic mssql --from-literal=SA_PASSWORD="password@1" -n evolution

 kubectl apply -f .\Deploy\K8s\local-pvc.yaml

# Start the SQL Server for Services to Use.
kubectl apply -f Deploy/K8S/mssql

kubectl apply -f .\deploy\dapr\redis-component.yml

kubectl create ns evolution

kubectl apply -f .\deploy\k8s\

kubectl port-forward svc/identityservice-api-cluster-ip 80 -n evolution

kubectl get pods



```


```sh
dotnet ef migrations add InitialCreate
dotnet ef database update InitialCreate


docker pull mcr.microsoft.com/mssql/server
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=password@1" -p 1433:1433 --name sql -d mcr.microsoft.com/mssql/server:2017-latest

dapr run --app-id identity --app-port 1000 -- dotnet run
dapr run --app-id identity --app-port 1000 --components-path ../../deploy/dapr/ -- dotnet run

```