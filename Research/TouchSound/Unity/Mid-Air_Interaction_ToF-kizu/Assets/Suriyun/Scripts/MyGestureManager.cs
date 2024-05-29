using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class MyGestureManager : MonoBehaviour
{
    private Controller controller;
    private int handWaveCount; // 手を何回振ったかを数える
    private bool isFirstInit; // 手の振りの検出時に使う、中心位置centerXを一度だけ初期化するためのフラグ
    private float centerX; // 手の振りの検出のための基準位置
    private float WAVE_REGION = 10f; // 手の振り幅を定める
    private Vector prevHandPos; // 1フレーム前の手の位置を保持する
    private float timer; // centerXの更新に使う

    private bool isWaveDetected;
    private bool isGrabDetected;
    private bool isPinchDetected;

    void Start()
    {
        // メインインタフェースであるControllerの初期化
        controller = new Controller();

        handWaveCount = 0;
        isFirstInit = true;
        timer = 0;

        isWaveDetected = false;
        isGrabDetected = false;
        isPinchDetected = false;
    }

    // 再生モードを止めたときに呼び出される。
    void OnApplicationQuit()
    {
        if (controller != null) {
            controller.StopConnection();
            controller.Dispose();
            controller = null;
        }
    }

    void Update()
    {
        // 直近のトラッキングデータを取得
        Frame frame = controller.Frame();

        // 検出されたHandオブジェクトが任意の順に並んだリストを取得
        List<Hand> hands = frame.Hands;
        
        isWaveDetected = WaveDetection(hands);
        isGrabDetected = GrabDetection(hands);
        isPinchDetected = PinchDetection(hands);

    }

    public bool isHandWaving() {
        return isWaveDetected;
    }

    public bool isHandGrabbing() {
        return isGrabDetected;
    }
    
    public bool isHandPinching() {
        return isPinchDetected;
    }

    private bool WaveDetection(List<Hand> hands) {

        Hand hand;

        // 手が検出されなければ
        if(hands.Count == 0) {
            isFirstInit = true;
            handWaveCount = 0;
            return false;
        }
        else if(hands.Count == 1) hand = hands[0];
        else hand = hands[1]; // 新しくかざされた方の手について処理する。（この時、hands.Count==2であることを想定している）

        // 手のない状態から、初めて手が検出されたなら
        if(isFirstInit) {
            isFirstInit = false;
            prevHandPos = hand.PalmPosition;
            centerX = hand.PalmPosition.x;
        }

        // 手を振るカウンタに変化がないまま0.5秒経ったら（手があまり動かなかったら）、
        // 手を振る基準の位置centerXを更新する
        if(timer >= 0.5f) {
            centerX = hand.PalmPosition.x;
            handWaveCount = 0;
            timer = 0;
        }

        // 領域内にprevHandPos.xが、領域外に今の手があれば「手を振った」とする
        if(Mathf.Abs(prevHandPos.x - centerX) <= WAVE_REGION) {
            if(Mathf.Abs(hand.PalmPosition.x - centerX) > WAVE_REGION) {
                handWaveCount++;
                timer = 0;
            }
        }

        // 3回手を振ったら
        if(handWaveCount >= 3) {
            handWaveCount = 0;
            return true;
        }

        // prevHandPosは手が検出される限り毎フレーム更新される
        prevHandPos = hand.PalmPosition;
        timer += Time.deltaTime;
        return false;
    }

    private bool GrabDetection(List<Hand> hands) {
        foreach(Hand hand in hands) {
            if(hand.GrabStrength == 1) return true;
        }
        return false;
    }

    private bool PinchDetection(List<Hand> hands) {
        foreach(Hand hand in hands) {
            if(hand.PinchStrength == 1) return true;
        }
        return false;
    }
}
