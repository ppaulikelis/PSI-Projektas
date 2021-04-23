using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Values : MonoBehaviour
{
    private const string defaultGold = "Gold: ";
    private const string defaultExperience = "Experience: ";

    public int gold = 0;
    public int experience = 0;

    public Text goldText;
    public Text experienceText;

    private void Start()
    {

        StartCoroutine(Increase(1, 2, 1f));
    }

    private void Update()
    {
        if (goldText != null && experienceText != null)
        {
            if (gold <= 0)
            {
                goldText.text = defaultGold + 0;
            }

            goldText.text = defaultGold + gold;
            experienceText.text = defaultExperience + experience;
        }
    }

    IEnumerator Increase(int goldIncrease, int expIncrease, float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            gold += goldIncrease;
            experience += expIncrease;
        }
    }
}
