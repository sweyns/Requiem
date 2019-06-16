using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	int totalScenes = 4;
	public int currentLevel = 0;
	public Animator animator;
    [SerializeField]
    public Light playerLight;
    // Start is called before the first frame update

    void Awake() 
    {
    	currentLevel = PlayerPrefs.GetInt("currentLevel");
    }
    void Start()
    {

        currentLevel = SceneManager.GetActiveScene().buildIndex;
    }
    public void LoadNextScene() {
    	//int scene = SceneManager.GetActiveScene().buildIndex;
    	if (currentLevel != totalScenes - 1) {
    		currentLevel += 1;
    		PlayerPrefs.SetInt("currentLevel", currentLevel);
    		
    		animator.SetTrigger("FadeOut");
    	}
    }

    public void IncreaseRange() {
        playerLight.range += 5;
        PlayerPrefs.SetInt("lightRange", PlayerPrefs.GetInt("lightRange") + 1);
    }

    public void LoadPreviousScene() {
    	//int scene = SceneManager.GetActiveScene().buildIndex;
    	if (currentLevel != 0) {
    		currentLevel -= 1;
    		PlayerPrefs.SetInt("currentLevel", currentLevel);
    		animator.SetTrigger("FadeOut");
    		
    	}
    	
    }

    public void OnFadeComplete() {
    	SceneManager.LoadScene(currentLevel);
    }
    void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
    }
}
