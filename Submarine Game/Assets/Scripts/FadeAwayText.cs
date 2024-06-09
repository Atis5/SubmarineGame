using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FadeAwayText : MonoBehaviour
{
    public static FadeAwayText FadeAwayTextScript; // Variable where the script reference is stored.
    public float FadeTime;
    public float CurrentFadeTime;
    private float AlphaValue;
    private float FadeSpeed;
    private TextMeshProUGUI FadeText;



    void OnEnable()
    {
        // Calculations:
        CurrentFadeTime = FadeTime;
        FadeSpeed = 1 / FadeTime;


        //References:
        FadeAwayTextScript = this; // Allows to reference this script in other scripts.
        FadeText = GetComponent<TextMeshProUGUI>();
        AlphaValue = FadeText.color.a;

    }



    void Update()
    {
        if (CurrentFadeTime > 0)
        {
            AlphaValue -= FadeSpeed * Time.deltaTime;
            FadeText.color = new Color(FadeText.color.r, FadeText.color.g, FadeText.color.b, AlphaValue);
            CurrentFadeTime -= Time.deltaTime;
        }

        else if (CurrentFadeTime <= 0)
        {
            FadeText.color = new Color(FadeText.color.r, FadeText.color.g, FadeText.color.b, 255f);
            this.gameObject.SetActive(false);
        }
    }
}
