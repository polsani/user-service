# User service

To run this application start having docker installed.

After that, build the docker image:

```shell
docker build -t user_service .
```

To run this application execute:

```shell
docker run -d -p 14666:14666 user_service
```

## All functions can be accessed by **SWAGGER**

http://localhost:{port}/swagger/index.html