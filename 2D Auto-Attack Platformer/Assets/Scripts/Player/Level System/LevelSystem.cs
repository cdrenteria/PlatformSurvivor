using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelSystem : MonoBehaviour
{
    public int currentExperience;
    private int experienceUntilNextLevel;
    public List<GameObject> allWeapons; //added through editor
    public List<GameObject> allPassives;
    private List<GameObject> upgradeOptionList;
    public List<GameObject> weaponSlots; //added through editor
    public List<GameObject> passiveSlots; //added through editor
    private int playerLevel;
    private int openWeaponSlots;
    private int openPassiveSlots;
    private UpgradeMenu upgradeMenu;
    System.Random rnd = new System.Random();


    // Start is called before the first frame update
    void Start()
    {
        currentExperience = 0;
        upgradeMenu = GameObject.FindGameObjectWithTag("UpgradeMenu").GetComponent<UpgradeMenu>();
        upgradeMenu.gameObject.SetActive(false);
        playerLevel = 1;
    }

    public void AddExperience(int experience)
    {
        currentExperience += experience;
        if (currentExperience >= experienceUntilNextLevel)
        {
            LevelUp();
        }
    }
    public void LevelUp()
    {
        playerLevel++;
        IncreaseExperienceUntilNextLevel(playerLevel);
        InitializeUpgradeMenu(GetRandomUpgrades());

    }

    private void IncreaseExperienceUntilNextLevel(int playerLevel)
    {
        experienceUntilNextLevel += playerLevel;
    }

    private List<GameObject> GetRandomUpgrades()
    {
        //find open weapon slots
        for (int x = 0; x < 3; x++)
        {
            if (weaponSlots[x].transform.childCount == 0)
            {
                openWeaponSlots++;
            }
        }
        //Find all passive slots
        for (int x = 0; x < 3; x++)
        {
            if (passiveSlots[x].transform.childCount == 0)
            {
                openPassiveSlots++;
            }
        }
        print("open weapons slots :" + openWeaponSlots);
        print("open passive slots :" + openPassiveSlots);
        // if there is an open slot offer a random selection of all items
        if (openWeaponSlots > 0 && openPassiveSlots > 0)
        {
            upgradeOptionList = new List<GameObject>();
            upgradeOptionList.AddRange(allPassives);
            upgradeOptionList.AddRange(allWeapons);

        }
        else if (openPassiveSlots > 0 && openWeaponSlots <= 0)
        {    
            upgradeOptionList = new List<GameObject>();
            AddPlayerItems(weaponSlots);
            upgradeOptionList.AddRange(allPassives);
        } else if (openWeaponSlots > 0 && openPassiveSlots <=0)
        {
            upgradeOptionList = new List<GameObject>();
            AddPlayerItems(passiveSlots);
            upgradeOptionList.AddRange(allWeapons);
        } else
        {
            upgradeOptionList = new List<GameObject>();
            AddPlayerItems(weaponSlots);
            AddPlayerItems(passiveSlots);
        }
        RandomizeAndTrimList(upgradeOptionList);
        openPassiveSlots = 0;
        openWeaponSlots = 0;
        return upgradeOptionList;
    }

    private void AddPlayerItems(List<GameObject> itemSlots)
    {
        for (int x = 0; x < 3; x++)
        {
            if (itemSlots[x].transform.childCount != 0)
            {
                openWeaponSlots++;
                upgradeOptionList.Add(itemSlots[x].transform.GetChild(0).gameObject);
            }
        }
    }

    private void RandomizeAndTrimList(List<GameObject> List)
    {
        for (int i = 0; i < List.Count; i++)
        {
            GameObject temp = List[i];
            int randomIndex = rnd.Next(i, List.Count);
            List[i] = List[randomIndex];
            List[randomIndex] = temp;
        }
        upgradeOptionList = upgradeOptionList.GetRange(0, 3);
    }

    private void InitializeUpgradeMenu(List<GameObject> upgradeObjects)
    {
        upgradeMenu.gameObject.SetActive(true);
        Time.timeScale = 0f;
        upgradeMenu.UpdateMenu(upgradeObjects);
    }
}
