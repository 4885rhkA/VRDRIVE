# thesis2016

---
## How to install
* First, you need to import package assetrequest.unitypackage which is in root directory of this project.  
** http://tsubakit1.hateblo.jp/entry/2015/07/29/073000
* Next, using this package, add the packages.
** Asset Request files are Assets/VRDrive/CloneFoles/*.imp
* Finally, patch the files.
** patch -u "${ProjectDirectpry}/Assets/Standard Assets/Vehicles/Car/Scripts/CarController.cs" < ${ProjectDirectpry}/Assets/VRDrive/CloneFiles/CarController.patch
** patch -u "${ProjectDirectpry}/Assets/Standard\sAssets/Vehicles/Car/Scripts/CarUserControl.cs" < ${ProjectDirectpry}/Assets/VRDrive/CloneFiles/CarUserControl.patch
* Moreover, if you want to play with handle, you need to install logicool profiler.
** http://support.logicool.co.jp/ja_jp/product/g27-racing-wheel
** If installed, you need to import file Assets/VRDrive/CloneFoles/unity-vrdrive.xml
---
## How to drive
* By keyboard(By GT-27 steering)
** PageUp(Button 8, 21, 23)
*** GO straight
** Space(Button 7, 20, 22)
*** Brake
** PageDown & Space(Paddle & Button 7, 20, 22)
*** Go Back
---
