using System.Collections.Generic;
using Runtime.Data.ValueObject;
using UnityEngine;

namespace Runtime.Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_Build", menuName = "ColorsRunnersMidterm/CD_Build", order = 0)]
    public class CD_Build : ScriptableObject
    {
        public List<BuildData> BuildDataList;
    }
}