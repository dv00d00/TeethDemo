namespace Teeth.Demo.Model
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Windows;
    using AForge;
    using AForge.Imaging;
    using AForge.Imaging.Filters;
    using AForge.Math.Geometry;
    using Domain;
    using Newtonsoft.Json;
    using Point = System.Windows.Point;

    /// <summary>
    /// Creates serialized JSON geometry out of preprocessed image
    /// </summary>
    public class ImageParser
    {
        public static string Parse()
        {
            using (var bitmap = Bitmap.FromFile(@".\Images\RawTeethBlack.png") as Bitmap)
            using (var apply = Grayscale.CommonAlgorithms.RMY.Apply(bitmap))
            {
                new Threshold(20).ApplyInPlace(apply);

                var counter = new BlobCounter
                {
                    BackgroundThreshold = Color.FromArgb(0, 0, 0, 0),
                    MinWidth = 5,
                    MinHeight = 5,
                    MaxWidth = 50,
                    MaxHeight = 50
                };

                var filtering = new BlobsFiltering
                {
                    CoupledSizeFiltering = true,
                };

                filtering.ApplyInPlace(apply);
                counter.ProcessImage(apply);

                var objectsInformation = counter.GetObjectsInformation();

                var detector = new MoravecCornersDetector(10, 3);
                var grahamConvexHull = new GrahamConvexHull();
                var fillHoles = new FillHoles();

                var tg = objectsInformation.Select(blob =>
                {
                    var rect = blob.Rectangle;

                    using (var cropped = apply.Clone(rect, apply.PixelFormat))
                    {
                        fillHoles.ApplyInPlace(cropped);

                        const int tolerance = 0;
                        var floodFill = new PointedColorFloodFill(Color.White)
                        {
                            StartingPoint = new IntPoint(cropped.Width / 2, cropped.Height / 2),
                            Tolerance = Color.FromArgb(tolerance, tolerance, tolerance)
                        };

                        var mask = floodFill.Apply(cropped);

                        var intersect = new Intersect { OverlayImage = mask };
                        intersect.ApplyInPlace(cropped);

                        var processImage = detector.ProcessImage(cropped);
                        var intPoints = grahamConvexHull.FindHull(processImage);

                        var bounds = new Rect(
                            new Point(rect.Location.X, rect.Location.Y),
                            new System.Windows.Size(rect.Width, rect.Height));

                        return new Geometry.Geometry(intPoints.Select(it => new Point(it.X, it.Y)), bounds);
                    }
                }).ToArray();

                var d = new Dictionary<int, List<Geometry.Geometry>>
                {
                    [1] = new List<Geometry.Geometry> { tg[0], tg[2], tg[4], tg[6], tg[8], tg[10], tg[12], tg[14], },
                    [2] = new List<Geometry.Geometry> { tg[1], tg[3], tg[5], tg[7], tg[9], tg[11], tg[13], tg[15], },
                    [3] = new List<Geometry.Geometry> { tg[31], tg[29], tg[26], tg[25], tg[23], tg[21], tg[18], tg[17], },
                    [4] = new List<Geometry.Geometry> { tg[30], tg[28], tg[27], tg[24], tg[22], tg[20], tg[19], tg[16], },
                };

                var serializeObject = JsonConvert.SerializeObject(d);
                return serializeObject;
            }
        }
    }
}