#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <winsock2.h>

#include <windows.h>
#include <io.h>
#include <process.h>
#include <sys/types.h>

#include <ws2tcpip.h>
#include <winsock.h>
#include <wtypes.h>

#include"dirent.h"

using namespace std;
#pragma comment(lib, "ws2_32.lib")

#define RCON_COMMAND_MAX_LENGTH 4096
#define RCON_RESPONSE_MAX_LENGTH 4096

DIR *d;
FILE *fp;
errno_t err;
struct dirent *dir;

void sendRconCommand(SOCKET socket, const char* command) 
{
    char buffer[RCON_COMMAND_MAX_LENGTH];
    int length = snprintf(buffer, sizeof(buffer), "\xFF\xFF\xFF\xFFrcon %s", command);

    if (length > 0 && length < sizeof(buffer)) 
    {
        send(socket, buffer, length, 0);
    }
}

void receiveRconResponse(SOCKET socket) 
{
    char buffer[RCON_RESPONSE_MAX_LENGTH];
    int bytesRead = recv(socket, buffer, sizeof(buffer) - 1, 0);

    if (bytesRead > 0) 
    {
        buffer[bytesRead] = '\0';
        printf("RCON Response:\n%s\n", buffer);
    }
}

void receiveRconInfoResponse(SOCKET socket) 
{
    /*DIR *d;
    FILE *fp;
    errno_t err;
    struct dirent *dir;*/

    err = fopen_s( &fp, "rconAdm.txt", "w" );
    if( fp == NULL )
        return;

    d = opendir( "./" );

    char buffer[RCON_RESPONSE_MAX_LENGTH];
    int bytesRead = recv(socket, buffer, sizeof(buffer) - 1, 0);

    if (bytesRead > 0) 
    {
        buffer[bytesRead] = '\0';
        //if( buffer == "sv_hostname" )
        //    printf("RCON Response:\n%s\n", buffer);
        if( d )
        {
            while( ( dir = readdir( d ) ) != NULL )
            {
                if( ( strcmp( dir->d_name, "." ) == 0 ) || ( strcmp( dir->d_name, ".." ) == 0 ) )
                    continue;
                fprintf( fp, "%s\n", buffer );
            }
            fclose( fp );
            closedir( d );
        }
    }
}

void sendMessage(const char* message)
{
    err = fopen_s( &fp, "rconAdm.txt", "w" );
    if( fp == NULL )
        return;

    d = opendir( "./" );

    if( d )
    {
        while( ( dir = readdir( d ) ) != NULL )
        {
            if( ( strcmp( dir->d_name, "." ) == 0 ) || ( strcmp( dir->d_name, ".." ) == 0 ) )
                continue;
            fprintf( fp, "%s\n", message );
        }
        fclose( fp );
        closedir( d );
    }
}

int main() 
{
    char IP[32];
    int PORT = 28960;
    char RCONPW[64] = "rconpassword";
    char RCONCMD[256] = "rcon";
    bool running = true;
    bool getServerInfo = false;
    char exitrcon[1] = "";

    // Initialize Winsock
    WSADATA wsaData;
    if( WSAStartup(MAKEWORD(2, 2), &wsaData) != 0 ) 
    {
        fprintf(stderr, "Failed to initialize Winsock\n");
        return 1;
    }

    // Create socket
    SOCKET serverSocket = socket(AF_INET, SOCK_DGRAM, 0);
    if( serverSocket == INVALID_SOCKET ) 
    {
        fprintf(stderr, "Failed to create socket\n");
        WSACleanup();
        return 1;
    }

    printf( "Please typing your cod4 server IP\n" );
    scanf_s( "%s", IP, sizeof(IP) );

    if( strcmp( IP, "." ) == 0 ) // Quitting...
        goto endOfFunc;

    printf( "Please typing your cod4 server PORT\n" );
    scanf_s( "%d", &PORT );

    // Server address information
    struct sockaddr_in serverAddr;
    serverAddr.sin_family = AF_INET;
    //serverAddr.sin_addr.s_addr = inet_addr(IP);
    inet_pton(AF_INET, IP, &serverAddr.sin_addr.s_addr);
    serverAddr.sin_port = htons(PORT); // Replace with your server's port

    // Connect to the server
    if( connect(serverSocket, (struct sockaddr*)&serverAddr, sizeof(serverAddr)) == SOCKET_ERROR ) 
    {
        fprintf(stderr, "Failed to connect to server\n");
        closesocket(serverSocket);
        WSACleanup();
        sendMessage( "failed_to_conn" );
        return 1;
    }

    printf( "Connection success\n" );
    printf( "Please typing your cod4 server rcon password\n" );
    //scanf_s( "login %c", &RCONPW );
    scanf_s("%s", RCONPW, sizeof(RCONPW));

    // RCON authentication
    sendRconCommand(serverSocket, RCONPW);
    receiveRconResponse(serverSocket);

    while( running )
    {
        if( !getServerInfo )
        {
            char fullCMD[256];
            snprintf(fullCMD, sizeof(fullCMD), "%s serverinfo", RCONPW);
            sendRconCommand(serverSocket, fullCMD);
            receiveRconInfoResponse(serverSocket);
            getServerInfo = true;
        }
        scanf_s(" %[^\n]", RCONCMD, sizeof(RCONCMD));
        if( strcmp( RCONCMD, ".." ) == 0 )
        {
            getServerInfo = false;
            goto skipCMDstuff;
        }
        if( strcmp( RCONCMD, "." ) == 0 ) // Quitting...
            goto skipCMDstuff;
        printf( "Typing your rcon command\n" );
        char fullCommand[256];
        snprintf(fullCommand, sizeof(fullCommand), "%s %s", RCONPW, RCONCMD);
        // Example: Send RCON command "status" to get server status
        sendRconCommand(serverSocket, fullCommand);
        receiveRconResponse(serverSocket);

        //printf( "Do you want to exit? Y/N\n" );
    skipCMDstuff:
        scanf_s(" %c", exitrcon, sizeof(exitrcon));

        if (exitrcon[0] == 'N' || exitrcon[0] == 'n')
            running = true;
        else if (exitrcon[0] == 'Y' || exitrcon[0] == 'y')
            running = false;
    }

endOfFunc:

    // Cleanup
    closesocket(serverSocket);
    WSACleanup();

    return 0;
}
