FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source
COPY . .
RUN dotnet restore

WORKDIR /source
RUN ls -la
RUN dotnet publish -c release -o /stage --no-restore

FROM mcr.microsoft.com/dotnet/runtime:6.0
ENV ECHO_SERVER_PORT=11111
EXPOSE ${ECHO_SERVER_PORT} 
WORKDIR /app
COPY --from=build /stage ./
ENTRYPOINT ["dotnet", "AsioEchoServer.dll"]