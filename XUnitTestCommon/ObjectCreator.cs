using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using XUnitTestCommon.DTOs;

namespace XUnitTestCommon
{
    public class ObjectCreator<T>
    {

        public static List<T> CreateListObjects(Type type)
        {
            var res = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(new TokenDTO()));

            var o = JObject.Parse(JsonConvert.SerializeObject(new TokenDTO()));

            var t1 = o.Children().ToList();

            //var token1 = res

           // var res = Math.Log(600, 3);

            var t = type.GetTypeInfo();

            var fields = t.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);    //GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

            var result = new List<T>();

            //for(int i=1; i < fields.Length; i++) //длина элементов, которые строим
            //{
            //    for(int j=0; j < fields.Length - i; j++) // первый элемент
            //    {

            //    }
            //}

            // вот так создавать
            var instance = (T)Activator.CreateInstance(type);
            fields.ToList().ForEach(f => 
            {
                var currentType = f.FieldType;

                if (currentType == typeof(String))
                {
                    f.SetValue(instance, "some value");
                }
                else if (currentType == typeof(double))
                {
                    f.SetValue(instance, 1d);
                }
                else if (currentType == typeof(Guid))
                {
                    f.SetValue(instance, Guid.NewGuid());
                }
                else
                {
                    f.SetValue(instance, currentType);
                }
            });


            


            return result;
        }
    }

    public class someClass
    {
        //[Test]
        public void debugTest()
        {



            var ob = new ClientRegisterResponseDTO();//new TokenDTO();

            var r = ObjectCreator<ClientRegisterResponseDTO>.CreateListObjects(ob.GetType());
        }
    }
}
