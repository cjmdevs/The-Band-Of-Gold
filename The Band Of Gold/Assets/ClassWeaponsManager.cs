using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassWeaponsManager : MonoBehaviour
{
    [Header("Weapons")]
    [SerializeField] private GameObject Sword;
    [SerializeField] private GameObject SwordPrefab;
    [SerializeField] private GameObject Staff;
    [SerializeField] private GameObject StaffPrefab;
    [SerializeField] private GameObject Bow;
    [SerializeField] private GameObject BowPrefab;

    // Start is called before the first frame update
    void Start()
    {
        Sword.SetActive(false);
        SwordPrefab.SetActive(false);
        Staff.SetActive(false);
        StaffPrefab.SetActive(false);
        Bow.SetActive(false);
        BowPrefab.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
