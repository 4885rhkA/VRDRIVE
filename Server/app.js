var port = 6666;

var io = require('socket.io')({
  transports: ['websocket'],
});

io.attach(6666);

io.on('connection', function(socket){
  socket.on("disconnect", function () {
    console.log("offline id: " + socket.id);
    socket.broadcast.emit("offline", { id: socket.id });
    padDisconnect(socket.id);
    });
  socket.on("/hello",function(obj){
    console.log("receive hello");
    padConnect(socket.id);
    socket.emit("/config/type", { value: 2 });

    });
  socket.on("/touch/start"         ,function(obj){ padState(socket.id,"/touch/start"       ,obj); });
  socket.on("/touch/repeat"        ,function(obj){ padState(socket.id,"/touch/repeat"      ,obj); });
  socket.on("/touch/pos"           ,function(obj){ padState(socket.id,"/touch/pos"         ,obj); });
  socket.on("/touch/end"           ,function(obj){ padState(socket.id,"/touch/end"         ,obj); });
  socket.on("/sensor/gyroUnbiased" ,function(obj){ padState(socket.id,"/sensor/gyroUnbiased"   ,obj); });
  socket.on("/sensor/gyroAttitude" ,function(obj){ padState(socket.id,"/sensor/gyroAttitude"   ,obj); });
  socket.on("/sensor/attitude"     ,function(obj){ padState(socket.id,"/sensor/attitude"       ,obj); });
  socket.on("/dialog/clicked"      ,function(obj){ padState(socket.id,"/dialog/clicked"        ,obj); });
});

function padState(index, name, obj){
  io.sockets.emit("padstate", {index:name,value:obj});
}

function padConnect(id){
  console.log("padconnct" + id);
  io.sockets.emit("padconnect", {id:id});
}

function padDisconnect(id){
  console.log("paddisconnct" + id);
  io.sockets.emit("paddisconnect", {id:id});
}
