/**
 * Copyright(c) Live2D Inc. All rights reserved.
 *
 * Use of this source code is governed by the Live2D Open Software license
 * that can be found at https://www.live2d.com/eula/live2d-open-software-license-agreement_en.html.
 */


namespace Live2D.CubismMotionSyncPlugin.Framework.Processor
{
    /// <summary>
    /// MotionSync task.
    /// </summary>
    public interface ICubismMotionSyncProcessor
    {
        /// <summary>
        /// Index in <see cref="CubismMotionSyncData.Settings"/>.
        /// </summary>
        int SettingIndex { get; set; }

        /// <summary>
        /// Update motion sync status.
        /// </summary>
        /// <param name="motionSyncData">motion sync data</param>
        void UpdateCubismMotionSync(CubismMotionSyncData motionSyncData);
    }
}
