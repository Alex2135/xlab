using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResetPasswordModel
{
    event Action SuccessResetPasswordEvent;
    event Action<string> ResetPasswordErrorEvent;
    bool OnEmailReset(string _email);
}

public class ResetPasswordModel : IResetPasswordModel
{
    public event Action SuccessResetPasswordEvent;
    public event Action<string> ResetPasswordErrorEvent;

    public bool OnEmailReset(string _email)
    {
        bool result = true;
        if (_email == null || _email == "")
        {
            ResetPasswordErrorEvent?.Invoke("Введите email");
            result = false;
        }
        else if (_email != "hh@hh.hh")
        {
            ResetPasswordErrorEvent?.Invoke("Данный email не найден");
            result = false;
        }

        if (result == true)
        {
            Debug.Log("Send reset password email!");
            SuccessResetPasswordEvent?.Invoke();
        }
        return result;
    }
}
