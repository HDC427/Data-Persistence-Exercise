using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour
{
    public static UIMenu Instance;
    public InputField nameInput;
    public string playerName;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            Instance.gameObject.SetActive(true);
            return;
        }
        
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        playerName = nameInput.text;
        gameObject.SetActive(false);
        SceneManager.LoadScene(1);
    }
}
