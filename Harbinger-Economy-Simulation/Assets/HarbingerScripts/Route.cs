using System.Collections.Generic;
using UnityEngine;

namespace HarbingerScripts
{
    public class Route : MonoBehaviour
    {
        [SerializeField] private LineRenderer lr;
        [SerializeField] private List<Transform> points;
        // Start is called before the first frame update
        private void Awake()
        {
            lr = GetComponent<LineRenderer>();
            var regionList = GameObject.FindGameObjectsWithTag("Regions");
            foreach (var regi in regionList) {
                points.Add(regi.transform);
            }
        }

        // Update is called once per frame
        private void Update()
        {
            for (var i = 0; i < points.Count; i++) {
                lr.SetPosition(i, points[i].position);
            }
        }
    
        public void SetUpLine(List<Transform> points)
        {
            lr.positionCount = points.Count;
            this.points = points;
        }
    }
}
