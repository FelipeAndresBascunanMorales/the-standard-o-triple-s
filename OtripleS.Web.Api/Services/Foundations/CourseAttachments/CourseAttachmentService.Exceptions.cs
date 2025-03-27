﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.CourseAttachments;
using OtripleS.Web.Api.Models.CourseAttachments.Exceptions;
using OtripleS.Web.Api.Models.CoursesAttachments.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.CourseAttachments
{
    public partial class CourseAttachmentService
    {
        private delegate ValueTask<CourseAttachment> ReturningCourseAttachmentFunction();
        private delegate IQueryable<CourseAttachment> ReturningCourseAttachmentsFunction();

        private async ValueTask<CourseAttachment> TryCatch(
            ReturningCourseAttachmentFunction returningCourseAttachmentFunction)
        {
            try
            {
                return await returningCourseAttachmentFunction();
            }
            catch (NullCourseAttachmentException nullCourseAttachmentException)
            {
                throw CreateAndLogValidationException(nullCourseAttachmentException);
            }
            catch (InvalidCourseAttachmentException invalidCourseAttachmentException)
            {
                throw CreateAndLogValidationException(invalidCourseAttachmentException);
            }
            catch (NotFoundCourseAttachmentException notFoundCourseAttachmentException)
            {
                throw CreateAndLogValidationException(notFoundCourseAttachmentException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsCourseAttachmentException =
                    new AlreadyExistsCourseAttachmentException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsCourseAttachmentException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidCourseAttachmentReferenceException =
                    new InvalidCourseAttachmentReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogValidationException(invalidCourseAttachmentReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedCourseAttachmentException =
                    new LockedCourseAttachmentException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedCourseAttachmentException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw CreateAndLogDependencyException(dbUpdateException);
            }
            catch (Exception exception)
            {
                var failedCourseAttachmentServiceException =
                    new FailedCourseAttachmentServiceException(exception);

                throw CreateAndLogServiceException(failedCourseAttachmentServiceException);
            }
        }

        private IQueryable<CourseAttachment> TryCatch(
            ReturningCourseAttachmentsFunction returningCourseAttachmentsFunction)
        {
            try
            {
                return returningCourseAttachmentsFunction();
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (Exception exception)
            {
                var failedCourseAttachmentServiceException =
                    new FailedCourseAttachmentServiceException(exception);

                throw CreateAndLogServiceException(failedCourseAttachmentServiceException);
            }
        }

        private CourseAttachmentValidationException CreateAndLogValidationException(Exception exception)
        {
            var courseAttachmentValidationException = new CourseAttachmentValidationException(exception);
            this.loggingBroker.LogError(courseAttachmentValidationException);

            return courseAttachmentValidationException;
        }

        private CourseAttachmentDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var courseAttachmentDependencyException = new CourseAttachmentDependencyException(exception);
            this.loggingBroker.LogCritical(courseAttachmentDependencyException);

            return courseAttachmentDependencyException;
        }

        private CourseAttachmentDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var courseAttachmentDependencyException = new CourseAttachmentDependencyException(exception);
            this.loggingBroker.LogError(courseAttachmentDependencyException);

            return courseAttachmentDependencyException;
        }

        private CourseAttachmentServiceException CreateAndLogServiceException(Exception exception)
        {
            var courseAttachmentServiceException = new CourseAttachmentServiceException(exception);
            this.loggingBroker.LogError(courseAttachmentServiceException);

            return courseAttachmentServiceException;
        }
    }
}
