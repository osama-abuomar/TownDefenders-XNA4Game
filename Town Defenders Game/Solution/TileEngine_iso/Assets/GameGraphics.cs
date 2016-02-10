using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Texture = Engine.Assets.Texture;

namespace Engine.Assets
{

    public enum AlignmentMode { Center }


    public class GameGraphics
    {
        static public bool GraphicsLoaded = false;

        // Use file names for Keys
        static private Dictionary<string, Texture2D> _imageSets;
        static private Dictionary<string, Texture2D> _animationSets;
        static private Dictionary<string, Texture2D> _singleImages;

        // Used by other classes
        static private Dictionary<string, Texture> _availableGameTextures;

        /// <summary>
        /// Gets the Texture object using string identifier.
        /// (Note: this method has access to all game texture graphics)
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static Texture GetTexture(string identifier)
        {
            if (_availableGameTextures.ContainsKey(identifier))
                return _availableGameTextures[identifier];
            throw new Exception("Wrong Texture Identifier");
        }

        /// <summary>
        /// Called only once when game loads
        /// (Warning: Heavy method)
        /// </summary>
        /// <param name="content"></param>
        public static void Load(ContentManager content)
        {
            if (GraphicsLoaded) return;

            _imageSets = new Dictionary<string, Texture2D>();
            _animationSets = new Dictionary<string, Texture2D>();
            _singleImages = new Dictionary<string, Texture2D>();
            _availableGameTextures = new Dictionary<string, Texture>();

            _imageSets.Add("terrain_imageset", content.Load<Texture2D>(@"Textures\ImageSets\terrain_imageset"));
            _imageSets.Add("slopemaps", content.Load<Texture2D>(@"Textures\ImageSets\slopemaps"));
            _imageSets.Add("grass", content.Load<Texture2D>(@"Textures\ImageSets\grass"));


            
            _animationSets.Add("swordsman_ss", content.Load<Texture2D>(@"Textures\Characters\swordsman_ss"));
            _animationSets.Add("horseman_ss", content.Load<Texture2D>(@"Textures\Characters\horseman_ss"));
            _animationSets.Add("green_knight_ss", content.Load<Texture2D>(@"Textures\Characters\green_knight_ss"));
            _animationSets.Add("smoke", content.Load<Texture2D>(@"Textures\miscellaneous\smoke_animation2"));
            _animationSets.Add("farmer_ss", content.Load<Texture2D>(@"Textures\Characters\farmer_ss"));

            _singleImages.Add("red_bar", content.Load<Texture2D>(@"Textures\miscellaneous\StatusLine\red"));
            _singleImages.Add("green_bar", content.Load<Texture2D>(@"Textures\miscellaneous\StatusLine\green"));
            _singleImages.Add("yellow_bar", content.Load<Texture2D>(@"Textures\miscellaneous\StatusLine\yellow"));
           
            _singleImages.Add("tree1", content.Load<Texture2D>(@"Textures\Trees\tree1"));
            _singleImages.Add("tree2", content.Load<Texture2D>(@"Textures\Trees\tree2"));
            _singleImages.Add("tree3", content.Load<Texture2D>(@"Textures\Trees\tree3"));
            _singleImages.Add("tree4", content.Load<Texture2D>(@"Textures\Trees\tree4"));
            _singleImages.Add("tree5", content.Load<Texture2D>(@"Textures\Trees\tree5"));
            _singleImages.Add("tree6", content.Load<Texture2D>(@"Textures\Trees\tree6"));
            _singleImages.Add("tree7", content.Load<Texture2D>(@"Textures\Trees\tree7"));
            _singleImages.Add("tree8", content.Load<Texture2D>(@"Textures\Trees\tree8"));
            _singleImages.Add("tree9", content.Load<Texture2D>(@"Textures\Trees\tree9"));
            _singleImages.Add("tree10", content.Load<Texture2D>(@"Textures\Trees\tree10"));
            _singleImages.Add("tree11", content.Load<Texture2D>(@"Textures\Trees\tree11"));
            _singleImages.Add("tree12", content.Load<Texture2D>(@"Textures\Trees\tree12"));
            _singleImages.Add("tree13", content.Load<Texture2D>(@"Textures\Trees\tree13"));
            _singleImages.Add("tree14", content.Load<Texture2D>(@"Textures\Trees\tree14"));
            _singleImages.Add("tree15", content.Load<Texture2D>(@"Textures\Trees\tree15"));
            _singleImages.Add("tree16", content.Load<Texture2D>(@"Textures\Trees\tree16"));
            _singleImages.Add("tree17", content.Load<Texture2D>(@"Textures\Trees\tree17"));
            _singleImages.Add("tree18", content.Load<Texture2D>(@"Textures\Trees\tree18"));
            _singleImages.Add("tree19", content.Load<Texture2D>(@"Textures\Trees\tree19"));
            _singleImages.Add("tree20", content.Load<Texture2D>(@"Textures\Trees\tree20"));
            _singleImages.Add("tree21", content.Load<Texture2D>(@"Textures\Trees\tree21"));
            _singleImages.Add("tree22", content.Load<Texture2D>(@"Textures\Trees\tree22"));
            _singleImages.Add("tree23", content.Load<Texture2D>(@"Textures\Trees\tree23"));
            _singleImages.Add("tree24", content.Load<Texture2D>(@"Textures\Trees\tree24"));
            _singleImages.Add("tree25", content.Load<Texture2D>(@"Textures\Trees\tree25"));
             _singleImages.Add("bg", content.Load<Texture2D>(@"Backgrounds\bg"));
             _singleImages.Add("gameplay_border", content.Load<Texture2D>(@"GUI\gameplay_border"));
             _singleImages.Add("title_screen_bg", content.Load<Texture2D>(@"Backgrounds\title_screen_bg"));
            _singleImages.Add("tree_shadow", content.Load<Texture2D>(@"Textures\Trees\shadow"));
            _singleImages.Add("char_select", content.Load<Texture2D>(@"Textures\Characters\char_select"));
            _singleImages.Add("horseman_select", content.Load<Texture2D>(@"Textures\Characters\horseman_select"));
            _singleImages.Add("camp_selection_line", content.Load<Texture2D>(@"Textures\miscellaneous\selectionLines\camp_selection_line"));
            _singleImages.Add("stable_selection_line", content.Load<Texture2D>(@"Textures\miscellaneous\selectionLines\stable_selection_line"));
            _singleImages.Add("farm_selection_line", content.Load<Texture2D>(@"Textures\miscellaneous\selectionLines\farm_selection_line"));
            _singleImages.Add("residence_selection_line", content.Load<Texture2D>(@"Textures\miscellaneous\selectionLines\residence_selection_line"));

            _singleImages.Add("cloud1", content.Load<Texture2D>(@"Textures\Clouds\cloud1"));
            _singleImages.Add("cloud1_shadow", content.Load<Texture2D>(@"Textures\Clouds\cloud1_shadow"));
            _singleImages.Add("cloud2", content.Load<Texture2D>(@"Textures\Clouds\cloud2"));
            _singleImages.Add("cloud2_shadow", content.Load<Texture2D>(@"Textures\Clouds\cloud2_shadow"));
            _singleImages.Add("cloud3", content.Load<Texture2D>(@"Textures\Clouds\cloud3"));
            _singleImages.Add("cloud3_shadow", content.Load<Texture2D>(@"Textures\Clouds\cloud3_shadow"));

            _singleImages.Add("mouse_hilight", content.Load<Texture2D>(@"Textures\ImageSets\hilight"));
            _singleImages.Add("mousemap", content.Load<Texture2D>(@"Textures\ImageSets\mousemap"));
            _singleImages.Add("building1", content.Load<Texture2D>(@"Textures\Buildings\residence"));
            _singleImages.Add("building2", content.Load<Texture2D>(@"Textures\Buildings\house_small_2"));
            _singleImages.Add("building3", content.Load<Texture2D>(@"Textures\Buildings\house2"));
            _singleImages.Add("building4", content.Load<Texture2D>(@"Textures\Buildings\house3"));
            _singleImages.Add("building5", content.Load<Texture2D>(@"Textures\Buildings\house4"));
            _singleImages.Add("building6", content.Load<Texture2D>(@"Textures\Buildings\camp"));
            _singleImages.Add("farm1", content.Load<Texture2D>(@"Textures\Farm\farm_244x125"));
            _singleImages.Add("right_arrow",content.Load<Texture2D>(@"GUI\right_arrow"));
            _singleImages.Add("left_arrow", content.Load<Texture2D>(@"GUI\left_arrow"));
            _singleImages.Add("left_indexer", content.Load<Texture2D>(@"GUI\left_indexer"));
            _singleImages.Add("right_indexer", content.Load<Texture2D>(@"GUI\right_indexer"));
            _singleImages.Add("seperator", content.Load<Texture2D>(@"GUI\seperator"));

            _singleImages.Add("button_border_96", content.Load<Texture2D>(@"GUI\ImageButton\button_border_96"));
            _singleImages.Add("button_border_48", content.Load<Texture2D>(@"GUI\ImageButton\button_border_48"));
            _singleImages.Add("button_press_96", content.Load<Texture2D>(@"GUI\ImageButton\button_press_96"));
            _singleImages.Add("button_press_48", content.Load<Texture2D>(@"GUI\ImageButton\button_press_48"));
            _singleImages.Add("close_button", content.Load<Texture2D>(@"GUI\ImageButton\close_button"));
            _singleImages.Add("close_button_press", content.Load<Texture2D>(@"GUI\ImageButton\close_button_press"));

            _singleImages.Add("farmer_create_button", content.Load<Texture2D>(@"GUI\ImageButton\farmer_create_button"));
            _singleImages.Add("horseman_create_button", content.Load<Texture2D>(@"GUI\ImageButton\horseman_create_button"));
            _singleImages.Add("knight_create_button", content.Load<Texture2D>(@"GUI\ImageButton\knight_create_button"));
            _singleImages.Add("swordsman_create_button", content.Load<Texture2D>(@"GUI\ImageButton\swordsman_create_button"));
            _singleImages.Add("house_create_button", content.Load<Texture2D>(@"GUI\ImageButton\house_create_button"));
            _singleImages.Add("trainingcamp_create_button", content.Load<Texture2D>(@"GUI\ImageButton\trainingcamp_create_button"));
            _singleImages.Add("stable_create_button", content.Load<Texture2D>(@"GUI\ImageButton\stable_create_button"));

            _singleImages.Add("farmer_96", content.Load<Texture2D>(@"GUI\ImageButton\96\farmer_96"));
            _singleImages.Add("knight_96", content.Load<Texture2D>(@"GUI\ImageButton\96\knight_96"));
            _singleImages.Add("horseman_96", content.Load<Texture2D>(@"GUI\ImageButton\96\horseman_96"));
            _singleImages.Add("swordsman_96", content.Load<Texture2D>(@"GUI\ImageButton\96\swordsman_96"));
            _singleImages.Add("house_96", content.Load<Texture2D>(@"GUI\ImageButton\96\house_96"));
            _singleImages.Add("trainingcamp_96", content.Load<Texture2D>(@"GUI\ImageButton\96\trainingcamp_96"));
            _singleImages.Add("stable_96", content.Load<Texture2D>(@"GUI\ImageButton\96\stable_96"));

            // The Usable Dictionary
            _availableGameTextures.Add("farmer_96", new Texture(_singleImages["farmer_96"]));
            _availableGameTextures.Add("knight_96", new Texture(_singleImages["knight_96"]));
            _availableGameTextures.Add("horseman_96", new Texture(_singleImages["horseman_96"]));
            _availableGameTextures.Add("swordsman_96", new Texture(_singleImages["swordsman_96"]));
            _availableGameTextures.Add("house_96", new Texture(_singleImages["house_96"]));
            _availableGameTextures.Add("trainingcamp_96", new Texture(_singleImages["trainingcamp_96"]));
            _availableGameTextures.Add("stable_96", new Texture(_singleImages["stable_96"]));
            
            _availableGameTextures.Add("button_border_96", new Texture(_singleImages["button_border_96"]));
            _availableGameTextures.Add("button_border_48", new Texture(_singleImages["button_border_48"]));
            _availableGameTextures.Add("button_press_96", new Texture(_singleImages["button_press_96"]));
            _availableGameTextures.Add("button_press_48", new Texture(_singleImages["button_press_48"]));
            _availableGameTextures.Add("close_button", new Texture(_singleImages["close_button"]));
            _availableGameTextures.Add("close_button_press", new Texture(_singleImages["close_button_press"]));

            _availableGameTextures.Add("farmer_create_button", new Texture(_singleImages["farmer_create_button"]));
            _availableGameTextures.Add("horseman_create_button", new Texture(_singleImages["horseman_create_button"]));
            _availableGameTextures.Add("knight_create_button", new Texture(_singleImages["knight_create_button"]));
            _availableGameTextures.Add("swordsman_create_button", new Texture(_singleImages["swordsman_create_button"]));
            _availableGameTextures.Add("house_create_button", new Texture(_singleImages["house_create_button"]));
            _availableGameTextures.Add("trainingcamp_create_button", new Texture(_singleImages["trainingcamp_create_button"]));
            _availableGameTextures.Add("stable_create_button", new Texture(_singleImages["stable_create_button"]));
            

            _availableGameTextures.Add("swordsman_ss", new Texture(_animationSets["swordsman_ss"]));
            _availableGameTextures.Add("horseman_ss", new Texture(_animationSets["horseman_ss"]));
            _availableGameTextures.Add("farmer_ss", new Texture(_animationSets["farmer_ss"]));
            _availableGameTextures.Add("green_knight_ss", new Texture(_animationSets["green_knight_ss"]));
            _availableGameTextures.Add("smoke", new Texture(_animationSets["smoke"]));
            _availableGameTextures.Add("char_select", new Texture(_singleImages["char_select"]));
            _availableGameTextures.Add("horseman_select", new Texture(_singleImages["horseman_select"]));

            // StatusLine
            _availableGameTextures.Add("red_bar", new Texture(_singleImages["red_bar"], AlignmentMode.Center));
            _availableGameTextures.Add("green_bar", new Texture(_singleImages["green_bar"], AlignmentMode.Center));
            _availableGameTextures.Add("yellow_bar", new Texture(_singleImages["yellow_bar"], AlignmentMode.Center));

            _availableGameTextures.Add("tree1", new Texture(_singleImages["tree1"], AlignmentMode.Center));
            _availableGameTextures.Add("tree2", new Texture(_singleImages["tree2"], AlignmentMode.Center));
            _availableGameTextures.Add("tree3", new Texture(_singleImages["tree3"], AlignmentMode.Center));
            _availableGameTextures.Add("tree4", new Texture(_singleImages["tree4"], AlignmentMode.Center));
            _availableGameTextures.Add("tree5", new Texture(_singleImages["tree5"], AlignmentMode.Center));
            _availableGameTextures.Add("tree6", new Texture(_singleImages["tree6"], AlignmentMode.Center));
            _availableGameTextures.Add("tree7", new Texture(_singleImages["tree7"], AlignmentMode.Center));
            _availableGameTextures.Add("tree8", new Texture(_singleImages["tree8"], AlignmentMode.Center));
            _availableGameTextures.Add("tree9", new Texture(_singleImages["tree9"], AlignmentMode.Center));
            _availableGameTextures.Add("tree10", new Texture(_singleImages["tree10"], AlignmentMode.Center));
            _availableGameTextures.Add("tree11", new Texture(_singleImages["tree11"], AlignmentMode.Center));
            _availableGameTextures.Add("tree12", new Texture(_singleImages["tree12"], AlignmentMode.Center));
            _availableGameTextures.Add("tree13", new Texture(_singleImages["tree13"], AlignmentMode.Center));
            _availableGameTextures.Add("tree14", new Texture(_singleImages["tree14"], AlignmentMode.Center));
            _availableGameTextures.Add("tree15", new Texture(_singleImages["tree15"], AlignmentMode.Center));
            _availableGameTextures.Add("bg", new Texture(_singleImages["bg"]));
            _availableGameTextures.Add("gameplay_border", new Texture(_singleImages["gameplay_border"]));
            _availableGameTextures.Add("title_screen_bg", new Texture(_singleImages["title_screen_bg"]));
            _availableGameTextures.Add("tree16", new Texture(_singleImages["tree16"], AlignmentMode.Center));
            _availableGameTextures.Add("tree17", new Texture(_singleImages["tree17"], AlignmentMode.Center));
            _availableGameTextures.Add("tree18", new Texture(_singleImages["tree18"], AlignmentMode.Center));
            _availableGameTextures.Add("tree19", new Texture(_singleImages["tree19"], AlignmentMode.Center));
            _availableGameTextures.Add("tree20", new Texture(_singleImages["tree20"], AlignmentMode.Center));
            _availableGameTextures.Add("tree21", new Texture(_singleImages["tree21"], AlignmentMode.Center));
            _availableGameTextures.Add("tree22", new Texture(_singleImages["tree22"], AlignmentMode.Center));
            _availableGameTextures.Add("tree23", new Texture(_singleImages["tree23"], AlignmentMode.Center));
            _availableGameTextures.Add("tree24", new Texture(_singleImages["tree24"], AlignmentMode.Center));
            _availableGameTextures.Add("tree25", new Texture(_singleImages["tree25"], AlignmentMode.Center));
            _availableGameTextures.Add("tree_shadow", new Texture(_singleImages["tree_shadow"]));

            _availableGameTextures.Add("cloud1", new Texture(_singleImages["cloud1"]));
            _availableGameTextures.Add("cloud1_shadow", new Texture(_singleImages["cloud1_shadow"]));
            _availableGameTextures.Add("cloud2", new Texture(_singleImages["cloud2"]));
            _availableGameTextures.Add("cloud2_shadow", new Texture(_singleImages["cloud2_shadow"]));
            _availableGameTextures.Add("cloud3", new Texture(_singleImages["cloud3"]));
            _availableGameTextures.Add("cloud3_shadow", new Texture(_singleImages["cloud3_shadow"]));

            _availableGameTextures.Add("right_arrow", new Texture(_singleImages["right_arrow"]));
            _availableGameTextures.Add("left_arrow", new Texture(_singleImages["left_arrow"]));
            _availableGameTextures.Add("left_indexer", new Texture(_singleImages["left_indexer"]));
            _availableGameTextures.Add("right_indexer", new Texture(_singleImages["right_indexer"]));
            _availableGameTextures.Add("seperator", new Texture(_singleImages["seperator"]));


            _availableGameTextures.Add("building1", new Texture(_singleImages["building1"], AlignmentMode.Center));
            _availableGameTextures.Add("building2", new Texture(_singleImages["building2"], AlignmentMode.Center));
            _availableGameTextures.Add("building3", new Texture(_singleImages["building3"], AlignmentMode.Center));
            _availableGameTextures.Add("building4", new Texture(_singleImages["building4"], AlignmentMode.Center));
            _availableGameTextures.Add("building5", new Texture(_singleImages["building5"], AlignmentMode.Center));
            _availableGameTextures.Add("building6", new Texture(_singleImages["building6"], AlignmentMode.Center));
            _availableGameTextures.Add("camp_selection_line", new Texture(_singleImages["camp_selection_line"], AlignmentMode.Center));
            _availableGameTextures.Add("stable_selection_line", new Texture(_singleImages["stable_selection_line"], AlignmentMode.Center));
            _availableGameTextures.Add("farm_selection_line", new Texture(_singleImages["farm_selection_line"], AlignmentMode.Center));
            _availableGameTextures.Add("residence_selection_line", new Texture(_singleImages["residence_selection_line"], AlignmentMode.Center));
            _availableGameTextures.Add("farm1", new Texture(_singleImages["farm1"], AlignmentMode.Center));

            _availableGameTextures.Add("mouse_hilight", new Texture(_singleImages["mouse_hilight"]));
            _availableGameTextures.Add("mousemap", new Texture(_singleImages["mousemap"]));

            ///
            /// Identifier: slopemap_[0-7]
            ///
            const int numberOfSlopeMaps = 8;
            for (int i = 0; i < numberOfSlopeMaps; i++)
            {
                _availableGameTextures.Add("slopemap_" + i.ToString(),
                    new Texture(_imageSets["slopemaps"], i * 64, 0, 64, 32, 0, 0));
            }

            /// add tiles .. some are not going to be used.
            ///
            /// Identifier: terrain_tile_[0-11]_[0-9]
            ///
            int tilesAcross = 10;
            int tilesBelow = 12;
            for (int row = 0; row < tilesBelow; row++)
            {
                for (int col = 0; col < tilesAcross; col++)
                {
                    _availableGameTextures.Add("terrain_tile_" + col.ToString() + "_" + row.ToString(),
                        new Texture(_imageSets["terrain_imageset"], x:col * 64, y:row * 64));
                }
            }


            /// add grass tiles
            ///
            /// Identifier: grass_tile_[0-3]_[0-3]
            ///
            tilesAcross = 4;
            tilesBelow = 4;
            for (int row = 0; row < tilesBelow; row++)
            {
                for (int col = 0; col < tilesAcross; col++)
                {
                    _availableGameTextures.Add("grass_tile_" + col.ToString() + "_" + row.ToString(),
                        new Texture(_imageSets["grass"], x: col * 64, y: row * 64));
                }
            }








            GraphicsLoaded = true;
        }
    }
}
