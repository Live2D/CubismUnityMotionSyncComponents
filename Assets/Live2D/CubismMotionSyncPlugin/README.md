[English](README.md) / [日本語](README.ja.md)


# Cubism MotionSync Plugin for Unity

This is a Cubism SDK plugin for using the motion-sync function on the Cubism SDK for Unity.

## License

Please read the [license](LICENSE.md) before use.

## Notice

Please read the [notices](NOTICE.md) before use.

## Structure

### Dependencies

#### Cubism SDK for Unity

This plugin is a Cubism SDK plugin for the Cubism SDK for Unity.

The Cubism SDK for Unity package is required for use.

If you are looking for the download page of the SDK package, please go to the [download page](https://www.live2d.com/download/cubism-sdk/download-unity/).

#### Live2D Cubism MotionSync Core

This library provides the motion-sync function. Live2D Cubism MotionSync Core is not included in this repository.

To download the plugin package that includes Live2D Cubism MotionSync Core, please refer to [this](https://www.live2d.com/sdk/download/motionsync/) page.

## Development environment

| Unity | Version |
| --- | --- |
| Latest | 6000.0.27f1 |
| LTS | 2022.3.52f1 |

| Library / Tool | Version |
| --- | --- |
| Android SDK / NDK | *1 |
| Visual Studio 2022 | 17.12.1 |
| Windows SDK | 10.0.26100.0 |
| Xcode | 16.1 |

*1 Use libraries embedded with Unity or the recommended libraries.

### C# compiler

Build using Roslyn or mcs compiler supported by Unity 2018.4 and above.

Note: The mcs compiler is deprecated, and only builds are checked.

Please refer to the following official documentation for the versions of C# you can use.

https://docs.unity3d.com/2018.4/Documentation/Manual/CSharpCompiler.html

## Tested environment

| Platform | Version |
| --- | --- |
| Android | 15 |
| iOS | 18.1.1 |
| iPadOS | 18.1.1 |
| macOS | 15.1 |
| Windows 11 | 23H2 |
| Google Chrome | 131.0.6778.86 |

### Cubism SDK for Unity

[Cubism 5 SDK for Unity R3](https://github.com/Live2D/CubismUnityComponents/releases/tag/5-r.3)

## Microphone Sample

In this sample, the input sound is played directly through the device's sound playback system.
Depending on your environment, feedback may occur. To prevent this, please mute the sample application and the device's sound playback, or move the microphone and speaker away from each other.

Unity builds with the assembly information of the Microphone sample even if the `UnityEngine.Microphone` class is not used in the scene. `Microphone Usage Description` must be included in `Player Settings` for MacOS and iOS builds.
If you do not use the Microphone class when building for MacOS and iOS, please delete the `Assets/Live2D/CubismMotionSyncPlugin/Samples/Microphone` directory.


## Branches

If you are looking for the latest features and/or fixes, please check the `develop` branch.

The `master` branch is brought into sync with the `develop` branch once for every official plugin release.

## Usage

Copy all files under `./Assets` into the folder where this plugin is located in your Unity project.

## Contribution to the project

There are many ways to contribute to the project: logging bugs, submitting pull requests on this GitHub, and reporting issues and making suggestions in Live2D Forum.

### Forking and Pull Requests

We very much appreciate your pull requests, whether they bring fixes, improvements, or even new features. To keep the main repository as clean as possible, create a personal fork and feature branches there as needed.

### Bugs

We are regularly checking bug reports and feature requests at Live2D Forum. Before submitting a bug report, please search the Live2D Forum to see if the bug report or feature request has already been submitted. Add your comment to the relevant issue if it already exists.

### Suggestions

We're also interested in your feedback for the future of the SDK. You can submit a suggestion or feature request at Live2D Forum. To make this process more effective, we're asking that you include more information to help define them more clearly.

## Coding Guidelines

### Naming

Try to stick to the [Microsoft guidelines](https://msdn.microsoft.com/en-us/library/ms229002(v=vs.110).aspx) whenever possible. Private fields are named in lowercase, starting with an underscore.

### Style

- In the Unity Editor extension, try to write expressive code with LINQ and all the other fancy stuff.
- Otherwise, do not use LINQ and prefer `for` to `foreach`.
- Try to make the access modifier explicit. Use `private void Update()` instead of `void Update()`.

## Forum

If you have any questions, please join the official Live2D forum and discuss with other users.

- [Live2D Creators Forum](https://community.live2d.com/)
- [Live2D 公式クリエイターズフォーラム (Japanese)](https://creatorsforum.live2d.com/)
