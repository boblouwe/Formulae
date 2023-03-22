using System.Globalization;
using FluentAssertions;
using Formulae;
using Xunit;

namespace FormulaeTests
{
    public class FormulaTests
    {
        [Fact]
        public void Formula_value_should_be_null_when_unevaluated()
        {
            var realValue = new Constant("realValue", new Number("23.1", CultureInfo.InvariantCulture));
            var meterReading = new Constant("meterReading", new Number("23,3"));
            var formulePar = new Formula("meterReading", "meterReading", new[] { meterReading });
            var formula = new Formula("AbsoluteError", "realValue - meterReading", new Variable[] { realValue, formulePar });

            var evaluation = formula.Evaluate(EvaluationTrace.Current(nameof(Formula_value_should_be_null_when_unevaluated)));
            evaluation.Number.Value.Should().Be(-0.2);

            evaluation = formula.Reevaluate();
            evaluation.Number.Precision.Should().Be(1);
            
        }
/*
        [Fact]
        public void Formula_value_should_not_be_null_when_evaluated()
        {
            var formula = new FormulaOLd("AbsoluteError", "realValue - meterReading", "ln");
            var parameters = new Parameters
            {
                { "realValue", new Number(23.1, 1), "ln" },
                { "meterReading", new Number(23.3, 1), "ln" }
            };
            formula.Parameters.Add(parameters);
            formula.Evaluate();
            formula.Value.Should().NotBeNull();
        }

        [Fact]
        public void Formula_value_should_be_changed_when_parameter_value_has_changed_and_re_evaluated()
        {
            var formula = new FormulaOLd("AbsoluteError", "realValue - meterReading", "ln");
            var parameters = new Parameters();
            var realValueParameter = new Parameter("realValue", new Number(23.4, 1), "ln");
            parameters.Add(realValueParameter);
            parameters.Add("meterReading", new Number(23.3, 1), "ln");
            formula.Parameters.Add(parameters);
            formula.Evaluate();
            formula.Value.Value.Should().Be(0.1);
            realValueParameter.SetValue(23.1, 1);
            formula.Evaluate();
            formula.Value.Value.Should().Be(-0.2);
        }

        [Fact]
        public void Formula_value_should_be_correct_when_another_formula_is_a_parameter()
        {
            var absoluteErrorFormula = new FormulaOLd("AbsoluteError", "realValue - meterReading", "ln");
            var parameters = new Parameters();
            var realValueParameter = new Parameter("realValue", new Number(23.1, 1), "ln");
            parameters.Add(realValueParameter);
            parameters.Add("meterReading", new Number(23.0, 1), "ln");
            absoluteErrorFormula.Parameters.Add(parameters);

            var formula = new FormulaOLd("kln", "factor * x", "kln");
            formula.AddParameter("factor", 1000, 0);
            formula.AddFormula("x", absoluteErrorFormula);
            var evaluationResult = formula.Evaluate();
            evaluationResult.Value.Value.Should().Be(100);
        }
        */
    }
}
