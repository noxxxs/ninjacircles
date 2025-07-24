using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Microlight.MicroAudio;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;    
    public List<ExpandBorder> borderList;
    public List<int> delayForBorders;

    public ShurikenScriptableObject ShurikenSO;
    private List<Transform> _allCircles = new List<Transform>();

    [Header("SFX")]
    public MicroSoundGroup OnDieGroup;
    public MicroSoundGroup ThrowShurikenGroup;
    public MicroSoundGroup ShurikenEquipGroup;
    public MicroSoundGroup MultiplyGroup;
    public List<Transform> AllCircles
    {
        get { return _allCircles; }
        set { _allCircles = value; }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance);
        } else
        {
            instance = this;
        }

        // Shaffle list to randomize targeting attack
        ShuffleAllCircles();
        StartCoroutine(ExpandCoroutine());
    }

    private IEnumerator ExpandCoroutine()
    {
        for (int i = 0; i < borderList.Count; i++)
        {
            // Wait before previous expand end (check one/0.35sec)
            // Skip for first border
            if (i != 0)
            {
                ExpandBorder previousBorder = borderList[i - 1].GetComponent<ExpandBorder>();
                while (!previousBorder.ExpandFinished)
                {
                    yield return new WaitForSeconds(0.35f);
                }
            }

            yield return new WaitForSeconds(delayForBorders[i]);

            borderList[i].GetComponent<ExpandBorder>().StartExpand();
        }
    }

    private static System.Random rng = new System.Random();

    public void ShuffleAllCircles()
    {
        int n = _allCircles.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Transform temp = _allCircles[k];
            _allCircles[k] = _allCircles[n];
            _allCircles[n] = temp;
        }
    }

}
