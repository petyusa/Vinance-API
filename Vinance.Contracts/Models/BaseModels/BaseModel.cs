namespace Vinance.Contracts.Models.BaseModels
{
    public class BaseModel
    {
        public int Id { get; set; }

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