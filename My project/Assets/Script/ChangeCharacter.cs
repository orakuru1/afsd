using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCharacter : MonoBehaviour //リストでバトルに行っても保存しておいて、０番はキャラクターを生成、１番と２番はボタンを生成ってすればいいと思う。
{
    [SerializeField]private GameObject NowPlayer;
    [SerializeField]private  List<GameObject> players = new List<GameObject>();
    [SerializeField]private List<Player> ScriptPlayers = new List<Player>();
    [SerializeField]private GameObject ChangeButton;
    [SerializeField]private Transform ButtonParent;
    [SerializeField]private List<Sprite> sprite = new List<Sprite>();
    [SerializeField]private List<string> playernames = new List<string>();
    private List<GameObject> buttons = new List<GameObject>();
    [SerializeField]private Vector3 spawnposition = new Vector3();

    void Start()
    {
        GameObject fast = Instantiate(NowPlayer);
        NowPlayer = fast;
        playernames.Add(fast.GetComponent<Player>().pn); //インスタンス化された奴の名前を覚えさせる。名前で判断する仲間の数
        playernames.Add("otamesi");
        players.Add(fast);
        ScriptPlayers.Add(fast.GetComponent<Player>());
        SpawnCharaButton();
    }

    // Update is called once per frame
    void Update()
    {
        
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
/*
        foreach(Player player in ScriptPlayers)
        {
            if(player != NowPlayer)
            {
                GameObject button = Instantiate(ChangeButton,ButtonParent);
                buttons.Add(button);
                
                Image btnImage = button.GetComponent<Image>();
                if(btnImage != null)
                {
                    Debug.Log("生成されました");
                    btnImage.sprite = player.sprite; //この要素を渡して、インスタンスさせたところで絵を入れるという処理にしよう
                }
                Button btn = button.GetComponent<Button>();
                btn.onClick.AddListener(() => OnPushChange());
            }

        }
*/
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
            players.Remove(NowPlayer);
            ScriptPlayers.Remove(NowPlayer.GetComponent<Player>());
            spawnposition = NowPlayer.transform.position;
            Destroy(NowPlayer);
        }
        
        if(playernames.Count > 0)
        {
            GameObject playerprefab = (GameObject)Resources.Load(str);
            GameObject newPlayer = Instantiate(playerprefab,spawnposition,Quaternion.identity);
            ScriptPlayers.Add(newPlayer.GetComponent<Player>());
            NowPlayer = newPlayer;

            players.Add(newPlayer);
        }
        else
        {
            Debug.LogWarning("プレイヤーリストが空です!");
        }
        SpawnCharaButton();
    }
}
