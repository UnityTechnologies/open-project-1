# Pool

![Pool Diagram](https://i.imgur.com/mhhSsx4.png)

Object pooling aims to alleviate the performance hit of instantiating/destroying many objects by activating/deactivating the objects instead, reusing objects when needed.

## Table of Contents

- [Pool](#pool)
  - [Table of Contents](#table-of-contents)
    - [Creating a new poolable type](#creating-a-new-poolable-type)
    - [Creating a factory for the new poolable type](#creating-a-factory-for-the-new-poolable-type)
    - [Creating a pool for the new poolable type](#creating-a-pool-for-the-new-poolable-type)
    - [Using the pool](#using-the-pool)

### Creating a new poolable type

```cs
using UOP1.Pool;
using UnityEngine;

public class MyPoolableComponent : MonoBehaviour, IPoolable
{
	public void OnRequest()
	{
	}

	public void OnReturn(Action onReturned)
	{
		onReturned.Invoke();
	}
}
```

### Creating a factory for the new poolable type

```cs
using UnityEngine;
using UOP1.Factory;

[CreateAssetMenu(fileName = "MyPoolableComponentFactory", menuName = "Factory/MyPoolableComponent")]
public class MyPoolableComponentFactorySO : FactorySO<MyPoolableComponent>
{
    public override MyPoolableComponent Create(){
        return new GameObject("MyPoolableComponent").AddComponent<MyPoolableComponent>();
    }
}
```

### Creating a pool for the new poolable type

```cs
using UnityEngine;
using UOP1.Pool;
using UOP1.Factory;

[CreateAssetMenu(fileName = "MyPoolableComponentPool", menuName = "Pool/MyPoolableComponent")]
public class MyPoolableComponentPoolSO : ComponentPoolSO<MyPoolableComponent>
{
	[SerializeField]
	private MyPoolableComponentFactorySO _factory;
	[SerializeField]
	private int _initialPoolSize;

	public override IFactory<MyPoolableComponent> Factory
	{
		get
		{
			return _factory;
		}
		set
		{
			_factory = value as MyPoolableComponentFactorySO;
		}
	}

	public override int InitialPoolSize
	{
		get
		{
			return _initialPoolSize;
		}
		set
		{
			_initialPoolSize = value;
		}
	}
}
```

### Using the pool

There are two ways to use a pool. Either you can create the pool and factory as an asset in the project and set it to an object reference in the inspector or you can create the pool and factory at runtime.

As an asset:

```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPoolManager : MonoBehaviour
{
	[SerializeField]
	private MyPoolableComponentPoolSO _pool = default;

	private void Start()
	{
		MyPoolableComponent myPoolableComponent = _pool.Request();
        //Do something with it
		_pool.Return(myPoolableComponent);
	}
}
```

At runtime:

```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRuntimePoolManager : MonoBehaviour
{
	[SerializeField]
	private int _initialPoolSize = 5;

	private MyPoolableComponentPoolSO _pool;

	private void Start()
	{
		_pool = ScriptableObject.CreateInstance<MyPoolableComponentPoolSO>();
		_pool.name = gameObject.name;
		_pool.Factory = ScriptableObject.CreateInstance<MyPoolableComponentFactorySO>();;
		_pool.InitialPoolSize = _initialPoolSize;
		MyPoolableComponent myPoolableComponent = _pool.Request();
		//Do something with it
		_pool.Return(myPoolableComponent);
	}

}
```
