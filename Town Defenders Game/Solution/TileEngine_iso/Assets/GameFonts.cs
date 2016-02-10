using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Assets
{
    public class GameFonts
    {
        public static bool FontsLoaded = false;
        static private Dictionary<string, SpriteFont> _fonts = new Dictionary<string, SpriteFont>();

        public static SpriteFont GetFont(string identifier)
        {
            if (_fonts.ContainsKey(identifier))
                return _fonts[identifier];
            throw new Exception("Wrong Font Identifier");
        }

        public static void Load(ContentManager content)
        {
            if (FontsLoaded) return;

            _fonts["f1"] = content.Load<SpriteFont>(@"Fonts\sf1");
            _fonts["f2"] = content.Load<SpriteFont>(@"Fonts\BlackAdder");
            _fonts["f3"] = content.Load<SpriteFont>(@"Fonts\gaslon");
            _fonts["f4"] = content.Load<SpriteFont>(@"Fonts\dock");
            _fonts["f5"] = content.Load<SpriteFont>(@"Fonts\statusbar");

            FontsLoaded = true;
        }
    }
}
