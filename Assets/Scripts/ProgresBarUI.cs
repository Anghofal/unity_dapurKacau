using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ProgresBarUI : MonoBehaviour
{
    [SerializeField] private Image barImage;
    [SerializeField] private CuttingCounter cuttingCounter;

    private void Start()
    {
        cuttingCounter.OnProgressChange += CuttingCounter_OnProgressChange;
        barImage.fillAmount = 0;

        Hide();
    }

    private void CuttingCounter_OnProgressChange(object sender, CuttingCounter.OnProgressChangeEventArgs e)
    {
        if (e.progresNormalized != 0f || e.progresNormalized == 1f)
        {
            Show();
        }
        else
        {
            Hide();
        }

        barImage.fillAmount = e.progresNormalized;
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
