# WinMix Desktop

## Overview

Organize and manage music from your desktop with this fast and powerful audio player. The main goal of WinMix Desktop is to be accessible and easy-to-use by the blind and visually impaired. It has been in development for over 10 years and continues to be maintained.

## Building and Configuration

This is a Win32 WPF  application. It's written in c# and uses the latest .NET. Below are some notes to ensure this project builds and runs correctly in Visual Studio 2022.

1. The app requires Windows Media Player 10 series or later to be installed. If it's not then go to *Optional Features* and make sure Windows Media Player is in the list of added features.

2. Once you have the solution loaded, use *Configuration Manager* to set solution platforms for Any CPU (for both debug and release). Each time you change the active solution configuration, you can build (F6) the solution.

3. Go into project settings. Confirm or change the following settings:

- The default namespace should be  WinMix and the .NET target should be 8.0 (or later).
- ImplicitUsings and Nullable should be enabled.
- *Enable ClickOnce security settings* should be disabled.

- Optionally, under the **signing** tab, uncheck both **Sign ClickOnce manifests** and **Sign the assembly**.
