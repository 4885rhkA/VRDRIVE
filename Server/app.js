var port = 6666;

var fs = require('fs');

function objectSort(object) {
    //戻り値用新オブジェクト生成
    var sorted = {};
    //キーだけ格納し，ソートするための配列生成
    var array = [];
    //for in文を使用してオブジェクトのキーだけ配列に格納
    for (key in object) {
        //指定された名前のプロパティがオブジェクトにあるかどうかチェック
        if (object.hasOwnProperty(key)) {
            //if条件がtrueならば，配列の最後にキーを追加する
            array.push(key);
        }
    }
    //配列のソート
    array.sort(); 
    //配列の逆ソート
    //array.reverse();
 
    //キーが入った配列の長さ分だけfor文を実行
    for (var i = 0; i < array.length; i++) {
        /*戻り値用のオブジェクトに
        新オブジェクト[配列内のキー] ＝ 引数のオブジェクト[配列内のキー]を入れる．
        配列はソート済みなので，ソートされたオブジェクトが出来上がる*/
        sorted[array[i]] = object[array[i]];
    }
    //戻り値にソート済みのオブジェクトを指定
    return sorted;
}

var io = require('socket.io')({
  transports: ['websocket'],
});

var filename;
var obj = {};
var labels = [];
var datasets = [];

io.on('connection', function(socket){

  // init
  socket.on("/init" ,function(obj){
    filename = obj.file.replace(/[^\x01-\x7E]/g, function(s){
      return String.fromCharCode(s.charCodeAt(0) - 0xFEE0);
    }).replace(/\D/g, '');
    filename = filename + ".json";
  });

  // items
  socket.on("/items" ,function(obj){
    datasets = Object.keys(obj).map(function (key) {
      var array = {};
      array["label"] = key;
      array["data"] = [];
      return array;
    });
    console.log(datasets);
  });

  // insert data
  socket.on("/car" ,function(obj){
    var aaa = true;
    Object.keys(obj).map(function (key) {
      datasets.filter(function(item, index){
        if (item.label == key) {
          item.data.push(obj[key]);
        }
        if(key == "time" && aaa) {
          labels.push(obj[key]);
          aaa = false;
        }
      });
    });
    console.log(obj);
  });

  socket.on("disconnect", function () {
    console.log("ENDED");
    obj["labels"] = labels;
    obj["datasets"] = datasets;
    fs.writeFileSync(filename, JSON.stringify(obj));
    json = {};
    labels = [];
    datasets = [];
  });

});

io.attach(port);