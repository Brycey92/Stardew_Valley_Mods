using Godot;
using System;
using System.Collections.Generic;

namespace SdvRevitalizeCreationUtility.Scripts
{

    /// <summary>
    /// TODO: Seperate out the different scenes for blueprint object creation into it's own scene that can be instantiated.
    /// TODO: Create a recipe creation scene where the field is just a TextEdit group.
    /// TODO: Create a Mail/Letter creation scene.
    /// TODO: Create a seperate scene for just Display string creation.
    /// TODO: Potentially add in a way to also autogenerate appropriate CS fields into Revitalize's mod constants files to also save time there.
    /// </summary>
    public class Game : Control
    {
        // Declare member variables here. Examples:
        // private int a = 2;
        // private string b = "text";

        // Called when the node enters the scene tree for the first time.\

        public static Game Self;

        public override void _Ready()
        {
            Self = this;
            // OS.WindowFullscreen = true;

            OS.WindowBorderless = false;
            OS.WindowMaximized = true;
        }

        //  // Called every frame. 'delta' is the elapsed time since the previous frame.
        //  public override void _Process(float delta)
        //  {
        //      
        //  }

        public virtual string GetPathToInputFields()
        {
            return System.IO.Path.Combine("ScrollContainer", "VBoxContainer");
        }

        /// <summary>
        /// Gets the path to the executable binary.
        /// </summary>
        /// <returns></returns>
        public virtual string getGameDirectory()
        {
            if (Game.IsEditor() == false)
            {
                return OS.GetExecutablePath().GetBaseDir();
            }
            else
            {
                return ProjectSettings.GlobalizePath("res://");
            }
            //return ProjectSettings.GlobalizePath("res://");
        }

        public virtual string getRevitalizeBaseFolder()
        {
            List<string> strs = new List<string>();

            string[] splits = this.getGameDirectory().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in splits)
            {
                strs.Add(s);
            }
            strs.RemoveAt(strs.Count - 1);
            //Gross, but strips out the beginning disk drive lettering/name and forces it into a directory.
            string path = "/" + (System.IO.Path.Combine(strs.ToArray()).Replace("\\", "/").Substring(2) + "/");
            return path;
        }

        public virtual string getRevitalizeEnglishContentPackFolder()
        {
            string contentPackPath = System.IO.Path.Combine(this.getRevitalizeBaseFolder(), "ContentPacks", "RevitalizeContentPack en-US" + "/").Replace("\\", "/");
            return contentPackPath;
        }

        /// <summary>
        /// Checks to see if the game is currently runnin inside the editor ui, or an editor build. Will return <code>false</code> for release builds.
        /// </summary>
        /// <returns></returns>
        public static bool IsEditor()
        {
            return OS.HasFeature("editor");
        }
    }
}
