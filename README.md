# Tcp echo server
Based on TcpListener class
# Issues
No restrictions and control of external resources, such as sockets. The server will hang under a heavy duty. The worst one.

One-by-one task completion handling leads to an increased load on the GC.

Fixed maximum response size.

Obvious architectural issues.

# Docker container environment variables
ECHO_SERVER_PORT - port to listen, defaults to 11111

ECHO_SERVER_POLING_INTERVAL_MS - time interval to wait in a case of no completed tasks is present in RequestManager
