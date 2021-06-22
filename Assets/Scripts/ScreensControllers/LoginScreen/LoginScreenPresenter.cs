using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoginPresenter
{
    event Action<string> EmailErrorsEvent;
    event Action<string> PasswordErrorsEvent;
    void OnModelEmailErrors(string _error);
    void OnModelPasswordErrors(string _error);
}

public class LoginScreenPresenter : ILoginPresenter
{
    private ILoginView view;
    private ILoginModel model;
    public event Action<string> EmailErrorsEvent;
    public event Action<string> PasswordErrorsEvent;

    public LoginScreenPresenter(ILoginView _view, ILoginModel _model)
    {
        view = _view;
        model = _model;

        model.RequestEmailErrorsEvent += OnModelEmailErrors;
        model.RequestPasswordErrorsEvent += OnModelPasswordErrors;

        EmailErrorsEvent += view.OnEmailError;
        PasswordErrorsEvent += view.OnPasswordError;
    }

    public bool CheckEmailAndPassword(string _email, string _pass)
    {
        bool result = false;
        if (model.IsLoginSuccess(_email, _pass))
        {
            view.SuccessLogin();
            result = true;
        }

        return result;
    }

    public void OnModelEmailErrors(string _error)
    {
        EmailErrorsEvent.Invoke(_error);
    }

    public void OnModelPasswordErrors(string _error)
    {
        PasswordErrorsEvent.Invoke(_error);
    }
}
