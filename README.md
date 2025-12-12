# 🌊 AeroSurf Browser

**AeroSurf** is a lightweight, high-performance web browser built with **C# (WPF)** and **CefSharp** (Chromium). It focuses on minimalism and speed.

<img width="1917" height="1036" alt="AeroSurf Banner" src="https://github.com/user-attachments/assets/ec5d3f6d-da57-4bab-8c32-fa7768980f3e" />

## 🚀 Features (v0.9.8)

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

1.  Clone the repository.
2.  Open `AeroSurf.sln` in **Visual Studio 2026**.
3.  Restore NuGet packages.
4.  Build and Run.

> **Note:** The browser creates a cache folder in `%LocalStorage%` to speed up startup times.

## 📄 License

Distributed under the MIT License. See `LICENSE` for more information.
