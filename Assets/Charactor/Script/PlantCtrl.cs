using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantCtrl : MonoBehaviour
{
    //Animatorに接続するための変数
    private Animator anim;
    //Playerオブジェクトに接続するための変数
    public GameObject player;

    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        //Playerオブジェクトを探す処理
        player = GameObject.Find("Player");
    //Animatorコンポーネントをanim変数に入れる
        this.anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        //Playerの座標を用意
        //Playerは外部のオブジェクトのため、Gameオブジェクト型の変数で用意をしておく必要がある
        //Gameオブジェクト型player変数の座標をpPos変数へ格納
        Vector2 pPos = player.transform.position;

        //Plantの座標を用意
        //このスクリプトをアタッチしたオブジェクトのため、thisで定義できる
        Vector2 myPos = this.transform.position;

        //距離を把握するためのDistance関数を定義
        //Distanceメソッドの引数になっているオブジェクトの距離を把握する
        float distance = Vector2.Distance(pPos, myPos);

        //距離が小さくなったら攻撃アニメーションするようにする
        //Plantの上にいるときは攻撃をしないようにする
        /*pPos.yからmyPos.yを引いた値が1以下であれば限りなく互いのy軸が同じ位置にいることになるため、
        Plantの上にPlayerがのっかっている場合は、攻撃アニメーションをしなくなる*/
        if ( distance < 4 & ( pPos.y - myPos.y) < 1)
        {
            //SetTriggerでAnimatorに設定したTrigerを実行する
            anim.SetTrigger("TrgAttack");
            
        }

        //Updateメソッドは1フレーム事に実行されるため、敵を倒した後、isDead = trueになり、以下が実行される。
        if (isDead)
        {
            //幽霊の効果
            float level = Mathf.Abs(Mathf.Sin(Time.time));

            //SpritRendererコンポーネントで色が変えられる  
            //Color(R,G,B,幽霊の効果)
            GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,level);
        }
    }

    //Is Triggerにチェックがないため、OnCollisionEnterを使用
    //敵に乗っかったら以下の処理を実行
    void OnCollisionEnter2D(Collision2D col)
    {
        anim.SetTrigger("TrgDead");

        //Coliderを消す処理
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;


        //Deadコルーチンを実行
        StartCoroutine("Dead");

        
    }

    //Deadコルーチン
    IEnumerator Dead()
    {
        //Enemyを倒す(厳密にはEnemyに乗る)と以下の処理が実行される
        //isDeadにはtrueが入る
        isDead = true;
        /*一時的に処理を中断。その間にUpdateメソッド(1フレーム事に実行されるメソッド)が
         実行される。*/
        yield return new WaitForSeconds(1.5f);
        //少し中断した後、以下を実行
        //Destroyで対象のオブジェクトを消す
        Destroy(this.gameObject);

    }
}
