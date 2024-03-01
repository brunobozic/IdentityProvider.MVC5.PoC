﻿using System.Text;

namespace Module.CrossCutting.Domain
{
    public abstract class ValueObjectBase
    {
        private readonly List<BusinessRule> _brokenRules = new List<BusinessRule>();
        protected abstract void Validate();

        public void ThrowExceptionIfInvalid()
        {
            _brokenRules.Clear();
            Validate();
            if (_brokenRules.Any())
            {
                var issues = new StringBuilder();
                foreach (var businessRule in _brokenRules)
                    issues.AppendLine(businessRule.Rule);

                throw new ValueObjectIsInvalidException(issues.ToString());
            }
        }

        protected void AddBrokenRule(BusinessRule businessRule)
        {
            _brokenRules.Add(businessRule);
        }
    }
}