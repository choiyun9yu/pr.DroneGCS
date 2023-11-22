// const signalR = require("@microsoft/signalr");
//
// const URL = process.env.HUB_ADDRESS || "https://localhost:5000/hub";
//
// class Connector {
//     constructor() {
//         this.connection = new signalR.HubConnectionBuilder()
//             .withUrl(URL)
//             .withAutomaticReconnect()
//             .build();
//         this.connection.start().catch(err => document.write(err));
//         this.events = onMessageReceived => {
//             this.connection.on("messageReceived", (username, message) => {
//                 onMessageReceived(username, message);
//             });
//         };
//     }
//
//     newMessage(messages) {
//         this.connection.send("newMessage", "foo", messages).then(x => console.log("sent"));
//     }
//
//     static getInstance() {
//         if (!Connector.instance)
//             Connector.instance = new Connector();
//         return Connector.instance;
//     }
// }
//
// module.exports = Connector.getInstance;