using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyTrigger : MonoBehaviour
{

    [SerializeField]private string enemyName = "Goblin"; // 敵の名前
    private int enemyHealth = 100; // 敵の体力
    private bool isstart = true;
    private CameraMove cameraMove;

    [SerializeField]private AudioSource audioSource;
    [SerializeField]private AudioClip StartBattle;
    public void SetCamera()
    {
        cameraMove.SetUp(this.gameObject.transform);
    }
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if(BattleData.Instance.isconhurikut())//一回だけ行われる処理
            {
                // BattleDataに敵の情報を設定
                BattleData.Instance.SetEnemyData(enemyName, enemyHealth);

                BattleData.Instance.RePosition(collision.collider.transform.position);

                // 戦闘シーンに遷移
                //SceneManager.LoadScene(battleSceneName);
                
                StartCoroutine(BattleData.Instance.LoadBattleScene()); // 非同期ロード
                if(StartBattle != null)
                {
                    audioSource.PlayOneShot(StartBattle);
                }
                StartCoroutine(BattleData.Instance.FadeInBuluck(1.5f));
                SetCamera();//プレイヤーに当たったのをターゲットしてる
                cameraMove.zoingcamera(0f,2f,false);//カメラがズームする処理

            }
            else
            {
                Debug.Log("バトルに入る処理をしてるよ");
            }
        }
    }

    public void TriggerStart()
    {
        isstart = false;
    }
    
    void Start()
    {
        cameraMove = Camera.main.GetComponent<CameraMove>(); // メインカメラのスクリプトを取得
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
