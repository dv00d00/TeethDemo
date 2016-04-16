namespace Teeth.Demo.Model.Domain
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Agregate root
    /// </summary>
    public class Mouth
    {
        public Mouth()
        {
            this.Teeth = Enumerable.Range(1, 4)
                .SelectMany(quarter =>
                {
                    return Enumerable.Range(1, 8).Select(number => new Tooth(quarter, number));
                }).ToList();
        }

        public List<Tooth> Teeth { get; set; }

        public Tooth GetPairFor(Tooth tooth)
        {
            int mirrored;
            switch (tooth.Quarter)
            {
                case 1:
                    mirrored = 4;
                    break;
                    
                case 2:
                    mirrored = 3;
                    break;

                case 3:
                    mirrored = 2;
                    break;

                default:
                    mirrored = 1;
                    break;
            }
            
            return new Tooth(mirrored, tooth.Number);
        }

        public IReadOnlyCollection<Tooth> GetRangeInDisplayOrder(Tooth start, Tooth end)
        {
            if (start.GetDisplayOrder() > end.GetDisplayOrder())
            {
                var temp = start;
                start = end;
                end = temp;
            }

            var orderedEnumerable = this.Teeth
                .OrderBy(it => it.GetDisplayOrder());

            var readOnlyCollection = orderedEnumerable
                .SkipWhile(it => it != start)
                .TakeWhile(it => it != end)
                .ToList();

            return readOnlyCollection;
        }
    }
}