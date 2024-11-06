﻿using Microsoft.AspNetCore.DataProtection;
using System;

namespace OnlineShoppingApp.Business.DataProtection
{
    
    public class DataProtection : IDataProtection
    { 
        private readonly IDataProtector _protector;

        public DataProtection(IDataProtectionProvider provider)
        { 
            _protector = provider.CreateProtector("OnlineShoppingApp-security-v1");
        }
        public string Protect(string text)
        {
            return _protector.Protect(text);
        }
        public string UnProtect(string protectedText)
        {
            return _protector.Unprotect(protectedText);
        }
    }
}
