[English](NOTICE.md) / [日本語](NOTICE.ja.md)

---

# Notices

## [Limitation] Regarding builds for macOS and iOS (2024-05-30)

When building for macOS and iOS, we have confirmed an issue where an error occurs during the build if `Microphone Usage Description` in the Player Settings is left empty. This issue is planned to be fixed in the next release.

The issue can be worked around using either of the following methods.

### Workaround

* Provide `Microphone Usage Description`.

* Delete `MotionSyncMicInput.cs` file located in `Assets/Live2D/CubismMotionSyncPlugin/Samples` folder.
  * If this workaround is applied, `Microphone` scene within `Assets/Live2D/CubismMotionSyncPlugin/Samples/Scenes` will no longer function.


## [Caution] Support for Apple's Privacy Manifest Policy

This product does not use the APIs or third-party products specified in Apple's privacy manifest policy.
This will be addressed in future updates if this product requires such support.
Please check the documentation published by Apple for details.

[Privacy updates for App Store submissions](https://developer.apple.com/news/?id=3d8a9yyh)
---

©Live2D
