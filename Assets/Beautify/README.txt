*******************************
*      BEAUTIFY FOR HDRP      *
*     Created by Kronnect     *   
*         README FILE         *
*******************************

Requirements & Setup
--------------------
1) This version of Beautify only works with High Definition Rendering Pipeline (HDRP 7.1.8 or later).
2) Make sure you have High Definition RP package imported in the project before importing Beautify to avoid compilation errors.
3) Make sure you have a High Definition RP asset assigned to Project Settings / Graphics. There's a HDRP sample asset in Demo folder.
4) Assign Beautify to the "After Post Process" list in Project Settings / HDRP Default Settings.

Setup video: https://youtu.be/HDC9G-_kf3Y

If you want to animate some properties at runtime using scripting, take a look at Demo.cs in the demo scene for some code examples.


Demo Scene
----------
There's a demo scene which lets you quickly check if Beautify is working correctly in your project.


License
-------
Beautify is licensed under the Unity Asset Store EULA or Kronnect EULA (depending where you purchased the asset).
Beautify also includes two custom ports of tonemap operators (ACES Fitted and AGX) which are licensed under the MIT license:
https://opensource.org/license/mit
Check the BeautifyACESFitted.hlsl and BeautifyAGX.hlsl files respectively. You can clear them if you don't use these tonemappers.


Support
-------
* Support-Web: https://kronnect.com/support
* Support-Discord: https://discord.gg/EH2GMaM
* Email: contact@kronnect.com
* Twitter: @Kronnect



Future updates
--------------
All our assets follow an incremental development process by which a few beta releases are published on our support forum (kronnect.com).
We encourage you to signup and engage our forum. The forum is the primary support and feature discussions medium.

Of course, all updates of Beautify HDRP will be eventually available on the Asset Store.


Version history
---------------

v8.0
- Added Film Grain and Artifacts effect

v7.1
- Added support for display HDR output

v7.0.1
- [Fix] Minor fixes for Unity 6

v7.0
- Added AGX tonemapper

v6.0
- Added support for Unity 6

v5.0.2
- [Fix] Fixed color bleeding when using "Final Blur" effect

v5.0.1
- Removed console warning in Unity 2022.2

v5.0
- Added "Final Blur" effect

v4.4.1
- Minimum Unity version required is now 2020.3.16
- [Fix] Fixes in depth of field scaling in Unity Editor

v4.4
- Support for 3D LUT textures and import of .CUBE format

v4.3.1
- [Fix] Fixed minor issue with compare mode

v4.3
- Added "Compare Mode"

v4.2.1
- Improvements to LUT operator

v4.2
- Depth of field: added real camera settings

v4.1
- Added LUT browser (access it from inspector under LUT section or from top menu Window -> LUT Browser

v4.0.1
- [Fix] Fixed an issue that could produce 2 instances of Beautify Settings when adding Beautify to HDRP settings for the first time

v4.0
- Added depth of field effect
- Added fitted ACES tonemap operator

v3.4.1 6/Aug/2021
- [Fix] Fixed editor compatibility issue with Unity 2021.1

v3.4 23/Jul/2021
- Added Frost FX effect

v3.3 15/Jul/2021
- Vignette transparency controlled by alpha color

v3.2 5/Jun/2021
- Added "Blink" method to automatic blink effect (see demo scene)

v3.1 28/May/2021
- Added LUT support

v3.0 4/May/2021
- Added Pixelate effect

v2.0 27/Mar/2021
- Added Hard Light color tweak effect
- Added Vignetting effect
- Minor improvements and compatibility updates

v1.0.1 June / 2020
- [Fix] Fixed fireflies in dark areas

v1.0 March / 2020
First release
