using Raylib_cs;
using System.Numerics;

namespace GameEngine
{
    /// <summary>
    /// An image or texture with a position.
    /// </summary>
    public class Sprite : PhysicsBody
    {
        public Sprite() : base(Vector2.Zero, Vector2.Zero)
        {
            _TextureName = string.Empty;
        }
        /// <summary>
        /// Creates a new instance of a <see cref="Sprite"/>.
        /// </summary>
        /// <param name="textureName">The name of the texture.</param>
        /// 
        public Sprite(string textureName) : this()
        {
            TextureName = textureName;
        }

        #region GameObject

        protected internal override void Draw(VideoService videoService)
        {
            if (!string.IsNullOrEmpty(TextureName))
                videoService.DrawImage(this);
        }

        #endregion

        /// <summary>
        /// Gets or sets the name of the sprite's texture.
        /// </summary>
        public string TextureName
        {
            get => _TextureName;
            set
            {
                _TextureName = value;
                if (!string.IsNullOrEmpty(value))
                {
                    var texture = VideoService.GetTexture(value);
                    Size = new Vector2(texture.width, texture.height);
                }
                else
                    Size = Vector2.Zero;
            }
        }
        private string _TextureName;

        /// <summary>
        /// Gets or sets the tint applied to the sprite's texture.
        /// </summary>
        public Color Tint { get; set; } = Color.WHITE;
    }
}
