# 🌊 AeroSurf Browser

**AeroSurf** is a lightweight, high-performance web browser built with **C# (WPF)** and **CefSharp** (Chromium). It focuses on minimalism and speed.

<img width="1918" height="1038" alt="AeroSurf Banner" src="https://github.com/user-attachments/assets/2b7fc716-f6bc-4342-8dfc-7996475fd948" />

## 🚀 Features (v0.9.0)

* **⚡ Blazing Fast:** Powered by the Chromium Embedded Framework (CEF) with GPU acceleration enabled.
* **🛡️ Native AdBlock:** Built-in network request filter that blocks ads before they even load (no heavy JavaScript injection).
* **dark Mode UI:** A fully custom, borderless window design.
* **🧘 Sidebar:** A slide-out sidebar overlay designed to maximize screen real estate for content.
* **Privacy First:** No telemetry, minimal footprint.

## 🛠️ Tech Stack

* **Language:** C#
* **Framework:** .NET Framework 4.8 (WPF)
* **Engine:** CefSharp.Wpf (Chromium)
* **Architecture:** MVVM (Model-View-ViewModel)
* **Design:** Custom XAML Styles & Vector Graphics (SVG)

## 📦 Installation / Build

1.  Clone the repository:
    ```bash
    git clone [https://github.com/enviGit/AeroSurf.git](https://github.com/enviGit/AeroSurf.git)
    ```
2.  Open `AeroSurf.sln` in **Visual Studio 2022**.
3.  Restore NuGet packages (specifically `CefSharp.Wpf`).
4.  Build and Run (Press F5).

> **Note:** The browser creates a cache folder in `%LocalStorage%` to speed up startup times.

## 📄 License

Distributed under the MIT License. See `LICENSE` for more information.
