using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

public class anim2 : MonoBehaviour
{
    public Animator anim;
    public GameObject bird;
    public MyHandController hand;
    public MyGestureManager gesture;
    private StreamWriter sw;
    private float MOVE_LIMIT;
    private float dx, dy, goalX, jump_speed;
    private float time_cnt;
    private bool isJumpingR, isJumpingL, isRandomJumping;
    private float DETECT_TIME = 0.3f;
    private float TIME_RANDOMJUMP = 6.0f;
    private float LEFT_END, RIGHT_END; // 初期設定で定義する
    private float BIRD_SPEED, OP_SPEED; // 初期設定で定義する
    private float init_posY; // 初期設定で定義する
    private float jumpHeight; // 初期設定で定義する


    void Start()
    {
        InitField();

        // 乱数生成の初期化
        UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);

        // ルートモーションの適用
        bird.GetComponent<Animator>().applyRootMotion = true;
    }

    void Update()
    {
        // ループ防止のための初期化
        anim.SetInteger("animation", 1);
        
        // 手をかざすだけでは反応せず、手を振ったらジャンプする
        // & 掴もうとしたら逃げる
        if(gesture.isHandGrabbing() || gesture.isHandPinching()) {
            anim.SetInteger("animation", 102);
        }
        else if(gesture.isHandWaving()) {
            if(hand.isHandInRightSide()) anim.SetInteger("animation", 100);
            else anim.SetInteger("animation", 101);
        }

        // フラグの更新
        isJumpingL = anim.GetCurrentAnimatorStateInfo(0).IsName("Jump_L"); // 101
        isJumpingR = anim.GetCurrentAnimatorStateInfo(0).IsName("Jump_R"); // 100
        if(hand.isLeftHandActive() || hand.isRightHandActive()) isRandomJumping = false;
        else isRandomJumping = true;

        PosSizeUpdate();
    }

    private void InitField()
    {
        this.dx = 0;
        this.isJumpingR = false;
        this.isJumpingL = false;
        this.isRandomJumping = true;
        this.time_cnt = 0;
        this.goalX = 0;
        this.jump_speed = 0;
    }

    private void PosSizeUpdate() {
        // 鳥の座標更新
        Vector3 bird_pos = bird.transform.position;

        // 手が検知されていれば手に従ったジャンプを、そうでなければランダムにジャンプする。
        if(isRandomJumping)
        {
            this.time_cnt += Time.deltaTime;

            if(this.time_cnt >= TIME_RANDOMJUMP)
            {
                this.time_cnt = 0;
                this.goalX = UnityEngine.Random.Range(LEFT_END, RIGHT_END);
                this.jump_speed = BIRD_SPEED;
                if(0 < goalX - bird_pos.x) anim.SetInteger("animation", 100);
                else if(goalX - bird_pos.x < 0) anim.SetInteger("animation", 101);
            }
        }
        else
        {
            this.time_cnt = 0;
            this.jump_speed = BIRD_SPEED;
        }

        if(isJumpingR) bird_pos.x += jump_speed;
        else if(isJumpingL) bird_pos.x -= jump_speed;
        
        
        if(isJumpingL || isJumpingR)
            bird_pos.y = jumpHeight
                * Mathf.Abs(Mathf.Sin(Mathf.PI * anim.GetCurrentAnimatorStateInfo(0).normalizedTime)) + this.init_posY;

        // キー操作による鳥の移動
        if(Input.GetKey(KeyCode.LeftArrow)) bird_pos.x -= OP_SPEED;
        if(Input.GetKey(KeyCode.RightArrow)) bird_pos.x += OP_SPEED;

        // 移動範囲の指定
        if(bird_pos.x < LEFT_END) bird_pos.x = LEFT_END;
        else if(RIGHT_END < bird_pos.x) bird_pos.x = RIGHT_END;
        
        bird.transform.position = bird_pos;

        // 鳥のサイズ拡大縮小
        float exSpeed = OP_SPEED * 2.5f;
        if(Input.GetKey(KeyCode.UpArrow)) bird.transform.localScale += new Vector3(exSpeed, exSpeed, 0);
        if(Input.GetKey(KeyCode.DownArrow)) bird.transform.localScale -= new Vector3(exSpeed, exSpeed, 0);
        if(bird.transform.localScale.x < 0.02f) bird.transform.localScale = new Vector3(0.02f, 0.02f, 3f); // 最小
    }

    public void SetBirdSpeed(float x) {
        this.BIRD_SPEED = x;
    }

    public void SetOperationSpeed(float x) {
        this.OP_SPEED = x;
    }

    public void SetInitPosY(float y) {
        this.init_posY = y;
    }

    public void SetLeftEnd(float x) {
        this.LEFT_END = x;
    }

    public void SetRightEnd(float x) {
        this.RIGHT_END = x;
    }

    public void SetJumpHeight(float x) {
        this.jumpHeight = x;
    }
}