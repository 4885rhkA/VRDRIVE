# thesis2016

---
* First, you need to import package assetrequest.unitypackage which is in root directory of this project.  
** http://tsubakit1.hateblo.jp/entry/2015/07/29/073000
* Next, using this package, add the packages.
** Asset Request files are Assets/VRDrive/CloneFoles/*.imp
* And you need to import Unity Standard Package contained in Unity.
** You can easily download using https://www.assetstore.unity3d.com/jp/#!/content/32351
* Finally, patch the files.
** patch -u "${ProjectDirectpry}/Assets/Standard Assets/Vehicles/Car/Scripts/CarController.cs" < ${ProjectDirectpry}/Assets/VRDrive/CloneFiles/CarController.patch
** patch -u "${ProjectDirectpry}/Assets/Standard\sAssets/Vehicles/Car/Scripts/CarUserControl.cs" < ${ProjectDirectpry}/Assets/VRDrive/CloneFiles/CarUserControl.patch
---
