using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    //publicで変数を宣言するとUnity上でInspector設定を変えられる
    public float speed = 5;
    public float jumpForce = 400f;
    private bool isGround;
    private bool isSloped;
    private bool area1;
    private bool area2;
    private bool isDead = false;

    //LayerMask型の変数を宣言
    public LayerMask groundLayer;

    //Rigidbody2Dコンポーネント(クラス)型の変数を宣言
    private Rigidbody2D rb2d;

    //Animatorコンポーネント(クラス)型の変数を宣言
    private Animator anim;

    private SpriteRenderer spRenderer;

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<Rigidbody2D>()でRigidbody2Dコンポーネントの設定値を変更できる
        this.rb2d = GetComponent<Rigidbody2D>();

        //GetComponent<Animator>()でAnimatorコンポーネントを設定できるようにする
        this.anim = GetComponent<Animator>();

        //GetComponent<SpriteRenderer>()でSpriteRendererコンポーネントを設定できるようにする
        this.spRenderer = GetComponent<SpriteRenderer>();

        Sound.LoadSe("death", "death");
        
    }

    // Update is called once per frame
    void Update()
    {
        //Inputクラス型のGetAxisRawメンバメソッドを定義
        //GetAxisRawメソッドは左－1、何もしない0、右＋１の値を返すメソッド
        float x = Input.GetAxisRaw("Horizontal");

        //Animatorクラス(コンポーネント)型のSetFloatメソッドを呼び出す
        //下記ではAnimator設定時に作成していた、名前「Speed」、float型の値を引数にしている
        //キー入力がない場合はアニメーションは実行されない
        //SetFloat()の値はAnimatorコンポーネントのSpeed変数に設定される
        //※初期値でspeed変数に5が入るため、Mathf.Abs()でキー入力の値を引数にしないと
        //常にアニメーションで走り続けることになる。
        anim.SetFloat("Speed",Mathf.Abs(x * speed));

        //キーを左に押したときはユニットを左、右に押したときはユニットを右に向ける
        //-は左、＋は右
        if ( x < 0 )
        {
            //SpriteRendererコンポーネントのflipXにチェックを入れる処理
            spRenderer.flipX = true;

        }else if( x > 0)
        {
            //SpriteRendererコンポーネントのflipXのチェックを外す
            spRenderer.flipX = false;

        }

        if (!isDead)
        {
            //Vector2はクラスと同じ構造体
            //以下ではVector2のright変数？の値を引数として呼び出し、RigitBodyクラスのAddForceメンバメソッドを実行している
            //変数xを乗算することで、キー入力がない場合は0がかけられるため、Playerが動かないようになる
            rb2d.AddForce(Vector2.right * speed * x);

        }
        //Jumpボタンを押して、かつ、isGround(地面にPlayerが当たっているとき)がtrueのとき実行
        if (Input.GetButtonDown("Jump") & isGround)    
        {
            anim.SetBool("isJump", true);
            rb2d.AddForce(Vector2.up * jumpForce);
        }

        if (isGround)
        {
            anim.SetBool("isJump", false);
            anim.SetBool("isFall", false);
        }

        float velX = rb2d.velocity.x;
        float velY = rb2d.velocity.y;

        //velocityが上向きに働いたらジャンプ
        if ( velY > 0.5f )
        {
            anim.SetBool("isJump", true);
        }
        //velocityが下向きに働いたらフォール
        if ( velY < -0.1f)
        {
            anim.SetBool("isFall", true);
        }

        if( Mathf.Abs(velX) > 5)
        {
            rb2d.velocity = new Vector2(5.0f, velY);

        }
        if ( velX < -5.0)
        {
            rb2d.velocity = new Vector2(-5.0f, velY);
        }

        if (isSloped)
        {
            this.gameObject.transform.Translate(0.1f * x, 0.0f, 0.0f);
        }



    }
    
    //当たり判定系の処理は、FixedUpdateメソッド内に記載
    //FixedUpdateメソッドは一定時間(0.02秒)毎に呼ばれるメソッド
    private void FixedUpdate()
    {
        isGround = false;
        float x = Input.GetAxisRaw("Horizontal");

        //Vector2型でgraoundPosインスタンスを定義
        Vector2 groundPos =
            new Vector2(transform.position.x, transform.position.y);

        //地面判定エリア
        //Vector2型でgraoundAreaインスタンスを定義
        Vector2 groundArea = new Vector2(0.5f, 0.5f);

        
        Vector2 wallArea1 = new Vector2( x * 0.8f, 1.5f);
        Vector2 wallArea2 = new Vector2( x * 0.3f, 1.0f);

        Vector2 wallArea3 = new Vector2(x * 1.5f, 0.6f);
        Vector2 wallArea4 = new Vector2(x * 1.0f, 0.1f);

        /*Vector2 wallArea3 = new Vector2(x * 1.5f, 0.6f);
        Vector2 wallArea4 = new Vector2(x * 1.0f, 0.1f);*/

        //DebugクラスのDrawLineメソッドに引数を渡して実行
        Debug.DrawLine(groundPos + wallArea1, groundPos + wallArea2, Color.red);
        Debug.DrawLine(groundPos + wallArea3, groundPos + wallArea4, Color.red);

        isGround =
            Physics2D.OverlapArea(
                groundPos + groundArea,
                groundPos - groundArea,
                groundLayer
                );

        //坂道に当たり判定を追加

        area1 = false;
        area2 = false;

        area1 =
            Physics2D.OverlapArea(
                groundPos + wallArea1,
                groundPos + wallArea2,
                groundLayer
                );

        area2 =
            Physics2D.OverlapArea(
                groundPos + wallArea3,
                groundPos + wallArea4,
                groundLayer
                );

        if( !area1 & area2)
        {
            isSloped = true;
        }
        else
        {
            isSloped = false;
        }



        //Debug.Log(isSloped);
    }

    //コルーチン
    //IEnumeratorクラスのDeadメソッドは返り値をyield returnで返さないとエラーになる
    IEnumerator Dead()
    {
        anim.SetBool("isDamage", true);
        
        /*コルーチンyield return
        実行を停止して Unity へ制御を戻し、
        その次のフレームで停止したところから続行することができる関数*/
        /*WaitForSeconds()で()内に指定した分、遅延して、次の処理を始められる*/
        yield return new WaitForSeconds(0.5f);


        GetComponent<CircleCollider2D>().enabled = false;
        rb2d.AddForce(Vector2.up * jumpForce);
        Sound.PlaySe("death", 0);
    }

    //敵の攻撃の当たり判定(CircleCircle2D)は敵と重なる(通り抜ける)ことで発生
    //下記では敵とPlayerが重なることPlayer死ぬ
    //物理的接触を発生させないため、is Triggerにチェックをし、OnTriggerEnter2Dメソッドを使用している
    void OnTriggerEnter2D(Collider2D col) //通り抜けたかどうか
    {
        if(col.gameObject.tag == "Enemy")
        {
            isDead = true;
            //コルーチンを呼び出す
            StartCoroutine("Dead");
        }
    }

    void OnCollisionEnter2D(Collision2D col) //乗ったかどうか
    {

        if (col.gameObject.tag == "Enemy")
        {
            //敵を踏んだら少しジャンプする(マリオ風)
            anim.SetBool("isJump", true);
            rb2d.AddForce(Vector2.up * jumpForce);
        }

        if(col.gameObject.tag == "Damage")
        {
            isDead = true;
            //コルーチンを呼び出す
            StartCoroutine("Dead");
        }
    }

}
