FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source
COPY . .
RUN dotnet restore

WORKDIR /source
RUN dotnet publish -c release -o /stage --no-restore

FROM mcr.microsoft.com/dotnet/runtime:6.0
ENV ECHO_SERVER_MAX_SOCKET_COUNT=500
ENV ECHO_SERVER_MAX_THREAD_COUNT=50
ENV ECHO_SERVER_PORT=11111
ENV ECHO_SERVER_SOCKET_BUFFER_SIZE=512
ENV ECHO_SERVER_POLLING_INTERVAL_MS=32
EXPOSE ${ECHO_SERVER_PORT}
WORKDIR /app
COPY --from=build /stage ./
ENTRYPOINT ["dotnet", "AsioEchoServer.dll"]