using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Assets;
using Microsoft.Xna.Framework.Content;

namespace XNA_Game
{
    public static class Extentions
    {
        /// <summary>
        /// if they are not loaded, content might be already loaded
        /// </summary>
        /// <param name="content"></param>
        public static void LoadAllGameContent(ContentManager content)
        {
            if (!GameGraphics.GraphicsLoaded)
                GameGraphics.Load(content);
            if (!GameFonts.FontsLoaded)
                GameFonts.Load(content);
            if (!GameAudio.AudioLoaded)
                GameAudio.Load(content);

        }
    }
}
