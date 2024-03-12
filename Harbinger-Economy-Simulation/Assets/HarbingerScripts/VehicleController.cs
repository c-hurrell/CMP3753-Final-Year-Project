using System;
using HarbingerCore;
using UnityEngine;
using UnityEngine.UIElements;

namespace HarbingerScripts
{
    public class VehicleController : MonoBehaviour
    {
        // Holds all the vehicle information. FactionManager will store a reference to each vehicle GameObject instance e.g. when a vehicle is bought a new instance of that vehicle is created as a GameObject
        //
        [SerializeField] public Vehicle vehicle;
        public bool preAssigned;
        [SerializeField] private VehicleSO vehicleSo;
        // [SerializeField] public Route route; // class is in development faction will feed a vehicle when it starts.
        public VehicleActionsSO vehicleActions;

        [SerializeField] private VoidEventChannelSO e_Update;

        private Rigidbody rb;
        
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            
            e_Update.OnEventRaised += OnEconomyUpdate;
            
            if (!preAssigned) {
                vehicle = vehicleSo.vehicle;
                return;
            }

            // Initialises anything defined for the vehicle.
            vehicle.InitDestinations();
            // Initialises the channel for the vehicle.
            vehicleActions = ScriptableObject.CreateInstance<VehicleActionsSO>();
            vehicle.target = vehicle.assignedRoute[0];
        }
        
        // Start is called before the first frame update
        void Start()
        {
            
        }
        
        // When enabled vehicle will choose the position on its route closest to it and engage in navigation
        public void OnEnable()
        {
            
        }

        // Handles movement of vehicle -> FixedUpdate as the movement is physics based for authenticity 
        private void FixedUpdate()
        {
            
            const float turnSpeed = 10.0f;

            // If Im waiting to dock do this.
            if (vehicle.status == Vehicle.Status.WaitingToDock)
            {
                if (rb.velocity.magnitude > 0)
                {
                    rb.drag = 100;
                }
                else
                {
                    vehicle.OnDockRequest();
                }
                    
            }
            // First ensure vehicle is facing the target...
            var targetRotation = Quaternion.LookRotation(vehicle.target.position - transform.position);
            targetRotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            rb.MoveRotation(targetRotation);
            
            // If I'm travelling perform acceleration 
            if (vehicle.status != Vehicle.Status.Travelling) return;
            rb.drag = 0.5f;
            
            // If I'm at max speed don't accelerate further.
            if (rb.velocity.magnitude >= vehicle.maxSpeed)
                return;
            rb.AddForce(transform.forward * vehicle.maxAcceleration, ForceMode.Impulse);
        }
        

        public void OnEconomyUpdate()
        {
            
        }

        private void OnTriggerEnter(Collider other)
        {
            // subscribe to that locations events...
            if (other.CompareTag("Location")) {
                if (other.gameObject == vehicle.target.location)
                {
                    Debug.Log("Vehicle has arrived at location!");
                    vehicle.status = Vehicle.Status.WaitingToDock;
                    
                }
                other.GetComponent<LocationManager>().location.DockApprove += vehicle.OnDockApprove;
                other.GetComponent<LocationManager>().location.Undock += vehicle.OnUndock;
                // Implement other logic e.g. current location etc.
                
                vehicle.atLocation = other.GetComponent<LocationManager>().location.identifier;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Location")) {
                other.GetComponent<LocationManager>().location.DockApprove -= vehicle.OnDockApprove;
                other.GetComponent<LocationManager>().location.Undock -= vehicle.OnUndock;
                // Implement other logic...
                vehicle.atLocation = new LocationIdentifier
                {
                    name = "Roaming...",
                    locationID = 0
                };
            }
        }
    }
}
