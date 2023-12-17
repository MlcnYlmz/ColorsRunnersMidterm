using Runtime.Enums;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Controllers.Collectables
{
    public class CollectableAnimationController : MonoBehaviour
    {
        [SerializeField] internal Animator animator;

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void Start()
        {
            if (animator != null)
            {
                animator.SetTrigger("Idle");
            }
            else
            {
                Debug.LogError("Animator component is not assigned to CollectableAnimationController on GameObject: " + gameObject.name);
            }
        }

        private void SubscribeEvents()
        {
            if (CollectableSignals.Instance != null)
            {
                CollectableSignals.Instance.onChangeCollectableAnimationState += OnChangeAnimationState;
            }
            else
            {
                Debug.LogError("CollectableSignals.Instance is null. Make sure CollectableSignals is properly set up.");
            }
        }

        internal void AnimationState(CollectableAnimationStates animationStates)
        {
            if (animator != null)
            {
                animator.SetTrigger(animationStates.ToString());
            }
            else
            {
                Debug.LogError("Animator component is null. Cannot set animation state.");
            }
        }

        private void OnChangeAnimationState(CollectableAnimationStates animationState)
        {
            
        }

        private void UnSubscribeEvents()
        {
            if (CollectableSignals.Instance != null)
            {
                CollectableSignals.Instance.onChangeCollectableAnimationState -= OnChangeAnimationState;
            }
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        internal void OnReset()
        {
            if (CollectableSignals.Instance != null)
            {
                CollectableSignals.Instance.onChangeCollectableAnimationState?.Invoke(CollectableAnimationStates.Idle);
            }
        }
    }
}
