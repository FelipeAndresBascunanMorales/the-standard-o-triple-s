﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace OtripleS.Web.Api.Models.Teachers.Exceptions
{
    public class NullTeacherException : Xeption
    {
        public NullTeacherException() : base(message: "The teacher is null.") { }
    }
}
