using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPasswordPresenter 
{
    public bool IsChecked => isChecked;
    private IResetPasswordView view;
    private IResetPasswordModel model;
    private bool isChecked;

    public ResetPasswordPresenter(IResetPasswordView _view, IResetPasswordModel _model)
    {
        isChecked = false;
        view = _view;
        model = _model;

        view.ResetPasswordEvent += ResetPassword;
        model.ResetPasswordErrorEvent += SetError;
        model.SuccessResetPasswordEvent += SccessReset;
    }

    public bool ResetPassword(string _email)
    {
        return model.OnEmailReset(_email);
    }

    private void SccessReset()
    {
        isChecked = true;
        view.SuccessReset();
    }

    private void SetError(string _error)
    {
        view.SetError(_error);
    }

}
