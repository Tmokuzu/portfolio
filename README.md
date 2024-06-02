# ポートフォリオ
- Research/  ...　研究についての制作物
  - TouchSound/  ...　UIのタッチフィードバックに関する研究
    - Arduino/  ...  Arduinoファイル
      - IRread  ...  IR測距センサの値を出力（センサはGP2Y0A21YK0F）
    - PD/  ...  Puredataファイル
      - patches/feedforwardOSC.pd  ...  Arduino → PD → Unity
      - patches/UnitySoundOSC.pd  ...  Arduino → Unity → PD
    - Unity/  ...  Unityファイル
      - Opti＿Button  ...  OptiTrackによるハンドトラッキングを使ったUIの操作
        - Assets/Scripts/  ...  C#コード
          - ChangeRadius.cs  ...  トラックポイントの距離に応じてポインティングフィードバックのサイズを変える
          - Collision.cs  ...  衝突しているCollisionを保存
          - MeasureDistance.cs  ...  トラックポイントとボタンの距離を保存
          - OSCSender.cs  ...  OSC通信でPuredataに値を送信
          - PlayTouchSound.cs  ...  クリック音をならす
          - PushJudge_Tenkey.cs  ...  テンキーの押されたボタンを判定
        - Assets/OptiTrack  ...  OptiTrackの公式プラグイン　クライアント用-Scripts/OptitrackStreamingClient.cs　ストリーミング用-Scripts/OptitrackRigidBody.cs
      - IR_Button  ...  IR測距センサを使ったUIの操作
        - Assets/Scripts/  ...  C#コード
          - ButtonChangeOSC.cs  ...  PuredataからOSC通信で送られてきた値によってタッチフィードバックを変化
          - ButtonChangeSerial.cs  ...  ArduinoからSerial通信で送られてきた値によってタッチフィードバックを変化
          - OSCReceive.cs  ...  OSC通信で値を取得
          - SerialReceive.cs  ...  Serial通信で値を取得
    -  
- vcf/ ... The directory of VCF generation module (`Under development. This is very different from the optical properties of an actual VCF.`)
  - \_\_init\_\_.py       ... Initialization file
  - vcf.py            ... Module to generate/delete VCF
  - myutil.py         ... Utility function module
  - vcf_clearer.py    ... Operator class to delete VCF
  - vcf_launcher.py   ... Operator class to generate VCF
  - vcf_manager.py    ... Panel class to manage parameters
- mmaps.zip           ... For installing MMAPs generation module
- vcf.zip             ... For installing VCF generation module

# Verified version
- Unity 2021.3.23


# Parameters settings
| Parameter        | Default |
| ---              | :-:     |
| MMAPs size       | 48.8    |
| Slit spacing     | 0.05    |
| Height scale     | 2.5     |
| Mirror detailing | 10      |
| Reflectance      | 0.87    |
| IOR              | 1.52    |
