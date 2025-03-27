﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.CourseAttachments;

namespace OtripleS.Web.Api.Services.Foundations.CourseAttachments
{
    public interface ICourseAttachmentService
    {
        ValueTask<CourseAttachment> AddCourseAttachmentAsync(CourseAttachment courseAttachment);

        IQueryable<CourseAttachment> RetrieveAllCourseAttachments();

        ValueTask<CourseAttachment> RetrieveCourseAttachmentByIdAsync(
                Guid courseId,
                Guid attachmentId);

        ValueTask<CourseAttachment> RemoveCourseAttachmentByIdAsync(
            Guid courseId,
            Guid attachmentId);
    }
}
