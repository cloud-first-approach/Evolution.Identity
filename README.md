# Identity Api

Api to manage user for login.

On he basic of the concept of Microservices we have broken down idenity to be a speprate service which can be indivisually scaled as required and deployed to k8s.

The service exposes swagger.

# Development

## Making changes in database Models

```sh
dotnet ef migrations add InitialCreate
dotnet ef database update InitialCreate
```


## SQL Server local

```sh
docker pull mcr.microsoft.com/mssql/server
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=password@1" -p 1433:1433 --name sql -d mcr.microsoft.com/mssql/server:2017-latest

```

## Run using dapr
```sh
dapr run --app-port 1000 --app-id processor --app-protocol http --dapr-http-port 1501 --components-path ../../dapr/components -- dotnet run
```

# Production

## Run in K8s
```sh
#spin up the infra
#from https://github.com/cloud-first-approach/Evolution.infra/tree/main/deploy/k8s/infra/overlays/dev
kubectl apply -k deploy/k8s/infra/overlays/dev

#deploy the service
kubectl apply -k ./deploy/k8s/identity/overlays/dev -n evolution

#check port-forward 
kubectl port-forward svc/identity-api-cluster-ip 80 -n evolution

```

## Deleteing the resources

```sh

#delete the service
kubectl delete -k deploy/k8s/identity/overlays/dev

#delete the infra
kubectl delete -k deploy/k8s/infra/overlays/dev

```
