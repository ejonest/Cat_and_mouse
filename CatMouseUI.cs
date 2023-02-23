//Ruler:=1=========2=========3=========4=========5=========6=========7=========8=========9=========0=========1=========2=========3**

//Author: Ethan Jones
//Preferred speciality: Graphics 
//Mail: ejonest@csu.fullerton.edu

//Program information
//Program name: CatMouse
//Programming language: C#
//Date this version began: 2022-Sep-23
//Date of last update: 2023-Feb-22
//Files in this program: CatMouseMain.cs, CatMouseUI.cs, run.sh

//Purpose
//Create two circles one representing a mouse and another representing a car. The user operates the cat is 
//supposed to catch the mouse

//This module information
//This module file name: CatMouseUI.cs
//Language: C Sharp

//Translation information
//The commands for translation from C# source code to binary dll files are found in the bash file, namely run.sh.
//In the event that the run.sh file is lost the compilation command is given here
//mcs -target:library -r:System.Windows.Forms.dll -r:System.Drawing.dll -out:UI.dll CatMouseUI.cs

//Development
//This program was developed on Xubuntu 20.4 using Mono for translation services.  Obtain an installed copy of mono
//by entering the following command in the shell: sudo apt install mono-complete



//Begin source code area.
//The source code is segmented into major blocks.  Each such block has its designated purpose.
//That purpose is expressed at the beginning of each block.

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Timers;

public class DrawUI : Form
{
  //public static bool IsKeyDown (System.Windows.Input.Key key);
  //Declare attributes of the whole form.
   private const int formwidth = 700;  //1250x1000 //Horizontal width of the user interface.
   private const int formheight = 700;  //Vertical height of the user interface.
   private Size ui_size = new Size(formwidth,formheight);

   //Declare attributes of Header label.
   private String header_message_text = "Collison";
   private Label header_message = new Label();
   private Point header_message_location = new Point(0,0);
   private Font  header_message_font = new Font("Arial",formheight/24,FontStyle.Bold);

   //Declare attributes of Header panel
   private const int header_panel_height = 100;
   private Panel header_panel = new Panel();
   private Point header_panel_location = new Point(0,0);
   private Size  header_panel_size = new Size(formwidth,header_panel_height);
   private Color header_panel_color = Color.Crimson;

   //Declare attributes of instruction label.
   private String instruction_mes_text = "Press enter to start/stop the ball," +
      " esc to exit, left to rotate counter clockwise, and right to rotate clockwise";
   private Label instruction_mes = new Label();
   private Point instruction_mes_location = new Point(0,0);
   private Font  instruction_mes_font = new Font("Arial", 12 ,FontStyle.Bold);

   //Declare attributes of instruction panel
   private const int instruction_panel_height = 100;
   private Panel instruction_panel = new Panel();
   private Point instruction_panel_location = new Point(0,600);
   private Size  instruction_panel_size = new Size(formwidth,header_panel_height);
   private Color instruction_panel_color = Color.Crimson;

   //Declare attributes of Graphic panel
   private const int graphic_panel_height = 500;
   private Graphic_panel drawingpane = new Graphic_panel();
   private Point graphic_panel_location = new Point(0, 100);
   private Size  graphic_panel_size = new Size(formwidth,graphic_panel_height);
   private Color graphic_panel_color = Color.AntiqueWhite;

   //Declare some mechanisms for managing the visibility of displayed geometric shapes.
   private static bool line_visible = true;
   private static bool ball_showing = true;

   //Declare some stuff for clocks
   private static System.Timers.Timer refresh_clock = new System.Timers.Timer();
   private static System.Timers.Timer motion_clock = new System.Timers.Timer();
   private static System.Timers.Timer motion_clock2 = new System.Timers.Timer();
   private const double refresh_clock_rate = 20;  //Hertz = tics per second
   private const double motion_clock_rate = 20;   //Hertz = tics per second
   private const double one_second = 1000.0;
   private const double refresh_interval = one_second / refresh_clock_rate;
   private const double motion_interval = one_second / motion_clock_rate;
   private int refresh_interval_int = (int)System.Math.Round(refresh_interval);
   private int motion_interval_int = (int)System.Math.Round(motion_interval);

   //create some colors
   private static System.Drawing.Brush color1 = Brushes.Crimson;
   private static System.Drawing.Brush color2 = Brushes.BlueViolet;

