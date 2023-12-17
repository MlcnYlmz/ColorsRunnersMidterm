using Runtime.Data.ValueObject;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Runtime.Controllers.Player
{
    public class PlayerMeshController : MonoBehaviour
    {
        [Header("Serialized Variables")]
        [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
        [SerializeField] private TextMeshPro scoreText;

        [Header("Private Variables")]
        [ShowInInspector] private CollectableColorData _collectableColorData;

        public void SetColorData(CollectableColorData colorData)
        {
            _collectableColorData = colorData;
        }

        public void UpdateStackScore(int value)
        {
            scoreText.text = value.ToString();
        }

        public void PlayerColorChanged(int value)
        {
            skinnedMeshRenderer.material = _collectableColorData.MaterialsList[value];
            Debug.LogWarning("Color Changed");
        }
    }
}