using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HelixSetup : MonoBehaviour
{
    [SerializeField] private GameObject _helixTop;
    [SerializeField] private GameObject _helixGoal;



    [SerializeField] private GameObject _helixLevelPrefab;

    [SerializeField] private Transform ballPos;
    private Vector3 startPos;



    [SerializeField] private List<Stage> _stages;

    public int RingsInStage;

    private List<GameObject> _spawnedLevels = new List<GameObject>();

    private float spawnPosY;

    [SerializeField] private float _distanceBetweenLevels;
    [SerializeField] private float _distanceBetweenTopAndGoal;
    [SerializeField] private float _realDistanceBetweenTopAndGoal;
    [SerializeField] private float _realDistanceBetweenLevels;

    private int maxNumberDeathParts = 2;

    private void Start()
    {

        _distanceBetweenTopAndGoal = _helixTop.transform.localPosition.y - _helixGoal.transform.localPosition.y - 0.1f;


        _realDistanceBetweenLevels = 2.6667f;


        LoadStage(0);
    }


    public void LoadStage(int stageNumber)
    {
        Stage stage;
        if (stageNumber >= _stages.Count)
        {
            if (stageNumber % 5 == 0)
                maxNumberDeathParts++;

            Color backGroundColor = new Color(209, 178, 157);
            Color ballColor = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
            Color partsColor = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));

            List<Level> levels = new List<Level>();

            for (int i = 0; i < 10 + stageNumber * 2; i++)
            {
                Level level = new Level();
                level.deathParts = Random.Range(1, maxNumberDeathParts + 1);
                var nonDeathParts = 11 - maxNumberDeathParts;
                level.normalParts = Random.Range(10, 12);
                levels.Add(level);
            }

            stage = new Stage(backGroundColor, ballColor, partsColor, levels);

            RingsInStage = stage.Levels.Count;

            _stages.Add(stage);
        }
        else
        {
            stage = _stages[stageNumber];
            RingsInStage = stage.Levels.Count;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).localScale = new Vector3(1f, 0.5f, 1);
        }


        var desirableHeight = 0.25f;



        // 10 0.10
        // 15 

        // 50 - 50
        // 30 - 30

        // 0.24 - 0.15
        // 0.35 - x





        if (stage == null)
        {
            Debug.LogError("No stage Assigned");
            return;
        }

        _realDistanceBetweenTopAndGoal = Mathf.Round(_realDistanceBetweenLevels * stage.Levels.Count);
        transform.localScale = new Vector3(2, _realDistanceBetweenTopAndGoal / 2, 2);

        var heightOfPlatform = _helixGoal.transform.GetChild(0).GetComponent<Renderer>().bounds.size.y;
        Debug.Log(heightOfPlatform);
        float yScale = (0.5f * (desirableHeight)) / heightOfPlatform;

        startPos = new Vector3(0, _realDistanceBetweenTopAndGoal / 2, -1.4f);
        ballPos.position = startPos;

        FindObjectOfType<BallBounce>().GetComponent<Renderer>().material.color = stage.BallColor;
        FindObjectOfType<BallBounce>().totalRingsPassed = 0;

        Camera.main.backgroundColor = stage.BackgroundColor;

        foreach (var level in _spawnedLevels)
        {
            Destroy(level);
        }

        //reset Helix rotation

        _distanceBetweenLevels = _distanceBetweenTopAndGoal / stage.Levels.Count;
        //_realDistanceBetweenLevels = _realDistanceBetweenTopAndGoal / stage.Levels.Count;

        spawnPosY = _helixTop.transform.localPosition.y;

        for (int i = 0; i < stage.Levels.Count; i++)
        {
            spawnPosY -= _distanceBetweenLevels;

            GameObject level = Instantiate(_helixLevelPrefab,transform);
            level.transform.localPosition = new Vector3(0, spawnPosY, 0);
            _spawnedLevels.Add(level);

            //Creating Gaps
            int numberOfPartsToDisable = 12 - stage.Levels[i].normalParts;
            List<GameObject> disabledParts = new List<GameObject>();
            
            while (disabledParts.Count < numberOfPartsToDisable)
            {
                GameObject randomPart = level.transform.GetChild(Random.Range(0, level.transform.childCount)).gameObject;
                if (disabledParts.Contains(randomPart) == false)
                {
                    randomPart.SetActive(false);
                    disabledParts.Add(randomPart);
                }
            }

            List<GameObject> leftParts = new List<GameObject>();

            //Collecting all the parts and painting them
            foreach (Transform transform in level.transform)
            {
                transform.GetComponent<Renderer>().material.color = stage.PartsColor;

                if (transform.gameObject.activeInHierarchy)
                {
                    leftParts.Add(transform.gameObject);
                }
            }

            List<GameObject> deathParts = new List<GameObject>();

            while (deathParts.Count < stage.Levels[i].deathParts)
            {
                GameObject randomPart = leftParts[Random.Range(0, leftParts.Count)];
                if (!deathParts.Contains(randomPart))
                {
                    randomPart.AddComponent<DeathPart>();
                    deathParts.Add(randomPart);
                }

            }

        }

        
        
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).localScale = new Vector3(1f, yScale, 1);
        }
        


        heightOfPlatform = _helixGoal.transform.GetChild(0).GetComponent<Renderer>().bounds.size.y;

        Debug.Log(heightOfPlatform);

        GameManager.Instance.SetColor(stage.BallColor);
        GameManager.Instance.UpdateSlide();

    }


}
