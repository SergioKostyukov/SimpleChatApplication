# SimpleChatApplication
*SimpleChatApplication is a chat application that allows users to create chats, connect to them, exchange messages in real-time using WebSockets, and manage chat participants via a REST API. The application is built using ASP.NET Core, MSSQL, and other modern technologies to ensure reliable operation and user convenience.*

## Technologies Used

- **Architeture style**: 3-tier architecture
- **Backend**: ASP.NET core API
- **Data Management**: MSSQL (Entity Framework)
- **Tests**: xUnit, Moq
- **Other Libraries**: SignalR, AutoMapper

## Application functionality

### SimpleChatApplication.Core
  - Defines the main entities of the application and the enumeration necessary for them
### SimpleChatApplication.Infrastructure.Data
  - Defines the context of interaction with the database
  - Implements configurations to all tables
  - Contains all migrations
  - Adds storage usage through an extension method
### SimpleChatApplication.Application
  - Implements the main interfaces and services od the application
  - Adds services through an extension method
### SimpleChatApplication.WebAPI
  - Implements the main controllers of the application

### API calls:
  - **GET /api/Chat/GetList** - getting a list of existing chats
  - **GET /api/Chat/Get** - getting detailed information about a specific chat
  - **POST /api/Chat/Create** - creating a new chat
  - **POST /api/Chat/ConnectTo** - adding a user to the chat participants
  - **DELETE /api/Chat/Delete** - deleting a chat

### Websockets:
  - **SendMessage** - sending a message to chat members
  - **JoinChat** - joining a user to a chat session
  - **LeaveChat** - removing a user from a chat session

## Usage

*Testing was carried out using Postman*

**WebSocket Endpoint**
```
wss://localhost:xxxx/chatHub
```

**WebSocket Connection Example**
  - *To connect to the WebSocket, you need to establish a connection with the following handshake request:*
    ```json
    {
      "protocol": "json",
      "version": 1
    }
    ```
**Sending a Message Example**
  - *To send a message (you must be part of the chat), you can use the following JSON structure:*
    ```json
    {
      "type": 1,
      "target": "SendMessage",
      "arguments": [1, 1, "Test message"] // [chatId, userId, message]
    }
    ```
**Receiving a Message Example**
  - *First you need to join the chat (you must be part of the chat by using API method):*
    ```json
    {
      "type": 1,
      "target": "JoinChat",
      "arguments": [1, 1] // [chatId, userId]
    }
    ```
  - *After that, you will receive messages from this chat*
