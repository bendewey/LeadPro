using System;
using System.Linq;
using System.Reflection;

namespace NetClientProfileAccessOutsideBounds
{
    class Program
    {
        static void Main(string[] args)
        {
            var webAssembly = Assembly.Load("System.Web, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");

            Console.WriteLine("Types in System.Web.UI.WebControls");
            Console.WriteLine();

            var webFormsControlTypes = from t in webAssembly.GetExportedTypes()
                                       where t.Namespace == "System.Web.UI.WebControls"
                                       orderby t.FullName
                                       select t.FullName;

            foreach (var typeName in webFormsControlTypes.Take(20))
            {
                Console.WriteLine("  " + typeName);
            }

            Console.WriteLine("(Showing 20 of " + webFormsControlTypes.Count() + ")");
            Console.ReadLine();
        }
    }
}
