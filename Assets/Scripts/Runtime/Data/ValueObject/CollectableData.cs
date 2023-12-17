using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Data.ValueObject
{
    [Serializable]
    public struct CollectableData
    {
        public CollectableMeshData MeshData;

        public CollectableColorData ColorData;


    }

    [Serializable]
    public struct CollectableMeshData
    {
        public List<Mesh> MeshList;
    }

    [Serializable]
    public struct CollectableColorData
    {
        public List<Material> MaterialsList;
    }
}