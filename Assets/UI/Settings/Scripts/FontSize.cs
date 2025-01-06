using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FontSize : MonoBehaviour
{
    [SerializeField] private Slider fontSizeSlider;
    public List<TextMeshProUGUI> texts = new();
    private List<float> textsOriginalSize = new();
    private float fontSize;
    private float sliderValue;

    void Start(){
        if (texts.Count > 0)
        {
            foreach (TextMeshProUGUI text in texts) 
            {
                float originalSize = text.fontSize;
                textsOriginalSize.Add(originalSize);
            }

            if (PlayerPrefs.HasKey("currentFontSize"))
            {
                LoadFontSize();
            }
            else
            {
                AdjustFontSize();   
            }
        }
    }

    public void AdjustFontSize()
    {
        sliderValue = fontSizeSlider.value;
        fontSize = sliderValue/100f;

        for (int i = 0; i < texts.Count; i++) 
        {
            texts[i].fontSize = textsOriginalSize[i] * fontSize;
        }

        PlayerPrefs.SetFloat("currentFontSize", fontSize);
    }

    public void LoadFontSize()
    {
        fontSizeSlider.value = PlayerPrefs.GetFloat("currentFontSize");
        AdjustFontSize();
    }
}