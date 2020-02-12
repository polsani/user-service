FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
WORKDIR /app

COPY src/UserService/*.csproj ./
RUN dotnet restore

COPY src/UserService/ ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
RUN apt-get update 
RUN apt-get install -y jq curl
RUN rm /etc/localtime \
    && cp /usr/share/zoneinfo/America/Sao_Paulo /etc/localtime \
    && echo "America/Sao_Paulo" > /etc/timezone

COPY --from=build-env /app/out .
EXPOSE 14666
ENTRYPOINT ["dotnet", "UserService.dll"]