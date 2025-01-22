using UnityEngine;

public class BGMSwitcher : MonoBehaviour
{
    public AudioSource audioSource; // 再生用のAudioSource
    public AudioClip newBGM;        // 切り替え先のBGM

    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーがトリガーに入ったとき
        if (other.CompareTag("Player")) 
        {
            if (audioSource.clip != newBGM) // 既に再生中のBGMと同じでない場合
            {
                audioSource.Stop();       // 現在のBGMを停止
                audioSource.clip = newBGM; // 新しいBGMをセット
                audioSource.Play();       // 新しいBGMを再生
            }
        }
    }
}
