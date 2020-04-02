using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//@Author
public class Activation_Spawner_Sub_System : MonoBehaviour
{
    //@Author

    //[SerializedFiled] is same as public in insoector but private in script
    [Tooltip("The object that you want to spawn")]
    [SerializeField] private GameObject spawnedObject;

    [SerializeField] private GameObject objectToActivatetrigger;
    // Start is called before the first frame update
    void Start()
    {
      
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void dataValidation()
    {
        if (spawnedObject == null)
        {
            Debug.LogError("Spawned object variable is empty");
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.Equals(objectToActivatetrigger))
        {

        }
        else
        {
            
        }
        
    }

    public void OnTriggerExit(Collider collision)
    {
        
    }
}
