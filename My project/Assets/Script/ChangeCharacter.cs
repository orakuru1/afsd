using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCharacter : MonoBehaviour //リストでバトルに行っても保存しておいて、０番はキャラクターを生成、１番と２番はボタンを生成ってすればいいと思う。
{
    [SerializeField]private GameObject NowPlayer2; //一時しのぎ
    [SerializeField]private GameObject NowPlayer3; //一時しのぎ
    [SerializeField]private GameObject NowPlayer;
    [SerializeField]private GameObject ChangeButton;
    [SerializeField]private GameObject targetcamera;
    [SerializeField]private  List<GameObject> players = new List<GameObject>();
    private List<GameObject> buttons = new List<GameObject>();

    [SerializeField]private Transform spanposition;
    [SerializeField]private Transform ButtonParent;

    [SerializeField]public static List<Player> ScriptPlayers = new List<Player>();

    [SerializeField]private List<Sprite> sprite = new List<Sprite>();

    [SerializeField]private Image FadeImage;
    
    [SerializeField]private List<string> playernames = new List<string>();

    [SerializeField]private Vector3 spawnposition = new Vector3();

    [SerializeField]private Slider hpSlider;
    [SerializeField]private Text playerNameText;

    private CameraMove cameraMove;

    void Start()
    {
        BattleData.Instance.SetImage(FadeImage);
        
        cameraMove = targetcamera.GetComponent<CameraMove>();

        if(BattleData.Instance.mainplayers.Count != 0)
        {
            StartCoroutine(BattleData.Instance.FadeOutBluck(1.5f));//元のシーンに戻ってくるときの演出

            spawnposition = BattleData.Instance.spawnposition;
            foreach(string str in BattleData.Instance.mainplayers)
            {
                playernames.Add(str);
            }
            GameObject playerprefab = (GameObject)Resources.Load(BattleData.Instance.mainplayers[0]);
            GameObject instance = Instantiate(playerprefab, spanposition.position, Quaternion.identity);
            NowPlayer = instance;
            cameraMove.SetUp(NowPlayer.transform);
            players.Add(instance);
            ScriptPlayers.Add(instance.GetComponent<Player>());
            NowPlayer3.GetComponent<EnemyRangedAttack>().bbbb();
            SpawnCharaButton();

            UpdateHPUI(NowPlayer); //HP.UIの更新
        }
        else
        {
            GameObject fast = Instantiate(NowPlayer,spanposition.position,Quaternion.identity);
            //GameObject fast1 = Instantiate(NowPlayer2,spanposition.position,Quaternion.identity);
           //GameObject fast2 = Instantiate(NowPlayer3,spanposition.position,Quaternion.identity);
            NowPlayer = fast;
           // NowPlayer2 = fast1;
            //NowPlayer3 = fast2;
            cameraMove.SetUp(NowPlayer.transform);
            playernames.Add(fast.GetComponent<Player>().pn); //インスタンス化された奴の名前を覚えさせる。名前で判断する仲間の数
            playernames.Add("mahou"); //お試しで仲間を増やしてる
            players.Add(fast);
            ScriptPlayers.Add(fast.GetComponent<Player>());
            
            if(NowPlayer2 != null)
            {
                NowPlayer2.GetComponent<EnemyTG>().aaaa();
                NowPlayer3.GetComponent<EnemyTG>().aaaa();
                NowPlayer3.GetComponent<EnemyRangedAttack>().bbbb();
            }

            BattleData.Instance.currentplayers(playernames);
            SpawnCharaButton();

            UpdateHPUI(NowPlayer);
        }
        NowPlayer.name = "Player";

    }

    // Update is called once per frame
    void Update()
    {
        NowPlayer.name = "Player";

        //HPスライダーのリアルタイム更新
        if(NowPlayer != null)
        {
            CharacterStatus status = NowPlayer.GetComponent<CharacterStatus>();
            if(status != null)
            {
                hpSlider.value = status.currentHP;
            }
        }
    }

    public void PlayerNameAdd(string playername)
    {
        playernames.Add(playername);
    }

    private void SpawnCharaButton() //ボタンを押したら生成するようにする。playernameがある数分だけやるほうがいいのかわからない。仲間をどこで判断するのか？
    {
        foreach(GameObject button in buttons)
        {
            Destroy(button);
        }
        buttons.Clear();

        foreach(string str in playernames)
        {
            if(str != NowPlayer.GetComponent<Player>().pn)
            {
                GameObject button = Instantiate(ChangeButton,ButtonParent);
                buttons.Add(button);

                Image btnImage = button.GetComponent<Image>();
                for(int i = 0; i < sprite.Count; i++)
                {
                    if(str == sprite[i].name)
                    {
                        btnImage.sprite = sprite[i];
                    }
                }

                Button btn = button.GetComponent<Button>();
                btn.onClick.AddListener(() => OnPushChange(str)); 
            }

        }
    }
    public void OnPushChange(string str)
    {
        if(NowPlayer != null)
        {
            playernames.Remove(str);
            playernames.Insert(0,str);
            BattleData.Instance.currentplayers(playernames);
            players.Remove(NowPlayer); //破壊する前にリストから削除
            ScriptPlayers.Remove(NowPlayer.GetComponent<Player>()); //破壊する前にplayerをリストから削除
            spawnposition = NowPlayer.transform.position; //破壊される前の位置を記憶
            Destroy(NowPlayer); //現在のプレイヤーを破壊
        }
        
        if(playernames.Count > 0)
        {
            GameObject playerprefab = (GameObject)Resources.Load(str);
            GameObject newPlayer = Instantiate(playerprefab,spawnposition,Quaternion.identity);
            ScriptPlayers.Add(newPlayer.GetComponent<Player>());
            NowPlayer = newPlayer;
            cameraMove.SetUp(NowPlayer.transform);

            players.Add(newPlayer);
            
            UpdateHPUI(newPlayer);
        }
        else
        {
            Debug.LogWarning("プレイヤーリストが空です!");
        }
        SpawnCharaButton();
    }
    private void UpdateHPUI(GameObject player)
    {
        if(player == null) return;

        CharacterStatus characterStatus = player.GetComponent<CharacterStatus>();
        if(characterStatus != null)
        {
            hpSlider.maxValue = characterStatus.maxHP;
            hpSlider.value = characterStatus.currentHP;
            playerNameText.text = characterStatus.CharacterName;
            Debug.Log("アタッチされてます");
        }
        else
        {
            Debug.Log("新しいプレイヤーにcharacterstatusがアタッチされていません");
        }
    }
}
