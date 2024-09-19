// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.Serialization;

namespace AntDesign.Docs.Build.CLI.Services.Translation
{
    [Serializable]
    internal class TranslationTooManyRequestsException : Exception
    {
        public TranslationTooManyRequestsException()
        {
        }

        public TranslationTooManyRequestsException(string message) : base(message)
        {
        }

        public TranslationTooManyRequestsException(string message, Exception innerException) : base(message, innerException)
        {
        }

#if NET8_0_OR_GREATER
        [Obsolete(DiagnosticId = "SYSLIB0051")]
#endif
        protected TranslationTooManyRequestsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
