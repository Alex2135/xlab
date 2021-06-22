using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoginModel
{
    event Action<string> RequestEmailErrorsEvent;
    event Action<string> RequestPasswordErrorsEvent;

    bool IsLoginSuccess(string _email, string _pass);
    void RememberUser(string _email, string _pass);
}

public class LoginScreenModel : ILoginModel
{
    public event Action<string> RequestEmailErrorsEvent;
    public event Action<string> RequestPasswordErrorsEvent;

    public bool IsLoginSuccess(string _email, string _pass)
    {
        bool result = true;

        if (!EmailValidation(_email))
        {
            RequestEmailErrorsEvent?.Invoke("Введен неверный email");
            result = false;
        }
        if (!PasswordValidation(_pass))
        {
            RequestPasswordErrorsEvent?.Invoke("Не правильный пароль");
            result = false;
        }

        return result;
    }

    public void RememberUser(string _email, string _pass)
    {
        
    }

    private bool EmailValidation(string _email)
    {
        bool result = true;

        if (!_email.Contains("@")) result = false;
        if (_email != "hh@hh.hh") result = false;

        return result;
    }

    private bool PasswordValidation(string _pass)
    {
        bool result = true;

        if (_pass.Length < 6) result = false;
        if (_pass != "111111") result = false;

        return result;
    }
}
