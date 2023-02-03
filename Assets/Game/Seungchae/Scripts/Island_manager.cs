using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Island_manager : MonoBehaviour
{
    public Text indicator; //It doesn't need atm. 
    public bool useFixedUpdate;
    public float variableToChange;
    public float changePerSecond;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!useFixedUpdate)
        {
            variableToChange += changePerSecond*Time.deltaTime;
            //indicator = variableToChange.ToString();
            Debug.Log(variableToChange);
        }
    }

    private void FixedUpdate() {
        if(useFixedUpdate){
            variableToChange+= changePerSecond*Time.deltaTime;
        }
    }
}