   //Create stuff for movement (there isn't enough)
   //cat/player controlled one
   private static double x1_start = (formwidth / 4);
   private static double y1_start = 300;
   private static double del_x = 10; //keep track of how much to move in x direction
   private static double del_y = 10; //keep track of how much to move in y direction
   private static double step = 6; //hypotenuse. The amount that should be moved diagnoaly each update
   private static double dist = 0; //used to keep track of the distance between the cat and the mouse

   private int status = 0; //used like an enum to track what state the go/stop buttom/enter key is in
   private static double angle = 0; //keeps track of the angle of the ball

   //mouse
   Random rand = new Random(); //used to pick a random direction for the ball on start
   private static double x2_start = 3 * (formwidth / 4);
   private static double y2_start = 300;
   private static double del2_x = 10;
   private static double del2_y = 10;
   private double angle2;

   public DrawUI()   //The constructor of this class
   {//Set the attributes of this form.
    Text = "Cat and Mouse game";
    Size = ui_size;
    MaximumSize = ui_size;        //This inhibits resizing by the user.
    MinimumSize = ui_size;

    //Construct the header panel
    header_message.Text = header_message_text;  //Header_message_text;
    header_message.Font = header_message_font;
    header_message.ForeColor = Color.AntiqueWhite;
    header_message.TextAlign = ContentAlignment.MiddleCenter;
    header_message.Size = new Size(formwidth,formheight/12);
    header_message.Location = header_message_location;
    header_panel.BackColor = header_panel_color;
    header_panel.Size = header_panel_size;
    header_panel.Location = header_panel_location;
    header_panel.Controls.Add(header_message);

    //instruction panel
    instruction_mes.Text = instruction_mes_text;  //Header_message_text;
    instruction_mes.Font = instruction_mes_font;
    instruction_mes.ForeColor = Color.AntiqueWhite;
    instruction_mes.TextAlign = ContentAlignment.MiddleCenter;
    instruction_mes.Size = new Size(formwidth,formheight/12);
    instruction_mes.Location = instruction_mes_location;
    instruction_panel.BackColor = instruction_panel_color;
    instruction_panel.Size = instruction_panel_size;
    instruction_panel.Location = instruction_panel_location;
    instruction_panel.Controls.Add(instruction_mes);

    //Construct the middle panel also called the "graphic panel".
    drawingpane.BackColor = graphic_panel_color;
    drawingpane.Size = graphic_panel_size;
    drawingpane.Location = graphic_panel_location;

    //Add panels to the UI form
    Controls.Add(header_panel);
    Controls.Add(drawingpane);
    Controls.Add(instruction_panel);

    //create the refresh clock
    refresh_clock.Enabled = false;
    refresh_clock.Elapsed += new ElapsedEventHandler(redraw);
    refresh_clock.Interval = refresh_interval_int;
    refresh_clock.Enabled = true;

    motion_clock.Enabled = false;
    motion_clock.Elapsed += new ElapsedEventHandler(update);
    motion_clock.Interval = motion_interval_int;

    //create the motion2_clock
    motion_clock2.Enabled = false;
    motion_clock2.Elapsed += new ElapsedEventHandler(update2);
    motion_clock2.Interval = motion_interval_int;

   }//End of constructor

//update for the first ball or the cat
   protected void update(System.Object sender, ElapsedEventArgs evt) {
     if (x1_start < 0) { //if going past the left wall
       angle += Math.PI / 2; //update angle
       del_x = step * Math.Cos(angle); //recalulate deltas
       del_y = step * Math.Sin(angle);
     }
     if (x1_start + 36 > formwidth) { // right wall
       angle += Math.PI / 2;
       del_x = step * Math.Cos(angle);
       del_y = step * Math.Sin(angle);
     }
     if (y1_start < 0) { //top wall
       angle += Math.PI / 2;
       del_x = step * Math.Cos(angle);
       del_y = step * Math.Sin(angle);
     }
     if (y1_start + 36 > 500) { //bottom wall
       angle += Math.PI / 2;
       del_x = step * Math.Cos(angle);
       del_y = step * Math.Sin(angle);
     }
     x1_start += del_x; //update the current x and y. Using x1_start and y1_start to save space
     y1_start += del_y;
   }
   //update for ball 2 or the mouse. Pretty much the same except no angles are involved
   protected void update2(System.Object sender, ElapsedEventArgs evt) {
     if (x2_start < 0) {
       del2_x *= -1;
     }
     if (x2_start + 36 > formwidth) {
       del2_x *= -1;
     }
     if (y2_start < 0) {
       del2_y *= -1;
     }
     if (y2_start + 36 > 500) {
       del2_y *= -1;
     }
     x2_start += del2_x;
     y2_start += del2_y;
     if (is_colliding(x2_start, y2_start)) { //if collison is detected print that the user won and stop the clocks
       System.Console.WriteLine("Congragulations you caught the mouse");
       motion_clock.Enabled = false;
       motion_clock2.Enabled = false;
     }
   }

