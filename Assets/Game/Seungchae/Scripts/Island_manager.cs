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

    
    
    // Game Objects that changes over time
    public GameObject Island_main;

    // Changing values such as health, points
    public float Health;
    public float Points; //Time & HowBitTree Get 1 point per second. But if you get level 2, 1 +0.1 * HowBigTree;
    public int HowBigTree; // basically level? 1 to 5?
    public int HowManyTrees; // 1~5? //Is it just go up same as howBigTree?
    
    public int SizeOfIsland; // 1 ~ 5, The more points, the bigger size // == HowManyTrees??
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
        Health = 100;
        useFixedUpdate = true;
        HowBigTree = 0;
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
            if(Health <=0){
                gameover();
            }
        }
        // if (Input.GetKeyDown(KeyCode.R) && !restart) {
        //     restart = true;  
        //     SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //     Debug.Log("Game Reset");
        // }
        
       getDamaged(0.101f); // Just for debug
    }

    private void FixedUpdate() {
        if(useFixedUpdate){
            variableToChange+= changePerSecond*Time.deltaTime;
            //Debug.Log("Time: "+((int)variableToChange).ToString());
            //Debug.Log(Time.deltaTime);

            ChangePoint(changePerSecond+0.1f * HowBigTree);

            if(Health <=0){
                gameover();
            }
            //Debug.Log("Time: "+((int)variableToChange).ToString());

        }
    }

    private void gameover(){
        Debug.Log("Game Over");

        //Reset the game
        //Health = 100;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("Game Reset");
    }
    
    private float ChangePoint(float deltaPoint){
        Points =Points+deltaPoint;
        //Debug.Log("Point: "+Points.ToString());
        SizeUpIsland();
        return Points;
    }

    private void getDamaged(float Damage){
        Health -= Damage;
        Debug.Log("Health: "+Health.ToString());

    }

    private void SizeUpIsland(){
        /*
            Size 1: Point 0 ~ 100 
            Size 2: Point 101 ~ 300
            Size 3: Point 301 ~ 800
            Size 4: Point 801 ~ 1500
            Size 5: Point 1501 ~ 
        */
        if(Points>=101 && Points <=300)
        {
            SizeOfIsland = 2;
            Debug.Log("Level Up: 2 ");
        }
        else if(Points>=301 && Points <=800)
        {
            SizeOfIsland = 3;
            Debug.Log("Level Up: 3 ");
        }
        else if(Points>=801 && Points <=1500)
        {
            SizeOfIsland = 4;
            Debug.Log("Level Up: 4 ");
        }
        else if(Points>=1501)
        {
            SizeOfIsland = 5;
            Debug.Log("Level Up: 5 ");
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