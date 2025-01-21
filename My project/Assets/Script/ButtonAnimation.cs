using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnimation : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("範囲内だよ");
        // カーソルがボタンに乗ったとき
        animator.ResetTrigger("Hover");
        animator.SetTrigger("Hover");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("外れたよ");
        // 必要に応じてカーソルが外れたときの処理を追加できます
        animator.ResetTrigger("Hover");
        animator.SetTrigger("Hover");
    }
}
