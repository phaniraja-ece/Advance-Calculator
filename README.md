# Advance-Calculator
[![Windows Build](https://github.com/phaniraja-ece/Advance-Calculator/actions/workflows/dotnet.yml/badge.svg)](https://github.com/phaniraja-ece/Advance-Calculator/actions/workflows/dotnet-windows.yml)




# Allows ypu to run any kind of calculation whether it is as simple as addition or as complex as pertration.

## Features

- Addition
- Subtraction
- Multiplication
- Division
- Square Root
- Cube Root
- Root
- Square
- Cube
- Exponent
- Factorial
- Tertation
- Petration

## How to use:

- Either download the latest [Releases](https://github.com/phaniraja-ece/Advance-Calculator/releases) from the releases tab or

- Paste the following commands in the terminal; Make sure you have .NET SDK 10, either Visual Studio 2026, Visual Studio 2022 or Visual Studio Code and Git
  ``` powershell
  git clone https://github.com/phaniraja-ece/Advance-Calculator.git 
  ```
  then:
  ``` powershell
  git clone https://github.com/phaniraja-ece/Advance-Calculator.git 
  ``` powershell
  cd Advance-Calculator
  ```
  then for Windows:

    For x64 bit:
    ```powershell
    dotnet publish -c Release -r win-x64 --self-contained true
    ```
    For x32 bit (or also known as x86 bit):
    ```powershell
    dotnet publish -c Release -r win-x64 --self-contained true
    ```

    For ARM (only ARM64 is supported. For ARM32, run the x86 version of dotnet and the app in x86 emulation):
    ```powershell
    dotnet publish -c Release -r win-arm64 --self-contained true
    ```

  then for Linux:

    For x64 bit:
    ```powershell
    dotnet publish -c Release -r linux-x64 --self-contained true
    ```
    For x32 bit (or also known as x86 bit):
    ```powershell
    dotnet publish -c Release -r linux-x86 --self-contained true
    ```

    For ARM32:

    ```powershell
    dotnet publish -c Release -r linux-arm --self-contained true
    ```

    For ARM64:
    ```powershell
    dotnet publish -c Release -r linux-arm64 --self-contained true
    ```

  and finally for MacOS:

    For x64 bit (Intel based Macs):
  ```powershell
  dotnet publish -c Release -r osx.10.12-x64 --self-contained true 
  ```
  
  ## Note: If you have a 32bit CPU in your Mac, then run the x64 binary in an emulator.

    For ARM (Apple Silicon):
    ```powershell
    dotnet publish -c Release -r osx-arm64 --self-contained true 
    ```
  
   
  
  
