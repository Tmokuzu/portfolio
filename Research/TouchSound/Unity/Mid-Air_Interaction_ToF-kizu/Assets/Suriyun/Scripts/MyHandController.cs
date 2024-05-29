using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class MyHandController : MonoBehaviour
{
    public GameObject leftHand, rightHand;
    // private float timeL, timeR;
    private Controller controller;
    private List<Hand> hands;

    // public float getTimeL(){
    //     return timeL;
    // }

    // public float getTimeR(){
    //     return timeR;
    // }

    public bool isLeftHandActive(){
        return leftHand.activeSelf;
    }

    public bool isRightHandActive(){
        return rightHand.activeSelf;
    }

    // public void deleteLeftHand(){
    //     leftHand.SetActive(false);
    // }

    // public void deleteRightHand(){
    //     rightHand.SetActive(false);
    // }

    // Start is called before the first frame update
    void Start()
    {
        // timeL = 0;
        // timeR = 0;
        controller = new Controller();
        hands = new List<Hand>();
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

    // Update is called once per frame
    void Update()
    {
        // if(isLeftHandActive()){
        //     timeL += Time.deltaTime;
        // }
        // else{
        //     timeL = 0;
        // }

        // if(isRightHandActive()){
        //     timeR += Time.deltaTime;
        // }
        // else{
        //     timeR = 0;
        // }

        // 直近のトラッキングデータを取得
        Frame frame = controller.Frame();

        // 検出されたHandオブジェクトが任意の順に並んだリストを取得
        hands = frame.Hands;
    }

    public bool isHandInRightSide() {

        if(hands.Count == 0) return false;
        if(hands.Count == 1) return hands[0].PalmPosition.x > 0;

        // 以下、hands.Count == 2のときを想定する。
        // 両手をかざしたとき、hands[0]には最初にかざした手が格納される。
        // 最後にかざした手に関して処理を行う。
        return hands[1].PalmPosition.x > 0;
    }
}
