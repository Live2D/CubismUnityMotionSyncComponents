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
| Latest | 2023.1.19f1 (*1) |
| LTS | 2022.3.12f1 |
| LTS | 2021.3.31f1 |

*1 ARMv7 Android is not supported.

| Library / Tool | Version |
| --- | --- |
| Android SDK / NDK | *1 |
| Visual Studio 2022 | 17.7.6 |
| Windows SDK | 10.0.22621.0 |
| Xcode | 14.3.1 |

*1 Use libraries embedded with Unity or the recommended libraries.

### C# compiler

Build using Roslyn or mcs compiler supported by Unity 2018.4 and above.

Note: The mcs compiler is deprecated, and only builds are checked.

Please refer to the following official documentation for the versions of C# you can use.

https://docs.unity3d.com/2018.4/Documentation/Manual/CSharpCompiler.html

## Tested environment

| Platform | Version |
| --- | --- |
| Android | 13 |
| iOS | 16.6.1 |
| iPadOS | 16.6.1 |
| macOS | 13.6 |
| Windows 10 | 22H2 |
| Google Chrome | 119.0.6045.106 |

### Cubism SDK for Unity

[Cubism 5 SDK for Unity R1 beta3](https://github.com/Live2D/CubismUnityComponents/releases/tag/5-r.1-beta.3)

## Branches

If you are looking for the latest features and/or fixes, please check the `develop` branch.

The `master` branch is brought into sync with the `develop` branch once for every official plugin release.

## Usage

Copy all files under `./Assets` into the folder where this plugin is located in your Unity project.

## Contribution to the project

There are many ways to contribute to the project: logging bugs, submitting pull requests on this GitHub, and reporting issues and making suggestions in Live2D Community.

### Forking and Pull Requests

We very much appreciate your pull requests, whether they bring fixes, improvements, or even new features. Note, however, that the wrapper is designed to be as lightweight and shallow as possible and should therefore only be subject to bug fixes and memory/performance improvements. To keep the main repository as clean as possible, please create a personal fork and feature branch as needed.

### Bugs

We are regularly checking bug reports and feature requests at Live2D Community. Before submitting a bug report, please search the Live2D Community to see if the bug report or feature request has already been submitted. Add your comment to the relevant issue if it already exists.

### Suggestions

We're also interested in your feedback for the future of the SDK. You can submit a suggestion or feature request at Live2D Community. To make this process more effective, we're asking that you include more information to help define them more clearly.

## Coding Guidelines

### Naming

Try to stick to the [Microsoft guidelines](https://msdn.microsoft.com/en-us/library/ms229002(v=vs.110).aspx) whenever possible. Private fields are named in lowercase, starting with an underscore.

### Style

- In the Unity Editor extension, try to write expressive code with LINQ and all the other fancy stuff.
- Otherwise, do not use LINQ and prefer `for` to `foreach`.
- Try to make the access modifier explicit. Use `private void Update()` instead of `void Update()`.

## Forum

If you have any questions, please join the official Live2D forum and discuss with other users.

- [Live2D Creator's Forum](https://community.live2d.com/)
- [Live2D 公式クリエイターズフォーラム (Japanese)](https://creatorsforum.live2d.com/)
