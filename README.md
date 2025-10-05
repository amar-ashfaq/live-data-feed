# Live Data Feed

This is a simple real-time session feed application built with:

- **Server:** .NET 8 Web API with WebSocket support
- **Client:** HTML, CSS, and JavaScript

---

## How it works

- The server polls the sample API (`http://dev-sample-api.tsl-timing.com/sessions`) every 5 seconds.
- Connected clients receive session updates via WebSockets.
- The client renders the sessions in a simple list format.

---

## Running the application

1. Open the solution in Visual Studio.
2. Make sure the **launch URL** in your Debug profile points to the root (`/`) instead of Swagger.
3. Run the application. It will serve the client from the `Client` folder.

> **Note:** Swagger is still available at `/swagger`.  
> By default, the application will launch your client from the root (`/`) if the launch URL is set correctly in Visual Studio.  
> You can access the API documentation at `https://localhost:7058/swagger` if needed.

---

## WebSocket URL

In `Client/app.js` the WebSocket is created like this:

```javascript
const socket = new WebSocket("wss://localhost:7058/ws");
