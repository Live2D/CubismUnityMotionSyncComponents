/**
 * Copyright(c) Live2D Inc. All rights reserved.
 *
 * Use of this source code is governed by the Live2D Open Software license
 * that can be found at https://www.live2d.com/eula/live2d-open-software-license-agreement_en.html.
 */


using UnityEngine;
using Live2D.Cubism.Framework.Motion;


namespace Live2D.CubismMotionSyncPlugin.Framework.Motion
{
    /// <summary>
    /// The controller that has the mechanism to play audio when playing back a motion.
    /// </summary>
    [RequireComponent(typeof(CubismMotionController))]
    public class CubismMotionSyncLinkController : MonoBehaviour
    {
        /// <summary>
        /// AudioSource used for audio playback.
        /// </summary>
        [SerializeField] public AudioSource AudioSource;

        /// <summary>
        /// Motion sync link list.
        /// </summary>
        [SerializeField] public CubismMotionSyncLinkList MotionSyncLinkList;

        /// <summary>
        /// Motion controller.
        /// </summary>
        private CubismMotionController _motionController;

        /// <summary>
        /// Whether MotionSync can be played or not.
        /// </summary>
        public bool CanPlayMotionSync
        {
            get { return MotionSyncLinkList != null && AudioSource != null; }
        }

        /// <summary>
        /// Called by Unity.
        /// </summary>
        private void Start()
        {
            if (!CanPlayMotionSync)
            {
                return;
            }

            _motionController = GetComponent<CubismMotionController>();
            _motionController.AnimationBeginHandler += OnBeginAnimation;
            _motionController.AnimationEndHandler += OnEndAnimation;
        }

        /// <summary>
        /// Called by Unity.
        /// </summary>
        private void LateUpdate()
        {
            if (!CanPlayMotionSync || !_motionController)
            {
                return;
            }

            if (_motionController.IsPlayingAnimation())
            {
                return;
            }

            if (AudioSource.isPlaying)
            {
                AudioSource.Stop();
            }
        }

        /// <summary>
        /// Called by <see cref="CubismMotionController.AnimationBeginHandler"/>.
        /// </summary>
        /// <param name="instanceId">Motion instance id.</param>
        public void OnBeginAnimation(int instanceId)
        {
            if (!CanPlayMotionSync || !_motionController)
            {
                return;
            }

            var motionSyncLinkObjects = MotionSyncLinkList.CubismMotionSyncLinkObjects;
            for (var i = 0; i < motionSyncLinkObjects.Length; i++)
            {
                if (motionSyncLinkObjects[i].MotionInstanceId == instanceId)
                {
                    AudioSource.clip = motionSyncLinkObjects[i].audioClip;
                    AudioSource.Play();
                    break;
                }
            }
        }

        /// <summary>
        /// Called by <see cref="CubismMotionController.AnimationEndHandler"/>.
        /// </summary>
        /// <param name="instanceId">Motion instance id.</param>
        public void OnEndAnimation(int instanceId)
        {
            if (!CanPlayMotionSync || !_motionController)
            {
                return;
            }

            AudioSource.Stop();
            AudioSource.clip = null;
        }
    }
}
