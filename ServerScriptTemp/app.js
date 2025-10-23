const express = require('express');
const http = require('http');
const socketIo = require('socket.io');

const app = express();
const server = http.createServer(app);
const io = socketIo(server);

const PORT = process.env.PORT || 3000;

app.get('/', (req, res) => {
    res.send('<h1>Char Server is running</h1>');
});

const users = {};

io.on('connection', (socket) => {
    console.log('New client connected');

    let currentRoom = null;
    let currentUserName = null;

    socket.on('join room', (data) => {
        if (typeof data == "string") {
            data = JSON.parse(data);
        }
        const { roomName, userName } = data;
        if (!roomName || !userName) {
            console.log('Invalid roomname or username');
            return;
        }
        if (currentRoom && socket.rooms.has(currentRoom)) {
            socket.leave(currentRoom);
            console.log(`${currentUserName} leave ${currentRoom} room`);

            io.to(currentRoom).emit('chat message', { userName: 'Syste', message: `${currentUserName} leave room` });
        }

        socket.join(roomName);
        currentRoom = roomName;
        currentUserName = userName;

        users[socket.id] = { userName: currentUserName, roomName: currentRoom };
        console.log(`${currentUserName} join ${currentRoom} room`);
        io.to(currentRoom).emit('chat message', { userName: 'System', message: `${currentUserName} join room` });
    });

    socket.on('chat message', (data) => {
        if (typeof data == "string") {
            data = JSON.parse(data);
        }
        const { message } = data;
        const user = users[socket.id];

        if (user && user.roomName) {
            console.log(`[${user.roomName}] ${user.userName} : ${message}`);
            io.to(user.roomName).emit('chat message', { userName: user.userName, message: message });

        } else {
            console.log('Invalid chat message from who not join player this room');
        }
    });

    socket.on('disconnect', () => {
        const user = users[socket.id];
        if (user) {
            console.log(`${user.userName} player ${user.roomName} disconneted this room`);
            io.to(user.roomName).emit('chat message', { userName: 'System', message: `${user.userName} Disconnected` });
            delete users[socket.id];
        } else {
            console.log('client disconneted');
        }
    });
});

server.listen(PORT, () => {
    console.log(`server running at http://localhost: ${PORT}`);
});