# ポートフォリオ
- Research/　...　研究についての制作物
  - TouchSound/　...　UIのタッチフィードバックに関する研究
    - Arduino/　...　Arduinoファイル
      - IRread.ino　...　IR測距センサの値を出力（センサはGP2Y0A21YK0F）
    - PD/　...　Puredataファイル
      - patches/feedforwardOSC.pd　...　Arduino → PD → Unity
      - patches/UnitySoundOSC.pd　...　Arduino → Unity → PD
    - Unity/　...　Unityファイル
      - Opti＿Button/　...　OptiTrackによるハンドトラッキングを使ったUIの操作
        - Assets/Scripts/　...　C#コード
          - ChangeRadius.cs　...　トラックポイントの距離に応じてポインティングフィードバックのサイズを変える
          - Collision.cs　...　衝突しているCollisionを保存
          - MeasureDistance.cs　...　トラックポイントとボタンの距離を保存
          - OSCSender.cs　...　OSC通信でPuredataに値を送信
          - PlayTouchSound.cs　...　クリック音をならす
          - PushJudge_Tenkey.cs　...　テンキーの押されたボタンを判定
        - Assets/OptiTrack/　...　OptiTrackの公式プラグイン　クライアント用-Scripts/OptitrackStreamingClient.cs　ストリーミング用-Scripts/OptitrackRigidBody.cs
      - IR_Button/　...　IR測距センサを使ったUIの操作
        - Assets/Scripts/　...　C#コード
          - ButtonChangeOSC.cs　...　PuredataからOSC通信で送られてきた値によってタッチフィードバックを変化
          - ButtonChangeSerial.cs　...　ArduinoからSerial通信で送られてきた値によってタッチフィードバックを変化
          - OSCReceive.cs　...　OSC通信で値を取得
          - SerialReceive.cs　...　Serial通信で値を取得
         
  - Mitsuba3_MMAP/　...　再帰透過光学素子(MMAP)のCGシミュレーションに関する研究
    - Output/　...　出力画像
    - Python/　...　Pythonコード
      - first_render.py　...　MMAPシーンをレンダリング
      - myUtility.py　...　自分用のUtility
    - scenes/　...　シーンファイル
      - meshes/　...　obj,plyファイル
      - textures/　...　光源用画像
      - MMAP.xml　...　Mitsuba用MMAPシーン

# Verified version
- Unity 2021.3.23
