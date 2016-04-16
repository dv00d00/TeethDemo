namespace Teeth.Demo.Model.Domain
{
    using System;

    /// <summary>
    /// Tooth represented as value object
    /// </summary>
    public struct Tooth : IComparable<Tooth>
    {
        public int Number { get;  }
        public int Quarter { get;  }

        public Tooth(int quarter, int number)
        {
            this.Number = number;
            this.Quarter = quarter;
        }

        public bool Equals(Tooth other)
        {
            return this.Number == other.Number && this.Quarter == other.Quarter;
        }

        public int CompareTo(Tooth other)
        {
            return this.Quarter*10 + this.Number - other.Quarter*10 - other.Number;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Tooth && this.Equals((Tooth) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (this.Number*397) ^ this.Quarter;
            }
        }

        public static bool operator ==(Tooth left, Tooth right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Tooth left, Tooth right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return $"{this.Quarter}{this.Number}";
        }

        public int GetDisplayOrder()
        {
            if (this.Quarter % 2 == 1)
                return this.Quarter*10 + Math.Abs(this.Number - 8);
            return this.Quarter * 10 + this.Number;
        }
    }
}