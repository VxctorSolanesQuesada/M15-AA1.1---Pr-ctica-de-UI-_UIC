using UnityEngine;

public class ChangePanel : MonoBehaviour
{
    public GameObject artPanel;
    public GameObject optionsPanel;

    public void ChangeToArtPanel()
    {

        artPanel.SetActive(true);
        optionsPanel.SetActive(false);

    }

    public void ChangeToOptionsPanel()
    {
        artPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }
}