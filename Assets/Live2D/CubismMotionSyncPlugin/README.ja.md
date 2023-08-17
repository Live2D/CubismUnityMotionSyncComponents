[English](README.md) / [日本語](README.ja.md)

---

# Cubism MotionSync Plugin for Unity

Cubism SDK for Unity上で、モーションシンク機能を利用するためのCubism SDKプラグインです。

## ライセンス

ご使用前に[ライセンス](LICENSE.md)をお読みください。

## お知らせ

ご使用前に[お知らせ](NOTICE.ja.md)をお読みください。

## 構造

### 依存関係

#### Cubism SDK for Unity

本プラグインは、Cubism SDK for Unity用のCubism SDKプラグインです。

利用するにはCubism SDK for Unityのパッケージが必要となります。

SDKパッケージのダウンロードページをお探しの場合は、[ダウンロードページ](https://www.live2d.com/download/cubism-sdk/download-unity/)にアクセスしてください。

#### Live2D Cubism MotionSync Core

モーションシンク機能を提供するライブラリです。当リポジトリにはLive2D Cubism MotionSync Coreは同梱されていません。

Live2D Cubism MotionSync Coreを同梱したプラグインパッケージをダウンロードするには[こちら](https://docs.live2d.com/cubism-sdk-manual/download-sdk-motionsync-plugin/)のページを参照ください。

## 開発環境

| Unity | バージョン |
| --- | --- |
| LTS | 2022.3.6f1 |
| LTS | 2021.3.29f1 |
| LTS | 2020.3.48f1 |

| ライブラリ / ツール | バージョン |
| --- | --- |
| Visual Studio 2022 | 17.7.0 |
| Windows SDK | 10.0.22621.0 |

*1 Unityに組み込まれたライブラリまたは推奨ライブラリを使用してください。

### C#コンパイラ

Unity2018.4以降でサポートされているRoslynまたはmcsコンパイラを使用してビルドします。

注：mcsコンパイラは非推奨であり、ビルドのみをチェックします。

使用できるC#のバージョンについては、次の公式ドキュメントを参照してください。

https://docs.unity3d.com/ja/2018.4/Manual/CSharpCompiler.html

## テスト済みの環境

| プラットフォーム | バージョン |
| --- | --- |
| macOS | 13.5 |
| Windows 10 | 22H2 |

### Cubism SDK for Unity

[Cubism 5 SDK for Unity R1 beta1](https://github.com/Live2D/CubismUnityComponents/releases/tag/5-r.1-beta.1)

## ブランチ

最新の機能や修正をお探しの場合、`develop`ブランチをご確認ください。

`master`ブランチは、公式のプラグインリリースごとに`develop`ブランチと同期されます。

## 使用法

`./Assets`の下にあるすべてのファイルを、Unityプロジェクト内の本プラグインがあるフォルダーにコピーしてください。

## プロジェクトへの貢献

プロジェクトに貢献する方法はたくさんあります。バグのログの記録、このGitHubでのプルリクエストの送信、Live2Dコミュニティでの問題の報告と提案の作成です。

### フォークとプルリクエスト

修正、改善、さらには新機能をもたらすかどうかにかかわらず、プルリクエストに感謝します。ただし、ラッパーは可能な限り軽量で浅くなるように設計されているため、バグ修正とメモリ/パフォーマンスの改善のみを行う必要があることに注意してください。メインリポジトリを可能な限りクリーンに保つために、必要に応じて個人用フォークと機能ブランチを作成してください。

### バグ

Live2Dコミュニティでは、問題のレポートと機能リクエストを定期的にチェックしています。バグレポートを提出する前に、Live2Dコミュニティで検索して、問題のレポートまたは機能リクエストがすでに投稿されているかどうかを確認してください。問題がすでに存在する場合は、関連するコメントを追記してください。

### 提案

SDKの将来についてのフィードバックにも関心があります。Live2Dコミュニティで提案や機能のリクエストを送信できます。このプロセスをより効果的にするために、それらをより明確に定義するのに役立つより多くの情報を含めるようお願いしています。

## コーディングガイドライン

### ネーミング

可能な限り、[Microsoftガイドライン](https://msdn.microsoft.com/en-us/library/ms229002(v=vs.110).aspx)に準拠するようにしてください。プライベートフィールドには、アンダースコアで始まる小文字の名前を付けます。

### スタイル

- Unity Editor拡張機能では、LINQやその他すべての凝ったものを使って表現力豊かなコードを書いてみてください。
- それ以外の場所ではLINQを使用せず、`foreach`よりも`for`を優先してください。
- アクセス修飾子を明示的にするようにしてください。`void Update()`ではなく `private void Update()`を使いましょう。

## コミュニティ

ご不明な点がございましたら、公式のLive2Dコミュニティに参加して、他のユーザーと話し合ってください。

- [Live2D 公式コミュニティ](https://creatorsforum.live2d.com/)
- [Live2D community (English)](https://community.live2d.com/)
