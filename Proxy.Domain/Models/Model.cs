using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Proxy.Domain.Models
{
    public class Model : IValidatableObject
    {
        /// <summary>
        /// Performs Validation
        /// </summary>
        public virtual void Validate()
        {
            var context = new ValidationContext(this, serviceProvider: null, items: null);
            Validator.ValidateObject(this, context);
        }
        /// <summary>
        /// Sets up rules for Validation
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext) => Array.Empty<ValidationResult>();
    }
}
