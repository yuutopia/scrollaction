using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    /*GameObjectクラス型で変数を宣言することで、UnityのInspector上で
    スクリプトにオブジェクトを指定することができる*/
    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position変数をVector3型でインスタンス化
        //引数にカメラの対象オブジェクトの位置を指定することで、カメラをオブジェクトに追従するようにする
        //今回は横移動のみのため、Playerのx軸の位置(値)にMainCameraを追従させる
        //y、zはMainCameraオブジェクトの値(position)を保つ
        //transform.position変数はスクリプトをADDしたオブジェクトの位置を決めている
        transform.position = new Vector3(
            //各フィールドに値を指定

            //Playerオブジェクトのx軸を値として返す
            Player.transform.position.x,

            //このy,x軸(MainCameraの)を値として返す
            this.transform.position.y,
            this.transform.position.z
            );

        //MainCameraのX軸(ここではキャラクターと同じ)がスタート地点(x軸＝0以下)になったとき
      　//MainCameraのx軸を0にする
        if(this.transform.position.x < 0)
        {
            transform.position = new Vector3(
            0,
            this.transform.position.y,
            this.transform.position.z
            ) ;
        }
        
    }
}
