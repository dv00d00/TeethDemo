namespace Teeth.Demo.UI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using Model;
    using Model.Geometry;

    /// <summary>
    /// Interaction logic for GroupView.xaml
    /// </summary>
    public partial class GroupView : ISelectable
    {
        private readonly ViewContext viewContext;

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
            "IsSelected", typeof (bool), typeof (GroupView), new PropertyMetadata(default(bool)));

        public GroupView()
        {
            InitializeComponent();
        }

        public GroupView(IReadOnlyCollection<DrawableTooth> teeth, ViewContext viewContext, int level, Guid groupId)
        {
            this.InitializeComponent();
            
            this.viewContext = viewContext;
            this.GroupId = groupId;
            this.Teeth = teeth;

            if (teeth.Count == 1)
            {
                var drawableTooth = teeth.First();

                var position = drawableTooth.Geometry.BoundingBox.GetMiddle()
                    .MovePointTowards(viewContext.Scene.SelectMiddleFor(drawableTooth.Tooth), -25 - 10 * level); 

                Canvas.SetLeft(this, position.X);
                Canvas.SetTop(this, position.Y);
                this.Dot.Visibility = Visibility.Visible;
            }
            else
            {
                var res = teeth
                .OrderBy(it => it.Tooth.GetDisplayOrder())
                .ThenBy(it => it.Geometry.Offset.X)
                .ThenBy(it => it.Geometry.Offset.Y)
                .Select(it =>
                {
                    return
                        it.Geometry.BoundingBox.GetMiddle()
                            .MovePointTowards(viewContext.Scene.SelectMiddleFor(it.Tooth), -25 - 10 * level);
                });

                this.Polyline.Points = new PointCollection(res);
                this.Polyline.Visibility = Visibility.Visible;
            }
        }

        public Guid GroupId { get; }

        public IReadOnlyCollection<DrawableTooth> Teeth { get; }

        public bool IsSelected
        {
            get { return (bool) GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        private void GroupClicked(object sender, MouseButtonEventArgs e)
        {
            this.viewContext.HandleGroupClick(this);
        }
    }
}
