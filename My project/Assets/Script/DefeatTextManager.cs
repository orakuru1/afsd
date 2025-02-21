using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DefeatTextManager : MonoBehaviour
{
    public GameObject defeatText; // 討伐完了テキスト
    public Text textComponent;
    private bool isTextActive = false; // テキストが表示中か判定
    private bool canClose = false; //拡大完了するまでクリック無効

    void Start()
    {
        defeatText.SetActive(false); // 最初は非表示
    }

    // 討伐完了テキストを表示して全体を停止
    public void ShowDefeatText()
    {
        defeatText.SetActive(true);
        isTextActive = true;
        canClose = false;
        Time.timeScale = 0; // **ゲーム全体を停止**

        StartCoroutine(ScaleTextEffect()); //テキスト拡大アニメーション
    }

    void Update()
    {
        // クリックでテキストを消してゲームを再開
        if (isTextActive && canClose && Input.GetMouseButtonDown(0))
        {
            defeatText.SetActive(false);
            isTextActive = false;
            Time.timeScale = 1; // **ゲーム再開**
        }
    }
    IEnumerator ScaleTextEffect()
    {
        float duration = 2.0f; //拡大にかかる時間
        float elapsed = 0f;
        Vector3 startScale = Vector3.one * 1.2f; //初期サイズ
        Vector3 endScale = Vector3.one * 5.0f; //最終サイズ

        textComponent.transform.localScale = startScale;

        while(elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime; //停止中でも時間をカウント
            float t = elapsed / duration;
            textComponent.transform.localScale = Vector3.Lerp(startScale, endScale,t);
            yield return null;
        }
        canClose = true;
    }
}
