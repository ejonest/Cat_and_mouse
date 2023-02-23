//Ruler:=1=========2=========3=========4=========5=========6=========7=========8=========9=========0=========1=========2=========3**

//Author information
//Author: Ethan Jones
//Mail: ejonest@csu.fullerton.edu

//Program information
//Program name: ExitSign
//Programming language: C#
//Date this version began: 2021-Aug-22
//Date of last update: 2021-Aug-29
//Notable feature: use graphic-enabled panel as one of the three panels.
//Files in this program: ExitMain.cs, ExitUI.cs, run.sh

//Purpose
//Draw an exit Sign

//This module information
//This module file name: ExitMain.cs
//Language: C Sharp
//This file calls the constructor ExitUI located in ExitUI.cs.  No parameters are passed during the call to the constructor.

//Translation information
//Compile this C# file: mcs -r:System.Windows.Forms.dll -r:System.Drawing.dll -r:UI.dll -out:Exit.exe ExitMain.cs
//Execution statement: ./Exit.exe

//Development platform: Linux distro, Xubuntu, version 20.4.
//Installed C# software: mono-complete.



using System;
using System.Windows.Forms;            //Needed for "Application.Run" near the end of Main function.
public class Drawmain
{  public static void Main()
   {  System.Console.WriteLine("ExitMain opening");
      DrawUI UI_form = new DrawUI();
      Application.Run(UI_form);
      System.Console.WriteLine("Graphics closed. Bye");
   }//End of Main function
}//End of Drawmain class
