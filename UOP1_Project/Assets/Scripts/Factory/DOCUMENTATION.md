# Factory

![Factory Diagram](https://i.imgur.com/8cleBXk.png)

Without some sort of abstraction, generic systems required to handle a variety of types must implement each creation implementation as well. For example, you must "new" non-abstract types such as Customer, "Instantiate" MonoBehaviours as a component on a GameObject, and "CreateInstance" ScriptableObjects. The **abstract factory pattern** allows for the creation of various types without the knowledge of their specific creation implementation.

## Table of Contents

- [Factory](#factory)
  - [Table of Contents](#table-of-contents)
    - [New-able type with an empty constructor](#new-able-type-with-an-empty-constructor)
    - [New-able type with constructor arguments](#new-able-type-with-constructor-arguments)
    - [Components](#components)
    - [Components on prefabs](#components-on-prefabs)
    - [ScriptableObjects](#scriptableobjects)

### New-able type with an empty constructor

```cs
using UnityEngine;
using UOP1.Factory;

[CreateAssetMenu(fileName = "NewCustomerFactory",menuName="Factory/Customer")]
public class CustomerFactorySO : FactorySO<Customer>
{
    public override Customer Create(){
        return new Customer();
    }
}
```

> **Note:** The CreateAssetMenu attribute may be omitted if the factory is only created via CreateInstance at runtime.

### New-able type with constructor arguments

```cs
using UnityEngine;
using UOP1.Factory;

[CreateAssetMenu(fileName = "NewCustomerFactory",menuName="Factory/Customer")]
public class CustomerFactorySO : FactorySO<Customer>
{
    [SerializeField]
    private int _name;

    public int Name{
        set{
            _name = value;
        }
    }

    public override Customer Create(){
        return new Customer(_name);
    }
}
```

> **Note:** The public property setter may be omitted if the factory is only created as an asset in the project.

### Components

```cs
using UnityEngine;
using UOP1.Factory;

[CreateAssetMenu(fileName = "NewAudioSourceFactory",menuName="Factory/AudioSource")]
public class AudioSourceFactorySO : FactorySO<AudioSource>
{
    public override AudioSource Create(){
        return new GameObject("AudioSource").AddComponent<AudioSource>();
    }
}
```

### Components on prefabs

```cs
using UnityEngine;
using UOP1.Factory;

[CreateAssetMenu(fileName = "NewAudioSourceFactory",menuName="Factory/AudioSource")]
public class AudioSourceFactorySO : FactorySO<AudioSource>
{
    [SerializeField]
    private AudioSource _prefab;

    public int Prefab{
        set{
            _prefab = value;
        }
    }

    public override AudioSource Create(){
        return Instantiate(_prefab);
    }
}
```

### ScriptableObjects

```cs
using UnityEngine;
using UOP1.Factory;

[CreateAssetMenu(fileName = "NewConfigFactory",menuName="Factory/Config")]
public class ConfigFactorySO : FactorySO<ConfigSO>
{
    public override ConfigSO Create(){
        return ScriptableObject.CreateInstance<ConfigSO>();
    }
}
```
