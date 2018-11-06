/* YBotAnimationController
 * Made for the Kinect Project of JIN 2018
 */
using System.Collections;
using UnityEngine;

namespace KinectOverlayDemonstration
{
    /// <summary>
    /// YBotAnimationController
    /// Used to control the animations of the Ybot on the Main Menu
    /// </summary>
    public class YBotAnimationController : MonoBehaviour
    {

        [SerializeField]
        private Animator ybotAnimator;

        private bool animationRunning = false;

        /// <summary>
        /// LockAnimation
        /// Locks the animator state until the current animation is done.
        /// </summary>
        /// <param name="triggerNameToReset">The name of the trigger to restart after the animation.</param>
        /// <returns></returns>
        private IEnumerator LockAnimation(string triggerNameToReset)
        {
            animationRunning = true;
            yield return new WaitForSeconds(0.8f);
            ybotAnimator.ResetTrigger(triggerNameToReset);
            animationRunning = false;
        }

        /// <summary>
        /// PlayAnimation
        /// Tries to play the wanted animation.
        /// </summary>
        /// <param name="triggerName">The trigger linked to the desired animation</param>
        private void PlayAnimation(string triggerName)
        {
            if (ybotAnimator != null && !animationRunning)
            {
                ybotAnimator.SetTrigger(triggerName);
                LockAnimation(triggerName);
            }
        }

        /// <summary>
        /// RightSweepAnimation
        /// Plays the animation picturing the Right Sweep.
        /// </summary>
        public void RightSweepAnimation()
        {
            PlayAnimation("RightSweep");
        }

        /// <summary>
        /// LeftSweepAnimation
        /// Plays the animation picturing the Left Sweep.
        /// </summary>
        public void LeftSweepAnimation()
        {
            PlayAnimation("LeftSweep");
        }

        /// <summary>
        /// UpperSweepAnimation
        /// Plays the animation picturing the Upper Sweep.
        /// </summary>
        public void UpperSwiftAnimation()
        {
            PlayAnimation("UpperSweep");
        }

        /// <summary>
        /// PunchAnimation
        /// Plays the animation picturing the Punch.
        /// </summary>
        public void PunchAnimation()
        {
            PlayAnimation("Punch");
        }

        /// <summary>
        /// RunAnimation
        /// Plays the animation picturing the Run.
        /// </summary>
        public void RunAnimation()
        {
            PlayAnimation("Run");
        }

        /// <summary>
        /// PraiseTheSunAnimation
        /// Plays the animation picturing the Pray The Sun.
        /// </summary>
        public void PraiseTheSunAnimation()
        {
            PlayAnimation("Pray");
        }
    }
}
