using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private float flashSpeed = 0.5f;
    
    private Coroutine coroutine;
    
    private void Start()
    {
        CharacterManager.Instance.Player.playerCondition.onTakeDamage += Flash;
        image = GetComponent<Image>();
        flashSpeed = 0.5f;
        image.enabled = false;
    }
    public void Flash()
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        image.enabled = true;
        image.color = new Color(1f, 105f/255f, 105f/255f);
        coroutine = StartCoroutine(FadeAway());
    }
    private IEnumerator FadeAway()
    {
        float startAlpha = 0.3f;
        float a = startAlpha;
        while(a > 0.0f)
        {
            a -= (startAlpha / flashSpeed) * Time.deltaTime;
            image.color = new Color(1f, 100f / 255f, 100f / 255f, a);
            yield return null;
        }
        image.enabled = false;
    }
}
