# STG Option Movement

オブジェクトの座標と回転を一定距離空けてトレースする。\
[HyperNova](https://kuritaro1122.github.io/HyperNova-Web/)のオプション砲移動処理にこのプログラムを使用しています。

# DEMO
![gif](/img/demo.gif)

# Requirement

* UnityEngine
* System.Collections.Generic

# Usage

① STGOptionMovement.cs を任意のGameObjectにコンポーネントする\
② STGOptionMovementにターゲットと追尾させるGameObjectをアタッチする\
③ 座標の更新タイミング(updateType)をinspectorから選択\
④ パラメータを調整

※ より違和感なく動かすために、updateTypeをmanualにして、ターゲットの座標を更新した直後にvoid Update_()を呼ぶことを推奨します。

# Contains

## Inspector

![image](/img/inspectorView.png)

## public Function
```
void Update_()
```


# Note

ターゲットが一定距離進むごとにTransformの情報を保存し、それらの座標や回転を線形補間(Lerp)することによりオプションを動かしています。

動きをより滑らかにするためには、distanceにより小さな値を設定し、Options/stepにより大きな値を設定してください。
ただし、キャッシュを取る回数が増加するため処理は重くなります。

# License

"STGOptionMovement" is under [MIT license](https://en.wikipedia.org/wiki/MIT_License).
