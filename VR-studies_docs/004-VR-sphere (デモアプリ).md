# VR-sphere 概要


HTC-VIVE用のデモアプリです。  
地球上の様々な都市の観光情報をVR空間上でダイナミックに閲覧することができます。

![image][1073980802]
![image][1073980801]
![image][1073980798]


### ソースの場所
git://vr_app_sample/VR-sphere/Assets/VR-sphere/VR-sphere.unity


### 推奨環境

- Unity / ver 5.5.1 ( ver 5.6ではSteamVRPluginがうまく動かないとのレポートがありますので、5.5を使用してください)
- SteamVR Unity Plugin /  ver 1.2.0
- Windows 10
---

## プログラムの実行方法

このプログラムの実行にはFourSquareAPIの開発者キーが必要です。  
下記のURLから開発者登録をして、クライアントIDと秘密キーを取得してください。
https://developer.foursquare.com/


### FoursquareAPIキーの設定方法

1.下記のシーンをUnity5.3以上で開きます  
git://VR-sphere/Asset/VR-sphere.unity

2.Hierarychyパネル上のVRSphereオブジェクトを選択し、Four-Square-APIコンポーネントの
Foursquare_client_idとFoursquare_client_secretに取得した値を入力します

![image][1073980800]


### 一度に取得＆表示する写真数の設定

FoursquareAPIは現状１時間ごとにアクセスできるリクエスト数に制限があります。  
現在のプログラムは一度取得した写真データはローカルにキャッシュする仕組みになっていますが、
APIに負荷をかけたくない、または表示が重い場合は、下記の値を小さく設定してください。

- Hierarychyパネル上のVRSphereオブジェクトを選択し、VRSphereコンポーネントのPhoto_max_num_xと_yを設定します

![image][1073980799]

---
