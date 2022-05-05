using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ButtonController : MonoBehaviour
{
    public static ButtonController instance;
    void Awake(){
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    public Button[] levleButtons;
   
    // Start is called before the first frame update
    void Start()
    {
        int levelAt = PlayerPrefs.GetInt("levelAt", 1); 
        for(int i = 0; i < levleButtons.Length; i++){
            if(i + 1 > levelAt){
                levleButtons[i].interactable = false;
            }
        }
       
        Debug.Log(levelAt);
    }

    // Update is called once per frame
    void Update()
    {
         for(int i = 1; i <= PlayerPrefs.GetInt("levelAt", 1); i++){
            if(GameObject.Find("Lock" + i)){
                GameObject.Find("Lock" + i).SetActive(false);
            }
        }
    }
    public void LoadLevel1(){
        SceneManager.LoadScene("Level1");
    }
     public void LoadLevel2(){
        SceneManager.LoadScene("Level2");
    }
     public void LoadLevel3(){
        SceneManager.LoadScene("Level3");
    }
    public void ShowSelectLevel(){
        SceneManager.LoadScene("SelectLevel");
    }
    public void ShowMenuGame(){
        SceneManager.LoadScene("Home");
    }
}
