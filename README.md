StarryEyes-DB-Reducer
=====================

### ~~EXPERIMENTAL!!!!~~ Alpha version

ごく稀にDB関連のエラーでKrileが強制終了することがあります。  
このアプリケーションを使用してDB関連のエラーが発生した場合には、開発者にレポートを送らないようにしてください。

----

Krile STARRYEYESで使用されている krile.db をダイエットさせます。  
160万postほど貯めたらKrileの起動に30秒以上かかるようになってしまったので作りました。

デフォルトでは直近10000件を残して全て削除します。  
残す件数を50000件などに変えたい場合は、引数に -d 50000 とかすると幸せになれると思います。
