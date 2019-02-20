﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace HueSharp.Messages
{
    public class HueResponseException : Exception
    {
        public List<ErrorMessage> Errors { get; }

        public HueResponseException(ErrorResponse response) : base(string.Join(Environment.NewLine, response.Select(p => $"({(int)p.Type}): {p.Description}")))
        {
            Errors = new List<ErrorMessage>(response);
        }
    }
}
