using UnityEngine;

namespace ActionSample.Utils
{
    public class AnimationUtil
    {
        public static bool IsAnimationDone(Animator animator, string clipName)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName(clipName))
            {
                return false;
            }
            return (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0);
        }
    }

}

