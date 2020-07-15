namespace Model
{
    public interface IMod
    {
        string ModID { get; set; }
        string ModName { get; set; }

        bool Equals(Mod other);
        bool Equals(object obj);
        int GetHashCode();
    }
}