# dotnet-microservices

docker build -t iwaldman/platformservice .

docker run -p 8080:80 -d iwaldman/platformservice

docker ps

docker stop <container id>
docker stop <container id>

docker push iwaldman/platformservice

kubectl apply -f platforms-depl.yaml

kubectl get deployments
kubectl get pods
kubectl delete deployment platforms-depl

kubectl apply -f platforms-np-srv.yaml

kubectl get services
