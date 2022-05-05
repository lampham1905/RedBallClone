using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameGUIManager : MonoBehaviour
{
    public GameObject PauseGameGUI;
    public static GameGUIManager Ins;
    private void Awake()
    {
        if(Ins == null)
        {
            Ins = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
        
    [SerializeField] private Image powerBarSlider; 

    public void UpdatePowerBar(float curVal, float totalVal)
    {
        if(powerBarSlider)
        {
            powerBarSlider.fillAmount = curVal / totalVal;
        }
    }
    public void CheckHp(int hp){
        if(hp == 2){
            GameObject.Find("Hp3").SetActive(false);
        }
        else if(hp == 1){
            GameObject.Find("Hp2").SetActive(false);
        }
        else if(hp == 0){
            GameObject.Find("Hp1").SetActive(false);
        }

    }
 
    public void ShowPauseGameGUI(){
        PauseGameGUI.SetActive(true);
        PlayerController.instance.canMove = false;
    }
    public void HidePauseGameGUI(){
        PauseGameGUI.SetActive(false);
        PlayerController.instance.canMove = true;
    }
    public void RestartScene(){
        PlayerController.instance.RestartScene();
    }
    public void ShowSelectLevel(){
        ButtonController.instance.ShowSelectLevel();
    }


    
}
