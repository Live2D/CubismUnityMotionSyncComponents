# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).


## [Unreleased] - 

### Fixed

Fix an issue with inadequate error handling for missing elements in `UpdateCubismMotionSync`. by [@ppcuni](https://github.com/ppcuni)


## [5-r.1-beta.3] - 2023-11-14

### Added

* Add a sample scene `MotionSyncForWebGL` .
* Add Google Chrome from the tested environment.

### Changed

* Change the version of the development project to `2021.3.30f1`.
* Adjust accessibility of functions and members within `CubismMotionSyncCriAudioInput`.


## [5-r.1-beta.2.1] - 2023-10-17

### Changed

* Update `Kei_basic` and `Kei_vowels` model.

### Fixed

* Fix a bug in the analysis calculation.
  * See `CHANGELOG.md` in MotionSync Core.
  * No modifications to Components.


## [5-r.1-beta.2] - 2023-09-28

### Added

* Add a sample scene `Microphone` using microphone input.
* Add delegate type `CubismMotionSyncEngine_CRI.csmMotionSyncLogFunction`.
* Add logging function `CubismMotionSyncEngine_CRI.SetLogFunction`.
* Add Android and iOS, iPadOS from the tested environment.

### Changed

* Change the value of `CubismMotionSyncData.EmphasisLevelDefaultValue` to match the default value in Cubism Editor.
* Update `Kei_basic` and `Kei_vowels` model.


## [5-r.1-beta.1] - 2023-08-17

### Added

* New released!


[5-r.1-beta.3]: https://github.com/Live2D/CubismUnityMotionSyncComponents/compare/5-r.1-beta.2.1...5-r.1-beta.3
[5-r.1-beta.2.1]: https://github.com/Live2D/CubismUnityMotionSyncComponents/compare/5-r.1-beta.2...5-r.1-beta.2.1
[5-r.1-beta.2]: https://github.com/Live2D/CubismUnityMotionSyncComponents/compare/5-r.1-beta.1...5-r.1-beta.2
