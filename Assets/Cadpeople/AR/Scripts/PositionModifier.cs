using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Cadpeople.AR
{
    public class PositionModifier : MonoBehaviour
    {
        [SerializeField]
        private VectorThreeVariable cameraPosition;
        [SerializeField]
        private Cadpeople.AR.ARKit.CadpeopleARKitSettings arSettings;

        private float stepSize;

        private void Awake()
        {
            stepSize = arSettings.modelMovementStepSize;
        }

        public void SetPositionX(float x)
        {
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        }

        public void SetPositionZ(float z)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, z);
        }

        private Vector3 GetDirToCamera()
        {
            var dir = cameraPosition.Value - transform.position;
            dir = new Vector3(dir.x, 0f, dir.z);
            return dir.normalized;
        }

        public void Up()
        {
            transform.position += (-GetDirToCamera()) * stepSize;
        }
        public void Down()
        {
            transform.position += (GetDirToCamera()) * stepSize;
        }
        public void Right()
        {
            transform.position += (Quaternion.AngleAxis(-90, Vector3.up) * GetDirToCamera()) * stepSize;
        }
        public void Left()
        {
            transform.position += (Quaternion.AngleAxis(90, Vector3.up) * GetDirToCamera()) * stepSize;
        }
    }
}
