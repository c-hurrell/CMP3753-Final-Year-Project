using System;
using HarbingerCore;
using UnityEngine;

namespace HarbingerScripts
{
    public class VehicleController : MonoBehaviour
    {
        public Vehicle vehicle;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        void FixedUpdate()
        {
            
        }

        private void OnTriggerEnter(Collider other)
        {
            // subscribe to that locations events...
            if (other.CompareTag("Location")) {
                other.GetComponent<LocationManager>().location.DockApprove += vehicle.OnDockApprove;
                other.GetComponent<LocationManager>().location.Undock += vehicle.OnUndock;
                // Implement other logic e.g. current location etc.
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Location")) {
                other.GetComponent<LocationManager>().location.DockApprove -= vehicle.OnDockApprove;
                other.GetComponent<LocationManager>().location.Undock -= vehicle.OnUndock;
                // Implement other logic...
            }
        }
    }
}
