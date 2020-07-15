using System.ComponentModel;

namespace Model
{
    public interface IBaseModel
    {
        string ID { get; set; }

        event PropertyChangedEventHandler PropertyChanged;

        bool Equals(BaseModel other);
        bool Equals(object obj);
        int GetHashCode();
    }
}