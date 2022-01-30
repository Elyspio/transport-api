# Building back
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS builder-back
WORKDIR /app

COPY back/TransportApi.sln ./TransportApi.sln
COPY back/Core/Core.csproj ./Core/Core.csproj
COPY back/Db/Db.csproj ./Db/Db.csproj
COPY back/Adapters/Adapters.csproj ./Adapters/Adapters.csproj
COPY back/Web/Web.csproj ./Web/Web.csproj

RUN dotnet restore

COPY back .
RUN dotnet publish -c Release -o out


# Building front
FROM --platform=linux/amd64 node:16 as builder-front

COPY front/package.json /front/package.json
COPY front/package-lock.json /front/package-lock.json
RUN cd /front && npm install

COPY front/tsconfig.json /front/tsconfig.json
COPY front/public /front/public
COPY front/src /front/src
RUN cd /front && npm run build


# Running
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine

WORKDIR /back
COPY --from=builder-back /app/out .

COPY --from=builder-front /front/build /back/wwwroot
ENV FRONT_PATH /back/wwwroot

EXPOSE 4003
ENTRYPOINT ["dotnet", "Web.dll"]
