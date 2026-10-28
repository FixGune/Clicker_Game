using DG.Tweening;
using Interfaces;
using Manager;
using UnityEngine;

public class ClickableStone : MonoBehaviour, IClickable
{

    [SerializeField] private float scale = 4f;
    [SerializeField] private float clickScale = 3f;
    [SerializeField] private float speedScale = 0.1f;

    private void Awake()
    {
        transform.localScale = Vector3.one * scale;
    }
    public void OnClick()
    {
        PlayerManager.Instance.CurrentCash++;
        DOTween.Kill(this, true);
        transform.DOScale(clickScale,speedScale).SetLoops(2,LoopType.Yoyo).OnComplete(()=> transform.localScale = Vector3.one * scale);
    }
}
