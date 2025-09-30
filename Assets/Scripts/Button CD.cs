using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonCD : MonoBehaviour 
{
    [SerializeField] private Image cooldownImage;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI timer2;

    private float timer;
    private float maxTimer;
    private bool active;

    void Start()
    {
        if (cooldownImage != null) cooldownImage.fillAmount = 0f;
        if (timerText != null) timerText.text = "";
        if (timer2 != null) timer2.text = "";
    }

    void Update()
    {
        if (active)
        {
            timer -= Time.deltaTime;
            float ratio = Mathf.Clamp01(timer / maxTimer);

            if (cooldownImage != null) cooldownImage.fillAmount = ratio;
            if (timerText != null) timerText.text = Mathf.Ceil(timer).ToString();
            if (timer2 != null) timer2.text = Mathf.Ceil(timer).ToString();

            if (timer <= 0)
            {
                active = false;
                if (cooldownImage != null) cooldownImage.fillAmount = 0f;
                if (timerText != null) timerText.text = "";
                if (timer2 != null) timer2.text = "";
            }
        }
    }

    public void StartTimer(float duration)
    {
        maxTimer = duration;
        timer = duration;
        active = true;
        if (cooldownImage != null) cooldownImage.fillAmount = 1f;
    }
}
