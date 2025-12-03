using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cadpeople.Events;

namespace Cadpeople.AR
{

    public delegate void OnPlaneFound();
    public delegate void OnImageFound(Vector3 origin);
    public delegate void OnModelPlaced();

    public interface IARHandler {

        //--------------------------------------------------------------------------------\\
        //									EVENTS
        //--------------------------------------------------------------------------------\\

        event OnPlaneFound OnPlaneFound;
        event OnImageFound OnImageFound;
        event OnModelPlaced OnModelPlaced;


        //--------------------------------------------------------------------------------\\
        //									METHODS
        //--------------------------------------------------------------------------------\\

        void InitializeARScene();
        void RepositionModel();
        void ScaleARModel(Vector3 scale);
        void PlaceModel();

    }
}