   protected void redraw(System.Object sender, ElapsedEventArgs evt) {
    drawingpane.Invalidate();
   }
   //used to check for a collison. Passed in two values of the x and y of the ball that needs to be checked if colliding with cat
   //making this a function allows for growth of system
   protected bool is_colliding(double x2, double y2) {
     dist = Math.Sqrt((x1_start - x2)*(x1_start - x2) + (y1_start - y2)*(y1_start - y2)); //find the distance between them
     if (dist <= 36) {  //if the distance is too small they are colliding
       return true; //return true
     } else {
       return false; //otherwise they are not colliding
     }
   }

   protected override void OnKeyDown(KeyEventArgs e) {
     if (e.KeyCode == Keys.Right) {
        angle += (Math.PI / 30); //add to the angle. Math.PI/30 is a number I thought was good
        del_x = step * Math.Cos(angle); //Update the del_x based on the new angle
        del_y = step * Math.Sin(angle); //Update the del_y based on the new angle
      } else if (e.KeyCode == Keys.Left) {
        angle -= (Math.PI / 30);
        del_x = step * Math.Cos(angle);
        del_y = step * Math.Sin(angle);
      } else if (e.KeyCode == Keys.Escape) { //if escape key pressed we take that as if the exit button had been clicked
        System.Console.WriteLine("This program will end execution.");
        Close();
      } else if (e.KeyCode == Keys.Return) { ;///return key acts like the play/pause button
        switch(status) {
          case 0:
            //do the starting math here for the balls and their motion
            angle2 = rand.Next(0, (int)(2 * Math.PI)); //make a random angle in the range stated
            del_x = step * Math.Cos(0); //calculate the deltas of the cat based on traveling directly right
            del_y = step * Math.Sin(0);
            //System.Console.WriteLine("Angle2: " + angle2);
            del2_x = step * Math.Cos(angle2); //calculate deltas from that angle
            del2_y = step * Math.Sin(angle2);
            ball_showing = true; //show the ball
            motion_clock.Enabled = true; //start the clocks
            motion_clock2.Enabled = true;
            status = 1; //update the status
            break;
          case 1:
            status = 2; //update status
            //stop the balls
            motion_clock.Enabled = false; //pause the game
            motion_clock2.Enabled = false;
            break;
          case 2:
            //set the balls back in motion
            status = 1; //update status
            motion_clock.Enabled = true; //play the game
            motion_clock2.Enabled = true;
            break;
        }
      }
   Invalidate();
     base.OnKeyDown(e);
  }

   //The next block created a new derived class from Panel class.  Inside of the derived class
   //is a copy of the OnPaint function.  The presence of OnPaint allows the program to call
   //graphic functions such as DrawLine, DrawRectangle, and others.
   public class Graphic_panel: Panel {          //Class Graphicpanel inherits from class Panel
    public Graphic_panel() {                     //Constructor for derived class Graphicpanel
      Console.WriteLine("A graphic enabled panel was created");
    }//End of constructor

    //The next method is the OnPaint that belongs to the middle panel only.  The outputs from this
    //method are located according to the local Cartesian system of that middle panel.  The draw
    //methods called inside of this OnPaint can only output onto the middle panel alone.
    protected override void OnPaint(PaintEventArgs ee) {
      //Add drawing of the line and hte disk here
      Graphics graph = ee.Graphics;
      if(line_visible) {
        if(ball_showing) {
            graph.FillEllipse(color1, (float)x1_start, (float)y1_start, 36, 36);
            graph.FillEllipse(color2, (float)x2_start, (float)y2_start, 36, 36);

        }
      }
    }


     //base.OnPaint(ee);
    }//End of OnPaint belonging only to Graph Panel class.


}//End of class DrawUI
