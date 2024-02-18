using UnityEngine;

namespace HarbingerScripts
{
    public class LocationManager : MonoBehaviour
    {
        [SerializeField] public Location location;

        void Awake()
        {
            location.position = transform.position;
        }
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
