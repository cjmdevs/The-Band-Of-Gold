using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;

public class ClassWeaponsManager : MonoBehaviour
{
    // Main Header
    [Header("Weapons")]

    // Sword Subheader
    [Header("Swordsman's Sword")]
    [SerializeField] private GameObject Sword;
    [SerializeField] private GameObject SwordPrefab;
    [SerializeField] private GameObject SwordInventory;

    // Staff Subheader
    [Header("Mage's Staff")]
    [SerializeField] private GameObject Staff;
    [SerializeField] private GameObject StaffPrefab;
    [SerializeField] private GameObject StaffInventory;

    // Bow Subheader
    [Header("Archer's Bow")]
    [SerializeField] private GameObject Bow;
    [SerializeField] private GameObject BowPrefab;
    [SerializeField] private GameObject BowInventory;

    // Loaded Character
    [Header("Loaded Character")]
    [SerializeField] private string loadedCharacter;

    // Start is called before the first frame update
    void Start()
    {
        // Sword
        Sword.SetActive(false);
        SwordPrefab.SetActive(false);
        SwordInventory.SetActive(false);

        // Staff
        Staff.SetActive(false);
        StaffPrefab.SetActive(false);
        StaffInventory.SetActive(false);

        // Bow
        Bow.SetActive(false);
        BowPrefab.SetActive(false);
        BowInventory.SetActive(false);

        LoadCharacter();
    }
    public void LoadCharacter()
    {
        loadedCharacter = PlayerPrefs.GetString("CharacterSelected");
        switch (loadedCharacter)
        {
            case "Swordsman":
                Sword.SetActive(true);
                SwordPrefab.SetActive(true);
                SwordInventory.SetActive(true);
                break;
            case "Mage":
                Staff.SetActive(true);
                StaffPrefab.SetActive(true);
                StaffInventory.SetActive(true);
                break;
            case "Archer":
                Bow.SetActive(true);
                BowPrefab.SetActive(true);
                BowInventory.SetActive(true);
                break;
        }
    }

    void Update()
    {
        
    }
}

