[![Build status](https://ci.appveyor.com/api/projects/status/c0fstat98x48y5g9?svg=true)](https://ci.appveyor.com/project/Unknown6656-Megacorp/Unknown6656.Runtime)
[![releases](https://img.shields.io/github/downloads/Unknown6656-Megacorp/Unknown6656.Runtime/total)](https://github.com/Unknown6656-Megacorp/Unknown6656.Runtime/releases)
[![nuget package](https://img.shields.io/nuget/vpre/Unknown6656.Runtime)](https://www.nuget.org/packages/Unknown6656.Runtime/)
[![nuget downloads](https://img.shields.io/nuget/dt/Unknown6656.Runtime)](https://www.nuget.org/packages/Unknown6656.Runtime/)
![issues](https://img.shields.io/github/issues/Unknown6656-Megacorp/Unknown6656.Runtime)
![repo size](https://img.shields.io/github/repo-size/Unknown6656-Megacorp/Unknown6656.Runtime)
![downloads](https://img.shields.io/github/downloads/Unknown6656-Megacorp/Unknown6656.Runtime/total)
![forks](https://img.shields.io/github/forks/Unknown6656-Megacorp/Unknown6656.Runtime)
![stars](https://img.shields.io/github/stars/Unknown6656-Megacorp/Unknown6656.Runtime)

# Unknown6656.#####
Part of the Unknown6656 Core libraries, providing runtime functionalities for various other Unknown6656 libraries.


## Installation
Use one of the follwing methods to install and use this library:

- **Package Manager:**
    ```batch
    PM> Install-Package Unknown6656.Runtime
    ```
- **.NET CLI:**
    ```batch
    > dotnet add package Unknown6656.Runtime
    ```
- **Package reference** (e.g. in a `.csproj`/`.vbproj`/`.fsproj` project file):
    ```xml
    <PackageReference Include="Unknown6656.Runtime" Version="*" />
    ```
- **Paket CLI:**
    ```batch
    > paket add Unknown6656.Runtime
    ```
- **F# Interactive:**
    ```fsharp
    #r "nuget: Unknown6656.Runtime, *"
    ```

## Documentation and Usage
To use the discriminated unions, simply include the namespace `Unknown6656.Runtime`:

```csharp
using Unknown6656.Runtime;
```
