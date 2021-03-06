﻿using System;

namespace CrossHMI.LibraryIntegration.Models
{
    /// <summary>
    /// Model for passing binding notification parameters.
    /// </summary>
    public class CreateBindingEventArgs
    {
        public string Repository { get; }
        public string ProcessValue { get; }
        public Type BindingType { get; }

        public CreateBindingEventArgs(string repository, string processValue, Type bindingType)
        {
            Repository = repository;
            ProcessValue = processValue;
            BindingType = bindingType;
        }
    }
}
