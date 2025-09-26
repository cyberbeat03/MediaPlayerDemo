# WinMix Desktop

## Overview

Stream and play audio files from your desktop with this simple lightweight music player. The main goal of WinMix Desktop is to be accessible and easy-to-use by the blind and visually impaired. It has been in development for over 10 years and continues to be improved.

## Building and Configuration

This is a Win32 WPF  application. It's written in c# and uses the latest .NET. Below are some notes to ensure this project builds and runs correctly in Visual Studio 2022.

1. The app requires Windows Media Player 10 series or later to be installed. If it's not then go to *Optional Features* and make sure Windows Media Player is in the list of added features.
2. Go into project settings. Confirm or change the following:

- The default namespace should be  WinMix and the .NET target should be 8.0 (or later).
- ImplicitUsings and Nullable should be enabled.
- The latest CommunityToolkit.MVVM extension must be installed.
- Make sure **Enable ClickOnce security settings** is disabled.
- Optionally, under the **signing** tab, uncheck both **Sign ClickOnce manifests** and **Sign the assembly**.
