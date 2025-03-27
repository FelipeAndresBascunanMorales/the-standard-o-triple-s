﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace OtripleS.Web.Api.Models.Teachers.Exceptions
{
    public class InvalidTeacherException : Xeption
    {
        public InvalidTeacherException()
            : base(message: "Invalid teacher. Please fix the errors and try again.") { }
    }
}
