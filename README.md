# MessageService

MessageService is a simple web-based messaging application that demonstrates a full-stack solution using ASP.NET Core, PostgreSQL, and WebSockets. The service enables clients to:

- **Send messages** via a REST API.
- **Receive real-time messages** using WebSockets.
- **Retrieve message history** via a REST endpoint.

This project is containerized with Docker and orchestrated via Docker Compose for easy deployment and testing.

## Features

- **REST API Endpoints:**
  - `POST /api/message/send` — Accepts a message, assigns a server-side creation timestamp, saves it to the database, and broadcasts it to connected WebSocket clients.
  - `GET /api/message/history` — Retrieves messages within a specified date range.
- **WebSockets:**
  - Real-time broadcasting of new messages to connected clients.
- **Swagger Documentation:**
  - Interactive API documentation generated automatically.
- **Containerized Deployment:**
  - Dockerfiles and a Docker Compose configuration for deploying the entire stack.
- **Database Migrations:**
  - Managed with FluentMigrator without using an ORM.

## Architecture

The system consists of three main components:

1. **Web Server:**  
   An ASP.NET Core application that implements:
   - REST API endpoints for sending and retrieving messages.
   - WebSocket endpoints for real-time message delivery.
   - Swagger UI for API documentation.

2. **SQL Database (PostgreSQL):**  
   Persists messages. The database schema is managed by FluentMigrator, and the `messages` table uses an auto-incrementing primary key.

3. **Client Interfaces:**  
   Simple web pages located in the `wwwroot` folder:
   - `send.html` — A client for sending messages.
   - `ws.html` — A client that receives and displays real-time messages via WebSocket.
   - `history.html` — A client for retrieving and displaying historical messages.

## Technology Stack

- **Backend:** ASP.NET Core (.NET 8)
- **Database:** PostgreSQL
- **Database Migrations:** FluentMigrator
- **Real-Time Communication:** WebSockets
- **API Documentation:** Swashbuckle/Swagger
- **Containerization:** Docker and Docker Compose


## Getting Started


### Running the Service with Docker Compose

1. **Clone the repository:**

    ```bash
    git clone https://github.com/tredeim/MessageService.git
    cd MessageService
    ```

2. **Build and start the containers:**

    ```bash
    docker compose up --build
    ```

    This command will:
    - Start the PostgreSQL container.
    - Run database migrations (if integrated or via a separate migration container).
    - Start the web service container, which listens on port **8000** (mapped appropriately).

3. **Access the service:**

    - **Swagger UI (API Documentation):** [http://localhost:8000/swagger/index.html](http://localhost:8000/swagger/index.html)
    - **Send Message Client:** [http://localhost:8000/send.html](http://localhost:8000/send.html)
    - **Real-Time Messages Client:** [http://localhost:8000/ws.html](http://localhost:8000/ws.html)
    - **Message History Client:** [http://localhost:8000/history.html](http://localhost:8000/history.html)

## API Endpoints

### POST `/api/message/send`

- **Description:** Sends a new message.
- **Request Body Example:**

    ```json
    {
      "text": "Hello, world!",
      "sequenceNumber": 1
    }
    ```

- **Response:** Returns the saved message with properties including the auto-generated ID, creation timestamp, text, and sequence number.

### GET `/api/message/history`

- **Description:** Retrieves messages within a specified date range.
- **Query Parameters:**
  - `from`: Start date/time in ISO 8601 format.
  - `to`: End date/time in ISO 8601 format.
- **Response:** An array of messages matching the date range.

## Real-Time Messaging

The service uses WebSockets to broadcast new messages to all connected clients. To see real-time updates:

1. Open the `ws.html` page in a browser.
2. Send messages using the `send.html` page or via API.
3. The connected `ws.html` client will display incoming messages as they are broadcast.
