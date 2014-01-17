using System;
using System.Reflection;

namespace PropertyAccessor.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            LoadAllTypes();

            GetProperty();

            SetProperty();

            GetGenericProperty();

            SetGenericProperty();

            // enumerables, array, dictionary
            // dynamic
            // static
            // struct, reference
            // deep models
            // public, private, internal, protected

            Console.ReadKey();
        }

        private static void SetGenericProperty()
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                LogError("SetGenericProperty failed", ex);
            }
        }

        private static void GetGenericProperty()
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogError("SetGenericProperty failed", ex);
            }
        }

        private static void SetProperty()
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogError("SetGenericProperty failed", ex);
            }
        }

        private static void GetProperty()
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogError("SetGenericProperty failed", ex);
            }
        }

        private static void LoadAllTypes()
        {
            try
            {
                Assembly assembly = typeof(string).Assembly;

                var types = assembly.GetTypes();

                foreach (Type type in types)
                {
                    PropertyAccessor.Manager.CreateTypeModel(type);

                    Console.WriteLine(type.Name);
                }
            }
            catch(Exception ex)
            {
                LogError("LoadAllTypes failed", ex);

            }
        }

        private static void LogError(string message, Exception ex)
        {
            Console.WriteLine("*************************************");
            Console.WriteLine(message);
            Console.WriteLine(ex.ToString());
            Console.WriteLine("*************************************");
        }
    }
}
