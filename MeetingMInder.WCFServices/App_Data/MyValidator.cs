
using System;
using System.Collections.Generic;
using System.Text;
using System.IdentityModel.Selectors;
using System.Collections;
using System.ServiceModel;
class MyValidator : UserNamePasswordValidator
{

    public override void Validate(string userName, string password)
    {

        if ((userName == "shiv123") && (password == "pass123"))
        {

        }
        else
        {
            throw new FaultException("Invalid credentials");
        }
    }
}

