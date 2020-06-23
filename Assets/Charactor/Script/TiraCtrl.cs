using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TiraCtrl : MonoBehaviour
{

    private Animator anim;

    private SpriteRenderer spRenderer;

    private Rigidbody2D rb2d;

    private GameObject player;

    //エフェクトを入れる変数
    public GameObject fxhit;

    public float speed = 15;
    private bool isAttack = false;
    private bool isDead = false;
    private bool isIdle = false;


    //HitCheckerの項目にチェックが入っているか確認する変数
    private HitChecker sChecker; //横の当たり判定
    private HitChecker gChecker; //下の当たり判定


　

    // Start is called before the first frame update
    void Start()
    {
        //スタートしたら、以下のコンポーネントを読み込んでおく

        player = GameObject.Find("Player");
        anim = GetComponent<Animator>();
        spRenderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();

        //子オブジェクトを探す
        //sideChecker(当たり判定用の空オブジェクト)のコンポーネントであるHitCheckerを読み込む
        sChecker = transform.Find("sideChecker").gameObject.GetComponent<HitChecker>();
        //groundChecker(当たり判定用の空オブジェクト)のコンポーネントであるHitCheckerを読み込む
        gChecker = transform.Find("groundChecker").gameObject.GetComponent<HitChecker>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //動かすときはUpdaeの中でプログラムする 

        float x = 1;

        //Debug.Log(this.transform.eulerAngles.y);

        //rotationはオブジェクトの向き
        if (this.transform.eulerAngles.y == 180)
        {
            x = -1;
        }
        else
        {
            x = 1;
        }

        CheckValue();

    　 //HitCheckerのisPlayerHitにチェックが入っている(true)であれば、攻撃アニメーションへ遷移
       if (sChecker.isPlayerHit)
        {
            if (!isAttack)
            {
                StartCoroutine("Attack");
            }

            //Playerに攻撃しているときは移動しない
            rb2d.velocity = new Vector2(0, 0);  
        }


        //rb2d.AddForce(Vector2.right * x * speed );         
        

    if(!isIdle & !isAttack & !isDead)
        {
            anim.SetBool("isWalk", true);
            rb2d.AddForce(Vector2.right * x * speed);
        }
        else
        {
            anim.SetBool("isWalk", false);
            rb2d.velocity = new Vector2(0,0);
        }

        //Updateメソッドは1フレーム事に実行されるため、敵を倒した後、isDead = trueになり、以下が実行される。
        if (isDead)
        {
            //幽霊の効果
            float level = Mathf.Abs(Mathf.Sin(Time.time));

            //SpritRendererコンポーネントで色が変えられる  
            //Color(R,G,B,幽霊の効果)
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, level);
        }
    }

    private void CheckValue()
    {
        if (!gChecker.isGroundHit & !isIdle)
        {
            gChecker.isGroundHit = true;
            StartCoroutine("ChangeRotate");
        
        }

        if (sChecker.isEnemyHit & !isIdle)
        {
            sChecker.isEnemyHit = false;
            StartCoroutine("ChangeRotate");

        }

        if (sChecker.isGroundHit & !isIdle)
        {
            sChecker.isGroundHit = false;
            StartCoroutine("ChangeRotate");

        }

    }

    void OnCollisionEnter2D(Collision2D col)
    {

        if(col.gameObject.name == "Player")
        {
            anim.SetTrigger("trgDead");
            //Deadコルーチンを実行
            StartCoroutine("Dead");
            

            

        }
 
    }

    IEnumerator ChangeRotate()
    {
        isIdle = true;

        yield return new WaitForSeconds(1.5f);

        if (this.transform.eulerAngles.y == 180)
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        isIdle = false;
    }


    IEnumerator Attack()
    {
        isAttack = true;
        anim.SetTrigger("trgAttack");
        yield return new WaitForSeconds(1.5f);
        isAttack = false;

    }


    IEnumerator Dead()
    {
        isDead = true;

        //Coliderを消す処理
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;

        yield return new WaitForSeconds(1.5f);

        //エフェクトを呼び出す
        Instantiate(fxhit, transform.position, transform.rotation);

        Destroy(this.gameObject);
    }
}
