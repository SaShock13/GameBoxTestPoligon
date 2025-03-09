using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class Test : MonoBehaviour
{
    private PlayerInput _input;


	[Inject]
	public void Construct(PlayerInput input)
	{
		_input = input;
	}

    private void Start()
    {

        Debug.Log($"Input is  {_input.gameObject.name}");
    }


}
