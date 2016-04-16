namespace Teeth.Demo.Model
{
    using System.Windows;
    using System.Windows.Controls;
    using Domain;


    /// <summary>
    /// Represents scene to draw geometry on
    /// </summary>
    public class Scene
    {
        public Scene(Canvas canvas)
        {
            this.SceneMiddle = new Point(canvas.Width / 2, canvas.Height / 2);
            this.UpperJawMiddle = new Point(canvas.Width / 2, canvas.Height / 2 - canvas.Height / 6);
            this.LowerJawMiddle = new Point(canvas.Width / 2, canvas.Height / 2 + canvas.Height / 6);
        }

        public Point SceneMiddle { get; }

        public Point UpperJawMiddle { get; }

        public Point LowerJawMiddle { get; }

        public Point SelectMiddleFor(Tooth tooth)
        {
            return tooth.Quarter > 2 ? this.LowerJawMiddle : this.UpperJawMiddle;
        }
    }
}