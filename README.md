# User service

To run this application start having docker installed.
Change the appsettings variables to valid values

[CONNECTION STRING]

[AMQP HOST]

After that, build the docker image:

```shell
docker build -t user_service .
```

To run this application execute:

```shell
docker run -d -p 14666:14666 user_service
```

To run more then one container just change de host port

```shell
docker run -d -p 14667:14666 user_service
docker run -d -p 14668:14666 user_service
docker run -d -p 14669:14666 user_service
docker run -d -p 14670:14666 user_service
```

## All functions can be accessed by **SWAGGER**

http://localhost:{port}/swagger/index.html

## Next steps

* Add more tests
* Change previous import to persist in Redis database
* Improve Swagger documentation 
* Deploy on AWS to decrease latency