using FluentValidation.TestHelper;
using MidnightLizard.Impressions.Commander.Requests.Common;
using MidnightLizard.Testing.Utilities;
using System;

namespace MidnightLizard.Impressions.Commander.Requests.Base
{
    public class RequestSpec
    {
        public class ValidatorSpec
        {
            private readonly RequestValidator validator = new RequestValidator();

            [It(nameof(RequestValidator))]
            public void Should_fail_when_some_required_fields_are_empty()
            {
                this.validator.ShouldHaveValidationErrorFor(x => x.Id, Guid.Empty);
                this.validator.ShouldHaveValidationErrorFor(x => x.AggregateId, Guid.Empty);
            }

            [It(nameof(RequestValidator))]
            public void Should_succeed_when_required_fields_have_values()
            {
                this.validator.ShouldNotHaveValidationErrorFor(x => x.Id, Guid.NewGuid());
                this.validator.ShouldNotHaveValidationErrorFor(x => x.AggregateId, Guid.NewGuid());
            }
        }
    }
}
