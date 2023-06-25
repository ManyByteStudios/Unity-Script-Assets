using UnityEngine;
using ByteAttributes;

public class DontDestroy : MonoBehaviour {
    string ObjectID;
    [HideInInspector] public bool enableDestroy = false;

    //Awake is called when the script instance is being loaded.
    private void Awake() {
        // Creates a string ID for current object to not destroy when loading. 
        ObjectID = name + transform.position.ToString() + transform.eulerAngles.ToString();
    }

    // Start is called before the first frame update.
    void Start() {
        // Find every object with this script attached.
        for (int i = 0; i < Object.FindObjectsOfType<DontDestroy>().Length; i++) {

            // Destroy any duplicate game object.
            if (Object.FindObjectsOfType<DontDestroy>()[i] != this) {
                if (Object.FindObjectsOfType<DontDestroy>()[i].ObjectID == ObjectID) {
                    Destroy(gameObject);
                }
            }
        }

        DontDestroyOnLoad(gameObject);
    }
}