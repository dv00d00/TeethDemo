namespace Teeth.Demo.Model.Geometry
{
    using System.Collections.Generic;
    using Domain;
    using Newtonsoft.Json;

    /// <summary>
    /// Provides geometry per tooth object
    /// </summary>
    public class GeometryRepository
    {
        public GeometryRepository()
        {
            this.Store = JsonConvert.DeserializeObject<Dictionary<int, List<Geometry>>>(Data.Default.ParsedGeometryJSON);
        }

        public GeometryRepository(string json)
        {
            this.Store = JsonConvert.DeserializeObject<Dictionary<int, List<Geometry>>>(json);
        }

        public Dictionary<int, List<Geometry>> Store { get; }

        public Geometry GetGeometryFor(Tooth tooth)
        {
            return this.Store[tooth.Quarter][tooth.Number - 1];
        }

        public DrawableTooth GetDrawableToothFor(Tooth tooth)
        {
            return new DrawableTooth(tooth, this.GetGeometryFor(tooth));
        }
    }
}