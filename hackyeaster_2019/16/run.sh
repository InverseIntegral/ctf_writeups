sudo docker run --name=mysql1 -v /home/hacker/Downloads:/input \
  -p 3306:3306 --env="MYSQL_ROOT_PASSWORD=password" \
  --env "MYSQL_ROOT_HOST=172.17.0.1" \
  --env="MYSQL_DATABASE=everything" \
  -d mysql/mysql-server:latest
