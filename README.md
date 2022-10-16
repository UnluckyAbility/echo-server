# Tcp echo server
Based on TcpListener class
# Issues
1 ms delay upon reach a resource constraint, rps mb better 

One-by-one task completion handling leads to an increased load on the GC.

Fixed maximum response size.

Obvious architectural issues.

# Docker container environment variables
ECHO_SERVER_MAX_SOCKET_COUNT - maximum pending sockets. Defaults to 500 sockets in queue. 

ECHO_SERVER_MAX_THREAD_COUNT - maximum uncompleted requests count. Defaults to 50 simultanious requests.

ECHO_SERVER_PORT - port to listen. Defaults to 11111.

ECHO_SERVER_SOCKET_BUFFER_SIZE - count of byte to read and mirror. Defaults to 512.

ECHO_SERVER_POLLING_INTERVAL_MS- time interval to wait in a case of no completed tasks is present in RequestManager. Defaults to 32 ms.
