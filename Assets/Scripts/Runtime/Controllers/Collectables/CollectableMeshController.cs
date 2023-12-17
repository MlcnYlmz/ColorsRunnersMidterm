using Runtime.Data.ValueObject;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Controllers.Collectables
{
    public class CollectableMeshController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private MeshFilter meshFilter;

        [SerializeField] private MeshRenderer meshRenderer;

        #endregion

        #region Private Variables

        [ShowInInspector] private CollectableMeshData _data;

        [ShowInInspector] private CollectableColorData _collectableColorData;

        #endregion

        #endregion


        private void OnEnable()
        {
            ActivateMeshVisuals();
        }

        internal void SetMeshData(CollectableMeshData meshData)
        {
            _data = meshData;
        }

        internal void SetColorData(CollectableColorData colorData)
        {
            _collectableColorData = colorData;
        }

        private void ActivateMeshVisuals()
        {
            meshFilter.mesh = _data.MeshList[0];
        }

        internal void UpgradeCollectableVisual(int value)
        {
            meshFilter.mesh = _data.MeshList[value];
        }


        internal void UpgradeCollectableVisualColor(int value)
        {
            meshRenderer.materials[0] = _collectableColorData.MaterialsList[value];
        }


        private void ActiveColorVisuals()
        {
            meshRenderer.material = _collectableColorData.MaterialsList[0];
        }
    }
}