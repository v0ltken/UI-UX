using UnityEngine;
using UnityEngine.UI;

public class PanelSwitcher : MonoBehaviour
{
    [Header("Panels")]
    public GameObject panel1;
    public GameObject panel2;
    public GameObject panel3;

    [Header("Buttons")]
    public Button button1;
    public Button button2;
    public Button button3;

    void Start()
    {
        // Assign button click listeners
        button1.onClick.AddListener(() => ShowPanel(1));
        button2.onClick.AddListener(() => ShowPanel(2));
        button3.onClick.AddListener(() => ShowPanel(3));

        // Show first panel by default
        ShowPanel(1);
    }

    public void ShowPanel(int panelNumber)
    {
        // Deactivate all panels
        panel1.SetActive(false);
        panel2.SetActive(false);
        panel3.SetActive(false);

        // Activate the selected panel
        switch (panelNumber)
        {
            case 1:
                panel1.SetActive(true);
                break;
            case 2:
                panel2.SetActive(true);
                break;
            case 3:
                panel3.SetActive(true);
                break;
        }
    }
}