# うさみゅ～タッチ
プロジェクターで投影した映像に出現する"うさみゅ～"というキャラクターを手足でタッチして遊ぶゲームです．  
アプリケーションはUnity，姿勢検出にはAIカメラOAK-D-Lite，Pythonを使用しています．  
PC内の画面クリックにも対応しているため，アプリケーション単体でもプレイすることができます．

## 環境構築
### ハードウェア
姿勢入力によるプレイを行うためには下記ハードウェアが必要です．
- AIカメラ OAK-D-Lite  
https://www.switch-science.com/catalog/7651/  

セットアップ方法は[こちら](Pose-Tracking/README.md)を参照してください．

#### 使用ライブラリ
- 姿勢追跡  
https://github.com/geaxgx/depthai_blazepose

### ソフトウェア
- Unity Hub + Unity 2021.3.3f1
    - Unity Hubにて，Windowsビルド用のモジュール Windows Build Support (IL2CPP)をインストール

#### 使用パッケージ
- OscJack  
https://github.com/keijiro/OscJack
