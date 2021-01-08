console.log("Hello");

const webSocket = new WebSocket(`wss://${location.host}/api/rdp/connect`);
webSocket.onopen = () => {
    webSocket.send("Secret Message");
};
webSocket.onmessage = (e) => {
    console.log("XXX", e);
};
webSocket.onerror = () => {};
webSocket.onclose = () => {};