namespace Teeth.Demo.Model.Geometry
{
    using System.Collections.Generic;
    using System.Windows;
    using Newtonsoft.Json;

    /// <summary>
    /// Drawable polygon with bounding box
    /// </summary>
    public class Geometry
    {
        [JsonProperty("bounds")]
        public Rect BoundingBox { get; set; }

        [JsonIgnore]
        public Point Offset => this.BoundingBox.TopLeft;

        [JsonProperty("polygon")]
        public List<Point> Data { get; set; }

        public Geometry(IEnumerable<Point> collection, Rect bounds)
        {
            this.BoundingBox = bounds;
            this.Data = new List<Point>(collection);
        }

        public Geometry() { }
    }
}