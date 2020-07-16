using Model;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace XmlDatabase
{
    public interface IXmlData
    {
        void Add(Mod mod);
        int Count();
        void Edit(Mod mod);
        BindingList<Mod> Get(string ID = null);
        void Remove(Mod mod);
    }
}