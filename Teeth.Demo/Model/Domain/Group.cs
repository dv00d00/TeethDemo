namespace Teeth.Demo.Model.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Group entity
    /// </summary>
    public class Group
    {
        public Guid Id { get; }

        public string Name { get; }

        public IReadOnlyCollection<Tooth> Teeth { get; }

        public Group(IReadOnlyCollection<Tooth> teeth, string name)
        {
            this.Teeth = teeth;
            this.Name = name;
            this.Id = Guid.NewGuid();
        }

        public string TeethPrint => string.Join(" ", this.Teeth.Select(it => it.ToString()));
    }
}
