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
using OtripleS.Web.Api.Models.ExamFees;
using OtripleS.Web.Api.Models.ExamFees.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.ExamFees
{
    public partial class ExamFeeService
    {
        private delegate ValueTask<ExamFee> ReturningExamFeeFunction();
        private delegate IQueryable<ExamFee> ReturningExamFeesFunction();

        private async ValueTask<ExamFee> TryCatch(
            ReturningExamFeeFunction returningExamFeeFunction)
        {
            try
            {
                return await returningExamFeeFunction();
            }

            catch (NullExamFeeException nullExamFeeException)
            {
                throw CreateAndLogValidationException(nullExamFeeException);
            }
            catch (InvalidExamFeeException invalidExamFeeInputException)
            {
                throw CreateAndLogValidationException(invalidExamFeeInputException);
            }
            catch (NotFoundExamFeeException nullExamFeeException)
            {
                throw CreateAndLogValidationException(nullExamFeeException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsExamFeeException =
                    new AlreadyExistsExamFeeException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsExamFeeException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidExamFeeReferenceException =
                    new InvalidExamFeeReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogValidationException(invalidExamFeeReferenceException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedExamFeeException = new LockedExamFeeException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedExamFeeException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw CreateAndLogDependencyException(dbUpdateException);
            }
            catch (Exception exception)
            {
                var failedExamFeeServiceException =
                    new FailedExamFeeServiceException(exception);

                throw CreateAndLogServiceException(failedExamFeeServiceException);
            }
        }

        private IQueryable<ExamFee> TryCatch(
            ReturningExamFeesFunction returningExamFeesFunction)
        {
            try
            {
                return returningExamFeesFunction();
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (Exception exception)
            {
                var failedExamFeeServiceException =
                    new FailedExamFeeServiceException(exception);

                throw CreateAndLogServiceException(failedExamFeeServiceException);
            }

        }

        private ExamFeeValidationException CreateAndLogValidationException(Exception exception)
        {
            var examFeeValidationException = new ExamFeeValidationException(exception);
            this.loggingBroker.LogError(examFeeValidationException);

            return examFeeValidationException;
        }

        private ExamFeeDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var examFeeDependencyException = new ExamFeeDependencyException(exception);
            this.loggingBroker.LogCritical(examFeeDependencyException);

            return examFeeDependencyException;
        }

        private ExamFeeDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var examFeeDependencyException = new ExamFeeDependencyException(exception);
            this.loggingBroker.LogError(examFeeDependencyException);

            return examFeeDependencyException;
        }

        private ExamFeeServiceException CreateAndLogServiceException(Exception exception)
        {
            var examFeeServiceException = new ExamFeeServiceException(exception);
            this.loggingBroker.LogError(examFeeServiceException);

            return examFeeServiceException;
        }
    }
}
