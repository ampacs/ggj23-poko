using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Island_manager : MonoBehaviour
{
    //private CharacterController controller;

    //Time
    public Text indicator; //It doesn't need atm. 
    public bool useFixedUpdate;
    public float variableToChange;
    public float changePerSecond;
    public float GettingPointForNext=0;
    public float HasTimePassedEnough=0;
    
    
    // Game Objects that changes over time
    public GameObject Island_main;
    public GameObject[] Islands_New;
    public GameObject[] TreeLocations;
    public GameObject pref_smallTree;

    // Changing values such as health, points
    public float Health_MainTree; //Main tree's Health
    private float Health_SmallTrees; //Small tree's Health
    private float[] Intensity_BetweenIslands; // When it reaches '0' the connection will be gone. AreTheyConnected[i] will be false; 
    private bool[] AreTheyConnected; //If they're, it's true. When it becomes true, Intensity_BetweenIslands get 100.
    public float Points; //Time & HowBitTree Get 1 point per second. But if you get level 2, 1 +0.1 * HowBigTree;

    //Point Mechanism (Time + How big + how many trees )
    public int HowBigTree; // basically level? 1 to 5?
    public int HowManyTrees; // 1~5? //Is it just go up same as howBigTree?
    public int MaxTree = 5; // = 5 * Level or SizeOfIsland
    
    public int SizeOfIsland; // 1 ~ 5, The more points, the bigger size // == HowManyTrees??
    private int Level=1;
    //SizeOfIsland = HowManyTrees = HowBigTree?
    

    bool restart;

    void Awake() {
        //DontDestroyOnLoad(this.gameObject);
        restart = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        //controller = gameObject.AddComponent<CharacterController>();

        changePerSecond = 1f; 
        Health_MainTree = 100;
        useFixedUpdate = true;
        HowBigTree = 0;
        HowManyTrees = 1;
        SizeOfIsland = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(!useFixedUpdate)
        {
            variableToChange += changePerSecond*Time.deltaTime;
            //indicator = variableToChange.ToString();
            Debug.Log(variableToChange);
            //Debug.Log(Time.deltaTime);
            if(Health_MainTree <=0){
                gameover();
            }
        }
        // if (Input.GetKeyDown(KeyCode.R) && !restart) {
        //     restart = true;  
        //     SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //     Debug.Log("Game Reset");
        // }
        
       //getDamaged(0.101f); // Just for debug

       //if(merged island) ++Level;? ++SizeOfIsland?
       //SizeUpIsland();
    }

    private void FixedUpdate() {
        if(useFixedUpdate){
            variableToChange+= changePerSecond*Time.deltaTime;
            HasTimePassedEnough += changePerSecond*Time.deltaTime;
            //Debug.Log("Time: "+((int)variableToChange).ToString());
            //Debug.Log(Time.deltaTime);

            //Point Mechanism (Time + How big + how many trees )
            ChangePoint(changePerSecond+0.1f * HowBigTree + 0.1f*HowManyTrees);

            if(Health_MainTree <=0){
                gameover();
            }
            //Debug.Log("Time: "+((int)variableToChange).ToString());
            if(HasTimePassedEnough>10) // every 10 second you will get a new tree until you meet the max
            {
                // HowManyTrees++;
                GenerateNewTree();
                HasTimePassedEnough = 0;
            }
        }
    }

    private void gameover(){
        Debug.Log("Game Over");

        //Reset the game
        //Health = 100;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("Game Reset");
    }
    
    public  float ChangePoint(float deltaPoint){
        Points =Points+deltaPoint;
        GettingPointForNext = GettingPointForNext+deltaPoint;


        //When you get another 300 point, the next Island will show up.
        if(GettingPointForNext > 300){ 
            showupNextIsland();
            SizeUpIsland(); // It should be deleted after testing.
            GettingPointForNext =0;
        }
        //Debug.Log("Point: "+Points.ToString());
        //SizeUpIsland();
        return Points;
    }

    private void getDamaged(float Damage){
        Health_MainTree -= Damage;
        Debug.Log("Health: "+Health_MainTree.ToString());

    }

    private void SizeUpIsland(){
        /*
            Size 1: first
            Size 2: Merge 1
            Size 3: Merge 2
            Size 4: Merge 3
            Size 5: Merge 4
        */
        SizeOfIsland++;
        MaxTree = 5 * SizeOfIsland;
        Debug.Log("SizeOfIsland: "+SizeOfIsland.ToString());
        Debug.Log("MaxTree: "+MaxTree.ToString());

    }

    private void showupNextIsland()
    {   
        // if you merge islands you will get level up.
        //
        Debug.Log("showupNextIsland");

    }

    private void GenerateNewTree(){
        Debug.Log("GenerateNewTree");

        if(HowManyTrees < MaxTree)
        {
            //pref_smallTree.
            // Instantiate at position (0, 0, 0) and zero rotation.
            // Instantiate(pref_smallTree, new Vector3(0, 0, 0), Quaternion.identity);
            Vector3 currentEulerAngles = new Vector3(0, 0, 0);
            Quaternion currentRotation  = Quaternion.identity;
            currentRotation.eulerAngles = currentEulerAngles;
            GameObject newTree = Instantiate(pref_smallTree, TreeLocations[HowManyTrees-1].transform.position, currentRotation) as GameObject; 
            newTree.transform.parent = GameObject.Find("Trees_new").transform;

            HowManyTrees++;
        }
    }
}



/*
(Seungchae)
Gameplay
    Islands
        Have weight // it will get heavier over time or depending on how big your trees are?
        Physics object
    Trees
        Health
            When main tree health reaches 0 -> game over
                restart for now
        Grow over time //TIME
        More are created over time on top of the island //TIME
        Generate points over time //TIME
            Bigger trees -> more points per second 
        Island size determines maximum ammount of trees
    Roots
        Grow over time //TIME
        More are created over time around the island
        % Attachment intensity


 Points : Over Time, how big your trees are


*/