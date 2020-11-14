using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TinyDI;
using BindingFlags = System.Reflection.BindingFlags;

namespace Weber.Core.Container
{
    public class CreateControllerInstance
    {
        /// <summary>
        /// Creates a controller object
        /// </summary>
        /// <param name="controllerType"></param>
        /// <param name="tinyDependencyInjection"></param>
        /// <returns></returns>
        public static object Create(Type controllerType, TinyDependencyInjection tinyDependencyInjection)
        {
            try
            {
                var constructor = controllerType.GetConstructors().FirstOrDefault();
                var parameters = constructor.GetParameters();
                if (parameters.Any())
                {
                    var objs = new List<object>();
                    objs.AddRange(parameters.Select(x => tinyDependencyInjection.GetService(x.ParameterType)));
                    var newControllerObj = Activator.CreateInstance(controllerType,  objs.ToArray());
                    return newControllerObj;
                }
                else
                {
                    return Activator.CreateInstance(controllerType);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"CreateControllerInstance Create Error: {e.Message}");
            }

            return null;
        }
    }
}
