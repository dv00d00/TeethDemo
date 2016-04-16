namespace Teeth.Demo.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Controls;
    using Domain;
    using Geometry;
    using Properties;
    using UI;

    /// <summary>
    /// Implements user interaction logic mapped to the scene
    /// Candidate for refactoring
    /// </summary>
    public class ViewContext
    {
        private readonly List<ToothView> toothViews;
        private readonly List<GroupView> groupViews;
        private readonly Mouth mouth;
        private readonly GeometryRepository geometryRepository;
        private readonly Canvas canvas;

        public ViewContext(Canvas canvas)
        {
            this.canvas = canvas;
            this.geometryRepository = new GeometryRepository(ImageParser.Parse());
            this.mouth = new Mouth();

            this.toothViews = this.mouth.Teeth.Select(tooth => new ToothView(
                this.geometryRepository.GetDrawableToothFor(tooth),
                this)).ToList();

            this.groupViews = new List<GroupView>();

            foreach (var toothView in this.toothViews)
            {
                canvas.Children.Add(toothView);
            }

            this.Scene = new Scene(canvas);
        }

        public Scene Scene { get; }
        public bool ShouldHighlightMirror { get; private set; } = true;
        public bool IsMultiselectEnabled { get; private set; }
        public bool IsRangeSelectEnabled { get; private set; }

        public event EventHandler SelectionChanged;

        [Pure]
        public Group CaptureSelection(string name)
        {
            var teeth = this.toothViews
                .Where(it => it.IsSelected)
                .Select(it => it.Tooth)
                .ToList();

            return new Group(teeth, name);
        }

        public void HandleClick(ToothView toothView)
        {
            this.Select(toothView);

            if (this.ShouldHighlightMirror)
            {
                var pairTooth = this.mouth.GetPairFor(toothView.Tooth);
                var pairView = this.toothViews.First(it => it.Tooth == pairTooth);
                this.toothViews.ForEach(it => it.IsHighlighted = false);
                pairView.IsHighlighted = true;
            }
        }

        public void HandleGroupClick(GroupView groupView)
        {
            groupView.IsSelected = !groupView.IsSelected;

            if (groupView.IsSelected)
            {
                this.toothViews.ForEach(it =>
                {
                    it.IsHighlighted = false;
                    it.IsSelected = groupView.Teeth.Any(x => x.Tooth == it.Tooth);
                });

                foreach (var gv in this.groupViews.Where(it => !Equals(it, groupView)))
                {
                    gv.IsSelected = false;
                }
            }
        }

        public void HandleNavigateNext()
        {
            this.Select(this.GetNext());
        }

        public void HandleNavigatePrevious()
        {
            this.Select(this.GetPrevious());
        }

        public void AddGroup(Group group)
        {
            var teeth = group.Teeth.Select(it => this.geometryRepository.GetDrawableToothFor(it))
                .ToList();

            int level = this.groupViews.Count(it => it.Teeth.Intersect(teeth).Any());

            var groupView = new GroupView(teeth, this, level, group.Id);

            this.groupViews.Add(groupView);
            this.canvas.Children.Add(groupView);
        }

        public void ToogleMultiselect(bool multiSelect)
        {
            this.IsMultiselectEnabled = multiSelect;
            this.ShouldHighlightMirror = !multiSelect;

            if (!this.ShouldHighlightMirror)
            {
                this.toothViews.ForEach(it => it.IsHighlighted = false);
            }
        }

        public void ToogleRangeSelect(bool rangeSelect)
        {
            this.IsRangeSelectEnabled = rangeSelect;
        }

        public void HandleSelectGroup(Guid id)
        {
            this.HandleGroupClick(this.groupViews.First(it => it.GroupId == id));

            this.RaiseSelectChanged();
        }

        public void DeleteGroup(Guid id)
        {
            var groupView = this.groupViews.FirstOrDefault(it => it.GroupId == id);
            if (groupView != null)
            {
                this.canvas.Children.Remove(groupView);
            }
        }

        private void Select(ToothView toothView)
        {
            if (this.IsRangeSelectEnabled)
            {
                var last = this.toothViews.LastOrDefault(it => it.IsSelected);

                if (last != null)
                {
                    foreach (var tooth in this.mouth.GetRangeInDisplayOrder(last.Tooth, toothView.Tooth))
                    {
                        this.toothViews.First(it => it.Tooth == tooth).IsSelected = true;
                    }

                    toothView.IsSelected = true;
                }
            }
            else
            {
                if (!this.IsMultiselectEnabled)
                {
                    foreach (var source in this.GetSelectable().Where(it => it.IsSelected))
                    {
                        source.IsSelected = false;
                    }
                }

                toothView.IsSelected = !toothView.IsSelected;
            }

            this.RaiseSelectChanged();
        }

        [Pure]
        private ToothView GetNext()
        {
            var findIndex = this.toothViews.FindIndex(it => it.IsSelected);
            findIndex = (findIndex + 1) % this.toothViews.Count;
            return this.toothViews[findIndex];
        }

        [Pure]
        private ToothView GetPrevious()
        {
            var findIndex = this.toothViews.FindIndex(it => it.IsSelected);
            findIndex = findIndex - 1;
            if (findIndex < 0)
            {
                findIndex = this.toothViews.Count - 1;
            }
            return this.toothViews[findIndex];
        }

        [Pure]
        private IEnumerable<ISelectable> GetSelectable()
        {
            foreach (var toothView in this.toothViews)
            {
                yield return toothView;
            }

            foreach (var groupView in this.groupViews)
            {
                yield return groupView;
            }
        }

        private void RaiseSelectChanged()
        {
            this.SelectionChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}