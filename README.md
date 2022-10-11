# Introduction 
Identity Api


## Local Development if required after changes

```sh
dotnet ef migrations add InitialCreate
dotnet ef database update InitialCreate
```


# Run SQL Server locally using docker

```sh
docker pull mcr.microsoft.com/mssql/server
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=password@1" -p 1433:1433 --name sql -d mcr.microsoft.com/mssql/server:2017-latest

```

# Run locally using dapr
```sh
dapr run --app-port 1000 --app-id processor --app-protocol http --dapr-http-port 1501 --components-path ../../dapr/components -- dotnet run
```

# Run in K8s
```sh
#spin up the infra
#from https://github.com/cloud-first-approach/Evolution.infra/tree/main/deploy/k8s/infra/overlays/dev
kubectl apply -k deploy/k8s/infra/overlays/dev

#deploy the service
kubectl apply -f deploy/k8s/services

#check port-forward 
kubectl port-forward svc/identityservice-api-cluster-ip 80 -n evolution


#deploy the service
kubectl delete -f deploy/k8s/services

#deploy the service
kubectl delete -k deploy/k8s/infra/overlays/dev

#deploy the service
helm uninstall redis

```
---
---
## More Info on redis setup

Redis&reg; can be accessed on the following DNS names from within your cluster:

    redis-master.default.svc.cluster.local for read/write operations (port 6379)
    redis-replicas.default.svc.cluster.local for read-only operations (port 6379)



To get your password run:

    export REDIS_PASSWORD=$(kubectl get secret --namespace default redis -o jsonpath="{.data.redis-password}" | base64 -d)

To connect to your Redis&reg; server:

1. Run a Redis&reg; pod that you can use as a client:

```sh
   kubectl run --namespace default redis-client --restart='Never'  --env REDIS_PASSWORD=$REDIS_PASSWORD  --image docker.io/bitnami/redis:6.2 --command -- leep #infinity
```
   Use the following command to attach to the pod:

```sh
   kubectl exec --tty -i redis-client \
   --namespace default -- bash
```
2. Connect using the Redis&reg; CLI:

```sh
   REDISCLI_AUTH="$REDIS_PASSWORD" redis-cli -h redis-master
   REDISCLI_AUTH="$REDIS_PASSWORD" redis-cli -h redis-replicas
```
To connect to your database from outside the cluster execute the following commands:
    
    ```sh
    kubectl port-forward --namespace default svc/redis-master 6379:6379 &
    REDISCLI_AUTH="$REDIS_PASSWORD" redis-cli -h 127.0.0.1 -p 6379
    ```
WARNING: Rolling tag detected (bitnami/redis:6.2), please note that it is strongly recommended to avoid using rolling tags in a production environment.
+info https://docs.bitnami.com/containers/how-to/understand-rolling-tags-containers/



