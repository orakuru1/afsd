using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;       // 追従するキャラクター（例えばプレイヤー）のTransform
    [SerializeField]private Transform teiten;
    public Vector3 offset;         // カメラのオフセット（キャラクターからの位置）
    private Vector3 SavePosition = new Vector3();

    public float smoothSpeed = 0.125f; // カメラ追従のスムーズさ
    public float rotationSpeed = 2.5f;

    private float rotationX = 0f; //上下回転の角度
    private float rotationY = 0f; //左右回転の角度
    private bool isCameraMove = false;
    private bool isScene = false;

    [SerializeField]private BattleManager battleManager;
    // Start is called before the first frame update
    void Start()
    {
        rotationY = transform.eulerAngles.y; //初期角度を設定
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUp(Transform transform)
    {
        target = transform;
    }

    public void CharacterToEnemy(Vector3 PlayerPosition) //カメラの位置をキャラクターの上に変える。スムーズに動けるようにしたい
    {
        SavePosition = transform.position;
        transform.position = PlayerPosition + new Vector3(0f,2.5f,-3f);
    }
    public IEnumerator ComeBuckCamera() //位置を元に戻す
    {
        yield return new WaitForSeconds(0.7f);
        transform.position = SavePosition;
        target = teiten;
    }

    public void rotationcamera(float duration)
    {
        StartCoroutine(KaitenCamera(duration));
    }

    private IEnumerator KaitenCamera(float duration)//左右に開店して、敵を見せる
    {
        isCameraMove = true;
        float elapsedTime = 0f;
        Vector3 originalOffset = offset;// 現在のカメラ位置を保存
        Vector3 targetOffset = (offset += new Vector3(-6f,0f,0f));

        while(elapsedTime < duration)
        {
            offset = Vector3.Lerp(originalOffset, targetOffset, elapsedTime / duration);//だんだん回転させる
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;
        Vector3 homeOffset = originalOffset += new Vector3(-3f,0f,0f);

        while(elapsedTime < (duration / 2))//上の処理の２倍の速さでやる
        {
            offset = Vector3.Lerp(targetOffset, originalOffset, elapsedTime / (duration / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        offset = homeOffset;
        isCameraMove = false;
    }
    public void zoingcamera(float zoomAmount, float duration, bool isbuck)
    {
        //ここに敵に当たった時のカメラの動きを作る
        StartCoroutine(ZoomCamera(zoomAmount, duration, isbuck));
    } 

    public void zoingoutcamera(float zoomAmount, float duration)
    {
        //ここに敵に当たった時のカメラの動きを作る
        StartCoroutine(ZoomOutCamera(zoomAmount, duration));
    } 

    private IEnumerator ZoomOutCamera(float zoomAmount, float duration) //カメラがターゲットから遠ざかる処理
    {
        while(isCameraMove == true)
        {
            yield return null;
        }

        float elapsedTime = 0f;
        Vector3 originalOffset = offset; // 現在のカメラ位置を保存
        Vector3 targetOffset = offset.normalized * (zoomAmount + originalOffset.magnitude) + new Vector3(0f,0f,-10f) ; // 適切なズームアウト量を計算

        while (elapsedTime < duration)
        {
            offset = Vector3.Lerp(originalOffset, targetOffset, elapsedTime / duration); // 徐々にズームアウト
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        offset = targetOffset; // 最終的なズームアウト位置を確定

        if(battleManager != null)
        {
            battleManager.saisyonohyouzi();//**********************************邪魔だから消しておく。本番はオン
        }

        isScene = true;
        
        //StartCoroutine(SkyCamera());
    }

    private IEnumerator ZoomCamera(float zoomAmount, float duration, bool isbuck) //********************x0,y2,z11
    {
        float elapsedTime = 0f;
        Vector3 originalOffset = offset; //元のカメラの距離を保存
        Vector3 targetOffset = offset.normalized * zoomAmount; //最終的な目的地     単位ベクトル（長さ１の距離）に目的までの距離を掛ける,元の方向を向いてるまま すでに指定して相対的な位置

        targetOffset += new Vector3(0f,1f,0f);

        while (elapsedTime < duration)
        {
            offset = Vector3.Lerp(originalOffset, targetOffset, elapsedTime / duration); //徐々にズーム
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        offset = targetOffset;
        elapsedTime = 0f;
        
        if(isbuck) //最初っから敵に近寄ってるほうがいいのかも*******元に戻る処理
        {
            yield return new WaitForSeconds(1f);

            while(elapsedTime < duration)
            {
                offset = Vector3.Lerp(targetOffset, originalOffset, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        offset = originalOffset;
    }
    private IEnumerator SkyCamera() //ターン制の基本位置*******使っていない？
    {
        target = teiten;
        Vector3 StartOffset = offset;
        float duratin = 1f;
        float elapsedTime = 0f;

        while(elapsedTime < duratin)
        {
            offset = Vector3.Lerp(StartOffset, new Vector3(0f,2f,-11f), elapsedTime / duratin);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
    }

    void LateUpdate()//*******プレイヤーが攻撃した後に２段階でカメラの視点が変わってる。どうしよう
    {
        if(target == null)
        {
            //  カメラを元に戻す前に何かアニメーションを入れて違和感なくしたい。
            StartCoroutine(ComeBuckCamera());
        }

        if (Input.GetMouseButton(1)) // 右クリックで回転を有効にする場合
        {
           float horizontal = Input.GetAxis("Mouse X") * rotationSpeed; //左右回転
           float vertical = Input.GetAxis("Mouse Y") * rotationSpeed; //上下回転

           //横回転(Y軸回転)
           offset = Quaternion.AngleAxis(horizontal, Vector3.up) * offset;

           //縦回転(X軸回転)
           Vector3 right = Vector3.Cross(Vector3.up, offset); //カメラの右方向
           offset = Quaternion.AngleAxis(-vertical, right) * offset;

           //上下の角度制限(90度以上に開店しないように)
           float angleX = Vector3.Angle(offset, Vector3.up);
           if(angleX < 40f) //真上を向かないよう制限
              offset = Quaternion.AngleAxis(40f - angleX, right) * offset;
           if(angleX > 90f)
              offset = Quaternion.AngleAxis(90f - angleX, right) * offset;

        }

        if(isScene == false)
        {
            // 目的地の位置を計算
            Vector3 desiredPosition = target.position + offset;
            // スムーズにカメラを移動させる
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition + new Vector3(0f,1f,0f), smoothSpeed);
            transform.position = smoothedPosition;
        }


        // カメラをキャラクターの方向に向ける
        transform.LookAt(target);
    }
}
