using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsUI : MonoBehaviour
{   
    private WeaponInfo weaponInfo;
    public GameObject[] statsSlots;
    public CanvasGroup statsCanvas;
    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void Start()
    {
        
        UpdateAllStats();
        
    }
    private void Update()
    {
        if(playerControls.Stats.OpenUI.triggered == true){
            statsCanvas.alpha = 0;
        }
    }

    public void UpdateDamage()
    {
        statsSlots[0].GetComponentInChildren<TMP_Text>().text = "Damage: " + StatsManager.Instance.damage;
    }
    public void UpdateSpeed()
    {
        statsSlots[1].GetComponentInChildren<TMP_Text>().text = "Speed: " + StatsManager.Instance.speed;
    }

    public void UpdateAllStats(){
        UpdateDamage();
        UpdateSpeed();
    }
}
