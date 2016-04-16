namespace Teeth.Demo.UI
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using Model;
    using Model.Domain;
    using Model.Geometry;
    using Geometry = Model.Geometry.Geometry;

    /// <summary>
    /// Interaction logic for ToothView.xaml
    /// </summary>
    public partial class ToothView : ISelectable
    {
        public Geometry Geometry { get; }
        public Tooth Tooth { get; }
        private readonly ViewContext viewContext;

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
            "IsSelected", typeof(bool), typeof(ToothView), new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty IsHighlightedProperty = DependencyProperty.Register(
            "IsHighlighted", typeof(bool), typeof(ToothView), new PropertyMetadata(default(bool)));

        public ToothView()
        {
            InitializeComponent();
        }

        public ToothView(DrawableTooth tooth, ViewContext viewContext)
        {
            this.Geometry = tooth.Geometry;
            this.viewContext = viewContext;
            this.Tooth = tooth.Tooth;

            InitializeComponent();

            this.Polygon.Points = new PointCollection(this.Geometry.Data);

            Canvas.SetLeft(this, this.Geometry.Offset.X);
            Canvas.SetTop(this, this.Geometry.Offset.Y);
        }

        public bool IsSelected
        {
            get { return (bool) GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public bool IsHighlighted
        {
            get { return (bool) GetValue(IsHighlightedProperty); }
            set { SetValue(IsHighlightedProperty, value); }
        }

        private void Polygon_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.viewContext.HandleClick(this);
        }
    }
}
