namespace Teeth.Demo.Model.Geometry
{
    using Domain;

    /// <summary>
    /// Tooth to geometry map
    /// </summary>
    public struct DrawableTooth
    {
        public Tooth Tooth { get; }
        public Geometry Geometry { get;  }

        public DrawableTooth(Tooth tooth, Geometry geometry)
        {
            this.Tooth = tooth;
            this.Geometry = geometry;
        }
    }
}