# Senior-Project
A 2D Bullet Hell game in Unity.

## Development

* **[Unity 2020.1.4](https://unity3d.com/unity/whats-new/2020.1.4)** Unity’s real-time 3D development platform lets artists, designers and developers work together to create amazing immersive and interactive experiences. (Available for Windows, Mac, and Linux.)

* **[Visual Studio Community 2019](https://visualstudio.microsoft.com/vs/community/)** A fully-featured, extensible, free IDE for creating modern applications for Android, iOS, Windows, as well as web applications and cloud services.

* **[Visual Studio - Extension: Warn About TODO's](https://github.com/mrlacey/WarnAboutTodos)**  Visual Studio automatically takes code comments that include `TODO` and turns them into User Tasks that are displayed on the Task List. This takes those same tasks and also creates warnings for them. 

  * This extension will allow us to quickly view and access commented tags such as "FIXME" and "TODO". 

![screenshot: inline comment tag.](./.readme/ex6.png)

![screenshot: Visual Studio fixme tast](./.readme/ex5.png)
  * Add this extension via the Visual Studio Menu Bar. 

  * **Extensions** > **Manage Extensions** 

![menu-bar screenshot](./.readme/ex1.png)

  * Search for "Warn About TODOs" and Install

![manage extensions screenshot](./.readme/ex2.png)

  * Once the extension has been installed and you have restarted Visual Studio, add the configuration file by opening the Unity Project in Visual Studio > right-clicking on the project in the Solution Explorer > Add > Existing Item

![Screenshot: Add existing item.](./.readme/ex3.png)

  * Select the "todo-warn.config" file and click Add
  
  * Once added, select the "todo-warn.config" file in the Solution Explorer and set the "Build Action" to "AdditionalFiles"

![Screenshot: set build action.](./.readme/ex4.png)