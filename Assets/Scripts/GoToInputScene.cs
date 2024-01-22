using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToInputScene : MonoBehaviour
{
    public void LoadInputScene()
    {
        SceneManager.LoadScene("InputScene");
    }
}
