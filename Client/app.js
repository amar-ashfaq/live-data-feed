const socket = new WebSocket("wss://localhost:7058/ws");
const sessionList = document.getElementById("sessionList");

socket.onopen = () => console.log("Connected to WebSocket");
socket.onclose = () => console.log("WebSocket closed");
socket.onerror = (err) => console.error("WebSocket error", err);

socket.onmessage = (e) => {
    try {
        const data = JSON.parse(e.data);

        sessionList.innerHTML = "";

        data.forEach(session => {
            const startTime = session.startTime ? new Date(session.startTime) : null;
            const displayTime = startTime ? startTime.toLocaleString() : "N/A";

            const li = document.createElement("li");
            li.innerHTML = `<strong>${session.name}</strong><br>
                            Start: ${displayTime}<br>
                            <span class="timestamp">ID: ${session.sessionId}</span>`;
            sessionList.appendChild(li);
        });
    } catch (err) {
        console.warn("Failed to parse JSON:", err);
    }
};

