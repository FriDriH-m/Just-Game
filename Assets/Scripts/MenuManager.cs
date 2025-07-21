using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _optionsMenu;
    public void StartClick()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void OptionsClick()
    {
        _mainMenu.SetActive(false);
        _optionsMenu.SetActive(true);
    }
    public void BackClick()
    {
        _mainMenu.SetActive(true);
        _optionsMenu.SetActive(false);
    }
    public void ExitClick()
    {
        Application.Quit();
    }
}
