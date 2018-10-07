using System;

namespace Vinance.Contracts.Models.BaseModels
{
    public class Transaction
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int Amount { get; set; }

        public string Comment { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as BaseModel;
            return other?.Id == Id;
        }

        public bool Equals(BaseModel other)
        {
            return Id == other?.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}