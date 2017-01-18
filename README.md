# VR DRIVE
* This is the project for the research about Excitement.
* It is the opencampus 2016 version.

---
## Environment
* OS
  * Windows 7
  * Mac(El Capitan)
* Unity
  * 5.5
* Logicool G27 Racing Wheel
* Oculus Rift DK2

---
## How to install
1. First, you need to import package assetrequest.unitypackage which is in root directory of this project.  
  1. http://tsubakit1.hateblo.jp/entry/2015/07/29/073000
* Next, using assetrequest.unitypackage, add the packages.
  * Asset Request files are `Assets/VRDrive/CloneFoles/*.imp`
* Moreover, if you want to play with Logicool G27 Racing Wheel, you need to install logicool profiler and import file `Assets/VRDrive/CloneFoles/unity-vrdrive.xml`
  * http://support.logicool.co.jp/ja_jp/product/g27-racing-wheel

---
## How to drive
* There are keyboard operation mode and steering opration mode. For changing the mode, you need to change the condition for option "KeyboardMode" in each unity scene of "GameController", "MenuController" or "ResultCOntroller". 
* By keyboard / By GT-27 steering and pedal
  * PageUp / Accel Pedal
    * GO straight
  * Space / Brake Pedal
    * Brake
  * PageUp & Key S / Accel Pedal & Paddle
    * Go Back
  * PageLeft or PageRight / Handle
    * Go Left or Go Right
  * Key E
    * Decide the stage(when in menu)
    * Game Start(when shown with How to use)
    * Go Menu(After shown result)
  * Key R
    * Reset orientation for Oculus Rift

---
