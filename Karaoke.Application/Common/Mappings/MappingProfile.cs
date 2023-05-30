﻿using System.Reflection;
using AutoMapper;

namespace Karaoke.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var mapFromType = typeof(IMap<,>);

        const string mappingMethodName = nameof(IMap<object, object>.Mapping);

        bool HasInterface(Type t)
        {
            return t.IsGenericType && t.GetGenericTypeDefinition() == mapFromType;
        }

        var types = assembly.GetExportedTypes().Where(t => t.GetInterfaces().Any(HasInterface)).ToList();

        var argumentTypes = new[] { typeof(Profile) };

        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);

            var methodInfo = type.GetMethod(mappingMethodName);

            if (methodInfo != null)
            {
                methodInfo.Invoke(instance, new object[] { this });
            }
            else
            {
                var interfaces = type.GetInterfaces().Where(HasInterface).ToList();

                if (interfaces.Count <= 0)
                {
                    continue;
                }

                foreach (var interfaceMethodInfo in interfaces.Select(@interface =>
                             @interface.GetMethod(mappingMethodName, argumentTypes)))
                {
                    interfaceMethodInfo?.Invoke(instance, new object[] { this });
                }
            }
        }
    }
}