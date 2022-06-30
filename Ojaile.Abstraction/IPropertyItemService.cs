using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ojaile.Abstraction
{
    public interface IPropertyItemService
    {
        void SavePropertyItem();
        void DeletePropertyItem();
        void UpdatePropertyItem(string name, object value);
        object GetPropertyItemByName(string name);
        object GetValue(string name, object defaultValue);
        object GetPropertyItemById(int Id);
        List<object> GetPropertyItem();

    }
}
