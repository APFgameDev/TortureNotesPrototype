using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NS_Managers;

namespace NS_MaterialManager 
{
    [System.Serializable]
    public enum MaterialTypes
    {
        Bottom_Target,     
        Mid_Target,
        Top_Target
    }


    [System.Serializable]
    public class ManageMaterial : ManagedObject<Material, MaterialTypes>
    {
    }

    [System.Serializable]
    //	Class Definition: The MaterialManager is a singleton class that will be the hub of accessablity to easily change any material on object we desire to change
    public class MaterialManager :  Manager<MaterialManager, Material, MaterialTypes, ManageMaterial>
    {
        void Awake()
        {
            base.Initialize(this);
        }

        public static Material GetMaterial(MaterialTypes matType)
        {
            return Instance.FindObject(matType);
        }

        public static void SetMaterial(MaterialTypes matType,GameObject aGo)
        {
            Renderer r = aGo.GetComponent<Renderer>();
            if(r != null)
            {
                Material mat = GetMaterial(matType);
                if (mat != null)
                    r.material = mat;
            }
            else
                Debug.LogError("MaterialManager.SetMaterial no Render Found in aGo");
        }
    }
}