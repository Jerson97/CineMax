﻿using System;
using System.Collections.Generic;

namespace CineMax.Domain.Result
{
    public class MessageResult<T>
    {
        public int Code { get; private set; }
        public string Message { get; private set; }
        public T Data { get; private set; }

        private MessageResult(string message, T data, int code)
        {
            Code = code;
            Message = message;
            Data = data;
        }

        public static MessageResult<T> Of(string message, T data, int? code = 1) => new MessageResult<T>(message, data, code.Value);
    }
}
