using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class DIContainer 
{
    private readonly Dictionary<Type, Registration> _registrations = new Dictionary<Type, Registration>();
    public class Registration // наш объект, который регистрируется (типа чертежа)
    {
        public Type TypeOfRegistration { get; }
        public bool IsSingleton { get; }
        public object Instance { get; set; }
        public Registration(Type type, bool isSingletone)
        {
            TypeOfRegistration = type;
            IsSingleton = isSingletone;
        }
    }
    public void Register<TService, TypeOfRegistration>(bool isSingletone) // метод, регистрирующий наш чертеж Registration.
        where TypeOfRegistration : TService
    {
        var _serviceType = typeof(TService);
        var _registringObjectType = typeof(TypeOfRegistration);
        _registrations[_serviceType] = new Registration(_registringObjectType, isSingletone); 
    }
    public TService Resolve<TService>()
    {
        return (TService)Resolve(typeof(TService)); 
    }
    private object Resolve(Type obj) 
    {
        if (!_registrations.TryGetValue(obj, out var registration)) // проверяем, зарегистрирован ли наш объект
        {
            throw new Exception("Не зарегистрирован");
        } 
        if (registration.IsSingleton && registration.Instance != null)
        {
            return registration.Instance; // если зарегистрирован как синглтон, то возвращаем его инстанс
        }

        var _createdObject = CreateInstance(registration.TypeOfRegistration);

        if (registration.IsSingleton)
        {
            return registration.Instance = _createdObject; // если зарегистрирован как синглтон, то сохраняем инстанс
        }
        else
        {
            return _createdObject; // если не синглтон, то просто возвращаем инстанс
        }
    }
    
    private object CreateInstance(Type type) // метод, который создает инстанс объекта
    {
        var _constructor = type.GetConstructors().First();
        if (_constructor == null)
        {
            var method = type.GetMethod("Init");
            var parameters = method.GetParameters();
            var argumentsOfMethod = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                argumentsOfMethod[i] = Resolve(parameters[i].ParameterType);
            }
            return _constructor.Invoke(argumentsOfMethod);
        }
        else
        {
            var _parameters = _constructor.GetParameters();
            var argumentsOfConstructor = new object[_parameters.Length];

            for (int i = 0; i < _parameters.Length; i++)
            {
                argumentsOfConstructor[i] = Resolve(_parameters[i].ParameterType);
            }
            return _constructor.Invoke(argumentsOfConstructor);
        }       
    }    
}
