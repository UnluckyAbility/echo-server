# Tcp echo server
Based on TcpListener class
# Issues
No restrictions and control of external resources, such as sockets. The server will hang under a heavy duty. The worst one.
One-by-one task completion handling leads to an increased load on GC.
Obvious architectural issues.
