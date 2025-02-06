using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;       // 追従するキャラクター（例えばプレイヤー）のTransform
    [SerializeField]private Transform teiten;
    public Vector3 offset;         // カメラのオフセット（キャラクターからの位置）
    public float smoothSpeed = 0.125f; // カメラ追従のスムーズさ
    public float rotationSpeed = 5.0f;
    private bool isCameraMove = false;
    [SerializeField]private BattleManager battleManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUp(Transform transform)
    {
        target = transform;
    }

    public void rotationcamera(float duration)
    {
        StartCoroutine(KaiatenCamera(duration));
    }
    private IEnumerator KaiatenCamera(float duration)
    {
        isCameraMove = true;
        float elapsedTime = 0f;
        Vector3 originalOffset = offset;
        Vector3 targetOffset = (offset += new Vector3(-6f,0f,0f));

        while(elapsedTime < duration)
        {
            offset = Vector3.Lerp(originalOffset, targetOffset, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;
        Vector3 homeOffset = originalOffset += new Vector3(-3f,0f,0f);

        while(elapsedTime < (duration / 2))
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

    private IEnumerator ZoomOutCamera(float zoomAmount, float duration)
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
            battleManager.saisyonohyouzi();
        }
        
        //StartCoroutine(SkyCamera());
    }

    private IEnumerator ZoomCamera(float zoomAmount, float duration, bool isbuck) //********************x0,y2,z11
    {
        float elapsedTime = 0f;
        Vector3 originalOffset = offset; //元のカメラの距離を保存
        Vector3 targetOffset = offset.normalized * zoomAmount; //最終的な目的地     単位ベクトル（長さ１の距離）に目的までの距離を掛ける,元の方向を向いてるまま すでに指定して相対的な位置

        targetOffset += new Vector3(0f,2f,0f);

        while (elapsedTime < duration)
        {
            offset = Vector3.Lerp(originalOffset, targetOffset, elapsedTime / duration); //徐々にズーム
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;
        
        if(isbuck) //最初っから敵に近寄ってるほうがいいのかも
        {
            yield return new WaitForSeconds(1f);

            while(elapsedTime < duration)
            {
                offset = Vector3.Lerp(targetOffset, originalOffset, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
    private IEnumerator SkyCamera()
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

    void LateUpdate()
    {
        if (Input.GetMouseButton(1)) // 右クリックで回転を有効にする場合
        {
            float horizontal = Input.GetAxis("Mouse X") * rotationSpeed;
            offset = Quaternion.AngleAxis(horizontal, Vector3.up) * offset;
        }

        // 目的地の位置を計算
        Vector3 desiredPosition = target.position + offset;
        // スムーズにカメラを移動させる
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // カメラをキャラクターの方向に向ける
        transform.LookAt(target);
    }
}
