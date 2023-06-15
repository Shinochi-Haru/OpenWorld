using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject titlePanel;
    OffObject ob;

    private void Start()
    {
        ob = GetComponent<OffObject>();
        titlePanel.SetActive(true);
    }

    public void StartGame()
    {
        foreach (GameObject obj in ob.objectsToDisable)
        {
            obj.SetActive(true);
        }
        titlePanel.SetActive(false);
    }
}
