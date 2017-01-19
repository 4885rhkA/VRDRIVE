var port = 6666;

var fs = require('fs');

var io = require('socket.io')({
  transports: ['websocket'],
});

io.on('connection', function(socket){
  socket.on("/car" ,function(obj){
    fs.appendFile('log.txt', JSON.stringify(obj)+ "\n");
    console.log(obj);
  });
  socket.on("disconnect", function () {
    fs.appendFile('log.txt', "EOF");
  });
});

io.attach(port);