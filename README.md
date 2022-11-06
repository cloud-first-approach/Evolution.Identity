# Identity Api

One of the basic concept of Microservices we have broken down idenity to be a speprate service which can be indivisually scaled as required and deployed to k8s.

Identity Api uses Identity Server4 to maintain login and generate token.

## Key Notes
- The service follows the `Open Api Spec` and `REST` standards.
- The service is configured to run using `kestrel` server on port `1000` 
- The service exposes a health check at `/health` and `/healthz` endpoint.
- The service exposes a swagger endpoint for `/swagger` only in `Development` env.
- The service exposes a metric endpoint `/metricstext` for text based and `/metrics` for protobuf in `prometheus` format.
- The service exposes the `.well-known` endpoint using `Identity Server4`
- The service uses `dapr components`
- The service uses `sql` for Identity based date


# Environment Setup

## .NET Commands

### Making changes in database Models

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update InitialCreate
```

### Run SQL Server local

```bash
docker pull mcr.microsoft.com/mssql/server
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=password@1" -p 1433:1433 --name sql -d mcr.microsoft.com/mssql/server:2017-latest

```

## Run using dapr
```bash
dapr run --app-port 1000 --app-id processor --app-protocol http --dapr-http-port 1501 --components-path ../../dapr/components -- dotnet run
```

# Kubernetes

## Run in K8s
```bash
#spin up the infra
kubectl apply -k Evolution.infra/deploy/k8s/infra/overlays/dev -n evolution

#deploy the service
kubectl apply -k Evolution.Identity/deploy/k8s/identity/overlays/dev -n evolution

#check port-forward 
kubectl port-forward svc/identity-api-cluster-ip 80 -n evolution

```

## Deleteing the resources

```sh

#delete the service
kubectl delete -k Evolution.Identity/deploy/k8s/identity/overlays/dev -n evolution

#delete the infra
kubectl delete -k Evolution.infra/deploy/k8s/infra/overlays/dev -n evolution

```
