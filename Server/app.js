var port = 6666;

var fs = require('fs');

function isExistFile(file) {
  try {
    fs.statSync(file);
    return true
  } catch(err) {
    if(err.code === 'ENOENT') return false
  }
}

var io = require('socket.io')({
  transports: ['websocket'],
});

var filename;

io.on('connection', function(socket){
  // file create
  socket.on("/file" ,function(obj){
    filename = obj.file.replace(/[^\x01-\x7E]/g, function(s){
      return String.fromCharCode(s.charCodeAt(0) - 0xFEE0);
    }).replace(/\D/g, '');
    filename = filename + ".txt";
    fs.writeFileSync(filename, "");
  });
  // insert data
  socket.on("/car" ,function(obj){
    if(isExistFile(filename)) {
      fs.appendFileSync(filename, JSON.stringify(obj)+ "\n");
    }
    console.log(obj);
  });
  socket.on("disconnect", function () {
    console.log("fin");
  });
});

io.attach(port);