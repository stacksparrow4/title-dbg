# Title Debug Mod

A debugging mod (incl. noclip, centering camera) for the game Title by FuzzyAidan. Can be downloaded [here](https://drive.google.com/file/d/1ATE4fRHQD2-ATUpKC564CKwETopNugWr/view?usp=sharing).

## How to use:

1. Move the file Assembly-CSharp.dll (found in your_Title_folder/Title_Data/Managed) to Assembly-CSharp.bak.dll (to make a backup of this file)
2. Download the release for your version of Title at [https://github.com/stacksparrow4/title-dbg/releases](https://github.com/stacksparrow4/title-dbg/releases)
3. Move the downloaded Assembly-CSharp.dll into your_Title_folder/Title_Data/Managed
4. Run the game

## How to develop/make your own mods

1. Download/clone the source code
2. Place the source code in a new folder inside your title installation (for example, the first folder Mods, the second folder DbgMod, it should be setup so that the file your_Title_folder/Mods/DbgMod/Assembly-CSharp.mm.sln exists)
3. Open the solution (.sln) file in Visual Studio (you will need .NET framework 4)
4. Right click the Assembly-CSharp.mm project (it's in the Solution Explorer, below the Assembly-CSharp.mm project), and click Properties
5. Go to the Build Events tab, and change the path described in the first line of Post-build event command line to reflect your game's Assembly-CSharp directory
6. Download the latest release of MonoMod ([https://github.com/MonoMod/MonoMod](https://github.com/MonoMod/MonoMod)) and extract all files to your game's Assembly-CSharp directory
7. Go to your game's Assembly-CSharp directory, rename Assembly-CSharp.dll to Assembly-CSharp_orig.dll
7. Go to the Debug tab, and change Start external program to the Title executable
8. Build and run and verify that you can use Debugging tools.

From there you can add your own code to the solution. It may be useful to use [dnSpy](https://github.com/dnSpy/dnSpy) on the original Assembly-CSharp to see the original code for Title.
