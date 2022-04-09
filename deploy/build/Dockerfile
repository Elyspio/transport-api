# Building back
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS builder-back
WORKDIR /app

COPY back/TransportApi.sln ./TransportApi.sln
COPY back/App/Core/Transport.Api.Core.csproj ./App/Core/
COPY back/App/Adapters/Transport.Api.Adapters.csproj ./App/Adapters/
COPY back/App/Abstractions/Transport.Api.Abstractions.csproj ./App/Abstractions/
COPY back/App/Db/Transport.Api.Db.csproj ./App/Db/
COPY back/App/Web/Transport.Api.Web.csproj ./App/Web/

COPY back/Tools/Db.Update/Transport.Api.Db.Update.csproj ./Tools/Db.Update/


RUN dotnet restore

COPY back .
RUN dotnet publish -c Release -o out


# Building front
FROM --platform=linux/amd64 node:16 as builder-front

COPY front/package.json /front/package.json
COPY front/package-lock.json /front/package-lock.json
RUN cd /front && npm install --legacy-peer-deps

COPY front/tsconfig.json /front/
COPY front/craco.config.js /front/
COPY front/public /front/public
COPY front/src /front/src
RUN cd /front && npm run build


# Running
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine

WORKDIR /back
COPY --from=builder-back /app/out .

COPY --from=builder-front /front/build /back/wwwroot
ENV FRONT_PATH /back/wwwroot

EXPOSE 4000
ENTRYPOINT ["dotnet", "Transport.Api.Web.dll"]

