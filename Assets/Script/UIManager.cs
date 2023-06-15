using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject instructionPanel;
    [SerializeField] private GameObject titlePanel;
    OffObject ob;

    private void Start()
    {
        ob = GetComponent<OffObject>();
        titlePanel.SetActive(true);
        instructionPanel.SetActive(false);
    }

    public void StartGame()
    {
        foreach (GameObject obj in ob.objectsToDisable)
        {
            obj.SetActive(true);
        }
        titlePanel.SetActive(false);
        instructionPanel.SetActive(false);
    }
    public void Menu()
    {
        instructionPanel.SetActive(true);
        titlePanel.SetActive(false);
    }
    public void ReturnTitle()
    {
        instructionPanel.SetActive(false);
        titlePanel.SetActive(true);
    }
}
