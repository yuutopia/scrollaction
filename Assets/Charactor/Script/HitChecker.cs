using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitChecker : MonoBehaviour
{
    //UnityのInspector上で値をチェックするために変数を宣言

    public bool isGroundHit;

    public bool isPlayerHit;

    public bool isEnemyHit;
　　

    // Start is called before the first frame update
    void Start()
    {
            
    }

    //Is Trrigerをチェックしているオブジェクトのため、OnTriggerEnter2Dを使用
    void OnTriggerEnter2D(Collider2D col)
    {
        //HitChecker.csがアタッチされたオブジェクトが、ステージマップのコライダーに乗ったかどうかを判定
        if(col.gameObject.name == "StageMap")
        {
            isGroundHit = true;
            Debug.Log("Ground");
        }
        //HitChecker.csがアタッチされたオブジェクトが、Playerのコライダーに当たったかどうかを判定
        if (col.gameObject.name == "Player")
        {
            isPlayerHit = true;
            Debug.Log("Player");
        }
    
        if (col.gameObject.tag == "Enemy")
        {
            isEnemyHit = true;
            Debug.Log("Enemy");
        }


    }

    void OnTriggerExit2D(Collider2D col)
    {
        //ステージマップからはなれた
        if(col.gameObject.name == "StageMap")
        {
            isGroundHit = false;
        }

        //プレイヤーからはなれた
        if(col.gameObject.name == "Player")
        {
            isPlayerHit = false;
        }

        //敵からはなれた
        if (col.gameObject.tag == "Enemy")
        {
            isEnemyHit = false;
        }

    }




    //コライダー用のスクリプトのため、Updaeメソッドは使用しない
    /*
      // Update is called once per frame
      void Update()
      {

      }
      */
}
