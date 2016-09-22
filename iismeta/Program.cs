using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;

namespace iismeta
{
    class Program
    {
        static void Main(string[] args)
        {
            string serverName = System.Environment.MachineName;            
            var root = new DirectoryEntry("IIS://" + serverName);
            PrintChildren(root, 0);
        }

        static void PrintChildren(DirectoryEntry root, int indent)
        {
            foreach (DirectoryEntry child in root.Children)
            {
                var pad = new String(' ', indent * 4);
                Console.WriteLine("{0}+ {1}", pad, child.Name);
                PrintProperties(child, indent + 1);
                PrintChildren(child, indent + 1);
            }
        }

        static void PrintProperties(DirectoryEntry node, int indent)
        {            
            foreach (string key in node.Properties.PropertyNames)
            {
                PropertyValueCollection values = node.Properties[key];                
                PrintPropertyValues(key, values, indent);
            }
        }

        static void PrintPropertyValues(string key, PropertyValueCollection values, int indent)
        {
            var pad = new String(' ', indent * 4);
            Console.Write("{0}- {1} = ", pad, key);
            if (values.Count == 1)
            {
                Console.WriteLine();
                if (key == "AdminACL")
                {
                    // trying to figure out how to print something reasonable for adminacl
                }
            }
            else
            {
                pad += "    ";
                Console.WriteLine(String.Format("({0} values)", values.Count));
                if (key == "MimeMap")
                {
                    foreach (var el in values)
                    {
                        IISOle.IISMimeType mimeType = (IISOle.IISMimeType)el;
                        Console.WriteLine("{0}{1}, {2}", pad, mimeType.Extension, mimeType.MimeType);
                    }
                }
                else
                {
                    foreach (var el in values)
                    {
                        Console.WriteLine("{0}{1}", pad, el);
                    }
                }
            }
        }
    }
}
