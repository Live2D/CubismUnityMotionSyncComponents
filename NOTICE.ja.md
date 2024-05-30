[English](NOTICE.md) / [日本語](NOTICE.ja.md)

---

# お知らせ

## [制限事項] macOS及びiOSにおけるビルドについて（2024-05-30）

macOS及びiOS向けビルドを行う際、Player Settings 内の `Microphone Usage Description` が空のままの場合、ビルド時にエラーが発生する問題を確認しております。
この問題は次回以降のリリースで修正を行う予定です。

以下のいずれかの方法で回避することが可能です。

### 回避方法

* `Microphone Usage Description` を記載する。

* `Assets/Live2D/CubismMotionSyncPlugin/Samples` フォルダ内にある `MotionSyncMicInput.cs` を削除する。
  * この対応を行う場合 `Assets/Live2D/CubismMotionSyncPlugin/Samples/Scenes` 内の `Microphone` シーンは動作しなくなります。


## [注意事項] Apple社のPrivacy Manifest Policy対応について

Apple社が対応を必要としているPrivacy Manifest Policyについて、本製品では指定されているAPI及びサードパーティ製品を使用しておりません。
もし本製品で対応が必要と判断した場合、今後のアップデートにて順次対応する予定です。
詳しくはApple社が公開しているドキュメントをご確認ください。

[Privacy updates for App Store submissions](https://developer.apple.com/news/?id=3d8a9yyh)
---

©Live2D
